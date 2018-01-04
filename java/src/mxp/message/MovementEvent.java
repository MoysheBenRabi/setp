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

/**
 * A movement event message, as specified by the MXP protocol.
 */
public class MovementEvent extends Message {
    /**
     * The index of the object that disappeared.
     */
    private int objectIndex;

    /**
     * The new location of the object.
     */
    private float[] location = new float[3];

    /**
     * The new orientation of the object.
     */
    private float[] orientation = new float[4];

    /**
     * Constructor.
     */
    public MovementEvent() {
        super(Message.Type.MOVEMENT_EVENT);
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

    /**
     * @return the location
     */
    public float[] getLocation() {
        return location;
    }

    /**
     * @param location the location to set
     */
    public void setLocation(float[] location) {
        this.location = location;
    }

    /**
     * @return the orientation
     */
    public float[] getOrientation() {
        return orientation;
    }

    /**
     * @param orientation the orientation to set
     */
    public void setOrientation(float[] orientation) {
        this.orientation = orientation;
    }

    @Override
    public int size() {
        return 4 + 12 + 16;
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        objectIndex    = in.readInt();
        location[0]    = in.readFloat();
        location[1]    = in.readFloat();
        location[2]    = in.readFloat();
        orientation[0] = in.readFloat();
        orientation[1] = in.readFloat();
        orientation[2] = in.readFloat();
        orientation[3] = in.readFloat();

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += out.put(objectIndex);
        counter += out.put(location[0]);
        counter += out.put(location[1]);
        counter += out.put(location[2]);
        counter += out.put(orientation[0]);
        counter += out.put(orientation[1]);
        counter += out.put(orientation[2]);
        counter += out.put(orientation[3]);

        return counter;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = super.hashCode();
        result = prime * result + Arrays.hashCode(location);
        result = prime * result + objectIndex;
        result = prime * result + Arrays.hashCode(orientation);
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
        if (!(obj instanceof MovementEvent)) {
            return false;
        }
        MovementEvent other = (MovementEvent) obj;
        if (!Arrays.equals(location, other.location)) {
            return false;
        }
        if (objectIndex != other.objectIndex) {
            return false;
        }
        if (!Arrays.equals(orientation, other.orientation)) {
            return false;
        }
        return true;
    }
}
