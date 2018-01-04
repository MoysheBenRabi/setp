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
import java.util.Arrays;

/**
 * An MXP message frame. A message frame either contains a complete message,
 * or part of a message. The useful payload for a message frame is at most 255
 * bytes.
 * Message fragments are then packed into Packet objects.
 */
public class MessageFrame {
    /**
     * The type of the message.
     */
    private byte type;

    /**
     * The message id - a unique id per session (?).
     */
    private int messageId;

    /**
     * The number of frames the message being sent is cut up into.
     */
    private short frameCount;

    /**
     * The frame index for this particular frame, between 0 .. frameCount.
     */
    private short frameIndex;

    /**
     * The actual data in this frame.
     */
    private byte[] frameData = new byte[0];

    /**
     * Constructor.
     */
    public MessageFrame() {
    }

    /**
     * Return the size of this message frame, when serialized.
     *
     * @return the size of this message frame, when serialized, or -1 if this
     *         cannot be determined.
     */
    public int size() {
        return frameData == null
             ? -1
             : 1 + 4 + 2 + 2 + 1 + frameData.length;
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

        type              = in.readByte();
        messageId         = in.readInt();
        frameCount        = in.readShort();
        frameIndex        = in.readShort();
        int frameDataSize = in.readByte();
        // adjust the value if it flips over, as Java only has signed bytes
        // while the protocol is using unsigned bytes
        if (frameDataSize < 0) {
            frameDataSize += 256;
        }
        frameData         = new byte[frameDataSize];
        in.readBytes(frameData);

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

        counter += out.put(type);
        counter += out.put(messageId);
        counter += out.put(frameCount);
        counter += out.put(frameIndex);
        counter += out.put((byte) frameData.length);
        counter += out.put(frameData);

        return counter;
    }

    /**
     * @return the type
     */
    public byte getType() {
        return type;
    }

    /**
     * @param type the type to set
     */
    public void setType(byte type) {
        this.type = type;
    }

    /**
     * @return the messageId
     */
    public int getMessageId() {
        return messageId;
    }

    /**
     * @param messageId the messageId to set
     */
    public void setMessageId(int messageId) {
        this.messageId = messageId;
    }

    /**
     * @return the frameCount
     */
    public short getFrameCount() {
        return frameCount;
    }

    /**
     * @param frameCount the frameCount to set
     */
    public void setFrameCount(short frameCount) {
        this.frameCount = frameCount;
    }

    /**
     * @return the frameIndex
     */
    public short getFrameIndex() {
        return frameIndex;
    }

    /**
     * @param frameIndex the frameIndex to set
     */
    public void setFrameIndex(short frameIndex) {
        this.frameIndex = frameIndex;
    }

    /**
     * @return the frameData
     */
    public byte[] getFrameData() {
        return frameData;
    }

    /**
     * @param frameData the frameData to set
     */
    public void setFrameData(byte[] frameData) {
        if (frameData.length > 255) {
            throw new IllegalArgumentException();
        }

        this.frameData = frameData;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = 1;
        result = prime * result + frameCount;
        result = prime * result + Arrays.hashCode(frameData);
        result = prime * result + frameIndex;
        result = prime * result + messageId;
        result = prime * result + type;
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
        if (!(obj instanceof MessageFrame)) {
            return false;
        }
        MessageFrame other = (MessageFrame) obj;
        if (frameCount != other.frameCount) {
            return false;
        }
        if (!Arrays.equals(frameData, other.frameData)) {
            return false;
        }
        if (frameIndex != other.frameIndex) {
            return false;
        }
        if (messageId != other.messageId) {
            return false;
        }
        if (type != other.type) {
            return false;
        }
        return true;
    }
}
