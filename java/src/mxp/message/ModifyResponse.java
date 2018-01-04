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
 * A modify response message, as specified by the MXP protocol.
 * This message only contains a response fragment.
 */
public class ModifyResponse extends Message {
    /**
     * The response header.
     */
    private ResponseFragment responseHeader = new ResponseFragment();

    /**
     * Constructor.
     */
    public ModifyResponse() {
        super(Message.Type.MODIFY_RESPONSE);
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

    @Override
    public int size() {
        return responseHeader == null
             ? -1
             : responseHeader.size();
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

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += responseHeader.serialize(out);

        return counter;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = super.hashCode();
        result = prime * result
                + ((responseHeader == null) ? 0 : responseHeader.hashCode());
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
        if (!(obj instanceof ModifyResponse)) {
            return false;
        }
        ModifyResponse other = (ModifyResponse) obj;
        if (responseHeader == null) {
            if (other.responseHeader != null) {
                return false;
            }
        } else if (!responseHeader.equals(other.responseHeader)) {
            return false;
        }
        return true;
    }
}
