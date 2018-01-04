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
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.util.Date;
import java.util.List;
import java.util.Vector;
import java.util.concurrent.TimeoutException;

import junit.framework.TestCase;
import mxp.message.Message;
import mxp.message.MessageTest;

/**
 * Test cases for client and server communication.
 */
public class CommunicationTest extends TestCase {
    /**
     * A message handler object used for testing.
     */
    private class TestMessageHandler implements MessageHandler {
        /**
         * A list of messages received.
         */
        private Vector<Message> messages = new Vector<Message>();

        /**
         * The ids of the messages received.
         */
        private Vector<Integer> messageIds = new Vector<Integer>();

        /**
         * The remote address and remote session ids of the received messages.
         */
        private Vector<AddressId> sessions = new Vector<AddressId>();

        @Override
        public void handleMessages(List<Message>    messages,
                                   List<Integer>    messageIds,
                                   int              sessionId,
                                   SocketAddress    address) {
            this.messages.addAll(messages);
            this.messageIds.addAll(messageIds);
            this.sessions.add(new AddressId(address, sessionId));
        }

        @Override
        public void handleClosedSession(int             sessionId,
                                        SocketAddress   address,
                                        int             remoteSessionId) {
            // well, don't really care
        }

        @Override
        public void handleNewSession(int            sessionId,
                                     SocketAddress  address,
                                     int            remoteSessionId) {
            // well, don't really care
        }
    }

    /**
     * Test a client and server communication object initializing and connecting
     * to each other, then sending a single message back and forth.
     *
     * @throws IOException on IO errors
     * @throws TimeoutException on message turn around timeouts
     */
    public void testSingleMassageTurnaround() throws IOException,
                                                     TimeoutException {
        SocketAddress       serverAddress =
                                    new InetSocketAddress("localhost", 5665);
        TestMessageHandler  serverMessageHandler = new TestMessageHandler();
        Communication serverComm = new Communication(serverAddress,
                                                     serverMessageHandler);

        TestMessageHandler  clientMessageHandler = new TestMessageHandler();
        Communication clientComm = new Communication(clientMessageHandler);

        // open a session by sending a message from the client to the server
        Message jr    = MessageTest.generateJoinRequest();
        int sessionId = clientComm.openSession(serverAddress, jr);

        assertTrue(clientComm.getSessions().contains(sessionId));

        Session clientSession = clientComm.getSessions().get(sessionId);
        assertEquals(clientSession.getAddress(), serverAddress);

        try {
            Thread.yield();
            Thread.sleep(100);
        } catch (InterruptedException e) {
        }

        assertEquals(serverMessageHandler.messages.size(), 1);
        assertEquals(serverMessageHandler.messages.firstElement(), jr);
        assertEquals(serverMessageHandler.messageIds.size(), 1);
        assertEquals(serverComm.getSessions().size(), 1);

        // verify that the client and server sessions correspond to each other
        assertTrue(serverComm.getSessions().contains(
                                        clientSession.getRemoteSessionId()));

        Session serverSession = serverComm.getSessions().get(
                                            clientSession.getRemoteSessionId());

        assertEquals(clientSession.getRemoteSessionId(),
                     serverSession.getSessionId());
        assertEquals(clientSession.getSessionId(),
                     serverSession.getRemoteSessionId());

        // send a message from the server to the client, and verify
        Message         jrr      = MessageTest.generateJoinResponse();
        serverComm.send(jrr, serverSession.getSessionId(), false);

        try {
            Thread.yield();
            Thread.sleep(100);
        } catch (InterruptedException e) {
        }

        assertEquals(clientMessageHandler.messages.size(), 1);
        assertEquals(clientMessageHandler.messages.firstElement(), jrr);
        assertEquals(serverMessageHandler.messageIds.size(), 1);
        assertTrue(serverMessageHandler.messageIds.firstElement() == 1);
        assertEquals(clientMessageHandler.sessions.size(), 1);

        clientComm.closeSession(clientSession.getSessionId());
        serverComm.closeSession(serverSession.getSessionId());

        clientComm.close();
        serverComm.close();
    }

    /**
     * Test the keep-alive nature and automatic connection kicking on not
     * receiving keep-alives anymore.
     *
     * @throws IOException on IO errors
     * @throws TimeoutException on message turn around timeouts
     */
    public void testKeepalive() throws IOException, TimeoutException {
        SocketAddress       serverAddress =
                                    new InetSocketAddress("localhost", 5665);
        TestMessageHandler  serverMessageHandler = new TestMessageHandler();
        Communication serverComm = new Communication(serverAddress,
                                                     serverMessageHandler);

        TestMessageHandler  clientMessageHandler = new TestMessageHandler();
        Communication clientComm = new Communication(clientMessageHandler);

        // open a session by sending a message from the client to the server
        Message jr    = MessageTest.generateJoinRequest();
        int sessionId = clientComm.openSession(serverAddress, jr);

        assertTrue(clientComm.getSessions().contains(sessionId));

        Session clientSession = clientComm.getSessions().get(sessionId);
        assertEquals(clientSession.getAddress(), serverAddress);

        try {
            Thread.yield();
            Thread.sleep(100);
        } catch (InterruptedException e) {
        }

        assertEquals(serverMessageHandler.messages.size(), 1);
        assertEquals(serverMessageHandler.messages.firstElement(), jr);
        assertEquals(serverMessageHandler.messageIds.size(), 1);
        assertEquals(serverComm.getSessions().size(), 1);

        // verify that the client and server sessions correspond to each other
        assertTrue(serverComm.getSessions().contains(
                                        clientSession.getRemoteSessionId()));

        Session serverSession = serverComm.getSessions().get(
                                            clientSession.getRemoteSessionId());

        assertEquals(clientSession.getRemoteSessionId(),
                     serverSession.getSessionId());
        assertEquals(clientSession.getSessionId(),
                     serverSession.getRemoteSessionId());

        // now just wait & see the last message timestamp always being
        // close to real time
        Date clientLast = clientSession.getLastMessageTime();
        Date serverLast = serverSession.getLastMessageTime();

        for (int i = 0; i < 5; ++i) {
            try {
                Thread.yield();
                Thread.sleep(Communication.KEEPALIVE_INTERVAL * 2L);
            } catch (InterruptedException e) {
            }

            Date cl = clientSession.getLastMessageTime();
            Date sl = serverSession.getLastMessageTime();

            assertTrue(clientLast.getTime() < cl.getTime());
            assertTrue(serverLast.getTime() < sl.getTime());

            clientLast = cl;
            serverLast = sl;
        }

        // now close the session on one side, and see that the other side
        // gets that session kicked
        assertTrue(serverComm.getSessions().size() == 1);
        clientComm.closeSession(clientSession.getSessionId());

        try {
            Thread.yield();
            Thread.sleep(Communication.KEEPALIVE_INTERVAL * 4L);
        } catch (InterruptedException e) {
        }
        // see that the server doesn't have any sessions now either
        assertTrue(serverComm.getSessions().size() == 0);

        clientComm.close();
        serverComm.close();
    }
}

