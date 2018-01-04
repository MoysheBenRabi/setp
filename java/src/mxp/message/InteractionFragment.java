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
 * A message fragment describing an 'interaction' event,
 * as specified by the MXP communication protocol.
 */
public class InteractionFragment extends MessageFragment {
    /**
     * The name of the interaction happening.
     */
    private String interactionName;

    /**
     * The source (initiator) of the interaction.
     */
    private UUID sourceParticipantId;

    /**
     * The source object of the interaction.
     */
    private UUID sourceObjectId;

    /**
     * The target participant of the interaction.
     */
    private UUID targetParticipantId;

    /**
     * The target object of the interaction.
     */
    private UUID targetObjectId;

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
    public InteractionFragment() {
    }

    /**
     * @return the interactionName
     */
    public String getInteractionName() {
        return interactionName;
    }

    /**
     * @param interactionName the interactionName to set
     */
    public void setInteractionName(String interactionName) {
        this.interactionName = interactionName;
    }

    /**
     * @return the sourceParticipantId
     */
    public UUID getSourceParticipantId() {
        return sourceParticipantId;
    }

    /**
     * @param sourceParticipantId the sourceParticipantId to set
     */
    public void setSourceParticipantId(UUID sourceParticipantId) {
        this.sourceParticipantId = sourceParticipantId;
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
     * @return the targetParticipantId
     */
    public UUID getTargetParticipantId() {
        return targetParticipantId;
    }

    /**
     * @param targetParticipantId the targetParticipantId to set
     */
    public void setTargetParticipantId(UUID targetParticipantId) {
        this.targetParticipantId = targetParticipantId;
    }

    /**
     * @return the targetObjectId
     */
    public UUID getTargetObjectId() {
        return targetObjectId;
    }

    /**
     * @param targetObjectId the targetObjectId to set
     */
    public void setTargetObjectId(UUID targetObjectId) {
        this.targetObjectId = targetObjectId;
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
     *        extensionDialectMajorVersion to set
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
     *        extensionDialectMinorVersion to set
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
        return 20 + 16 + 16 + 16 + 16 + 4 + 1 + 1 + 4 + extensionData.length;
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        interactionName              = in.readString(20);
        sourceParticipantId          = in.readUUID();
        sourceObjectId               = in.readUUID();
        targetParticipantId          = in.readUUID();
        targetObjectId               = in.readUUID();
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

        counter += out.put(interactionName, 20);
        counter += out.put(sourceParticipantId);
        counter += out.put(sourceObjectId);
        counter += out.put(targetParticipantId);
        counter += out.put(targetObjectId);
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
        result = prime * result + Arrays.hashCode(extensionData);
        result = prime
                * result
                + ((extensionDialect == null) ? 0
                                              : extensionDialect.hashCode());
        result = prime * result + extensionDialectMajorVersion;
        result = prime * result + extensionDialectMinorVersion;
        result = prime * result
                + ((interactionName == null) ? 0 : interactionName.hashCode());
        result = prime * result
                + ((sourceObjectId == null) ? 0 : sourceObjectId.hashCode());
        result = prime
                * result
                + ((sourceParticipantId == null) ? 0 : sourceParticipantId
                        .hashCode());
        result = prime * result
                + ((targetObjectId == null) ? 0 : targetObjectId.hashCode());
        result = prime
                * result
                + ((targetParticipantId == null) ? 0 : targetParticipantId
                        .hashCode());
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
        if (!(obj instanceof InteractionFragment)) {
            return false;
        }
        InteractionFragment other = (InteractionFragment) obj;
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
        if (interactionName == null) {
            if (other.interactionName != null) {
                return false;
            }
        } else if (!interactionName.equals(other.interactionName)) {
            return false;
        }
        if (sourceObjectId == null) {
            if (other.sourceObjectId != null) {
                return false;
            }
        } else if (!sourceObjectId.equals(other.sourceObjectId)) {
            return false;
        }
        if (sourceParticipantId == null) {
            if (other.sourceParticipantId != null) {
                return false;
            }
        } else if (!sourceParticipantId.equals(other.sourceParticipantId)) {
            return false;
        }
        if (targetObjectId == null) {
            if (other.targetObjectId != null) {
                return false;
            }
        } else if (!targetObjectId.equals(other.targetObjectId)) {
            return false;
        }
        if (targetParticipantId == null) {
            if (other.targetParticipantId != null) {
                return false;
            }
        } else if (!targetParticipantId.equals(other.targetParticipantId)) {
            return false;
        }
        return true;
    }
}
