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
 * A join request message, as specified by the MXP protocol.
 */
public class JoinRequest extends Message {
    /**
     * The id of the bubble to join.
     */
    private UUID bubbleId;

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
     * The name of the landing location. (may be an empty string, at most 28
     * bytes encoded)
     */
    private String locationName;

    /**
     * The id of the participant. (at most 32 bytes encoded)
     */
    private String participantId;

    /**
     * Pass phrase or authentication token for the participant.
     * (at most 32 bytes encoded)
     */
    private String participantSecret;

    /**
     * The current time of the participant.
     */
    private Date participantRealtime;

    /**
     * URL to an identity provider, say an OpenID provider.
     * (at most 50 bytes encoded)
     */
    private String identityProviderUrl;

    /**
     * Details of the client program that wants to join.
     */
    private ProgramFragment clientProgram = new ProgramFragment();

    /**
     * Constructor.
     */
    public JoinRequest() {
        super(Message.Type.JOIN_REQUEST);
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
     * @return the locationName
     */
    public String getLocationName() {
        return locationName;
    }

    /**
     * @param locationName the locationName to set
     */
    public void setLocationName(String locationName) {
        this.locationName = locationName;
    }

    /**
     * @return the participantId
     */
    public String getParticipantId() {
        return participantId;
    }

    /**
     * @param participantId the participantId to set
     */
    public void setParticipantId(String participantId) {
        this.participantId = participantId;
    }

    /**
     * @return the participantSecret
     */
    public String getParticipantSecret() {
        return participantSecret;
    }

    /**
     * @param participantSecret the participantSecret to set
     */
    public void setParticipantSecret(String participantSecret) {
        this.participantSecret = participantSecret;
    }

    /**
     * @return the participantRealtime
     */
    public Date getParticipantRealtime() {
        return participantRealtime;
    }

    /**
     * @param participantRealtime the participantRealtime to set
     */
    public void setParticipantRealtime(Date participantRealtime) {
        this.participantRealtime = participantRealtime;
    }

    /**
     * @return the identityProviderUrl
     */
    public String getIdentityProviderUrl() {
        return identityProviderUrl;
    }

    /**
     * @param identityProviderUrl the identityProviderUrl to set
     */
    public void setIdentityProviderUrl(String identityProviderUrl) {
        this.identityProviderUrl = identityProviderUrl;
    }

    /**
     * @return the clientProgram
     */
    public ProgramFragment getClientProgram() {
        return clientProgram;
    }

    /**
     * @param clientProgram the clientProgram to set
     */
    public void setClientProgram(ProgramFragment clientProgram) {
        this.clientProgram = clientProgram;
    }

    @Override
    public int size() {
        return clientProgram == null
             ? -1
             : 16 + 16 + 40 + 28 + 32 + 32 + 8 + 50 + clientProgram.size();
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        bubbleId            = in.readUUID();
        avatarId            = in.readUUID();
        bubbleName          = in.readString(40);
        locationName        = in.readString(28);
        participantId       = in.readString(32);
        participantSecret   = in.readString(32);
        participantRealtime = in.readDate();
        identityProviderUrl = in.readString(50);
        clientProgram = new ProgramFragment();
        clientProgram.deserialize(in, length - (in.counter() - counter));

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += out.put(bubbleId);
        counter += out.put(avatarId);
        counter += out.put(bubbleName, 40);
        counter += out.put(locationName, 28);
        counter += out.put(participantId, 32);
        counter += out.put(participantSecret, 32);
        counter += out.put(participantRealtime);
        counter += out.put(identityProviderUrl, 50);
        counter += clientProgram.serialize(out);

        return counter;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = super.hashCode();
        result = prime * result
                + ((avatarId == null) ? 0 : avatarId.hashCode());
        result = prime * result
                + ((bubbleId == null) ? 0 : bubbleId.hashCode());
        result = prime * result
                + ((bubbleName == null) ? 0 : bubbleName.hashCode());
        result = prime * result
                + ((clientProgram == null) ? 0 : clientProgram.hashCode());
        result = prime
                * result
                + ((identityProviderUrl == null) ? 0 : identityProviderUrl
                        .hashCode());
        result = prime * result
                + ((locationName == null) ? 0 : locationName.hashCode());
        result = prime * result
                + ((participantId == null) ? 0 : participantId.hashCode());
        result = prime
                * result
                + ((participantRealtime == null) ? 0 : participantRealtime
                        .hashCode());
        result = prime
                * result
                + ((participantSecret == null) ? 0 : participantSecret
                        .hashCode());
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
        if (!(obj instanceof JoinRequest)) {
            return false;
        }
        JoinRequest other = (JoinRequest) obj;
        if (avatarId == null) {
            if (other.avatarId != null) {
                return false;
            }
        } else if (!avatarId.equals(other.avatarId)) {
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
        if (clientProgram == null) {
            if (other.clientProgram != null) {
                return false;
            }
        } else if (!clientProgram.equals(other.clientProgram)) {
            return false;
        }
        if (identityProviderUrl == null) {
            if (other.identityProviderUrl != null) {
                return false;
            }
        } else if (!identityProviderUrl.equals(other.identityProviderUrl)) {
            return false;
        }
        if (locationName == null) {
            if (other.locationName != null) {
                return false;
            }
        } else if (!locationName.equals(other.locationName)) {
            return false;
        }
        if (participantId == null) {
            if (other.participantId != null) {
                return false;
            }
        } else if (!participantId.equals(other.participantId)) {
            return false;
        }
        if (participantRealtime == null) {
            if (other.participantRealtime != null) {
                return false;
            }
        } else if (!participantRealtime.equals(other.participantRealtime)) {
            return false;
        }
        if (participantSecret == null) {
            if (other.participantSecret != null) {
                return false;
            }
        } else if (!participantSecret.equals(other.participantSecret)) {
            return false;
        }
        return true;
    }
}
