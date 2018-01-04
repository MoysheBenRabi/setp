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

import mxp.serialization.SerializationInputStream;
import mxp.serialization.SerializationOutputStream;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.util.Date;
import java.util.Vector;

import junit.framework.TestCase;

/**
 * Test cases for the Packet object.
 */
public class PacketTest extends TestCase {
    /**
     * Create a test packet.
     *
     * @return a test packet.
     */
    public static Packet generatePacket() {
        Vector<MessageFrame>    frames = new Vector<MessageFrame>();

        for (int i = 0; i < 5; ++i) {
            MessageFrame              mf = new MessageFrame();

            mf.setType((byte) i);
            mf.setMessageId(123 + i);
            mf.setFrameCount((short) 1);
            mf.setFrameIndex((short) 0);
            byte[] data = new byte[250];
            for (int j = 0; j < data.length; ++j) {
                data[j] = (byte) j;
            }
            mf.setFrameData(data);

            frames.add(mf);
        }

        Packet  packet = new Packet();

        packet.setSessionId(1234);
        packet.setPacketId(4567);
        packet.setFirstSendTime(new Date());
        packet.setGuaranteed((byte) 0);
        packet.setResendCount((byte) 0);
        packet.setMessageFrames(frames);

        return packet;
    }

    /**
     * Test for trying to add too many message frames to a packet, that
     * would exceed the overall packet size.
     */
    public void testTooMuchData() {
        Vector<MessageFrame>    frames = new Vector<MessageFrame>();

        // add 5 message frames, a total of 5 x 256 = 1280 bytes
        for (int i = 0; i < 5; ++i) {
            MessageFrame mf              = new MessageFrame();
            byte[]       data            = new byte[255];

            mf.setFrameData(data);
            frames.add(mf);
        }

        Packet  packet = new Packet();

        // this should go through fine
        packet.setMessageFrames(frames);

        // now add an extra message frame, bringing the total size to 1545
        // bytes, which is over 1438 bytes that fits into a packet
        boolean exceptionThrown = false;
        try {
            MessageFrame mf              = new MessageFrame();
            byte[]       data            = new byte[255];

            mf.setFrameData(data);
            frames.add(mf);
            // this should fail
            packet.setMessageFrames(frames);
        } catch (IllegalArgumentException e) {
            exceptionThrown = true;
        }
        assertTrue(exceptionThrown);
    }

    /**
     * Test serialization of a packet.
     *
     * @throws IOException on I/O errors.
     */
    public void testSerialization() throws IOException {
        Packet  packet = generatePacket();

        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = packet.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        Packet                      ppacket = new Packet();

        ppacket.deserialize(in, counter);

        assertEquals(packet, ppacket);
    }
}
