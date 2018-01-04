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

import java.io.IOException;
import java.util.Date;
import java.util.Vector;

/**
 * An MXP packet - the basic holder of message frames. A packet can be sent
 * directly as a UDP packet. Such a packet can be at most 1452 bytes long
 * in total, and can hold MessageFrame objects for a total of 1434 bytes.
 */
public class Packet {
    /**
     * The maximum total size of a packet when serialized, in bytes.
     */
    public static final int MAX_SIZE = 1452;

    /**
     * The maximum useful (payload) size of the packet when serialized,
     * in bytes.
     */
    public static final int MAX_PAYLOAD = 1452 - 18;

    /**
     * The session id, unique for each session.
     */
    private int sessionId;

    /**
     * The packet id, unique within a session.
     */
    private int packetId;

    /**
     * The time when this message was sent for the first time.
     */
    private Date firstSendTime = new Date();

    /**
     * Flag to indicate if this is a guaranteed packet - that is, an
     * acknowledgement is expected when the packet was received.
     */
    private byte guaranteed;

    /**
     * The number of times this packet has been resent.
     */
    private byte resendCount;

    /**
     * Message frames contained in this packet.
     */
    private Vector<MessageFrame> messageFrames = new Vector<MessageFrame>();

    /**
     * Constructor.
     */
    public Packet() {
    }

    /**
     * Return the size of this packet when serialized.
     *
     * @return the size of this packet when serialized, or -1 if this cannot
     *         be calculated.
     */
    public int size() {
        if (firstSendTime == null || messageFrames == null) {
            return -1;
        }

        int size = 4 + 4 + 8 + 1 + 1;
        for (MessageFrame mf : messageFrames) {
            if (mf.size() == -1) {
                return -1;
            }

            size += mf.size();
        }

        return size;
    }

    /**
     * Tell how much useful payload this packet can transfer, in addition
     * to what's already in it.
     *
     * @return the size of the additional useful payload this packet can
     *         transfer, in bytes.
     */
    public int available() {
        return MAX_SIZE - size();
    }

    /**
     * Add a message frame to the end of the message frames list.
     *
     * @param messageFrame the message frame to add.
     */
    public void addMessageFrame(MessageFrame messageFrame) {
        messageFrames.add(messageFrame);
    }

    /**
     * Build a message frame by reading its serialized form from an input
     * stream.
     *
     * @param in the input stream to read from.
     * @param length the maximum number of bytes to read from in.
     * @return the number of bytes read from the serialization stream.
     * @throws IOException on I/O errors.
     */
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        sessionId     = in.readInt();
        packetId      = in.readInt();
        firstSendTime = in.readDate();
        guaranteed    = in.readByte();
        resendCount   = in.readByte();

        messageFrames = new Vector<MessageFrame>();
        while (length > (in.counter() - counter)) {
            MessageFrame mf = new MessageFrame();
            mf.deserialize(in, length - (in.counter() - counter));

            messageFrames.add(mf);
        }

        return in.counter() - counter;
    }

    /**
     * Serialize the message frame.
     *
     * @param out the output stream to serialize into.
     * @return the number of bytes written into the serialization stream.
     * @throws IOException on I/O errors.
     */
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += out.put(sessionId);
        counter += out.put(packetId);
        counter += out.put(firstSendTime);
        counter += out.put(guaranteed);
        counter += out.put(resendCount);

        for (MessageFrame mf : messageFrames) {
            counter += mf.serialize(out);
        }

        return counter;
    }

    /**
     * @return the sessionId
     */
    public int getSessionId() {
        return sessionId;
    }

    /**
     * @param sessionId the sessionId to set
     */
    public void setSessionId(int sessionId) {
        this.sessionId = sessionId;
    }

    /**
     * @return the packetId
     */
    public int getPacketId() {
        return packetId;
    }

    /**
     * @param packetId the packetId to set
     */
    public void setPacketId(int packetId) {
        this.packetId = packetId;
    }

    /**
     * @return the firstSendTime
     */
    public Date getFirstSendTime() {
        return firstSendTime;
    }

    /**
     * @param firstSendTime the firstSendTime to set
     */
    public void setFirstSendTime(Date firstSendTime) {
        this.firstSendTime = firstSendTime;
    }

    /**
     * @return the guaranteed
     */
    public byte getGuaranteed() {
        return guaranteed;
    }

    /**
     * @param guaranteed the guaranteed to set
     */
    public void setGuaranteed(byte guaranteed) {
        this.guaranteed = guaranteed;
    }

    /**
     * @return the resendCount
     */
    public byte getResendCount() {
        return resendCount;
    }

    /**
     * @param resendCount the resendCount to set
     */
    public void setResendCount(byte resendCount) {
        this.resendCount = resendCount;
    }

    /**
     * @return the messageFrames
     */
    public Vector<MessageFrame> getMessageFrames() {
        return messageFrames;
    }

    /**
     * @param messageFrames the messageFrames to set
     */
    public void setMessageFrames(Vector<MessageFrame> messageFrames) {
        Vector<MessageFrame> oldFrames = this.messageFrames;

        this.messageFrames = messageFrames;

        if (size() > MAX_SIZE) {
            this.messageFrames = oldFrames;
            throw new IllegalArgumentException();
        }
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = 1;
        result = prime * result
                + ((firstSendTime == null) ? 0 : firstSendTime.hashCode());
        result = prime * result + guaranteed;
        result = prime * result
                + ((messageFrames == null) ? 0 : messageFrames.hashCode());
        result = prime * result + packetId;
        result = prime * result + resendCount;
        result = prime * result + sessionId;
        return result;
    }

    @Override
    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (obj == null) {
            return false;
        }
        if (!(obj instanceof Packet)) {
            return false;
        }
        Packet other = (Packet) obj;
        if (firstSendTime == null) {
            if (other.firstSendTime != null) {
                return false;
            }
        } else if (!firstSendTime.equals(other.firstSendTime)) {
            return false;
        }
        if (guaranteed != other.guaranteed) {
            return false;
        }
        if (messageFrames == null) {
            if (other.messageFrames != null) {
                return false;
            }
        } else if (!messageFrames.equals(other.messageFrames)) {
            return false;
        }
        if (packetId != other.packetId) {
            return false;
        }
        if (resendCount != other.resendCount) {
            return false;
        }
        if (sessionId != other.sessionId) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "Packet [firstSendTime=" + firstSendTime + ", guaranteed="
                + guaranteed + ", messageFrames=" + messageFrames
                + ", packetId=" + packetId + ", resendCount=" + resendCount
                + ", sessionId=" + sessionId + "]";
    }
}
