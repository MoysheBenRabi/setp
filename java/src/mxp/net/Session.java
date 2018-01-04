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
package mxp.net;

import java.net.SocketAddress;
import java.util.Date;
import java.util.List;
import java.util.Map;

import mxp.packet.MessageFrame;
import mxp.packet.Packet;

/**
 * A session state between MXP protocol participants.
 * Sessions in MXP have two identifiers: one from our end, and one from the
 * remote end. It is assumed that each id is unique on the respective end of
 * the communication.
 *
 * Session and message ids of the value 0 are treated as invalid / empty /
 * null ids.
 */
public interface Session {
    /**
     * @return the sessionId
     */
    int getSessionId();

    /**
     * @return the address
     */
    SocketAddress getAddress();

    /**
     * @return the remoteSessionId
     */
    int getRemoteSessionId();

    /**
     * @return the messageId
     */
    int getMessageId();

    /**
     * Return the next message id to use - a unique message id for this session.
     *
     * @return the next messageId
     */
    int nextMessageId();

    /**
     * @return the messageQueue
     */
    List<MessageQueueItem> getMessageQueue();

    /**
     * @return the packetsPendingAck
     */
    Map<Integer, Packet> getPacketsPendingAck();

    /**
     * @return the packetsToAck
     */
    List<Integer> getPacketsToAck();

    /**
     * @return the messageFrames
     */
    Map<Integer, List<MessageFrame>> getMessageFrames();

    /**
     * The time-stamp of the last acknowledge message sent.
     *
     * @return the times-tamp of the last acknowledge message sent.
     */
    Date getLastAckTime();

    /**
     * Set the time-stamp of the last acknowledge message sent.
     *
     * @param timestamp the time-stamp of the last acknowledge message sent.
     */
    void setLastAckTime(Date timestamp);

    /**
     * The time stamp of the last keep-alive message we sent.
     *
     * @return the time stamp of the last keep-alive message we sent.
     */
    Date getLastKeepalive();

    /**
     * Set the time stamp of the last keep-alive message we sent.
     *
     * @param timestamp the time stamp of the most recent keep-alive message.
     */
    void setLastKeepalive(Date timestamp);

    /**
     * The time stamp of the last message received from the remote end.
     *
     * @return the time stamp of the last message received from the remote end.
     */
    Date getLastMessageTime();

    /**
     * Set the time stamp of the last message received from the remote end.
     *
     * @param timestamp the time stamp of the last message received from the
     *        remote end.
     */
    void setLastMessageTime(Date timestamp);
}
