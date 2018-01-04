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
 * An interaction request message, as specified by the MXP protocol.
 */
public class InteractRequest extends Message {
    /**
     * The interaction requested.
     */
    private InteractionFragment request = new InteractionFragment();

    /**
     * Constructor.
     */
    public InteractRequest() {
        super(Message.Type.INTERACT_REQUEST);
    }

    /**
     * @return the interaction
     */
    public InteractionFragment getRequest() {
        return request;
    }

    /**
     * @param request the request to set
     */
    public void setRequest(InteractionFragment request) {
        this.request = request;
    }

    @Override
    public int size() {
        return request == null
             ? -1
             : request.size();
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        request = new InteractionFragment();
        request.deserialize(in, length);

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += request.serialize(out);

        return counter;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = super.hashCode();
        result = prime * result
                + ((request == null) ? 0 : request.hashCode());
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
        if (!(obj instanceof InteractRequest)) {
            return false;
        }
        InteractRequest other = (InteractRequest) obj;
        if (request == null) {
            if (other.request != null) {
                return false;
            }
        } else if (!request.equals(other.request)) {
            return false;
        }
        return true;
    }
}
