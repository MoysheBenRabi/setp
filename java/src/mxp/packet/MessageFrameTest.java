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

import junit.framework.TestCase;

/**
 * Test cases for the MessageFrame object.
 */
public class MessageFrameTest extends TestCase {
    /**
     * Test for trying to add too big frame data to a MessageFrame.
     */
    public void testTooBigFrameData() {
        MessageFrame mf              = new MessageFrame();
        byte[]       data            = new byte[255];

        // this should go through OK
        mf.setFrameData(data);

        boolean exceptionThrown = false;
        try {
            // this should fail
            data = new byte[256];
            mf.setFrameData(data);
        } catch (IllegalArgumentException e) {
            exceptionThrown = true;
        }
        assertTrue(exceptionThrown);
    }

    /**
     * Test serialization of a message frame.
     *
     * @throws IOException on I/O errors.
     */
    public void testSerialization() throws IOException {
        MessageFrame              mf = new MessageFrame();

        mf.setType((byte) 0x01);
        mf.setMessageId(123);
        mf.setFrameCount((short) 1);
        mf.setFrameIndex((short) 0);
        byte[] data = new byte[40];
        for (int i = 0; i < data.length; ++i) {
            data[i] = (byte) i;
        }
        mf.setFrameData(data);

        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = mf.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        MessageFrame                mmf = new MessageFrame();

        mmf.deserialize(in, counter);

        assertEquals(mf, mmf);
    }
}
