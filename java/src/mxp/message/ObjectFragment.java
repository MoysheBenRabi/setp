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
import java.util.UUID;

/**
 * A message fragment that represents an object inside a bubble,
 * as defined by the MXP protocol.
 */
public class ObjectFragment extends MessageFragment {
    /**
     * The id of the object at hand.
     */
    private UUID objectId;

    /**
     * The index of the object in the bubble (?).
     */
    private int objectIndex;

    /**
     * The type of the object.
     */
    private UUID typeId;

    /**
     * The id of the parent object of this object.
     */
    private UUID parentObjectId;

    /**
     * The name of the object.
     */
    private String objectName;

    /**
     * The name of the type of the object (?).
     */
    private String typeName;

    /**
     * The id of the participant owning this object.
     */
    private UUID ownerId;

    /**
     * The location of the object, relative to the bubble.
     */
    private float[] location = new float[3];

    /**
     * The velocity vector of the object.
     */
    private float[] velocity = new float[3];

    /**
     * The acceleration of the object.
     */
    private float[] acceleration = new float[3];

    /**
     * The orientation of the object.
     */
    private float[] orientation = new float[4];

    /**
     * The angular velocity of the object.
     */
    private float[] angularVelocity = new float[4];

    /**
     * The angular acceleration of the object.
     */
    private float[] angularAcceleration = new float[4];

    /**
     * The radius of the smallest bounding sphere of the object.
     */
    private float boundingSphereRadius;

    /**
     * The mass of the object.
     */
    private float mass;

    /**
     * The type of extension data, if present.
     */
    private String extensionDialect;

    /**
     * The major version of the extension data.
     */
    private byte extensionDialectMajorVersion;

    /**
     * The minor version of the extension data.
     */
    private byte extensionDialectMinorVersion;

    /**
     * The extension data itself.
     */
    private byte[] extensionData = new byte[0];

    /**
     * Constructor.
     */
    public ObjectFragment() {
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

    /**
     * @return the typeId
     */
    public UUID getTypeId() {
        return typeId;
    }

    /**
     * @param typeId the typeId to set
     */
    public void setTypeId(UUID typeId) {
        this.typeId = typeId;
    }

    /**
     * @return the parentObjectId
     */
    public UUID getParentObjectId() {
        return parentObjectId;
    }

    /**
     * @param parentObjectId the parentObjectId to set
     */
    public void setParentObjectId(UUID parentObjectId) {
        this.parentObjectId = parentObjectId;
    }

    /**
     * @return the objectName
     */
    public String getObjectName() {
        return objectName;
    }

    /**
     * @param objectName the objectName to set
     */
    public void setObjectName(String objectName) {
        this.objectName = objectName;
    }

    /**
     * @return the typeName
     */
    public String getTypeName() {
        return typeName;
    }

    /**
     * @param typeName the typeName to set
     */
    public void setTypeName(String typeName) {
        this.typeName = typeName;
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
     * @return the velocity
     */
    public float[] getVelocity() {
        return velocity;
    }

    /**
     * @param velocity the velocity to set
     */
    public void setVelocity(float[] velocity) {
        this.velocity = velocity;
    }

    /**
     * @return the acceleration
     */
    public float[] getAcceleration() {
        return acceleration;
    }

    /**
     * @param acceleration the acceleration to set
     */
    public void setAcceleration(float[] acceleration) {
        this.acceleration = acceleration;
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

    /**
     * @return the angularVelocity
     */
    public float[] getAngularVelocity() {
        return angularVelocity;
    }

    /**
     * @param angularVelocity the angularVelocity to set
     */
    public void setAngularVelocity(float[] angularVelocity) {
        this.angularVelocity = angularVelocity;
    }

    /**
     * @return the angularAcceleration
     */
    public float[] getAngularAcceleration() {
        return angularAcceleration;
    }

    /**
     * @param angularAcceleration the angularAcceleration to set
     */
    public void setAngularAcceleration(float[] angularAcceleration) {
        this.angularAcceleration = angularAcceleration;
    }

    /**
     * @return the boundingSphereRadius
     */
    public float getBoundingSphereRadius() {
        return boundingSphereRadius;
    }

    /**
     * @param boundingSphereRadius the boundingSphereRadius to set
     */
    public void setBoundingSphereRadius(float boundingSphereRadius) {
        this.boundingSphereRadius = boundingSphereRadius;
    }

    /**
     * @return the mass
     */
    public float getMass() {
        return mass;
    }

    /**
     * @param mass the mass to set
     */
    public void setMass(float mass) {
        this.mass = mass;
    }

    /**
     * @return the extensionDialect
     */
    public String getExtensionDialect() {
        return extensionDialect;
    }

    /**
     * @param extensionDialect the extensionDialect to set
     */
    public void setExtensionDialect(String extensionDialect) {
        this.extensionDialect = extensionDialect;
    }

    /**
     * @return the extensionDialectMajorVersion
     */
    public byte getExtensionDialectMajorVersion() {
        return extensionDialectMajorVersion;
    }

    /**
     * @param extensionDialectMajorVersion the
     *                                     extensionDialectMajorVersion to set
     */
    public void
    setExtensionDialectMajorVersion(byte extensionDialectMajorVersion) {
        this.extensionDialectMajorVersion = extensionDialectMajorVersion;
    }

    /**
     * @return the extensionDialectMinorVersion
     */
    public byte getExtensionDialectMinorVersion() {
        return extensionDialectMinorVersion;
    }

    /**
     * @param extensionDialectMinorVersion the
     *                                     extensionDialectMinorVersion to set
     */
    public void
    setExtensionDialectMinorVersion(byte extensionDialectMinorVersion) {
        this.extensionDialectMinorVersion = extensionDialectMinorVersion;
    }

    /**
     * @return the extensionData
     */
    public byte[] getExtensionData() {
        return extensionData;
    }

    /**
     * @param extensionData the extensionData to set
     */
    public void setExtensionData(byte[] extensionData) {
        this.extensionData = extensionData;
    }

    @Override
    public int size() {
        return 16 + 4 + 16 + 16 + 20 + 20 + 16 + 12 + 12 + 12 + 16 + 16 + 16
             + 4 + 4 + 4 + 1 + 1 + 4 + extensionData.length;
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        objectId                                = in.readUUID();
        objectIndex                             = in.readInt();
        typeId                                  = in.readUUID();
        parentObjectId                          = in.readUUID();
        objectName                              = in.readString(20);
        typeName                                = in.readString(20);
        ownerId                                 = in.readUUID();
        location[0]                             = in.readFloat();
        location[1]                             = in.readFloat();
        location[2]                             = in.readFloat();
        velocity[0]                             = in.readFloat();
        velocity[1]                             = in.readFloat();
        velocity[2]                             = in.readFloat();
        acceleration[0]                         = in.readFloat();
        acceleration[1]                         = in.readFloat();
        acceleration[2]                         = in.readFloat();
        orientation[0]                          = in.readFloat();
        orientation[1]                          = in.readFloat();
        orientation[2]                          = in.readFloat();
        orientation[3]                          = in.readFloat();
        angularVelocity[0]                      = in.readFloat();
        angularVelocity[1]                      = in.readFloat();
        angularVelocity[2]                      = in.readFloat();
        angularVelocity[3]                      = in.readFloat();
        angularAcceleration[0]                  = in.readFloat();
        angularAcceleration[1]                  = in.readFloat();
        angularAcceleration[2]                  = in.readFloat();
        angularAcceleration[3]                  = in.readFloat();
        boundingSphereRadius                    = in.readFloat();
        mass                                    = in.readFloat();
        extensionDialect                        = in.readString(4);
        extensionDialectMajorVersion            = in.readByte();
        extensionDialectMinorVersion            = in.readByte();
        int extensionLength                     = in.readInt();
        extensionData = new byte[extensionLength];
        in.readBytes(extensionData);

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += out.put(objectId);
        counter += out.put(objectIndex);
        counter += out.put(typeId);
        counter += out.put(parentObjectId);
        counter += out.put(objectName, 20);
        counter += out.put(typeName, 20);
        counter += out.put(ownerId);
        counter += out.put(location[0]);
        counter += out.put(location[1]);
        counter += out.put(location[2]);
        counter += out.put(velocity[0]);
        counter += out.put(velocity[1]);
        counter += out.put(velocity[2]);
        counter += out.put(acceleration[0]);
        counter += out.put(acceleration[1]);
        counter += out.put(acceleration[2]);
        counter += out.put(orientation[0]);
        counter += out.put(orientation[1]);
        counter += out.put(orientation[2]);
        counter += out.put(orientation[3]);
        counter += out.put(angularVelocity[0]);
        counter += out.put(angularVelocity[1]);
        counter += out.put(angularVelocity[2]);
        counter += out.put(angularVelocity[3]);
        counter += out.put(angularAcceleration[0]);
        counter += out.put(angularAcceleration[1]);
        counter += out.put(angularAcceleration[2]);
        counter += out.put(angularAcceleration[3]);
        counter += out.put(boundingSphereRadius);
        counter += out.put(mass);
        counter += out.put(extensionDialect, 4);
        counter += out.put(extensionDialectMajorVersion);
        counter += out.put(extensionDialectMinorVersion);
        counter += out.put(extensionData.length);
        counter += out.put(extensionData);

        return counter;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = 1;
        result = prime * result + Arrays.hashCode(acceleration);
        result = prime * result + Arrays.hashCode(angularAcceleration);
        result = prime * result + Arrays.hashCode(angularVelocity);
        result = prime * result + Float.floatToIntBits(boundingSphereRadius);
        result = prime * result + Arrays.hashCode(extensionData);
        result = prime
                * result
               + ((extensionDialect == null) ? 0 : extensionDialect.hashCode());
        result = prime * result + extensionDialectMajorVersion;
        result = prime * result + extensionDialectMinorVersion;
        result = prime * result + Arrays.hashCode(location);
        result = prime * result + Float.floatToIntBits(mass);
        result = prime * result
                + ((objectId == null) ? 0 : objectId.hashCode());
        result = prime * result + objectIndex;
        result = prime * result
                + ((objectName == null) ? 0 : objectName.hashCode());
        result = prime * result + Arrays.hashCode(orientation);
        result = prime * result + ((ownerId == null) ? 0 : ownerId.hashCode());
        result = prime * result
                + ((parentObjectId == null) ? 0 : parentObjectId.hashCode());
        result = prime * result + ((typeId == null) ? 0 : typeId.hashCode());
        result = prime * result
                + ((typeName == null) ? 0 : typeName.hashCode());
        result = prime * result + Arrays.hashCode(velocity);
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
        if (!(obj instanceof ObjectFragment)) {
            return false;
        }
        ObjectFragment other = (ObjectFragment) obj;
        if (!Arrays.equals(acceleration, other.acceleration)) {
            return false;
        }
        if (!Arrays.equals(angularAcceleration, other.angularAcceleration)) {
            return false;
        }
        if (!Arrays.equals(angularVelocity, other.angularVelocity)) {
            return false;
        }
        if (Float.floatToIntBits(boundingSphereRadius) != Float
                .floatToIntBits(other.boundingSphereRadius)) {
            return false;
        }
        if (!Arrays.equals(extensionData, other.extensionData)) {
            return false;
        }
        if (extensionDialect == null) {
            if (other.extensionDialect != null) {
                return false;
            }
        } else if (!extensionDialect.equals(other.extensionDialect)) {
            return false;
        }
        if (extensionDialectMajorVersion
                                      != other.extensionDialectMajorVersion) {
            return false;
        }
        if (extensionDialectMinorVersion
                                        != other.extensionDialectMinorVersion) {
            return false;
        }
        if (!Arrays.equals(location, other.location)) {
            return false;
        }
        if (Float.floatToIntBits(mass) != Float.floatToIntBits(other.mass)) {
            return false;
        }
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
        if (objectName == null) {
            if (other.objectName != null) {
                return false;
            }
        } else if (!objectName.equals(other.objectName)) {
            return false;
        }
        if (!Arrays.equals(orientation, other.orientation)) {
            return false;
        }
        if (ownerId == null) {
            if (other.ownerId != null) {
                return false;
            }
        } else if (!ownerId.equals(other.ownerId)) {
            return false;
        }
        if (parentObjectId == null) {
            if (other.parentObjectId != null) {
                return false;
            }
        } else if (!parentObjectId.equals(other.parentObjectId)) {
            return false;
        }
        if (typeId == null) {
            if (other.typeId != null) {
                return false;
            }
        } else if (!typeId.equals(other.typeId)) {
            return false;
        }
        if (typeName == null) {
            if (other.typeName != null) {
                return false;
            }
        } else if (!typeName.equals(other.typeName)) {
            return false;
        }
        if (!Arrays.equals(velocity, other.velocity)) {
            return false;
        }
        return true;
    }

}
