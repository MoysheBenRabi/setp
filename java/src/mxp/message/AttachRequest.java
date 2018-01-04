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
import java.util.UUID;

/**
 * An bubble attach request message, as specified by the MXP protocol.
 */
public class AttachRequest extends Message {
    /**
     * The bubble id - which bubble to attach to (?).
     */
    private UUID targetBubbleId;

    /**
     * The bubble to attach.
     */
    private BubbleFragment  sourceBubble = new BubbleFragment();

    /**
     * The bubble server, serving the source bubble.
     */
    private ProgramFragment sourceBubbleServer = new ProgramFragment();

    /**
     * Constructor.
     */
    public AttachRequest() {
        super(Message.Type.ATTACH_REQUEST);
    }

    /**
     * @return the targetBubbleId
     */
    public UUID getTargetBubbleId() {
        return targetBubbleId;
    }

    /**
     * @param targetBubbleId the targetBubbleId to set
     */
    public void setTargetBubbleId(UUID targetBubbleId) {
        this.targetBubbleId = targetBubbleId;
    }

    /**
     * @return the sourceBubble
     */
    public BubbleFragment getSourceBubble() {
        return sourceBubble;
    }

    /**
     * @param sourceBubble the sourceBubble to set
     */
    public void setSourceBubble(BubbleFragment sourceBubble) {
        this.sourceBubble = sourceBubble;
    }

    /**
     * @return the sourceBubbleServer
     */
    public ProgramFragment getSourceBubbleServer() {
        return sourceBubbleServer;
    }

    /**
     * @param sourceBubbleServer the sourceBubbleServer to set
     */
    public void setSourceBubbleServer(ProgramFragment sourceBubbleServer) {
        this.sourceBubbleServer = sourceBubbleServer;
    }

    @Override
    public int size() {
        return sourceBubble == null || sourceBubbleServer == null
             ? -1
             : 16 + sourceBubble.size() + sourceBubbleServer.size();
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        targetBubbleId = in.readUUID();
        sourceBubble = new BubbleFragment();
        sourceBubble.deserialize(in, length - (in.counter() - counter));
        sourceBubbleServer = new ProgramFragment();
        sourceBubbleServer.deserialize(in, length - (in.counter() - counter));

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += out.put(targetBubbleId);
        counter += sourceBubble.serialize(out);
        counter += sourceBubbleServer.serialize(out);

        return counter;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = super.hashCode();
        result = prime * result
                + ((sourceBubble == null) ? 0 : sourceBubble.hashCode());
        result = prime
                * result
                + ((sourceBubbleServer == null) ? 0 : sourceBubbleServer
                        .hashCode());
        result = prime * result
                + ((targetBubbleId == null) ? 0 : targetBubbleId.hashCode());
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
        if (!(obj instanceof AttachRequest)) {
            return false;
        }
        AttachRequest other = (AttachRequest) obj;
        if (sourceBubble == null) {
            if (other.sourceBubble != null) {
                return false;
            }
        } else if (!sourceBubble.equals(other.sourceBubble)) {
            return false;
        }
        if (sourceBubbleServer == null) {
            if (other.sourceBubbleServer != null) {
                return false;
            }
        } else if (!sourceBubbleServer.equals(other.sourceBubbleServer)) {
            return false;
        }
        if (targetBubbleId == null) {
            if (other.targetBubbleId != null) {
                return false;
            }
        } else if (!targetBubbleId.equals(other.targetBubbleId)) {
            return false;
        }
        return true;
    }

}
