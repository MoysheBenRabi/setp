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
import java.util.Arrays;
import java.util.Date;
import java.util.UUID;

/**
 * A message fragment describing a bubble,
 * as described by the MXP protocol.
 */
public class BubbleFragment extends MessageFragment {
    /**
     * The id of the bubble.
     */
    private UUID bubbleId;

    /**
     * The name of the bubble.
     */
    private String bubbleName;

    /**
     * The asset cache URL of the bubble.
     */
    private String bubbleAssetCacheUrl;

    /**
     * The owner of the bubble.
     */
    private UUID ownerId;

    /**
     * The address of the bubble - a domain name or IP address.
     */
    private String bubbleAddress;

    /**
     * The port of the bubble.
     */
    private int bubblePort;

    /**
     * The center of the bubble in the simulated space, as a 3d coordinate.
     */
    private float[] bubbleCenter = new float[3];

    /**
     * The range (size?) of the bubble.
     */
    private float bubbleRange;

    /**
     * The perception range of the bubble (?).
     */
    private float bubblePerceptionRange;

    /**
     * The current real time at the bubble.
     */
    private Date bubbleRealtime;

    /**
     * Constructor.
     */
    public BubbleFragment() {
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
     * @return the ownerId
     */
    public UUID getOwnerId() {
        return ownerId;
    }

    /**
     * @param ownerId the ownerId to set
     */
    public void setOwnerId(UUID ownerId) {
        this.ownerId = ownerId;
    }

    /**
     * @return the bubbleAddress
     */
    public String getBubbleAddress() {
        return bubbleAddress;
    }

    /**
     * @param bubbleAddress the bubbleAddress to set
     */
    public void setBubbleAddress(String bubbleAddress) {
        this.bubbleAddress = bubbleAddress;
    }

    /**
     * @return the bubblePort
     */
    public int getBubblePort() {
        return bubblePort;
    }

    /**
     * @param bubblePort the bubblePort to set
     */
    public void setBubblePort(int bubblePort) {
        this.bubblePort = bubblePort;
    }

    /**
     * @return the bubbleCenter
     */
    public float[] getBubbleCenter() {
        return bubbleCenter;
    }

    /**
     * @param bubbleCenter the bubbleCenter to set
     */
    public void setBubbleCenter(float[] bubbleCenter) {
        this.bubbleCenter = bubbleCenter;
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

    @Override
    public int size() {
        return 16 + 40 + 51 + 16 + 40 + 4 + 12 + 4 + 4 + 8;
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        bubbleId                = in.readUUID();
        bubbleName              = in.readString(40);
        bubbleAssetCacheUrl     = in.readString(51);
        ownerId                 = in.readUUID();
        bubbleAddress           = in.readString(40);
        bubblePort              = in.readInt();
        bubbleCenter[0]         = in.readFloat();
        bubbleCenter[1]         = in.readFloat();
        bubbleCenter[2]         = in.readFloat();
        bubbleRange             = in.readFloat();
        bubblePerceptionRange   = in.readFloat();
        bubbleRealtime          = in.readDate();

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += out.put(bubbleId);
        counter += out.put(bubbleName, 40);
        counter += out.put(bubbleAssetCacheUrl, 51);
        counter += out.put(ownerId);
        counter += out.put(bubbleAddress, 40);
        counter += out.put(bubblePort);
        counter += out.put(bubbleCenter[0]);
        counter += out.put(bubbleCenter[1]);
        counter += out.put(bubbleCenter[2]);
        counter += out.put(bubbleRange);
        counter += out.put(bubblePerceptionRange);
        counter += out.put(bubbleRealtime);

        return counter;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = 1;
        result = prime * result
                + ((bubbleAddress == null) ? 0 : bubbleAddress.hashCode());
        result = prime
                * result
                + ((bubbleAssetCacheUrl == null) ? 0 : bubbleAssetCacheUrl
                        .hashCode());
        result = prime * result + Arrays.hashCode(bubbleCenter);
        result = prime * result
                + ((bubbleId == null) ? 0 : bubbleId.hashCode());
        result = prime * result
                + ((bubbleName == null) ? 0 : bubbleName.hashCode());
        result = prime * result + Float.floatToIntBits(bubblePerceptionRange);
        result = prime * result + bubblePort;
        result = prime * result + Float.floatToIntBits(bubbleRange);
        result = prime * result
                + ((bubbleRealtime == null) ? 0 : bubbleRealtime.hashCode());
        result = prime * result + ((ownerId == null) ? 0 : ownerId.hashCode());
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
        if (!(obj instanceof BubbleFragment)) {
            return false;
        }
        BubbleFragment other = (BubbleFragment) obj;
        if (bubbleAddress == null) {
            if (other.bubbleAddress != null) {
                return false;
            }
        } else if (!bubbleAddress.equals(other.bubbleAddress)) {
            return false;
        }
        if (bubbleAssetCacheUrl == null) {
            if (other.bubbleAssetCacheUrl != null) {
                return false;
            }
        } else if (!bubbleAssetCacheUrl.equals(other.bubbleAssetCacheUrl)) {
            return false;
        }
        if (!Arrays.equals(bubbleCenter, other.bubbleCenter)) {
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
        if (bubblePort != other.bubblePort) {
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
        if (ownerId == null) {
            if (other.ownerId != null) {
                return false;
            }
        } else if (!ownerId.equals(other.ownerId)) {
            return false;
        }
        return true;
    }

}
