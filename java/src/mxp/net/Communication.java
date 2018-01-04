/*
 * Copyright (c) 2009-2010 Tyrell Corporation.
 *
 * The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 *
 * The Original Code is an implementation of the Metaverse eXchange Protocol.
 *
 * The Initial Developer of the Original Code is Akos Maroy.
 * All Rights Reserved.
 *
 * Contributor(s): Akos Maroy.
 *
 * Alternatively, the contents of this file may be used under the terms
 * of the Affero General Public License (the  "AGPL"), in which case the
 * provisions of the AGPL are applicable instead of those
 * above. If you wish to allow use of your version of this file only
 * under the terms of the AGPL and not to allow others to use
 * your version of this file under the MPL, indicate your decision by
 * deleting the provisions above and replace them with the notice and
 * other provisions required by the AGPL. If you do not delete
 * the provisions above, a recipient may use your version of this file
 * under either the MPL or the AGPL.
 */
package mxp.net;

import java.io.IOException;
import java.net.SocketAddress;
import java.net.SocketException;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Timer;
import java.util.TimerTask;
import java.util.TreeMap;
import java.util.Vector;
import java.util.concurrent.TimeoutException;

import mxp.message.Acknowledge;
import mxp.message.Keepalive;
import mxp.message.Message;
import mxp.packet.MessageFrame;
import mxp.packet.Packet;
import mxp.packet.Packetizer;

/**
 * Class handing an ongoing MXP-based communication by sending and receiving
 * MXP messages to and from other parties.
 * <p/>
 * This class either binds to a specific interface / port, which is a typical
 * server-side usage, or client usage behind a firewall where only a number of
 * UDP ports are let through, or binds to any port on all local interfaces,
 * which is a generic usage for a client.
 * <p/>
 * The class also maintains a list of packets to be acknowledged via an
 * acknowledge message, and sends such ack messages automatically. It will
 * also silently consume & process received acknowledge messages, and resend
 * packets which have not been acknowledged. This is all done because
 * semantically speaking, package acknowledgements should belong to the
 * packet layer, even though in MXP it's done by a message - a design flaw,
 * in the opinion of this implementor.
 * <p/>
 * The class also takes care of initial hand-shaking - establishing mutual
 * session ids on both sides of a communication session. In MXP, this means
 * that after an initial connection message is sent, the corresponding
 * acknowledge message has to be caught, and the session id on the remote side
 * has to be stored and used from there. In MXP, for each session there are two
 * unique ids, one from each side of the communication.
 * <p/>
 * The class sends keep-alive messages for each open session on a regular
 * basis, and silently consumes keep-alive messages for each session.
 * <p/>
 * If needed, this class does not store & maintain resources - in this mode,
 * all resources are expected to be maintained by the caller.
*/
public class Communication {
    /**
     * The frequency of packet sending - and the highest frequency of all
     * activity - in milliseconds.
     */
    static final long TICK_INTERVAL = 10;

    /**
     * The interval to send keep alive messages at - in milliseconds.
     */
    static final long KEEPALIVE_INTERVAL = 200;

    /**
     * The interval to send keep acknowledge messages at - in milliseconds.
     */
    static final long ACK_INTERVAL = 200;

    /**
     * The amount of time to wait for a packet acknowledgement, before resending
     * the packet - in milliseconds.
     */
    static final long ACK_TIMEOUT = 1000;

    /**
     * The maximum number of retries a guaranteed packet is sent, before
     * being discarded.
     */
    static final long MAX_PACKET_RESEND_COUNT = 3;

    /**
     * Inner class handling reception of packets from the server connection.
     */
    private class InnerPacketHandler implements PacketHandler {

        @Override
        public void packetReceived(Packet          packet,
                                   SocketAddress   client) {
            handlePacket(packet, client);
        }
    }

    /**
     * Inner class to perform timely activities, like sending messages,
     * acknowledging packets, re-sending packets, etc.
     */
    private class Ticker extends TimerTask {
        /**
         * Acknowledge the receipt of packets by sending acknowledge
         * messages.
         */
        public synchronized void run() {
            try {
                kickDormantSessions();

                for (Session session : sessions) {
                    generateKeepaliveMessages(session);
                    generateAcknowledgeMessages(session);
                    sendPendingMessages(session);
                    resendPackets(session);
                }
            } catch (IOException e) {
            }
        }
    }

    /**
     * The connection object, handling receiving and receiving packets.
     */
    private Connection              connection;

    /**
     * The object that handles reception of packets from the underlying
     * server connection.
     */
    private InnerPacketHandler      packetHandler;

    /**
     * Handler for receiving new messages.
     */
    private MessageHandler          messageHandler;

    /**
     * The session store to store session related information.
     */
    private SessionStore            sessions;

    /**
     * A map of sessions just being opened, keyed by an address and a packet
     * id pair - the address is the remote address we're opening the session
     * with, the packet id is the id of the packet carrying the first message
     * towards the other in for this session. we're expecting to read the
     * remote session id in an ack message as a response to the first packet.
     */
    private Map<AddressId, Session> openingSessions;

    /**
     * The timer object for executing recurring events.
     */
    private Timer                   timer;

    /**
     * Create a communication object which binds to a specific network interface
     * and port. The supplied session store will be used to manage session
     * related resources, and can be re-used in subsequent communication
     * objects.
     * When the object is not needed anymore, make sure to close this object
     * with the close() function.
     *
     * @param address the address and port to bind to.
     * @param messageHandler the object that will handle messages received.
     * @param sessions a store of sessions - new sessions initiated by this
     *        communication object will be put into this store.
     * @throws SocketException on socket errors
     */
    public
    Communication(SocketAddress                    address,
                  MessageHandler                   messageHandler,
                  SessionStore                     sessions)
                                                    throws SocketException {
        this.messageHandler       = messageHandler;
        this.sessions             = sessions;

        openingSessions  = new HashMap<AddressId, Session>();

        packetHandler    = new InnerPacketHandler();
        connection       = new Connection(address, packetHandler);
        timer            = new Timer();
        timer.scheduleAtFixedRate(new Ticker(), TICK_INTERVAL, TICK_INTERVAL);
    }

    /**
     * Create a communication object which binds to a specific network interface
     * and port. All resources are handled by this connection object.
     * Any pending resources, such as packets to be acknowledged or partial
     * message frames are lost after the connection object is closed /
     * destroyed.
     *
     * When the object is not needed anymore, make sure to close this object
     * with the close() function.
     *
     * @param address the address and port to bind to.
     * @param messageHandler the object that will handle messages received.
     * @throws SocketException on socket errors
     */
    public
    Communication(SocketAddress     address,
                  MessageHandler    messageHandler)
                                                    throws SocketException {
        this.messageHandler       = messageHandler;

        sessions         = new SimpleSessionStore();
        openingSessions  = new HashMap<AddressId, Session>();

        packetHandler    = new InnerPacketHandler();
        connection       = new Connection(address, packetHandler);
        timer            = new Timer();
        timer.scheduleAtFixedRate(new Ticker(), TICK_INTERVAL, TICK_INTERVAL);
    }

    /**
     * Create a communication object which binds to a random port on all local
     * network interfaces.
     *
     * The supplied session store will be used to manage session
     * related resources, and can be re-used in subsequent communication
     * objects.
     * When the object is not needed anymore, make sure to close this object
     * with the close() function.
     *
     * @param messageHandler the object that will handle messages received.
     * @param sessions a store of sessions - new sessions initiated by this
     *        communication object will be put into this store.
     * @throws SocketException on socket errors
     */
    public
    Communication(MessageHandler                   messageHandler,
                  SimpleSessionStore               sessions)
                                                    throws SocketException {
        this.messageHandler       = messageHandler;
        this.sessions             = sessions;

        openingSessions  = new HashMap<AddressId, Session>();

        packetHandler    = new InnerPacketHandler();
        connection       = new Connection(packetHandler);
        timer            = new Timer();
        timer.scheduleAtFixedRate(new Ticker(), TICK_INTERVAL, TICK_INTERVAL);
    }

    /**
     * Create a communication object which binds to a random port on all local
     * network interfaces.
     *
     * All resources are handled by this connection object.
     * Any pending resources, such as packets to be acknowledged or partial
     * message frames are lost after the connection object is closed /
     * destroyed.
     *
     * When the object is not needed anymore, make sure to close this object
     * with the close() function.
     *
     * @param messageHandler the object that will handle messages received.
     * @throws SocketException on socket errors
     */
    public
    Communication(MessageHandler messageHandler)
                                                    throws SocketException {
        this.messageHandler       = messageHandler;

        sessions         = new SimpleSessionStore();
        openingSessions  = new HashMap<AddressId, Session>();

        packetHandler    = new InnerPacketHandler();
        connection       = new Connection(packetHandler);
        timer            = new Timer();
        timer.scheduleAtFixedRate(new Ticker(), TICK_INTERVAL, TICK_INTERVAL);
    }

    /**
     * Close the communication object, and stop listening for messages.
     */
    public synchronized void close() {
        for (Session session : sessions) {
            closeSession(session.getSessionId());
        }

        timer.cancel();
        connection.close();
    }

    /**
     * Access the session store that this communication object uses.
     *
     * @return the session store of this communication object.
     */
    public SessionStore getSessions() {
        return sessions;
    }

    /**
     * Start a session by sending an initial message. This call blocks until
     * an acknowledgement is received from the server side, and both session
     * ids are established.
     *
     * @param address the address of the remote end we want to start a session
     *        with.
     * @param message the initial message to send to the server side, must be
     *        one of: join - what else?
     * @return the local id of the new session just started.
     * @throws IOException on I/O errors
     * @throws TimeoutException when a session could not be established
     *         within the specified timeout
     * @see #closeSession
     */
    public synchronized
    int openSession(SocketAddress  address,
                    Message        message) throws IOException,
                                                   TimeoutException {
        List<Message>   messages  = new Vector<Message>();
        int             sessionId = sessions.newSession();
        Session         session   = sessions.get(sessionId);

        messages.add(message);

        // send the message - we're putting it together here, as we're
        // interested in the id of the packet carrying the message
        Vector<Packet>  packets  = new Vector<Packet>();

        Packetizer.messagesToPackets(messages,
                                     session.nextMessageId(),
                                     packets,
                                     session.getSessionId(),
                                     0, // packet id, set by the send() call
                                     new Date(),
                                     (byte) 1);

        if (packets.isEmpty() || messages.isEmpty()) {
            throw new IOException();
        }

        for (Packet packet : packets) {
            connection.send(address, packet);
            session.getPacketsPendingAck().put(packet.getPacketId(), packet);
        }

        // when processing acknowledge message, we'll catch the ack for
        // this packet there, which enables us to associate this
        // session with the remote ends session
        int       packetId = packets.firstElement().getPacketId();
        AddressId ai       = new AddressId(address, packetId);
        openingSessions.put(ai, session);

        // now wait for an ack to come
        // TODO: do resend?
        long startTime = System.currentTimeMillis();
        long endTime   = startTime + ACK_TIMEOUT;
        while (System.currentTimeMillis() < endTime) {
            try {
                wait(ACK_TIMEOUT - (System.currentTimeMillis() - startTime));
            } catch (InterruptedException e) {
            }

            // if we received an ack for this message, it has been removed
            // from the opening sessions map, and we're all done
            if (!openingSessions.containsKey(ai)) {
                return sessionId;
            }
        }

        throw new TimeoutException();
    }

    /**
     * Close a session.
     *
     * @param sessionId the local id of the session to close.
     * @throws IllegalArgumentException if the specified session does not exist
     */
    public synchronized
    void closeSession(int sessionId) throws IllegalArgumentException {
        Session session = sessions.get(sessionId);

        sessions.remove(sessionId);

        messageHandler.handleClosedSession(session.getSessionId(),
                                           session.getAddress(),
                                           session.getRemoteSessionId());
    }

    /**
     * Put a single message into the message queue, so that it is sent to the
     * other end in the next iteration.
     *
     * @param message the message to send.
     * @param sessionId the id of the local session this message is part of.
     * @param guaranteed signal if the packages carrying this message have to
     *        be guaranteed or not.
     * @throws IllegalArgumentException if the specified session does not exist
     */
    public synchronized
    void send(Message        message,
              int            sessionId,
              boolean        guaranteed) throws IllegalArgumentException {

        Session session = sessions.get(sessionId);

        SimpleMessageQueueItem mqi = new SimpleMessageQueueItem();

        mqi.setMessage(message);
        mqi.setGuaranteed(guaranteed);

        session.getMessageQueue().add(mqi);
    }

    /**
     * Put messages into the message queue, so they are sent to the other end
     * in the next iteration.
     *
     * @param messages the messages to send.
     * @param sessionId the id of the local session this message is part of.
     * @param guaranteed signal if the packages carrying this message have to
     *        be guaranteed or not.
     * @throws IllegalArgumentException if the specified session does not exist
     */
    public synchronized
    void send(List<Message>  messages,
              int            sessionId,
              boolean        guaranteed) throws IllegalArgumentException {

        Session session = sessions.get(sessionId);

        for (Message message : messages) {
            SimpleMessageQueueItem mqi = new SimpleMessageQueueItem();

            mqi.setMessage(message);
            mqi.setGuaranteed(guaranteed);

            session.getMessageQueue().add(mqi);
        }
    }

    /**
     * Kick sessions which have not sent a keep-alive packet for over
     * three times our keep-alive interval.
     */
    private synchronized void kickDormantSessions() {
        final long kickIfBefore = System.currentTimeMillis()
                                - KEEPALIVE_INTERVAL * 3L;
        List<Integer> closeSessionIds = new Vector<Integer>();

        for (Session session : sessions) {
            if (session.getLastMessageTime().getTime() < kickIfBefore) {
                closeSessionIds.add(session.getSessionId());
            }
        }

        for (Integer sessionId : closeSessionIds) {
            closeSession(sessionId);
        }
    }

    /**
     * Generate keep-alive messages for all open sessions that were sent a
     * keep-alive too long ago. Put the generated messages into the message
     * queue.
     *
     * @param session to session to perform this activity for.
     */
    private synchronized
    void generateKeepaliveMessages(Session session) {
        final long sendIfBefore = System.currentTimeMillis()
                                - KEEPALIVE_INTERVAL;

        if (session.getLastKeepalive().getTime() < sendIfBefore) {
            send(new Keepalive(), session.getSessionId(), true);
            session.setLastKeepalive(new Date());
        }
    }

    /**
     * Generate acknowledge messages for each session from which packets have
     * been received that require acknowledgement.
     *
     * @param session to session to perform this activity for.
     */
    private synchronized
    void generateAcknowledgeMessages(Session session) {
        List<Integer> packetsToAck = session.getPacketsToAck();

        if (packetsToAck.isEmpty()) {
            return;
        }

        final long ackIfBefore = System.currentTimeMillis() - ACK_INTERVAL;

        if (session.getLastAckTime().getTime() < ackIfBefore) {
            List<Message> messages = new Vector<Message>();

            acknowledgePackets(messages, packetsToAck);
            send(messages, session.getSessionId(), false);

            packetsToAck.clear();
            session.setLastAckTime(new Date());
        }
    }

    /**
     * Send all messages that are in the message queue.
     *
     * @param session to session to perform this activity for.
     * @throws IOException on I/O errors
     */
    private synchronized
    void sendPendingMessages(Session session) throws IOException {
        if (session.getAddress() == null) {
            return;
        }

        Vector<Packet>  packets           = new Vector<Packet>();
        Vector<Packet>  guaranteedPackets = new Vector<Packet>();
        Vector<Message> messages          = new Vector<Message>(1);
        Map<Integer, Packet> packetsPendingAck =
                                            session.getPacketsPendingAck();

        for (MessageQueueItem mqi : session.getMessageQueue()) {

            messages.clear();
            messages.add(mqi.getMessage());

            if (mqi.isGuaranteed()) {
                Packetizer.messagesToPackets(messages,
                                             session.nextMessageId(),
                                             guaranteedPackets,
                                             session.getSessionId(),
                                             0, // packet id, set by send()
                                             new Date(),
                                             (byte) 1);
            } else {
                Packetizer.messagesToPackets(messages,
                                             session.nextMessageId(),
                                             packets,
                                             session.getSessionId(),
                                             0, // packet id, set by send()
                                             new Date(),
                                             (byte) 0);
            }
        }

        session.getMessageQueue().clear();

        for (Packet packet : packets) {
            connection.send(session.getAddress(), packet);
        }
        for (Packet packet : guaranteedPackets) {
            connection.send(session.getAddress(), packet);
            packetsPendingAck.put(packet.getPacketId(), packet);
        }
    }

    /**
     * Re-send packets that are guaranteed, but have not received
     * acknowledgement within an expected timeout.
     *
     * @param session to session to perform this activity for.
     * @throws IOException on I/O errors
     */
    private synchronized
    void resendPackets(Session session) throws IOException {
        if (session.getAddress() == null) {
            return;
        }

        final long now = System.currentTimeMillis();

        Map<Integer, Packet> packetsPendingAck =
                                            session.getPacketsPendingAck();

        for (Packet packet : packetsPendingAck.values()) {
            if (packet.getFirstSendTime().getTime()
              < now - (ACK_TIMEOUT * (packet.getResendCount() + 1))) {

                if (packet.getResendCount() > MAX_PACKET_RESEND_COUNT) {
                    packetsPendingAck.remove(packet.getPacketId());
                } else {
                    packet.setResendCount((byte) (packet.getResendCount() + 1));
                    connection.send(session.getAddress(), packet);
                }
            }
        }
    }

    /**
     * Handle the event of receiving new packets.
     *
     * This function will create and register a new session object if a new
     * packet from a previously unknown session is received.
     *
     * This function will process and consume acknowledge and keep-alive
     * messages.
     *
     * @param packet the packet received.
     * @param address the address and port of the other party, sending the
     *                packets.
     */
    private synchronized
    void handlePacket(Packet            packet,
                      SocketAddress     address) {
        try {
            if (!sessions.contains(address, packet.getSessionId())) {
                handleOpeningPacket(packet, address);
            }
            if (!sessions.contains(address, packet.getSessionId())) {
                return;
            }

            Session session = sessions.get(address, packet.getSessionId());

            if (packet.getGuaranteed() != 0) {
                // if the packet expects an ack, put it in the ack list
                session.getPacketsToAck().add(packet.getPacketId());
            }

            // turn the packet into a message
            Vector<Packet>  packets    = new Vector<Packet>();
            Vector<Message> messages   = new Vector<Message>();
            Vector<Integer> messageIds = new Vector<Integer>();
            packets.add(packet);

            Packetizer.packetsToMessages(packets,
                                         messages,
                                         messageIds,
                                         session.getMessageFrames());

            if (messages.isEmpty()) {
                return;
            }

            session.setLastMessageTime(new Date());

            // get ack messages from the queue, and process them locally
            Vector<Message> nonAckMessages =
                                        new Vector<Message>(messages.size());
            Vector<Integer> nonAckMessageIds =
                                        new Vector<Integer>(messageIds.size());

            Iterator<Message> mit  = messages.iterator();
            Iterator<Integer> idit = messageIds.iterator();
            while (mit.hasNext()) {
                Message message   = mit.next();
                Integer messageId = idit.next();

                if (message.getType() == Message.Type.ACKNOWLEDGE) {
                    processAcknowledge((Acknowledge) message,
                                       address,
                                       packet.getPacketId(),
                                       packet.getSessionId());
                } else if (message.getType() == Message.Type.KEEPALIVE) {
                    // silently ignore keep-alive messages
                } else {
                    nonAckMessages.add(message);
                    nonAckMessageIds.add(messageId);
                }
            }

            // send all non-ack messages to the message handler
            if (!nonAckMessages.isEmpty()) {
                messageHandler.handleMessages(nonAckMessages,
                                              nonAckMessageIds,
                                              packet.getSessionId(),
                                              address);
            }
        } catch (IOException e) {
            // can't really do much about it - the packet received is bad
        }
    }

    /**
     * Handle the first response to a session opening message - which is
     * expected to be an acknowledgement message, or the very first
     * packet from a remote end.
     * This function wil create a new session object if needed, and
     * register it in the session store. it will not consume the packet
     * otherwise.
     *
     * @param packet the packet received.
     * @param address the address and port of the other party, sending the
     *                packets.
     * @throws IOException on I/O errors
     */
    private synchronized
    void handleOpeningPacket(Packet            packet,
                             SocketAddress     address) throws IOException {

        // turn the packet into a message
        Vector<Packet>  packets    = new Vector<Packet>();
        Vector<Message> messages   = new Vector<Message>();
        Vector<Integer> messageIds = new Vector<Integer>();
        Map<Integer, List<MessageFrame>> messageFrames =
                                   new TreeMap<Integer, List<MessageFrame>>();
        packets.add(packet);

        Packetizer.packetsToMessages(packets,
                                     messages,
                                     messageIds,
                                     messageFrames);

        if (messages.isEmpty()) {
            return;
        }
        Message message = messages.firstElement();

        // don't start a new session on keepalive messages
        if (message.getType() == Message.Type.KEEPALIVE) {
            return;
        }

        if (message.getType() != Message.Type.ACKNOWLEDGE) {
            // this is a brand new first packet from a brand new connection
            // from a new session
            int sessionId = sessions.newSession();
            sessions.updateRemoteSession(sessionId,
                                         address,
                                         packet.getSessionId());

            // notify interested parties
            messageHandler.handleNewSession(sessionId,
                                            address,
                                            packet.getSessionId());

            return;
        }

        // this is the first ack on a session that we just initiated
        Acknowledge ack = (Acknowledge) message;

        for (Integer pId : ack.getPacketIds()) {
            AddressId ai = new AddressId(address, pId);

            if (!openingSessions.containsKey(ai)) {
                continue;
            }

            Session session = openingSessions.get(ai);

            // this is why we came here - to update the session with the
            // remote address - remote session id pair
            sessions.updateRemoteSession(session.getSessionId(),
                                         address,
                                         packet.getSessionId());

            openingSessions.remove(ai);
            notifyAll();

            // notify interested parties
            messageHandler.handleNewSession(session.getSessionId(),
                                            address,
                                            packet.getSessionId());
        }
    }

    /**
     * Process a received acknowledge message by removing the packets
     * acknowledged from the local pending ack packet map.
     *
     * @param ack the acknowledge message to process
     * @param address the address of the remote end
     * @param packetId the id of the packet the ack message came in
     * @param sessionId the remote session this message was sent from
     */
    private synchronized
    void processAcknowledge(Acknowledge     ack,
                            SocketAddress   address,
                            int             packetId,
                            int             sessionId) {

        for (Integer pId : ack.getPacketIds()) {
            if (sessions.contains(address, sessionId)) {
                Session session = sessions.get(address, sessionId);
                session.getPacketsPendingAck().remove(pId);
            }
        }
    }

    /**
     * Generate acknowledge messages for a list of packet ids to be
     * acknowledged.
     *
     * @param messages a list of messages - the new ack messages will be
     *        appended to this list
     * @param packetIds the packed ids to acknowledge.
     */
    private synchronized
    void acknowledgePackets(List<Message> messages,
                            List<Integer> packetIds) {

        Acknowledge ack = new Acknowledge();
        int         ix  = 0;
        while (ix < packetIds.size()) {
            int size = Math.min(packetIds.size() - ix,
                                Acknowledge.MAX_PACKET_IDS);

            Vector<Integer> p = new Vector<Integer>(
                                                packetIds.subList(ix, size));
            ack.setPacketIds(p);

            messages.add(ack);

            ack = new Acknowledge();
            ix += size;
        }
    }
}
