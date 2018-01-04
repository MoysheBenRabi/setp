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
 * An action event message, as specified by the MXP protocol.
 */
public class ActionEvent extends Message {
    /**
     * The name of the action.
     */
    private String actionName;

    /**
     * The source object for the action.
     */
    private UUID sourceObjectId;

    /**
     * The observation radius for the action.
     */
    private float observationRadius;

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
    public ActionEvent() {
        super(Message.Type.ACTION_EVENT);
    }

    /**
     * @return the actionName
     */
    public String getActionName() {
        return actionName;
    }

    /**
     * @param actionName the actionName to set
     */
    public void setActionName(String actionName) {
        this.actionName = actionName;
    }

    /**
     * @return the sourceObjectId
     */
    public UUID getSourceObjectId() {
        return sourceObjectId;
    }

    /**
     * @param sourceObjectId the sourceObjectId to set
     */
    public void setSourceObjectId(UUID sourceObjectId) {
        this.sourceObjectId = sourceObjectId;
    }

    /**
     * @return the observationRadius
     */
    public float getObservationRadius() {
        return observationRadius;
    }

    /**
     * @param observationRadius the observationRadius to set
     */
    public void setObservationRadius(float observationRadius) {
        this.observationRadius = observationRadius;
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
     * @param extensionDialectMajorVersion
     *        the extensionDialectMajorVersion to set
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
     * @param extensionDialectMinorVersion
     *        the extensionDialectMinorVersion to set
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
        return extensionData == null
             ? -1
             : 20 + 16 + 4 + 4 + 1 + 1 + 4 + extensionData.length;
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        actionName                   = in.readString(20);
        sourceObjectId               = in.readUUID();
        observationRadius            = in.readFloat();
        extensionDialect             = in.readString(4);
        extensionDialectMajorVersion = in.readByte();
        extensionDialectMinorVersion = in.readByte();
        int extensionLength          = in.readInt();
        extensionData                = new byte[extensionLength];
        in.readBytes(extensionData);

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += out.put(actionName, 20);
        counter += out.put(sourceObjectId);
        counter += out.put(observationRadius);
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
        int result = super.hashCode();
        result = prime * result
                + ((actionName == null) ? 0 : actionName.hashCode());
        result = prime * result + Arrays.hashCode(extensionData);
        result = prime
                * result
                + ((extensionDialect == null) ? 0
                                              : extensionDialect.hashCode());
        result = prime * result + extensionDialectMajorVersion;
        result = prime * result + extensionDialectMinorVersion;
        result = prime * result + Float.floatToIntBits(observationRadius);
        result = prime * result
                + ((sourceObjectId == null) ? 0 : sourceObjectId.hashCode());
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
        if (!(obj instanceof ActionEvent)) {
            return false;
        }
        ActionEvent other = (ActionEvent) obj;
        if (actionName == null) {
            if (other.actionName != null) {
                return false;
            }
        } else if (!actionName.equals(other.actionName)) {
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
        if (Float.floatToIntBits(observationRadius) != Float
                .floatToIntBits(other.observationRadius)) {
            return false;
        }
        if (sourceObjectId == null) {
            if (other.sourceObjectId != null) {
                return false;
            }
        } else if (!sourceObjectId.equals(other.sourceObjectId)) {
            return false;
        }
        return true;
    }

}
