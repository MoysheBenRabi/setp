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
package mxp.packet;

import mxp.message.InjectRequest;
import mxp.message.JoinRequest;
import mxp.message.Message;
import mxp.message.MessageTest;
import mxp.message.ObjectFragment;

import java.io.IOException;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Vector;

import junit.framework.TestCase;

/**
 * Test the Packetizer class.
 */
public class PacketizerTest extends TestCase {
    /**
     * Check serializing and de-serializing a message that fits into a single
     * message frame.
     *
     * @throws IOException on I/O issues
     */
    public void testSingleFrame() throws IOException {
        // build up a join request message
        JoinRequest                jr = MessageTest.generateJoinRequest();

        // now convert the message into a single frame segment
        Vector<MessageFrame>    frames = new Vector<MessageFrame>();
        int                     counter;

        counter = Packetizer.messageToMessageFrames(jr, 1, frames);

        assertEquals(frames.size(), 1);
        assertEquals(frames.firstElement().size(), counter);
        assertEquals(frames.firstElement().getFrameCount(), 1);
        assertEquals(frames.firstElement().getFrameIndex(), 0);
        assertEquals(frames.firstElement().getMessageId(), 1);
        assertEquals(frames.firstElement().getType(), jr.getType().getType());
        assertEquals(jr.size() + 10, counter);

        // so far so good, now de-serialize from the frame
        Message message = Packetizer.messageFramesToMessage(frames);

        assertEquals(message.getType(), Message.Type.JOIN_REQUEST);
        assertTrue(message instanceof JoinRequest);

        JoinRequest     jjr = (JoinRequest) message;

        assertEquals(jr, jjr);
    }

    /**
     * Check serializing and de-serializing a message that fits into multiple
     * message frames.
     *
     * @throws IOException on I/O errors
     */
    public void testMultipleFrames() throws IOException {
        InjectRequest               ir = MessageTest.generateInjectRequest();
        ObjectFragment              of = ir.getObject();

        byte[] data = new byte[1024];
        for (int i = 0; i < data.length; ++i) {
            data[i] = (byte) (i % 256);
        }
        of.setExtensionData(data);
        ir.setObject(of);

        // now convert the message into a single frame segment
        Vector<MessageFrame>    frames = new Vector<MessageFrame>();
        int                     counter;

        counter = Packetizer.messageToMessageFrames(ir, 1, frames);

        assertEquals(frames.size(), 5);
        int totalSize = 0;
        for (MessageFrame mf : frames) {
            totalSize += mf.size();
        }
        assertEquals(totalSize, counter);
        assertEquals(ir.size() + 5 * 10, counter);

        // so far so good, now de-serialize from the frame
        Message message = Packetizer.messageFramesToMessage(frames);

        assertEquals(message.getType(), Message.Type.INJECT_REQUEST);
        assertTrue(message instanceof InjectRequest);

        InjectRequest     iir = (InjectRequest) message;

        assertEquals(ir, iir);
    }

    /**
     * Test a round-trip of turning a message into packets and then
     * turning it from packets into a message.
     *
     * @throws IOException on I/O errors
     */
    public void testMessagePacketRoundTrip() throws IOException {
        // first turn a single message into packets
        Message         message  = MessageTest.generateJoinRequest();
        Vector<Message> messages = new Vector<Message>();
        messages.add(message);
        Vector<Packet>  packets  = new Vector<Packet>();

        Packetizer.messagesToPackets(messages,
                                     0,
                                     packets,
                                     0,
                                     0,
                                     new Date(),
                                     (byte) 0);

        assertEquals(packets.size(), 1);
        assertEquals(packets.firstElement().getPacketId(), 0);
        assertEquals(packets.firstElement().getSessionId(), 0);
        assertEquals(packets.firstElement().getGuaranteed(), (byte) 0);
        assertEquals(packets.firstElement().getResendCount(), 0);
        assertEquals(packets.firstElement().getMessageFrames().size(), 1);
        assertTrue(packets.firstElement().size() <= Packet.MAX_SIZE);

        // so far so good, let's turn it back into a message
        Map<Integer, List<MessageFrame>>    messageFrames =
                                    new HashMap<Integer, List<MessageFrame>>();
        List<Integer> messageIds = new Vector<Integer>();
        messages.clear();

        Packetizer.packetsToMessages(packets,
                                     messages,
                                     messageIds,
                                     messageFrames);

        assertEquals(messageFrames.size(), 0);
        assertEquals(messages.size(), 1);
        assertEquals(messageIds.size(), 1);
        assertTrue(messageIds.get(0) == 0);

        Message     mmessage = messages.firstElement();

        assertEquals(message, mmessage);
    }

    /**
     * Test a round-trip of turning a message into packets and then
     * turning it from packets into a message, where the message only fits
     * into a number of frames, but still within one packet.
     *
     * @throws IOException on I/O errors
     */
    public void testMessagePacketRoundTripMultipleFrame() throws IOException {
        // create a big message that doesn't fit into a fragment
        InjectRequest               ir = MessageTest.generateInjectRequest();
        ObjectFragment              of = ir.getObject();

        byte[] data = new byte[1024];
        for (int i = 0; i < data.length; ++i) {
            data[i] = (byte) (i % 256);
        }
        of.setExtensionData(data);
        ir.setObject(of);

        // first turn a single message into packets
        Vector<Message> messages = new Vector<Message>();
        messages.add(ir);
        Vector<Packet>  packets  = new Vector<Packet>();

        Packetizer.messagesToPackets(messages,
                                     0,
                                     packets,
                                     0,
                                     0,
                                     new Date(),
                                     (byte) 0);

        assertEquals(packets.size(), 1);
        assertEquals(packets.firstElement().getPacketId(), 0);
        assertEquals(packets.firstElement().getSessionId(), 0);
        assertEquals(packets.firstElement().getGuaranteed(), (byte) 0);
        assertEquals(packets.firstElement().getResendCount(), 0);
        assertEquals(packets.firstElement().getMessageFrames().size(), 5);
        assertTrue(packets.firstElement().size() <= Packet.MAX_SIZE);

        // so far so good, let's turn it back into a message
        Map<Integer, List<MessageFrame>>    messageFrames =
                                    new HashMap<Integer, List<MessageFrame>>();
        List<Integer> messageIds = new Vector<Integer>();
        messages.clear();

        Packetizer.packetsToMessages(packets,
                                     messages,
                                     messageIds,
                                     messageFrames);

        assertEquals(messageFrames.size(), 0);
        assertEquals(messages.size(), 1);
        assertEquals(messageIds.size(), 1);
        assertTrue(messageIds.get(0) == 0);

        Message     mmessage = messages.firstElement();

        assertEquals(ir, mmessage);
    }

    /**
     * Test a round-trip of turning messages into packets and then
     * turning it from packets into a messages, when the messages only
     * fit into multiple packets.
     *
     * @throws IOException on I/O errors
     */
    public void testMessagePacketRoundTripMultiplePackets() throws IOException {
        // create a few big messages that won't fit into Packet.MAX_PAYLOAD
        Vector<Message> messages     = new Vector<Message>();
        int             totalPayload = 0;
        for (int i = 0; i < 3; ++i) {
            InjectRequest   ir = MessageTest.generateInjectRequest();
            ObjectFragment  of = ir.getObject();

            byte[] data = new byte[768];
            for (int j = 0; j < data.length; ++j) {
                data[j] = (byte) (j % 256);
            }
            of.setExtensionData(data);
            ir.setObject(of);

            messages.add(ir);
            totalPayload += ir.size();
        }
        assertTrue(totalPayload > Packet.MAX_PAYLOAD);

        // first turn the messages into packets
        Vector<Packet>  packets  = new Vector<Packet>();

        Packetizer.messagesToPackets(messages,
                                     0,
                                     packets,
                                     0,
                                     0,
                                     new Date(),
                                     (byte) 0);

        assertEquals(packets.size(), 3);

        assertEquals(packets.firstElement().getPacketId(), 0);
        assertEquals(packets.firstElement().getSessionId(), 0);
        assertEquals(packets.firstElement().getGuaranteed(), (byte) 0);
        assertEquals(packets.firstElement().getResendCount(), 0);
        assertEquals(packets.firstElement().getMessageFrames().size(), 5);
        assertTrue(packets.firstElement().size() <= Packet.MAX_SIZE);

        assertEquals(packets.get(1).getPacketId(), 1);
        assertEquals(packets.get(1).getSessionId(), 0);
        assertEquals(packets.get(1).getGuaranteed(), (byte) 0);
        assertEquals(packets.get(1).getResendCount(), 0);
        assertEquals(packets.get(1).getMessageFrames().size(), 5);
        assertTrue(packets.get(1).size() <= Packet.MAX_SIZE);

        assertEquals(packets.get(2).getPacketId(), 2);
        assertEquals(packets.get(2).getSessionId(), 0);
        assertEquals(packets.get(2).getGuaranteed(), (byte) 0);
        assertEquals(packets.get(2).getResendCount(), 0);
        assertEquals(packets.get(2).getMessageFrames().size(), 2);
        assertTrue(packets.get(2).size() <= Packet.MAX_SIZE);

        // so far so good, let's turn it back into a message
        Map<Integer, List<MessageFrame>>    messageFrames =
                                    new HashMap<Integer, List<MessageFrame>>();
        List<Integer> messageIds = new Vector<Integer>();

        Vector<Message> mmessages = new Vector<Message>();

        Packetizer.packetsToMessages(packets,
                                     mmessages,
                                     messageIds,
                                     messageFrames);

        assertEquals(messageFrames.size(), 0);
        assertEquals(mmessages.size(), 3);
        assertEquals(messageIds.size(), 3);
        assertTrue(messageIds.get(0) == 0);
        assertTrue(messageIds.get(1) == 1);
        assertTrue(messageIds.get(2) == 2);

        assertEquals(messages, mmessages);
    }

    /**
     * Test de-serializing packets when not all frames are always available
     * for some of the messages. This test also tests for existing packets
     * available in the packets list submitted to the Packetizer.
     *
     * @throws IOException on I/O errors
     */
    public void testMissingFramePackets() throws IOException {
        // create a few big messages that won't fit into Packet.MAX_PAYLOAD
        Vector<Message> messages     = new Vector<Message>();
        int             totalPayload = 0;
        for (int i = 0; i < 3; ++i) {
            InjectRequest   ir = MessageTest.generateInjectRequest();
            ObjectFragment  of = ir.getObject();

            byte[] data = new byte[768];
            for (int j = 0; j < data.length; ++j) {
                data[j] = (byte) (j % 256);
            }
            of.setExtensionData(data);
            ir.setObject(of);

            messages.add(ir);
            totalPayload += ir.size();
        }
        assertTrue(totalPayload > Packet.MAX_PAYLOAD);

        // first turn a single message into packets
        Vector<Packet>  packets  = new Vector<Packet>();

        // first turn message 1 into packets
        Packetizer.messagesToPackets(messages.subList(0, 1),
                                     0,
                                     packets,
                                     0,
                                     0,
                                     new Date(),
                                     (byte) 0);

        assertEquals(packets.size(), 1);

        // now add the second message
        Packetizer.messagesToPackets(messages.subList(1, 2),
                                     1,
                                     packets,
                                     0,
                                     1,
                                     new Date(),
                                     (byte) 0);

        assertEquals(packets.size(), 2);

        // now add the third and final message
        Packetizer.messagesToPackets(messages.subList(2, 3),
                                     2,
                                     packets,
                                     0,
                                     2,
                                     new Date(),
                                     (byte) 0);

        assertEquals(packets.size(), 3);

        // now let's turn these packets one-by-one into messages,
        // which means there's going to be partial messages remaining

        // so far so good, let's turn it back into a message
        Map<Integer, List<MessageFrame>>    messageFrames =
                                    new HashMap<Integer, List<MessageFrame>>();
        List<Integer>   messageIds = new Vector<Integer>();
        Vector<Packet>  p = new Vector<Packet>();
        Vector<Message> mmessages = new Vector<Message>();

        // let's process the first packet
        p.add(packets.get(0));
        Packetizer.packetsToMessages(p, mmessages, messageIds, messageFrames);

        // the expectation is that we have the first message, and
        // parts of the second message
        assertEquals(messageFrames.size(), 1);
        assertEquals(mmessages.size(), 1);
        assertEquals(messages.get(0), mmessages.get(0));
        assertEquals(messageIds.size(), 1);
        assertTrue(messageIds.get(0) == 0);

        // let's process the second packet
        p.clear();
        p.add(packets.get(1));
        Packetizer.packetsToMessages(p, mmessages, messageIds, messageFrames);

        // the expectation is that we have the second message, and
        // parts of the third message
        assertEquals(messageFrames.size(), 1);
        assertEquals(mmessages.size(), 2);
        assertEquals(messages.get(1), mmessages.get(1));
        assertEquals(messageIds.size(), 2);
        assertTrue(messageIds.get(1) == 1);

        // let's process the third packet
        p.clear();
        p.add(packets.get(2));
        Packetizer.packetsToMessages(p, mmessages, messageIds, messageFrames);

        // the expectation is that we have the third message and
        // no unfinished frames
        assertEquals(messageFrames.size(), 0);
        assertEquals(mmessages.size(), 3);
        assertEquals(messages.get(2), mmessages.get(2));
        assertEquals(messageIds.size(), 3);
        assertTrue(messageIds.get(2) == 2);
    }

}
