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

import java.util.Arrays;
import java.util.GregorianCalendar;
import java.util.UUID;
import java.io.ByteArrayOutputStream;
import java.io.IOException;

import junit.framework.TestCase;

/**
 * Unit test for the SerializationOutputStream class.
 */
public class SerializationOutputStreamTest extends TestCase {

    /**
     * Constuctor.
     *
     * @param name the name of the test case.
     */
    public SerializationOutputStreamTest(String name) {
        super(name);
    }

    /**
     * Test for float serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testFloat() throws IOException {
        ByteArrayOutputStream      out = new ByteArrayOutputStream();
        SerializationOutputStream  pOut = new SerializationOutputStream(out);
        byte[]                     expected = { 0x39, (byte) 0x8e,
                                                (byte) 0xe3, 0x3d };

        int ret = pOut.put(0.1111111111111111f);
        pOut.flush();

        assertEquals(4, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }

    /**
     * Test for double serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testDouble() throws IOException {
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);
        byte[]                      expected = { 0x1c, (byte) 0xc7,
                                                 0x71, 0x1c,
                                                 (byte) 0xc7, 0x71,
                                                 (byte) 0xbc, 0x3f };

        int ret = pOut.put((double) 0.1111111111111111111);
        pOut.flush();

        assertEquals(8, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }

    /**
     * Test for long serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testLong() throws IOException {
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);
        byte[]                      expected = { (byte) 0xc7, 0x71,
                                                (byte) 0xc4, 0x2b,
                                                (byte) 0xab, 0x75,
                                                0x6b, 0x0f };

        int ret = pOut.put((long) 1111111111111111111L);
        pOut.flush();

        assertEquals(8, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }

    /**
     * Test for int serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testInt() throws IOException {
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);
        byte[]                      expected = { (byte) 0xc7, 0x35,
                                                  0x3a, 0x42 };

        int ret = pOut.put((int) 1111111111);
        pOut.flush();

        assertEquals(4, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }

    /**
     * Test for short serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testShort() throws IOException {
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);
        byte[]                      expected = { 0x67, 0x2b };

        int ret = pOut.put((short) 11111);
        pOut.flush();

        assertEquals(2, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }

    /**
     * Test for byte serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testByte() throws IOException {
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);
        byte[]                      expected = { 0x0b };

        int ret = pOut.put((byte) 11);
        pOut.flush();

        assertEquals(1, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }

    /**
     * Test for string serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testString() throws IOException {
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);
        byte[]                      expected = { 'H', 'e', 'l', 'l',
                                                 (byte) 0xc3, (byte) 0xb3,
                                                 '!', 0x00 };

        int ret = pOut.put("Helló!");
        pOut.flush();

        assertEquals(expected.length, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }

    /**
     * Test for string serialization into a fixed length space.
     *
     * @throws IOException on I/O issues.
     */
    public void testStringFixed() throws IOException {
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);
        byte[]                      expected = { 'H', 'e', 'l', 'l',
                                                 (byte) 0xc3, (byte) 0xb3,
                                                 '!', 0x00, 0x00, 0x00 };

        int ret = pOut.put("Helló!", expected.length);
        pOut.flush();

        assertEquals(expected.length, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }

    /**
     * Test for string serialization into a fixed length space, when the
     * string has to be truncated.
     *
     * @throws IOException on I/O issues.
     */
    public void testStringFixedTruncate() throws IOException {
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);
        byte[]                      expected = { 'H', 'e', 'l', 'l',
                                                 (byte) 0xc3, (byte) 0xb3 };

        int ret = pOut.put("Helló!", expected.length);
        pOut.flush();

        assertEquals(expected.length, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }

    /**
     * Test for string serialization into a fixed length space, when the
     * string has to be truncated, and a partial UTF-8 sequence would be
     * present at the point of truncation.
     *
     * @throws IOException on I/O issues.
     */
    public void testStringFixedTruncatePartial() throws IOException {
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);
        byte[]                      expected = { 'H', 'e', 'l', 'l',
                                                 0x00 };

        int ret = pOut.put("Helló!", expected.length);
        pOut.flush();

        assertEquals(expected.length, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }

    /**
     * Test for UUID serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testUUID() throws IOException {
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);
        byte[]                      expected = { (byte) 0xd4, (byte) 0x36,
                                                 (byte) 0xef, (byte) 0xa8,
                                                 (byte) 0x91, (byte) 0xbe,
                                                 (byte) 0x48, (byte) 0xf8,
                                                 (byte) 0xaa, (byte) 0xd4,
                                                 (byte) 0x9b, (byte) 0x85,
                                                 (byte) 0xbd, (byte) 0xe1,
                                                 (byte) 0xc6, (byte) 0x03 };

        int ret = pOut.put(
                      UUID.fromString("d436efa8-91be-48f8-aad4-9b85bde1c603"));
        pOut.flush();

        assertEquals(16, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }

    /**
     * Test for Date serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testDate() throws IOException {
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);
        byte[]                      expected = { 0x00, (byte) 0xa0,
                                                 (byte) 0xad, 0x24,
                                                 0x42, 0x00,
                                                 0x00, 0x00 };

        int ret = pOut.put(
                        new GregorianCalendar(2009, 0, 1, 0, 0, 0).getTime());
        pOut.flush();

        assertEquals(8, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }

    /**
     * Test for byte array serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testByteArray() throws IOException {
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);
        byte[]                      expected = { 0x01, 0x02 };

        int ret = pOut.put(expected);
        pOut.flush();

        assertEquals(2, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }

    /**
     * Test for byte array serialization.
     *
     * @throws IOException on I/O issues.
     */
    public void testPartialByteArray() throws IOException {
        ByteArrayOutputStream       out = new ByteArrayOutputStream();
        SerializationOutputStream   pOut = new SerializationOutputStream(out);
        byte[]                      bytes = { 0x01, 0x02, 0x03, 0x04 };
        byte[]                      expected = { 0x02, 0x03 };

        int ret = pOut.put(bytes, 1, 2);
        pOut.flush();

        assertEquals(2, ret);
        assertEquals(out.size(), expected.length);
        assertTrue(Arrays.equals(out.toByteArray(), expected));
    }
};
