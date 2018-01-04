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

import java.util.ArrayList;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.UUID;
import java.io.InputStream;
import java.io.FilterInputStream;
import java.io.IOException;
import java.nio.charset.Charset;

/**
 * A filter input stream that allows the de-serialization of primitive
 * data types from an underlying input stream - most probably a
 * ByteArrayInputStream.
 *
 * See the MXP Draft Encoding section for serialization details.
 */
public class SerializationInputStream extends FilterInputStream {

    /**
     * The number of milliseconds at 1st January 2000 elapsed since
     * the usual UNIX epoch of 1st January 1970.
     */
     private static final long EPOCH2K =
                new GregorianCalendar(2000, 0, 1, 0, 0, 0).getTime().getTime();

     /**
      * An internal counter for how many bytes were read from the underlying
      * input stream.
      */
     private int bytesRead;

    /**
     * Constructor.
     *
     * @param in the underlying input stream to get raw data from
     */
    public SerializationInputStream(InputStream in) {
        super(in);

        bytesRead = 0;
    }

    /**
     * Return the counter for how many bytes were read from the input
     * stream. This can be used to gauge how many bytes were used by a
     * series of read operations.
     *
     * @return the number of bytes read from the underlying input stream.
     */
    public int counter() {
        return bytesRead;
    }

    /**
     * Read raw bytes from the underlying input stream.
     *
     * @param b read the bytes into this array.
     * @return the number of bytes read
     * @throws IOException on I/O issues
     */
    private int readInBytes(byte[] b) throws IOException {
        int read = in.read(b);
        bytesRead += read;

        return read;
    }

    /**
     * Read raw bytes from the underlying input stream.
     *
     * @param b read the bytes into this array.
     * @param off the offset in b to start to put bytes in
     * @param len the maximum number of bytes to read
     * @return the number of bytes read
     * @throws IOException on I/O issues
     */
    private int readInBytes(byte[] b, int off, int len) throws IOException {
        int read = in.read(b, off, len);
        bytesRead += read;

        return read;
    }

    /**
     * Read a single byte from the underlying input stream.
     *
     * @return the byte read
     * @throws IOException on I/O issues
     */
    private int readInByte() throws IOException {
        int value = in.read();
        ++bytesRead;

        return value;
    }

    /**
     *  De-serialize a float from IEEE 754-1985 notation, and in little endian
     *  byte order.
     *
     *  @return the de-serialized float
     *  @throws IOException on I/O issues
     */
    public float readFloat() throws IOException {
        int floatBits = readInt();

        return Float.intBitsToFloat(floatBits);
    }

    /**
     *  De-serialize a double from IEEE 754-1985 notation, and in little endian
     *  byte order.
     *
     *  @return the de-serialized double
     *  @throws IOException on I/O issues
     */
    public double readDouble() throws IOException {
        long doubleBits = readLong();

        return Double.longBitsToDouble(doubleBits);
    }

    /**
     *  De-serialize a long (64 bit signed integer), in little endian
     *  byte order.
     *
     *  @return the de-serialized long
     *  @throws IOException on I/O issues
     */
    public long readLong() throws IOException {
        long value = 0;

        value |= ((long) readInByte());
        value |= ((long) readInByte()) << 8;
        value |= ((long) readInByte()) << 16;
        value |= ((long) readInByte()) << 24;
        value |= ((long) readInByte()) << 32;
        value |= ((long) readInByte()) << 40;
        value |= ((long) readInByte()) << 48;
        value |= ((long) readInByte()) << 56;

        return value;
    }

    /**
     *  De-serialize an int (32 bit signed integer), in little endian
     *  byte order.
     *
     *  @return the de-serialized int
     *  @throws IOException on I/O issues
     */
    public int readInt() throws IOException {
        int value = 0;

        value |= readInByte();
        value |= readInByte() << 8;
        value |= readInByte() << 16;
        value |= readInByte() << 24;

        return value;
    }

    /**
     *  De-serialize a short (16 bit signed integer), in little endian
     *  byte order.
     *
     *  @return the de-serialized short
     *  @throws IOException on I/O issues
     */
    public short readShort() throws IOException {
        short value = 0;

        value |= readInByte();
        value |= readInByte() << 8;

        return value;
    }

    /**
     *  De-serialize a byte (8 bit integer).
     *
     *  @return the de-serialized byte
     *  @throws IOException on I/O issues
     */
    public byte readByte() throws IOException {
        return (byte) readInByte();
    }

    /**
     *  De-serialize a zero-terminated UTF-8 string.
     *
     *  @return the de-serialized string
     *  @throws IOException on I/O issues
     */
    public String readString() throws IOException {
        ArrayList<Byte>     buffer = new ArrayList<Byte>(10);
        byte                b;

        while ((b = readByte()) != 0x00) {
            buffer.add(b);
        }

        byte[] bytes = new byte[buffer.size()];
        for (int i = 0; i < bytes.length; ++i) {
            bytes[i] = buffer.get(i);
        }

        return new String(bytes, Charset.forName("UTF-8"));
    }

    /**
     *  De-serialize a zero-terminated UTF-8 string from a fixed sized space.
     *  The string is interpreted as the bytes before the first zero-character
     *  in the input stream. All of the bytes in the space are read up.
     *
     *  @param length the number of bytes to consume.
     *  @return the de-serialized string
     *  @throws IOException on I/O issues
     */
    public String readString(int length) throws IOException {
        ArrayList<Byte>     buffer = new ArrayList<Byte>(length);
        int                 counter = 0;
        byte                b;

        while (counter < length) {
            b = readByte();
            ++counter;

            if (b != 0x00) {
                buffer.add(b);
            } else {
                break;
            }
        }
        while (counter < length) {
            readByte();
            ++counter;
        }

        byte[] bytes = new byte[buffer.size()];
        for (int i = 0; i < bytes.length; ++i) {
            bytes[i] = buffer.get(i);
        }

        return new String(bytes, Charset.forName("UTF-8"));
    }

    /**
     *  De-serialize a UUID as a 16 byte big endian value.
     *
     *  @return the UUID de-serialized.
     *  @throws IOException on I/O issues
     */
    public UUID readUUID() throws IOException {
        // read the higher 64 bits forts, in big endian order
        long highValue = 0;
        highValue |= ((long) readInByte()) << 56;
        highValue |= ((long) readInByte()) << 48;
        highValue |= ((long) readInByte()) << 40;
        highValue |= ((long) readInByte()) << 32;
        highValue |= ((long) readInByte()) << 24;
        highValue |= ((long) readInByte()) << 16;
        highValue |= ((long) readInByte()) << 8;
        highValue |= ((long) readInByte());

        // read the lower 64 bits forts, in big endian order
        long lowValue = 0;
        lowValue |= ((long) readInByte()) << 56;
        lowValue |= ((long) readInByte()) << 48;
        lowValue |= ((long) readInByte()) << 40;
        lowValue |= ((long) readInByte()) << 32;
        lowValue |= ((long) readInByte()) << 24;
        lowValue |= ((long) readInByte()) << 16;
        lowValue |= ((long) readInByte()) << 8;
        lowValue |= ((long) readInByte());

        UUID uuid = new UUID(highValue, lowValue);

        return uuid;
    }

    /**
     *  De-serialize a Date from a long value, the number of milliseconds since
     *  1st January 2000, 00:00:00 UTC.
     *
     *  @return the de-serialized date value
     *  @throws IOException on I/O issues
     */
    public Date readDate() throws IOException {
        long    date = readLong();

        return new Date(EPOCH2K + date);
    }

    /**
     *  De-serialize a complete byte array by simply reading it from downstream.
     *
     *  @param bytes the array to read into
     *  @return the number of bytes read and put into bytes
     *  @throws IOException on I/O issues
     */
    public int readBytes(byte[] bytes) throws IOException {
        return readInBytes(bytes);
    }

    /**
     *  De-serialize a partial byte array by simply reading it from downstream.
     *
     *  @param bytes the array to read into
     *  @param offset the offet at which to start to insert bytes
     *  @param length the maximum number of bytes to read
     *  @return the number of bytes read and put into bytes
     *  @throws IOException on I/O issues
     */
    public int readBytes(byte[] bytes, int offset, int length)
                                                        throws IOException {
        return readInBytes(bytes, offset, length);
    }

}

