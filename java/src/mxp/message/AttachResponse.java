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

/**
 * An bubble attach response message, as specified by the MXP protocol.
 */
public class AttachResponse extends Message {
    /**
     * The response header.
     */
    private ResponseFragment responseHeader = new ResponseFragment();

    /**
     * The bubble to attach.
     */
    private BubbleFragment  targetBubble = new BubbleFragment();

    /**
     * The bubble server, serving the source bubble.
     */
    private ProgramFragment targetBubbleServer = new ProgramFragment();

    /**
     * Constructor.
     */
    public AttachResponse() {
        super(Message.Type.ATTACH_RESPONSE);
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
     * @return the targetBubble
     */
    public BubbleFragment getTargetBubble() {
        return targetBubble;
    }

    /**
     * @param targetBubble the targetBubble to set
     */
    public void setTargetBubble(BubbleFragment targetBubble) {
        this.targetBubble = targetBubble;
    }

    /**
     * @return the targetBubbleServer
     */
    public ProgramFragment getTargetBubbleServer() {
        return targetBubbleServer;
    }

    /**
     * @param targetBubbleServer the targetBubbleServer to set
     */
    public void setTargetBubbleServer(ProgramFragment targetBubbleServer) {
        this.targetBubbleServer = targetBubbleServer;
    }

    @Override
    public int size() {
        return responseHeader == null || targetBubble == null
                                      || targetBubbleServer == null
             ? -1
             : responseHeader.size() + targetBubble.size()
                                     + targetBubbleServer.size();
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        responseHeader = new ResponseFragment();
        responseHeader.deserialize(in, length);
        targetBubble = new BubbleFragment();
        targetBubble.deserialize(in, length - (in.counter() - counter));
        targetBubbleServer = new ProgramFragment();
        targetBubbleServer.deserialize(in, length - (in.counter() - counter));

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += responseHeader.serialize(out);
        counter += targetBubble.serialize(out);
        counter += targetBubbleServer.serialize(out);

        return counter;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = super.hashCode();
        result = prime * result
                + ((responseHeader == null) ? 0 : responseHeader.hashCode());
        result = prime * result
                + ((targetBubble == null) ? 0 : targetBubble.hashCode());
        result = prime
                * result
                + ((targetBubbleServer == null) ? 0 : targetBubbleServer
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
        if (!(obj instanceof AttachResponse)) {
            return false;
        }
        AttachResponse other = (AttachResponse) obj;
        if (responseHeader == null) {
            if (other.responseHeader != null) {
                return false;
            }
        } else if (!responseHeader.equals(other.responseHeader)) {
            return false;
        }
        if (targetBubble == null) {
            if (other.targetBubble != null) {
                return false;
            }
        } else if (!targetBubble.equals(other.targetBubble)) {
            return false;
        }
        if (targetBubbleServer == null) {
            if (other.targetBubbleServer != null) {
                return false;
            }
        } else if (!targetBubbleServer.equals(other.targetBubbleServer)) {
            return false;
        }
        return true;
    }

 }
