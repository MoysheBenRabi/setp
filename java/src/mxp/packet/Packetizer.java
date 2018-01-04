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

import mxp.message.Message;
import mxp.serialization.SerializationInputStream;
import mxp.serialization.SerializationOutputStream;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.Vector;

/**
 * A class that turns messages into message fragments, and then puts those
 * into packets.
 */
public final class Packetizer {
    /**
     * Hidden constructor - there's no need to instantiate this class.
     */
    private Packetizer() {
    }

    /**
     * Turn a message into message frames, by serializing the message and
     * breaking it up into a number of message fragments as needed.
     *
     * @param message the message to turn into message fragments.
     * @param messageId the unique id of this message within the active session
     * @param frames a list of message frames. the generated frames
     *        will be appended to the end of this list.
     * @return the total size of the message fragments if serialized, including
     *         the message frame headers, that were added to the fragments list.
     * @throws IOException on serialization errors.
     */
    public static int
    messageToMessageFrames(final Message          message,
                           int                    messageId,
                           List<MessageFrame>     frames)
                                                            throws IOException {
        // serialize the message
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);

        message.serialize(out);
        out.flush();

        // put it into message fragments by cutting the serialized data into
        // chunks of at most 255 bytes
        byte[] bytes      = baOut.toByteArray();
        int    index      = 0;
        int    frameCount = (bytes.length / 255);
        if (frameCount * 255 < bytes.length) {
            ++frameCount;
        }
        if (frameCount == 0) {
            ++frameCount;
        }
        int    frameIndex = 0;
        int    counter    = 0;

        if (bytes.length == 0) {
            // create an empty frame for messages of 0 length
            MessageFrame mf = new MessageFrame();
            mf.setType((byte) message.getType().getType());
            mf.setMessageId(messageId);
            mf.setFrameCount((short) frameCount);
            mf.setFrameIndex((short) frameIndex);

            frames.add(mf);

            counter += mf.size();
        } else {
            // create as many frames as needed to send the whole message
            while (index < bytes.length) {
                int     chunkSize = Math.min(bytes.length - index, 255);
                byte[]  chunk     = Arrays.copyOfRange(bytes,
                                                       index,
                                                       index + chunkSize);

                MessageFrame mf = new MessageFrame();
                mf.setType((byte) message.getType().getType());
                mf.setMessageId(messageId);
                mf.setFrameCount((short) frameCount);
                mf.setFrameIndex((short) frameIndex);
                mf.setFrameData(chunk);

                frames.add(mf);

                counter += mf.size();
                index += chunkSize;
                frameIndex++;
            }
        }

        return counter;
    }

    /**
     * Turn a number of message frames back into a message, by getting the
     * serialized data out of message frames, putting then together, and
     * de-serializing them to form an appropriate message structure.
     * It is expected that all frames required for the message are present,
     * and are presented in proper order.
     *
     * @param frames the message frames that make up a message, ordered by
     *        their frameIndex property.
     * @return the de-serialized message that the message frames contained.
     * @throws IOException on de-serialization errors.
     */
    public static Message
    messageFramesToMessage(final List<MessageFrame> frames) throws IOException {
        if (frames.size() == 0) {
            throw new IllegalArgumentException();
        }

        ByteArrayOutputStream   baOut       = new ByteArrayOutputStream();
        MessageFrame            firstFrame  = frames.get(0);
        int                     messageId   = firstFrame.getMessageId();
        int                     frameCount  = firstFrame.getFrameCount();
        int                     frameIndex  = firstFrame.getFrameIndex();
        byte                    typeId      = firstFrame.getType();

        if (frameIndex != 0) {
            throw new IllegalArgumentException();
        }

        for (MessageFrame mf : frames) {
            if (mf.getMessageId() != messageId) {
                throw new IllegalArgumentException();
            }
            if (mf.getFrameIndex() != frameIndex) {
                throw new IllegalArgumentException();
            }

            baOut.write(mf.getFrameData());

            if (++frameIndex == frameCount) {
                break;
            }
        }

        ByteArrayInputStream     baIn = new ByteArrayInputStream(
                                                        baOut.toByteArray());
        SerializationInputStream in = new SerializationInputStream(baIn);
        Message                  message = Message.forType(
                                                Message.Type.forTypeId(typeId));

        message.deserialize(in, baOut.toByteArray().length);

        return message;
    }

    /**
     * Convert a series of messages into a series of packets transport them.
     * This is done by serializing the messages, putting them into message
     * frames, and then putting these message frames into packets.
     * Each generated packet will have a resend count of 0, and parameters
     * set as supplied to this function.
     *
     * @param messages a list of messages to convert
     * @param baseMessageId messages will be assigned ids starting with
     *        this id. each subsequent message will be assigned an id
     *        one greater - in total, ids ranging from baseMessageId to
     *        baseMessageId + message.size() - 1 will be used.
     * @param packets a list of packets. the new packets generated will
     *        be appended to this list. if the list is not empty, and there
     *        is space for additional message frames in the last packet,
     *        that space will be utilized as well.
     * @param sessionId the sessionId to set in the newly created packets.
     * @param basePacketId packets will be assigned ids starting from this id,
     *        with each new packet getting an id greater than the previous one.
     *        in total, ids ranging from basePacketId ... basePacketId +
     *        (number of created packet) - 1 will be used.
     * @param firstSendTime the first sending time to set for the generated
     *        packets.
     * @param guaranteed the guaranteed flag to set for each newly generated
     *        packet.
     * @throws IOException on I/O errors
     */
    public static void
    messagesToPackets(final List<Message>   messages,
                      int                   baseMessageId,
                      List<Packet>          packets,
                      int                   sessionId,
                      int                   basePacketId,
                      Date                  firstSendTime,
                      byte                  guaranteed)
                                                            throws IOException {
        if (messages.isEmpty()) {
            return;
        }

        // first convert the messages into message frames
        int                  messageId = baseMessageId;
        Vector<MessageFrame> frames = new Vector<MessageFrame>(messages.size());
        for (Message message : messages) {
            messageToMessageFrames(message, messageId++, frames);
        }

        // now put these frames into packets
        // re-use the last packet if there's still room in it
        if (!packets.isEmpty()) {
            Packet packet = packets.get(packets.size() - 1);

            while (!frames.isEmpty()
                && packet.available() > frames.firstElement().size()) {

                packet.addMessageFrame(frames.firstElement());
                frames.remove(0);
            }
        }

        // now add additional packets as necessary
        Packet packet = null;
        int    packetId = basePacketId;
        while (!frames.isEmpty()) {
            if (packet == null) {
                packet = new Packet();
                packet.setSessionId(sessionId);
                packet.setPacketId(packetId++);
                packet.setFirstSendTime(firstSendTime);
                packet.setGuaranteed(guaranteed);
                packet.setResendCount((byte) 0);
            }

            if (packet.available() > frames.firstElement().size()) {
                packet.addMessageFrame(frames.firstElement());
                frames.remove(0);
            } else {
                packets.add(packet);
                packet = null;
            }
        }
        if (packet != null) {
            packets.add(packet);
        }
    }

    /**
     * Convert a series of packets into messages.
     * This is achieved by getting the message frames out of the packets,
     * assembling messages from the frames and de-serializing these masseges.
     * Messages that can fully be de-serialized are deserialized and returned
     * as messages. Messages that have missing message fragments are collected
     * in a map of message fragments, keyed by the message id. this map can be
     * re-used in subsequent calls to complete these messages.
     *
     * @param packets a list of packets to process.
     * @param messages messages de-serialized from the supplied packets
     *        are appended to this list.
     * @param messageIds the ids for the de-serialized messages are put into
     *        this list, in the same order as in the messages list
     * @param messageFrames a map of message ids mapped to an array of message
     *        frames. used to store messages with missing frames. the message
     *        frame arrays contain each frame at their respective index,
     *        and thus is as long as many frames the message is made up in,
     *        with the missing frames as null values at their respective
     *        indexes. any messages that cannot be processed because of missing
     *        frames are put here. this map can be re-used for subsequent calls
     *        so as the missing frames re received, the complete messages are
     *        processed and returned.
     * @throws IOException on I/O errors
     */
    public static void
    packetsToMessages(final List<Packet>                packets,
                      List<Message>                     messages,
                      List<Integer>                     messageIds,
                      Map<Integer, List<MessageFrame>>  messageFrames)
                                                        throws IOException {
        // first extract the message frames from the packets, and put them
        // into the message frame map, keyed by the message id
        for (Packet packet : packets) {
            List<MessageFrame> frames = packet.getMessageFrames();

            for (MessageFrame frame : frames) {
                int                 messageId = frame.getMessageId();
                List<MessageFrame>  frameList;

                if (messageFrames.containsKey(messageId)) {
                    frameList = messageFrames.get(messageId);

                    // resize the framelist if the framecount we're getting
                    // is not consistent with it - this would mean a broken
                    // implementation on the other side :(
                    if (frame.getFrameCount() < frameList.size()) {
                        frameList = frameList.subList(0, frame.getFrameCount());
                        messageFrames.put(messageId, frameList);
                    } else if (frame.getFrameCount() > frameList.size()) {
                        List<MessageFrame> fl =
                            new ArrayList<MessageFrame>(frame.getFrameCount());

                        fl.addAll(frameList);
                        for (int i = frameList.size();
                             i < frame.getFrameCount(); ++i) {
                            fl.add(null);
                        }

                        frameList = fl;
                        messageFrames.put(messageId, frameList);
                    }
                } else {
                    frameList =
                            new ArrayList<MessageFrame>(frame.getFrameCount());
                    for (int i = 0; i < frame.getFrameCount(); ++i) {
                        frameList.add(null);
                    }
                }

                frameList.set(frame.getFrameIndex(), frame);

                messageFrames.put(messageId, frameList);
            }
        }

        // now process each entry in the message frames map, and build up
        // messages which have all their frames
        Set<Integer>    messagesToRemove =
                                    new HashSet<Integer>(messageFrames.size());
        message_id_loop:
        for (Integer messageId : messageFrames.keySet()) {
            List<MessageFrame>  frames = messageFrames.get(messageId);

            // if a message frame is missing, don't process these frames
            for (MessageFrame frame : frames) {
                if (frame == null) {
                    continue message_id_loop;
                }
            }

            Message message = messageFramesToMessage(frames);
            messages.add(message);
            messageIds.add(messageId);
            messagesToRemove.add(messageId);
        }

        for (Integer removeMessageId : messagesToRemove) {
            messageFrames.remove(removeMessageId);
        }
    }
}
