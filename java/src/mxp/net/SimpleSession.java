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
public final class SimpleSession implements Session {
    /**
     * The session id, from our side.
     */
    private int sessionId = 0;

    /**
     * The session id of the remote end.
     */
    private int remoteSessionId = 0;

    /**
     * The current message id, which is incremented for each message.
     */
    private int messageId = 0;

    /**
     * The address of the other party in the communication.
     */
    private SocketAddress address;

    /**
     * A message queue - the messages that need to be sent for this session.
     */
    private List<MessageQueueItem>  messageQueue;

    /**
     * Packets that were sent for this session, but that have not yet
     * received acknowledgements from the other side.
     */
    private Map<Integer, Packet> packetsPendingAck;

    /**
     * Packet ids that we need to send acknowledgement for - these are ids
     * of packets that were received, and flagged with the guaranteed flag.
     */
    private List<Integer> packetsToAck;

    /**
     * Message frame sequences that are missing some frames.
     */
    private Map<Integer, List<MessageFrame>> messageFrames;

    /**
     * The time-stamp of the last acknowledge message sent.
     */
    private Date lastAckTime = new Date();

    /**
     * The time stamp of the last kee-palive message that was sent for this
     * session.
     */
    private Date lastKeepalive = new Date();

    /**
     * The timestamp of the last message we received from the other end.
     */
    private Date lastMessageTime = new Date();

    /**
     * Private constructor. One most create a session via the factor method,
     * to ensure a unique session id.
     */
    SimpleSession() {
    }

    @Override
    public int getSessionId() {
        return sessionId;
    }

    /**
     * Set the session id.
     *
     * @param sessionId the sessions id.
     */
    void setSessionId(int sessionId) {
        this.sessionId = sessionId;
    }

    @Override
    public SocketAddress getAddress() {
        return address;
    }

    /**
     * @param address the address to set
     */
    void setAddress(SocketAddress address) {
        this.address = address;
    }

    @Override
    public int getMessageId() {
        return messageId;
    }

    @Override
    public int nextMessageId() {
        return ++messageId;
    }

    /**
     * @param messageId the messageId to set
     */
    void setMessageId(int messageId) {
        this.messageId = messageId;
    }

    @Override
    public int getRemoteSessionId() {
        return remoteSessionId;
    }

    /**
     * @param remoteSessionId the remoteSessionId to set
     */
    void setRemoteSessionId(int remoteSessionId) {
        this.remoteSessionId = remoteSessionId;
    }

    @Override
    public List<MessageQueueItem> getMessageQueue() {
        return messageQueue;
    }

    /**
     * @param messageQueue the messageQueue to set
     */
    void setMessageQueue(List<MessageQueueItem> messageQueue) {
        this.messageQueue = messageQueue;
    }

    @Override
    public Map<Integer, Packet> getPacketsPendingAck() {
        return packetsPendingAck;
    }

    /**
     * @param packetsPendingAck the packetsPendingAck to set
     */
    void setPacketsPendingAck(Map<Integer, Packet> packetsPendingAck) {
        this.packetsPendingAck = packetsPendingAck;
    }

    @Override
    public List<Integer> getPacketsToAck() {
        return packetsToAck;
    }

    /**
     * @param packetsToAck the packetsToAck to set
     */
    void setPacketsToAck(List<Integer> packetsToAck) {
        this.packetsToAck = packetsToAck;
    }

    @Override
    public Map<Integer, List<MessageFrame>> getMessageFrames() {
        return messageFrames;
    }

    /**
     * @param messageFrames the messageFrames to set
     */
    void setMessageFrames(Map<Integer, List<MessageFrame>> messageFrames) {
        this.messageFrames = messageFrames;
    }

    @Override
    public Date getLastAckTime() {
        return lastAckTime;
    }

    @Override
    public void setLastAckTime(Date timestamp) {
        lastAckTime = timestamp;
    }

    @Override
    public Date getLastKeepalive() {
        return lastKeepalive;
    }

    @Override
    public void setLastKeepalive(Date lastKeepalive) {
        this.lastKeepalive = lastKeepalive;
    }

    @Override
    public Date getLastMessageTime() {
        return lastMessageTime;
    }

    @Override
    public void setLastMessageTime(Date timestamp) {
        lastMessageTime = timestamp;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = 1;
        result = prime * result + ((address == null) ? 0 : address.hashCode());
        result = prime * result
        + ((lastKeepalive == null) ? 0 : lastKeepalive.hashCode());
        result = prime * result
        + ((lastMessageTime == null) ? 0 : lastMessageTime.hashCode());
        result = prime * result
        + ((messageFrames == null) ? 0 : messageFrames.hashCode());
        result = prime * result + messageId;
        result = prime * result
        + ((messageQueue == null) ? 0 : messageQueue.hashCode());
        result = prime
        * result
        + ((packetsPendingAck == null) ? 0 : packetsPendingAck
                .hashCode());
        result = prime * result
        + ((packetsToAck == null) ? 0 : packetsToAck.hashCode());
        result = prime * result + remoteSessionId;
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
        if (!(obj instanceof SimpleSession)) {
            return false;
        }
        SimpleSession other = (SimpleSession) obj;
        if (address == null) {
            if (other.address != null) {
                return false;
            }
        } else if (!address.equals(other.address)) {
            return false;
        }
        if (lastKeepalive == null) {
            if (other.lastKeepalive != null) {
                return false;
            }
        } else if (!lastKeepalive.equals(other.lastKeepalive)) {
            return false;
        }
        if (lastMessageTime == null) {
            if (other.lastMessageTime != null) {
                return false;
            }
        } else if (!lastMessageTime.equals(other.lastMessageTime)) {
            return false;
        }
        if (messageFrames == null) {
            if (other.messageFrames != null) {
                return false;
            }
        } else if (!messageFrames.equals(other.messageFrames)) {
            return false;
        }
        if (messageId != other.messageId) {
            return false;
        }
        if (messageQueue == null) {
            if (other.messageQueue != null) {
                return false;
            }
        } else if (!messageQueue.equals(other.messageQueue)) {
            return false;
        }
        if (packetsPendingAck == null) {
            if (other.packetsPendingAck != null) {
                return false;
            }
        } else if (!packetsPendingAck.equals(other.packetsPendingAck)) {
            return false;
        }
        if (packetsToAck == null) {
            if (other.packetsToAck != null) {
                return false;
            }
        } else if (!packetsToAck.equals(other.packetsToAck)) {
            return false;
        }
        if (remoteSessionId != other.remoteSessionId) {
            return false;
        }
        if (sessionId != other.sessionId) {
            return false;
        }
        return true;
    }

    @Override
    public String toString() {
        return "SimpleSession [address=" + address + ", lastKeepalive="
        + lastKeepalive + ", lastMessageTime=" + lastMessageTime
        + ", messageFrames=" + messageFrames + ", messageId="
        + messageId + ", messageQueue=" + messageQueue
        + ", packetsPendingAck=" + packetsPendingAck
        + ", packetsToAck=" + packetsToAck + ", remoteSessionId="
        + remoteSessionId + ", sessionId=" + sessionId + "]";
    }
}
