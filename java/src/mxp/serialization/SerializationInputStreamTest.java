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
import java.io.IOException;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.UUID;

import junit.framework.TestCase;

/**
 * Unit test for the SerializationOutputStream class.
 */
public class SerializationInputStreamTest extends TestCase {

    /**
     * Constructor.
     *
     * @param name the name of the test case
     */
    public SerializationInputStreamTest(String name) {
        super(name);
    }

    /**
     * Test float serialization.
     *
     * @throws IOException on I/O errors.
     */
    public void testFloat() throws IOException {
        byte[]                 bytes = { 0x39, (byte) 0x8e,
                                        (byte) 0xe3, 0x3d };
        ByteArrayInputStream   in = new ByteArrayInputStream(bytes);
        SerializationInputStream      pIn = new SerializationInputStream(in);
        float                  expected = 0.1111111111111111f;
        int                    counter = pIn.counter();

        float value = pIn.readFloat();

        assertEquals(4, pIn.counter() - counter);
        assertEquals(value, expected);
    }

    /**
     * Test double serialization.
     *
     * @throws IOException on I/O errors.
     */
    public void testDouble() throws IOException {
        byte[]                 bytes = { 0x1c, (byte) 0xc7,
                                         0x71, 0x1c,
                                         (byte) 0xc7, 0x71,
                                         (byte) 0xbc, 0x3f };
        ByteArrayInputStream   in = new ByteArrayInputStream(bytes);
        SerializationInputStream      pIn = new SerializationInputStream(in);
        double                 expected = 0.1111111111111111111;
        int                    counter = pIn.counter();

        double value = pIn.readDouble();

        assertEquals(8, pIn.counter() - counter);
        assertEquals(value, expected);
    }

    /**
     * Test long serialization.
     *
     * @throws IOException on I/O errors.
     */
    public void testLong() throws IOException {
        byte[]                 bytes = { (byte) 0xc7, 0x71,
                                         (byte) 0xc4, 0x2b,
                                         (byte) 0xab, 0x75,
                                         0x6b, 0x0f };
        ByteArrayInputStream   in = new ByteArrayInputStream(bytes);
        SerializationInputStream      pIn = new SerializationInputStream(in);
        long                   expected = 1111111111111111111L;
        int                    counter = pIn.counter();

        long value = pIn.readLong();

        assertEquals(8, pIn.counter() - counter);
        assertEquals(value, expected);
    }

    /**
     * Test int serialization.
     *
     * @throws IOException on I/O errors.
     */
    public void testInt() throws IOException {
        byte[]                 bytes = { (byte) 0xc7, 0x35,
                                          0x3a, 0x42 };
        ByteArrayInputStream   in = new ByteArrayInputStream(bytes);
        SerializationInputStream      pIn = new SerializationInputStream(in);
        int                    expected = 1111111111;
        int                    counter = pIn.counter();

        int value = pIn.readInt();

        assertEquals(4, pIn.counter() - counter);
        assertEquals(value, expected);
    }

    /**
     * Test short serialization.
     *
     * @throws IOException on I/O errors.
     */
    public void testShort() throws IOException {
        byte[]                 bytes = { 0x67, 0x2b };
        ByteArrayInputStream   in = new ByteArrayInputStream(bytes);
        SerializationInputStream      pIn = new SerializationInputStream(in);
        short                  expected = 11111;
        int                    counter = pIn.counter();

        short value = pIn.readShort();

        assertEquals(2, pIn.counter() - counter);
        assertEquals(value, expected);
    }

    /**
     * Test byte serialization.
     *
     * @throws IOException on I/O errors.
     */
    public void testByte() throws IOException {
        byte[]                 bytes = { 0x0b };
        ByteArrayInputStream   in = new ByteArrayInputStream(bytes);
        SerializationInputStream      pIn = new SerializationInputStream(in);
        byte                   expected = 11;
        int                    counter = pIn.counter();

        byte value = pIn.readByte();

        assertEquals(1, pIn.counter() - counter);
        assertEquals(value, expected);
    }

    /**
     * Test string serialization.
     *
     * @throws IOException on I/O errors.
     */
    public void testString() throws IOException {
        byte[]                 bytes = { 'H', 'e', 'l', 'l',
                                         (byte) 0xc3, (byte) 0xb3,
                                         '!', 0x00 };
        ByteArrayInputStream     in = new ByteArrayInputStream(bytes);
        SerializationInputStream pIn = new SerializationInputStream(in);
        String                   expected = "Helló!";
        int                      counter = pIn.counter();

        String value = pIn.readString();

        assertEquals(bytes.length, pIn.counter() - counter);
        assertEquals(value, expected);
    }

    /**
     * Test string serialization from a fixed space.
     *
     * @throws IOException on I/O errors.
     */
    public void testStringFixed() throws IOException {
        byte[]                 bytes = { 'H', 'e', 'l', 'l',
                                         (byte) 0xc3, (byte) 0xb3,
                                         '!', 0x00, 0x00, 0x00 };
        ByteArrayInputStream     in = new ByteArrayInputStream(bytes);
        SerializationInputStream pIn = new SerializationInputStream(in);
        String                   expected = "Helló!";
        int                      counter = pIn.counter();

        String value = pIn.readString(bytes.length);

        assertEquals(bytes.length, pIn.counter() - counter);
        assertEquals(value, expected);
    }

    /**
     * Test UUID serialization.
     *
     * @throws IOException on I/O errors.
     */
    public void testUUID() throws IOException {
        byte[]                 bytes = { (byte) 0xd4, (byte) 0x36,
                                         (byte) 0xef, (byte) 0xa8,
                                         (byte) 0x91, (byte) 0xbe,
                                         (byte) 0x48, (byte) 0xf8,
                                         (byte) 0xaa, (byte) 0xd4,
                                         (byte) 0x9b, (byte) 0x85,
                                         (byte) 0xbd, (byte) 0xe1,
                                         (byte) 0xc6, (byte) 0x03 };
        ByteArrayInputStream   in = new ByteArrayInputStream(bytes);
        SerializationInputStream      pIn = new SerializationInputStream(in);
        UUID                   expected =
                        UUID.fromString("d436efa8-91be-48f8-aad4-9b85bde1c603");
        int                    counter = pIn.counter();

        UUID value = pIn.readUUID();

        assertEquals(16, pIn.counter() - counter);
        assertEquals(value, expected);
    }

    /**
     * Test Date serialization.
     *
     * @throws IOException on I/O errors.
     */
    public void testDate() throws IOException {
        byte[]                 bytes = { 0x00, (byte) 0xa0,
                                         (byte) 0xad, 0x24,
                                         0x42, 0x00,
                                         0x00, 0x00 };
        ByteArrayInputStream   in = new ByteArrayInputStream(bytes);
        SerializationInputStream      pIn = new SerializationInputStream(in);
        Date                   expected =
                        new GregorianCalendar(2009, 0, 1, 0, 0, 0).getTime();
        int                    counter = pIn.counter();

        Date value = pIn.readDate();

        assertEquals(8, pIn.counter() - counter);
        assertEquals(value, expected);
    }

    /**
     * Test byte array serialization.
     *
     * @throws IOException on I/O errors.
     */
    public void testByteArray() throws IOException {
        byte[]                 bytes = { 0x01, 0x02 };
        ByteArrayInputStream   in = new ByteArrayInputStream(bytes);
        SerializationInputStream      pIn = new SerializationInputStream(in);
        byte[]                 value = new byte[bytes.length];
        int                    counter = pIn.counter();

        int size = pIn.readBytes(value);

        assertEquals(2, pIn.counter() - counter);
        assertEquals(size, bytes.length);
        assertEquals(bytes[0], value[0]);
        assertEquals(bytes[1], value[1]);
    }

    /**
     * Test byte array serialization.
     *
     * @throws IOException on I/O errors.
     */
    public void testPartialByteArray() throws IOException {
        byte[]                    bytes = { 0x02, 0x03 };
        ByteArrayInputStream      in = new ByteArrayInputStream(bytes);
        SerializationInputStream  pIn = new SerializationInputStream(in);
        byte[]                    value = new byte[4];
        int                       counter = pIn.counter();

        int size = pIn.readBytes(value, 1, 2);

        assertEquals(2, pIn.counter() - counter);
        assertEquals(size, 2);
        assertEquals(bytes[0], value[1]);
        assertEquals(bytes[1], value[2]);
    }
};

