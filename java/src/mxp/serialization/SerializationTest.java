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
package mxp.serialization;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.util.Date;
import java.util.UUID;

import junit.framework.TestCase;

/**
 * Unit test to check for two-way serialization.
 */
public class SerializationTest extends TestCase {

    /**
     * Test for float serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testFloat() throws IOException {
        float                       expected = 123.32132f;
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);

        pOut.put(expected);
        pOut.flush();

        ByteArrayInputStream   in = new ByteArrayInputStream(out.toByteArray());
        SerializationInputStream      pIn = new SerializationInputStream(in);

        float value = pIn.readFloat();

        assertEquals(expected, value);
    }

    /**
     * Test for double serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testDouble() throws IOException {
        double                      expected = 12332312.3213312312;
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);

        pOut.put(expected);
        pOut.flush();

        ByteArrayInputStream   in = new ByteArrayInputStream(out.toByteArray());
        SerializationInputStream      pIn = new SerializationInputStream(in);

        double value = pIn.readDouble();

        assertEquals(expected, value);
    }

    /**
     * Test for long serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testLong() throws IOException {
        long                        expected = 123323123213312312L;
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);

        pOut.put(expected);
        pOut.flush();

        ByteArrayInputStream   in = new ByteArrayInputStream(out.toByteArray());
        SerializationInputStream      pIn = new SerializationInputStream(in);

        long value = pIn.readLong();

        assertEquals(expected, value);
    }

    /**
     * Test for int serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testInt() throws IOException {
        int                         expected = 1233231232;
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);

        pOut.put(expected);
        pOut.flush();

        ByteArrayInputStream   in = new ByteArrayInputStream(out.toByteArray());
        SerializationInputStream      pIn = new SerializationInputStream(in);

        int value = pIn.readInt();

        assertEquals(expected, value);
    }

    /**
     * Test for short serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testShort() throws IOException {
        short                       expected = 12332;
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);

        pOut.put(expected);
        pOut.flush();

        ByteArrayInputStream   in = new ByteArrayInputStream(out.toByteArray());
        SerializationInputStream      pIn = new SerializationInputStream(in);

        short value = pIn.readShort();

        assertEquals(expected, value);
    }

    /**
     * Test for byte serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testByte() throws IOException {
        byte                        expected = 123;
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);

        pOut.put(expected);
        pOut.flush();

        ByteArrayInputStream   in = new ByteArrayInputStream(out.toByteArray());
        SerializationInputStream      pIn = new SerializationInputStream(in);

        byte value = pIn.readByte();

        assertEquals(expected, value);
    }

    /**
     * Test for string serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testString() throws IOException {
        String                      expected = "Helló!";
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);

        pOut.put(expected);
        pOut.flush();

        ByteArrayInputStream   in = new ByteArrayInputStream(out.toByteArray());
        SerializationInputStream      pIn = new SerializationInputStream(in);

        String value = pIn.readString();

        assertEquals(expected, value);
    }

    /**
     * Test for Date serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testDate() throws IOException {
        Date                        expected = new Date();
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);

        pOut.put(expected);
        pOut.flush();

        ByteArrayInputStream   in = new ByteArrayInputStream(out.toByteArray());
        SerializationInputStream      pIn = new SerializationInputStream(in);

        Date value = pIn.readDate();

        assertEquals(expected, value);
    }

    /**
     * Test for UUID serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testUUID() throws IOException {
        UUID                        expected = UUID.randomUUID();
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);

        pOut.put(expected);
        pOut.flush();

        ByteArrayInputStream   in = new ByteArrayInputStream(out.toByteArray());
        SerializationInputStream      pIn = new SerializationInputStream(in);

        UUID value = pIn.readUUID();

        assertEquals(expected, value);
    }

}
