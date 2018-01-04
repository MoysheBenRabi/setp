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
package mxp.iot;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.List;
import java.util.UUID;
import java.util.Vector;
import java.util.concurrent.TimeoutException;

import mxp.message.DisappearanceEvent;
import mxp.message.EjectRequest;
import mxp.message.EjectResponse;
import mxp.message.ExamineRequest;
import mxp.message.InjectRequest;
import mxp.message.InjectResponse;
import mxp.message.InteractRequest;
import mxp.message.InteractionFragment;
import mxp.message.JoinRequest;
import mxp.message.LeaveRequest;
import mxp.message.LeaveResponse;
import mxp.message.Message;
import mxp.message.ModifyRequest;
import mxp.message.ModifyResponse;
import mxp.message.ObjectFragment;
import mxp.message.PerceptionEvent;
import mxp.message.ProgramFragment;
import mxp.message.ResponseFragment;
import mxp.message.SynchronizationBeginEvent;
import mxp.net.AddressId;
import mxp.net.Communication;
import mxp.net.MessageHandler;
import mxp.net.Session;

/**
 * Perform a test of connecting to a reference server and exchanging
 * reference messages.
 *
 * @see <a href="http://iot.bubblecloud.org/">MXP IOT</a>
 */
public final class ClientToReferenceServer {
    /**
     * The number of milliseconds at 1st January 2000 elapsed since
     * the usual UNIX epoch of 1st January 1970.
     */
     private static final long EPOCH2K =
                new GregorianCalendar(2000, 0, 1, 0, 0, 0).getTime().getTime();

     /**
      * The generic timeout value we're using, in milliseconds.
      */
     private static final long TIMEOUT = 200;

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
        public void handleClosedSession(int sessionId, SocketAddress address,
                int remoteSessionId) {
            // don't care
        }

        @Override
        public void handleNewSession(int sessionId, SocketAddress address,
                int remoteSessionId) {
            // don't care
        }
     }

    /**
     * The host to connect to.
     */
    private String hostname;

    /**
     * The port to connect to.
     */
    private int port;

    /**
     * The session id towards the server.
     */
    private int sessionId;

    /**
     * The handler for receiving new messages.
     */
    private TestMessageHandler messageHandler = new TestMessageHandler();

    /**
     *  No need to instantiate this class from the outside.
     *
     *  @param hostname the host to connect to.
     *  @param port the port to connect to
     */
    private ClientToReferenceServer(String hostname, int port) {
        this.hostname = hostname;
        this.port     = port;
    }

    /**
     * Program entry point. Provide the host name and the port to connect to.
     *
     * @param args the program arguments. a hostname and a port is expected.
     * @throws IOException on I/O errors
     */
    public static void main(String[] args) throws IOException {
        if (args.length != 2) {
            System.out.println("please provide a hostname and a port");

            System.exit(-1);
        }

        String hostname = args[0];
        int    port     = Integer.parseInt(args[1]);

        ClientToReferenceServer ref =
                                new ClientToReferenceServer(hostname, port);

        try {
            ref.exchangeReferenceMessages();
        } catch (TimeoutException e) {
            System.out.println("connection timeout...");
        }
    }

    /**
     * Connect to a reference server, and perform the exchange of reference
     * messages.
     *
     * @throws IOException on I/O errors
     * @throws TimeoutException if the session could not be established
     *         in time
     */
    public void exchangeReferenceMessages() throws IOException,
                                                   TimeoutException {
        SocketAddress serverAddress = new InetSocketAddress(hostname, port);
        Communication comm          = new Communication(messageHandler);
        Message             response;
        ResponseFragment    rh;
        ObjectFragment      of;

        sessionId = comm.openSession(serverAddress, generateJoinRequest());
        Session session = comm.getSessions().get(sessionId);

        sendMessage(comm, generateInjectRequest());
        response = waitForMessage(Message.Type.INJECT_RESPONSE, TIMEOUT);
        rh       = ((InjectResponse) response).getResponseHeader();
        assert (rh.getFailureCode() == 0);
        assert (rh.getRequestMessageId() == session.getMessageId());

        response = waitForMessage(Message.Type.SYNCHRONIZATION_BEGIN_EVENT,
                                  TIMEOUT);
        SynchronizationBeginEvent sbe = (SynchronizationBeginEvent) response;
        assert (sbe.getObjectCount() == 1);

        response = waitForMessage(Message.Type.PERCEPTION_EVENT, TIMEOUT);
        of = ((PerceptionEvent) response).getObject();
        assert (of.equals(generateObjectFragment()));

        response = waitForMessage(Message.Type.SYNCHRONIZATION_END_EVENT,
                                  TIMEOUT);

        sendMessage(comm, generateModifyRequest());
        response = waitForMessage(Message.Type.MODIFY_RESPONSE, TIMEOUT);
        rh       = ((ModifyResponse) response).getResponseHeader();
        assert (rh.getFailureCode() == 0);
        assert (rh.getRequestMessageId() == session.getMessageId());

        response = waitForMessage(Message.Type.PERCEPTION_EVENT, TIMEOUT);
        of = ((PerceptionEvent) response).getObject();
        assert (of.equals(generateModifiedObjectFragment()));

        sendMessage(comm, generateInteractRequest());
        // interact response sending is not implemented in the test server

        sendMessage(comm, generateExamineRequest());
        // examine response sending is not implemented in the test server

        sendMessage(comm, generateEjectRequest());
        response = waitForMessage(Message.Type.EJECT_RESPONSE, TIMEOUT);
        rh       = ((EjectResponse) response).getResponseHeader();
        assert (rh.getFailureCode() == 0);
        assert (rh.getRequestMessageId() == session.getMessageId());

        response = waitForMessage(Message.Type.DISAPPEARANCE_EVENT, TIMEOUT);
        assert (((DisappearanceEvent) response).getObjectIndex() == 1);

        sendMessage(comm, generateLeaveRequest());
        response = waitForMessage(Message.Type.LEAVE_RESPONSE, TIMEOUT);
        rh       = ((LeaveResponse) response).getResponseHeader();
        assert (rh.getFailureCode() == 0);
        assert (rh.getRequestMessageId() == session.getMessageId());

        comm.closeSession(sessionId);
        comm.close();
    }

    /**
     * Send a message to the server.
     *
     * @param comm the communication object to send the message through
     * @param message the message to send
     * @throws IOException on I/O errors
     */
    void sendMessage(Communication comm, Message message)
                                                    throws IOException {
        Vector<Message> messages      = new Vector<Message>();

        messages.add(message);

        comm.send(messages, sessionId, true);
    }

    /**
     * Wait for a specific message to arrive. The function will return only when
     * the server sends a message of specified type back, or if the specified
     * timeout is exceeded.
     *
     * @param responseType an expected response message type to wait for
     * @param timeout the maximum time in milliseconds to wait
     * @return the received response message
     * @throws IOException on I/O errors
     * @throws TimeoutException if a response message of the specified type
     *         was not returned
     */
    Message waitForMessage(Message.Type        responseType,
                           long                timeout)
                                                    throws IOException,
                                                           TimeoutException {
        long            endTime       = System.currentTimeMillis() + timeout;

        while (System.currentTimeMillis() < endTime) {
            try {
                Thread.yield();
                Thread.sleep(10);
            } catch (InterruptedException e) {
            }

            for (int i = 0;
                 i < messageHandler.messages.size();
                 ++i) {

                if (messageHandler.messages.get(i).getType() == responseType) {
                    return messageHandler.messages.get(i);
                }
            }
        }

        throw new TimeoutException();
    }

    /**
     * Generate a test Program fragment.
     *
     * @return a Program fragment
     */
    public ProgramFragment generateProgramFragment() {
        ProgramFragment            pf = new ProgramFragment();

        pf.setProgramName("ClientProgram");
        pf.setProgramMajorVersion((byte) 5);
        pf.setProgramMinorVersion((byte) 6);
        pf.setProtocolMajorVersion((byte) 0);
        pf.setProtocolMinorVersion((byte) 5);
        pf.setProtocolSourceRevision(245);

        return pf;
    }

    /**
     * Generate the join request to be sent to the reference server.
     *
     * @return the generated join request.
     */
    public JoinRequest generateJoinRequest() {
        ProgramFragment            pf = generateProgramFragment();

        JoinRequest                jr = new JoinRequest();

        jr.setBubbleId(new UUID(0x16fa9d53525b9f4cL,
                                0x9a09ad746520873eL));
        jr.setAvatarId(new UUID(0x16fa3d12525b9f4cL,
                                0x9a09ad7465208321L));
        jr.setBubbleName("");
        jr.setLocationName("");
        jr.setParticipantId("TestParticipantName");
        jr.setParticipantSecret("TestParticipantPassphrase");
        jr.setParticipantRealtime(new Date(EPOCH2K));
        jr.setIdentityProviderUrl("http://test.identityprovider");
        jr.setClientProgram(pf);

        return jr;
    }

    /**
     * Generate an object fragment.
     *
     * @return an object fragment.
     */
    ObjectFragment generateObjectFragment() {
        ObjectFragment              of = new ObjectFragment();

        of.setObjectId(new UUID(0x16fa3d12525b9f4cL,
                                0x9a09ad7465208321L));
        of.setObjectIndex(0);
        of.setTypeId(new UUID(0x9a89161364b30f47L,
                              0xbb19b030a131df4fL));
        of.setParentObjectId(new UUID(0x39dcff04519e6d4aL,
                                      0x8112e6dd3498dbe9L));
        of.setObjectName("TestObjectName");
        of.setTypeName("TestObjectType");
        of.setOwnerId(new UUID(0x96f6cb72cb189146L,
                               0x960f08175da6f492L));
        of.setLocation(new float[] { 2f, 3f, 4f });
        of.setVelocity(new float[] { 5f, 6f, 7f });
        of.setAcceleration(new float[] { 8f, 9f, 10f });
        of.setOrientation(new float[] { 11f, 12f, 13f, 14f });
        of.setAngularVelocity(new float[] { 15f, 16f, 17f, 18f });
        of.setAngularAcceleration(new float[] { 19f, 20f, 21f, 22f });
        of.setBoundingSphereRadius(23f);
        of.setMass(24f);
        of.setExtensionDialect("TEDI");
        of.setExtensionDialectMajorVersion((byte) 24);
        of.setExtensionDialectMinorVersion((byte) 23);
        of.setExtensionData(new byte[] {
             48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
             48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
             48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
             48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
             48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
             48, 49, 50, 51, 52, 53, 54, 55, 56, 57 });

        return of;
    }

    /**
     * Generate an Inject request message.
     *
     * @return a test Inject request.
     */
    public InjectRequest generateInjectRequest() {
        ObjectFragment              of = generateObjectFragment();
        InjectRequest               ir = new InjectRequest();

        ir.setObject(of);

        return ir;
    }

    /**
     * Generate an Examine request message.
     *
     * @return a test examine request message
     */
    public ExamineRequest generateExamineRequest() {
        ExamineRequest  er = new ExamineRequest();

        er.setObjectId(new UUID(0x0000000000000000L,
                                0x0000000000000000L));
        er.setObjectIndex(1);

        return er;
    }

    /**
     * Generate a modified object fragment.
     *
     * @return a modified object fragment.
     */
    ObjectFragment generateModifiedObjectFragment() {
        ObjectFragment              of = generateObjectFragment();

        of.setLocation(new float[] { 20f, 30f, 40f });

        return of;
    }

    /**
     * Generate an Modify request message.
     *
     * @return a test modify request message
     */
    public ModifyRequest generateModifyRequest() {
        ObjectFragment of = generateModifiedObjectFragment();
        ModifyRequest  mr = new ModifyRequest();

        mr.setObject(of);

        return mr;
    }

    /**
     * Generate an a test Interaction Fragment.
     *
     * @return a test Interaction fragment.
     */
    public static InteractionFragment generateInteractionFragment() {
        InteractionFragment         iaf = new InteractionFragment();

        iaf.setInteractionName("");
        iaf.setSourceParticipantId(new UUID(0x72cbf69618cb4691L,
                                            0x960f08175da6f492L));
        iaf.setSourceObjectId(new UUID(0x0000000000000000L,
                                       0x0000000000000000L));
        iaf.setTargetParticipantId(new UUID(0x0000000000000000L,
                                            0x0000000000000000L));
        iaf.setTargetObjectId(new UUID(0x123dfa165b524c9fL,
                                       0x9a09ad7465208321L));
        iaf.setExtensionDialect("TEDI");
        iaf.setExtensionDialectMajorVersion((byte) 24);
        iaf.setExtensionDialectMinorVersion((byte) 25);
        iaf.setExtensionData(new byte[] {
                48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                48, 49, 50, 51, 52, 53, 54, 55, 56, 57 });

        return iaf;
    }

    /**
     * Generate an Interact request message.
     *
     * @return a test interact request message
     */
    public InteractRequest generateInteractRequest() {
        InteractionFragment iaf = generateInteractionFragment();
        InteractRequest     ir  = new InteractRequest();

        ir.setRequest(iaf);

        return ir;
    }

    /**
     * Generate an Eject request message.
     *
     * @return a test eject request message
     */
    public EjectRequest generateEjectRequest() {
        EjectRequest  er = new EjectRequest();

        er.setObjectId(new UUID(0x16fa3d12525b9f4cL,
                                0x9a09ad7465208321L));

        return er;
    }

    /**
     * Generate a Leave request message.
     *
     * @return a test leave request message
     */
    public LeaveRequest generateLeaveRequest() {
        LeaveRequest  lr = new LeaveRequest();

        return lr;
    }
}
