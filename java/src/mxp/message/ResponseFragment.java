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
 * A message fragment that is a generic 'response header',
 * as defined by the MXP protocol.
 */
public class ResponseFragment extends MessageFragment {
    /**
     * The request message ID this is a response to.
     */
    private int requestMessageId;

    /**
     * The failure code, 0 == success.
     */
    private byte failureCode;

    /**
     * Constructor.
     */
    public ResponseFragment() {
    }

    /**
     * @return the requestMessageId
     */
    public int getRequestMessageId() {
        return requestMessageId;
    }

    /**
     * @param requestMessageId the requestMessageId to set
     */
    public void setRequestMessageId(int requestMessageId) {
        this.requestMessageId = requestMessageId;
    }

    /**
     * @return the failureCode
     */
    public byte getFailureCode() {
        return failureCode;
    }

    /**
     * @param failureCode the failureCode to set
     */
    public void setFailureCode(byte failureCode) {
        this.failureCode = failureCode;
    }

    @Override
    public int size() {
        return 4 + 1;
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        requestMessageId       = in.readInt();
        failureCode            = in.readByte();

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += out.put(requestMessageId);
        counter += out.put(failureCode);

        return counter;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = 1;
        result = prime * result + failureCode;
        result = prime * result + requestMessageId;
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
        if (!(obj instanceof ResponseFragment)) {
            return false;
        }
        ResponseFragment other = (ResponseFragment) obj;
        if (failureCode != other.failureCode) {
            return false;
        }
        if (requestMessageId != other.requestMessageId) {
            return false;
        }
        return true;
    }
}
