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
package mxp.message;

import mxp.serialization.SerializationInputStream;
import mxp.serialization.SerializationOutputStream;

import java.io.IOException;
import java.util.Date;
import java.util.UUID;

/**
 * A join response message, as specified by the MXP protocol.
 */
public class JoinResponse extends Message {
    /**
     * The response header.
     */
    private ResponseFragment responseHeader = new ResponseFragment();

    /**
     * The id of the bubble to join.
     */
    private UUID bubbleId;

    /**
     * The id of the participant that joins.
     */
    private UUID participantId;

    /**
     * The id of the avatar that joins.
     */
    private UUID avatarId;

    /**
     * The name of the bubble.
     * (may be an empty string, at most 40 bytes encoded)
     */
    private String bubbleName;

    /**
     * A URL for accessing binary assets in relation to this bubble.
     */
    private String bubbleAssetCacheUrl;

    /**
     * The range of the bubble (?).
     */
    private float bubbleRange;

    /**
     * The range of perception in the bubble (?).
     */
    private float bubblePerceptionRange;

    /**
     * The current time of the bubble.
     */
    private Date bubbleRealtime;

    /**
     * Details of the server program that sends the join response.
     */
    private ProgramFragment serverProgram = new ProgramFragment();

    /**
     * Constructor.
     */
    public JoinResponse() {
        super(Message.Type.JOIN_RESPONSE);
    }

    /**
     * @return the responseHeader
     */
    public ResponseFragment getResponseHeader() {
        return responseHeader;
    }

    /**
     * @param responseHeader the responseHeader to set
     */
    public void setResponseHeader(ResponseFragment responseHeader) {
        this.responseHeader = responseHeader;
    }

    /**
     * @return the bubbleId
     */
    public UUID getBubbleId() {
        return bubbleId;
    }

    /**
     * @param bubbleId the bubbleId to set
     */
    public void setBubbleId(UUID bubbleId) {
        this.bubbleId = bubbleId;
    }

    /**
     * @return the participantId
     */
    public UUID getParticipantId() {
        return participantId;
    }

    /**
     * @param participantId the participantId to set
     */
    public void setParticipantId(UUID participantId) {
        this.participantId = participantId;
    }

    /**
     * @return the avatarId
     */
    public UUID getAvatarId() {
        return avatarId;
    }

    /**
     * @param avatarId the avatarId to set
     */
    public void setAvatarId(UUID avatarId) {
        this.avatarId = avatarId;
    }

    /**
     * @return the bubbleName
     */
    public String getBubbleName() {
        return bubbleName;
    }

    /**
     * @param bubbleName the bubbleName to set
     */
    public void setBubbleName(String bubbleName) {
        this.bubbleName = bubbleName;
    }

    /**
     * @return the bubbleAssetCacheUrl
     */
    public String getBubbleAssetCacheUrl() {
        return bubbleAssetCacheUrl;
    }

    /**
     * @param bubbleAssetCacheUrl the bubbleAssetCacheUrl to set
     */
    public void setBubbleAssetCacheUrl(String bubbleAssetCacheUrl) {
        this.bubbleAssetCacheUrl = bubbleAssetCacheUrl;
    }

    /**
     * @return the bubbleRange
     */
    public float getBubbleRange() {
        return bubbleRange;
    }

    /**
     * @param bubbleRange the bubbleRange to set
     */
    public void setBubbleRange(float bubbleRange) {
        this.bubbleRange = bubbleRange;
    }

    /**
     * @return the bubblePerceptionRange
     */
    public float getBubblePerceptionRange() {
        return bubblePerceptionRange;
    }

    /**
     * @param bubblePerceptionRange the bubblePerceptionRange to set
     */
    public void setBubblePerceptionRange(float bubblePerceptionRange) {
        this.bubblePerceptionRange = bubblePerceptionRange;
    }

    /**
     * @return the bubbleRealtime
     */
    public Date getBubbleRealtime() {
        return bubbleRealtime;
    }

    /**
     * @param bubbleRealtime the bubbleRealtime to set
     */
    public void setBubbleRealtime(Date bubbleRealtime) {
        this.bubbleRealtime = bubbleRealtime;
    }

    /**
     * @return the serverProgram
     */
    public ProgramFragment getServerProgram() {
        return serverProgram;
    }

    /**
     * @param serverProgram the serverProgram to set
     */
    public void setServerProgram(ProgramFragment serverProgram) {
        this.serverProgram = serverProgram;
    }

    @Override
    public int size() {
        return responseHeader == null || serverProgram == null
             ? -1
             : responseHeader.size() + 16 + 16 + 16 + 40 + 50 + 4 + 4 + 4
               + serverProgram.size();
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        responseHeader        = new ResponseFragment();
        responseHeader.deserialize(in, length);
        bubbleId              = in.readUUID();
        participantId         = in.readUUID();
        avatarId              = in.readUUID();
        bubbleName            = in.readString(40);
        bubbleAssetCacheUrl   = in.readString(50);
        bubbleRange           = in.readFloat();
        bubblePerceptionRange = in.readFloat();
        bubbleRealtime        = in.readDate();
        serverProgram         = new ProgramFragment();
        serverProgram.deserialize(in, length - (in.counter() - counter));

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += responseHeader.serialize(out);
        counter += out.put(bubbleId);
        counter += out.put(participantId);
        counter += out.put(avatarId);
        counter += out.put(bubbleName, 40);
        counter += out.put(bubbleAssetCacheUrl, 50);
        counter += out.put(bubbleRange);
        counter += out.put(bubblePerceptionRange);
        counter += out.put(bubbleRealtime);
        counter += serverProgram.serialize(out);

        return counter;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = super.hashCode();
        result = prime * result
                + ((avatarId == null) ? 0 : avatarId.hashCode());
        result = prime
                * result
                + ((bubbleAssetCacheUrl == null) ? 0 : bubbleAssetCacheUrl
                        .hashCode());
        result = prime * result
                + ((bubbleId == null) ? 0 : bubbleId.hashCode());
        result = prime * result
                + ((bubbleName == null) ? 0 : bubbleName.hashCode());
        result = prime * result + Float.floatToIntBits(bubblePerceptionRange);
        result = prime * result + Float.floatToIntBits(bubbleRange);
        result = prime * result
                + ((bubbleRealtime == null) ? 0 : bubbleRealtime.hashCode());
        result = prime * result
                + ((participantId == null) ? 0 : participantId.hashCode());
        result = prime * result
                + ((responseHeader == null) ? 0 : responseHeader.hashCode());
        result = prime * result
                + ((serverProgram == null) ? 0 : serverProgram.hashCode());
        return result;
    }

    @Override
    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (!super.equals(obj)) {
            return false;
        }
        if (!(obj instanceof JoinResponse)) {
            return false;
        }
        JoinResponse other = (JoinResponse) obj;
        if (avatarId == null) {
            if (other.avatarId != null) {
                return false;
            }
        } else if (!avatarId.equals(other.avatarId)) {
            return false;
        }
        if (bubbleAssetCacheUrl == null) {
            if (other.bubbleAssetCacheUrl != null) {
                return false;
            }
        } else if (!bubbleAssetCacheUrl.equals(other.bubbleAssetCacheUrl)) {
            return false;
        }
        if (bubbleId == null) {
            if (other.bubbleId != null) {
                return false;
            }
        } else if (!bubbleId.equals(other.bubbleId)) {
            return false;
        }
        if (bubbleName == null) {
            if (other.bubbleName != null) {
                return false;
            }
        } else if (!bubbleName.equals(other.bubbleName)) {
            return false;
        }
        if (Float.floatToIntBits(bubblePerceptionRange) != Float
                .floatToIntBits(other.bubblePerceptionRange)) {
            return false;
        }
        if (Float.floatToIntBits(bubbleRange) != Float
                .floatToIntBits(other.bubbleRange)) {
            return false;
        }
        if (bubbleRealtime == null) {
            if (other.bubbleRealtime != null) {
                return false;
            }
        } else if (!bubbleRealtime.equals(other.bubbleRealtime)) {
            return false;
        }
        if (participantId == null) {
            if (other.participantId != null) {
                return false;
            }
        } else if (!participantId.equals(other.participantId)) {
            return false;
        }
        if (responseHeader == null) {
            if (other.responseHeader != null) {
                return false;
            }
        } else if (!responseHeader.equals(other.responseHeader)) {
            return false;
        }
        if (serverProgram == null) {
            if (other.serverProgram != null) {
                return false;
            }
        } else if (!serverProgram.equals(other.serverProgram)) {
            return false;
        }
        return true;
    }

}
