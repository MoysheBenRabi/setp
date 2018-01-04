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

import java.io.FilterOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.nio.charset.Charset;
import java.util.Arrays;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.UUID;

/**
 * A filter output stream that allows the serialization of primitive
 * data types unto an underlying output stream - most probably a
 * ByteArrayOutputStream.
 *
 * See the MXP Draft Encoding section for serialization details.
 */
public class SerializationOutputStream extends FilterOutputStream {

    /**
     * The number of milliseconds at 1st January 2000 elapsed since
     * the usual UNIX epoch of 1st January 1970.
     */
     private static final long EPOCH2K =
                new GregorianCalendar(2000, 0, 1, 0, 0, 0).getTime().getTime();

    /**
     * Constructor.
     *
     * @param out the underlying output stream to send the raw data to
     */
    public SerializationOutputStream(OutputStream out) {
        super(out);
    }

    /**
     * Write bytes to the underlying output stream.
     *
     * @param b the array of bytes to write.
     * @throws IOException on I/O issues
     */
    public void write(byte[] b) throws IOException {
        out.write(b);
    }

    /**
     * Write bytes to the underlying output stream.
     *
     * @param b the array of bytes to write.
     * @param off write bytes from b starting at this offset
     * @param len write this many bytes
     * @throws IOException on I/O issues
     */
    public void write(byte[] b, int off, int len) throws IOException {
        out.write(b, off, len);
    }

    /**
     * Write a single byte to the underlying output stream.
     *
     * @param b the byte to write
     * @throws IOException on I/O issues
     */
    public void write(int b) throws IOException {
        out.write(b);
    }

    /**
     *  Serialize a float in IEEE 754-1985 notation, and in little endian
     *  byte order.
     *
     *  @param f the float to serialize
     *  @return the number of bytes written to the underlying output stream.
     *  @throws IOException on I/O issues
     */
    public int put(float f) throws IOException {
        int floatBits = Float.floatToRawIntBits(f);

        return put(floatBits);
    }

    /**
     *  Serialize a double in IEEE 754-1985 notation, and in little endian
     *  byte order.
     *
     *  @param d the double to serialize
     *  @return the number of bytes written to the underlying output stream.
     *  @throws IOException on I/O issues
     */
    public int put(double d) throws IOException {
        long doubleBits = Double.doubleToRawLongBits(d);

        return put(doubleBits);
    }

    /**
     *  Serialize a long (64 bit signed integer), in little endian
     *  byte order.
     *
     *  @param l the long to serialize
     *  @return the number of bytes written to the underlying output stream.
     *  @throws IOException on I/O issues
     */
    public int put(long l) throws IOException {
        write((int) (l         & 0xff));
        write((int) ((l >> 8)  & 0xff));
        write((int) ((l >> 16) & 0xff));
        write((int) ((l >> 24) & 0xff));
        write((int) ((l >> 32) & 0xff));
        write((int) ((l >> 40) & 0xff));
        write((int) ((l >> 48) & 0xff));
        write((int) ((l >> 56) & 0xff));

        return 8;
    }

    /**
     *  Serialize an int (32 bit signed integer), in little endian
     *  byte order.
     *
     *  @param i the int to serialize
     *  @return the number of bytes written to the underlying output stream.
     *  @throws IOException on I/O issues
     */
    public int put(int i) throws IOException {
        write((int) (i         & 0xff));
        write((int) ((i >> 8)  & 0xff));
        write((int) ((i >> 16) & 0xff));
        write((int) ((i >> 24) & 0xff));

        return 4;
    }

    /**
     *  Serialize a short (16 bit signed integer), in little endian
     *  byte order.
     *
     *  @param s the short to serialize
     *  @return the number of bytes written to the underlying output stream.
     *  @throws IOException on I/O issues
     */
    public int put(short s) throws IOException {
        write((int) (s         & 0xff));
        write((int) ((s >> 8)  & 0xff));

        return 2;
    }

    /**
     *  Serialize a byte (8 bit integer).
     *
     *  @param b the byte to serialize
     *  @return the number of bytes written to the underlying output stream.
     *  @throws IOException on I/O issues
     */
    public int put(byte b) throws IOException {
        write((int) b);

        return 1;
    }

    /**
     *  Serialize a string as UTF-8, adding a zero-termination at the end.
     *
     *  @param str the string to serialize
     *  @return the number of bytes written to the underlying output stream.
     *  @throws IOException on I/O issues
     */
    public int put(String str) throws IOException {
        byte[] buf = str.getBytes(Charset.forName("UTF-8"));

        write(buf);
        write((byte) 0x00);

        return buf.length + 1;
    }

    /**
     *  Serialize a string as UTF-8 into a fixed space, adding a
     *  zero-termination at the end, and padding the space with zero
     *  characters. This function truncates the string into the given space.
     *  WARNING: the function does not yet handle multi-byte UTF-8 sequences
     *  properly at the truncation point.
     *
     *  @param str the string to serialize
     *  @param length the space to fit the string into.
     *  @return the number of bytes written to the underlying output stream.
     *  @throws IOException on I/O issues
     */
    public int put(String str, int length) throws IOException {
        byte[] buf = str.getBytes(Charset.forName("UTF-8"));

        if (buf.length > length) {
            // if the first byte to be 'missed' is part of a multi-byte
            // UTF-8 sequence, let's backtrack so that this sequence is not
            // included at all - otherwise, it would be truncated
            // here, 0xc0 == 11000000 binary, and 0x80 = 10000000 binary
            // the non-first byte of a multi-byte sequence would have the
            // two highest bits as '10', e.g. of the form 10xxxxxxx
            int end = length;
            while (end > 0 && (buf[end] & 0xc0) == 0x80) {
                --end;
            }
            buf = Arrays.copyOf(buf, end);
        }

        write(buf);
        for (int count = buf.length; count < length; ++count) {
            write((byte) 0x00);
        }

        return length;
    }

    /**
     *  Serialize a UUID as a 16 byte big endian value.
     *
     *  @param id the UUID to serialize
     *  @return the number of bytes written to the underlying output stream.
     *  @throws IOException on I/O issues
     */
    public int put(UUID id) throws IOException {
        long l = id.getMostSignificantBits();
        write((int) ((l >> 56) & 0xff));
        write((int) ((l >> 48) & 0xff));
        write((int) ((l >> 40) & 0xff));
        write((int) ((l >> 32) & 0xff));
        write((int) ((l >> 24) & 0xff));
        write((int) ((l >> 16) & 0xff));
        write((int) ((l >> 8)  & 0xff));
        write((int) (l         & 0xff));

        l = id.getLeastSignificantBits();
        write((int) ((l >> 56) & 0xff));
        write((int) ((l >> 48) & 0xff));
        write((int) ((l >> 40) & 0xff));
        write((int) ((l >> 32) & 0xff));
        write((int) ((l >> 24) & 0xff));
        write((int) ((l >> 16) & 0xff));
        write((int) ((l >> 8)  & 0xff));
        write((int) (l         & 0xff));


        return 16;
    }

    /**
     *  Serialize a Date as long, the number of milliseconds since
     *  1st January 2000, 00:00:00 UTC.
     *
     *  @param date the Date to serialize
     *  @return the number of bytes written to the underlying output stream.
     *  @throws IOException on I/O issues
     */
    public int put(Date date) throws IOException {
        return put(date.getTime() - EPOCH2K);
    }

    /**
     *  Serialize a complete byte array by simply sending it downstream.
     *
     *  @param bytes the byte arraw to serialize
     *  @return the number of bytes written to the underlying output stream.
     *  @throws IOException on I/O issues
     */
    public int put(byte[] bytes) throws IOException {
        write(bytes);

        return bytes.length;
    }

    /**
     *  Serialize a partial byte array by simply sending it downstream.
     *
     *  @param bytes the byte arraw to serialize
     *  @param offset the offset in the array to write from
     *  @param length the number of bytes to write from offset
     *  @return the number of bytes written to the underlying output stream.
     *  @throws IOException on I/O issues
     */
    public int put(byte[] bytes, int offset, int length) throws IOException {
        write(bytes, offset, length);

        return length;
    }

}
