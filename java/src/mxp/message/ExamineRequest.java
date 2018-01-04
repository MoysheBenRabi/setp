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
 * An examination request message, as specified by the MXP protocol.
 */
public class ExamineRequest extends Message {
    /**
     * The object id to be examined. This can be an all-zero UUID as well,
     * then the object index is to be examined.
     */
    private UUID objectId;

    /**
     * The object index to examine. This can be set to 0, in which case the
     * supplied object id is to be examined.
     */
    private int objectIndex;

    /**
     * Constructor.
     */
    public ExamineRequest() {
        super(Message.Type.EXAMINE_REQUEST);
    }

    /**
     * @return the objectId
     */
    public UUID getObjectId() {
        return objectId;
    }

    /**
     * @param objectId the objectId to set
     */
    public void setObjectId(UUID objectId) {
        this.objectId = objectId;
    }

    /**
     * @return the objectIndex
     */
    public int getObjectIndex() {
        return objectIndex;
    }

    /**
     * @param objectIndex the objectIndex to set
     */
    public void setObjectIndex(int objectIndex) {
        this.objectIndex = objectIndex;
    }

    @Override
    public int size() {
        return 16 + 4;
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        objectId    = in.readUUID();
        objectIndex = in.readInt();

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += out.put(objectId);
        counter += out.put(objectIndex);

        return counter;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = super.hashCode();
        result = prime * result
                + ((objectId == null) ? 0 : objectId.hashCode());
        result = prime * result + objectIndex;
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
        if (!(obj instanceof ExamineRequest)) {
            return false;
        }
        ExamineRequest other = (ExamineRequest) obj;
        if (objectId == null) {
            if (other.objectId != null) {
                return false;
            }
        } else if (!objectId.equals(other.objectId)) {
            return false;
        }
        if (objectIndex != other.objectIndex) {
            return false;
        }
        return true;
    }
}
