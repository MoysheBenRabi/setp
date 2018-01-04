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
 * A message fragment describing a 'program' as 'a piece of software'
 * that is on either side of an MXP communication.
 */
public class ProgramFragment extends MessageFragment {
    /**
     * The name of the program. (at most 25 bytes encoded)
     */
    private String programName;

    /**
     * Program major version.
     */
    private byte programMajorVersion;

    /**
     * Program minor version.
     */
    private byte programMinorVersion;

    /**
     * The MXP major version this program speaks.
     */
    private byte protocolMajorVersion;

    /**
     * The MXP minor version this program speaks.
     */
    private byte protocolMinorVersion;

    /**
     * The MXP source revision this program speaks.
     */
    private int protocolSourceRevision;

    /**
     * Constructor.
     */
    public ProgramFragment() {
    }

    /**
     * @return the programName
     */
    public String getProgramName() {
        return programName;
    }

    /**
     * @param programName the programName to set
     */
    public void setProgramName(String programName) {
        this.programName = programName;
    }

    /**
     * @return the programMajorVersion
     */
    public byte getProgramMajorVersion() {
        return programMajorVersion;
    }

    /**
     * @param programMajorVersion the programMajorVersion to set
     */
    public void setProgramMajorVersion(byte programMajorVersion) {
        this.programMajorVersion = programMajorVersion;
    }

    /**
     * @return the programMinorVersion
     */
    public byte getProgramMinorVersion() {
        return programMinorVersion;
    }

    /**
     * @param programMinorVersion the programMinorVersion to set
     */
    public void setProgramMinorVersion(byte programMinorVersion) {
        this.programMinorVersion = programMinorVersion;
    }

    /**
     * @return the protocolMajorVersion
     */
    public byte getProtocolMajorVersion() {
        return protocolMajorVersion;
    }

    /**
     * @param protocolMajorVersion the protocolMajorVersion to set
     */
    public void setProtocolMajorVersion(byte protocolMajorVersion) {
        this.protocolMajorVersion = protocolMajorVersion;
    }

    /**
     * @return the protocolMinorVersion
     */
    public byte getProtocolMinorVersion() {
        return protocolMinorVersion;
    }

    /**
     * @param protocolMinorVersion the protocolMinorVersion to set
     */
    public void setProtocolMinorVersion(byte protocolMinorVersion) {
        this.protocolMinorVersion = protocolMinorVersion;
    }

    /**
     * @return the protocolSourceRevision
     */
    public int getProtocolSourceRevision() {
        return protocolSourceRevision;
    }

    /**
     * @param protocolSourceRevision the protocolSourceRevision to set
     */
    public void setProtocolSourceRevision(int protocolSourceRevision) {
        this.protocolSourceRevision = protocolSourceRevision;
    }

    @Override
    public int size() {
        return 25 + 1 + 1 + 1 + 1 + 4;
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();

        programName            = in.readString(25);
        programMajorVersion    = in.readByte();
        programMinorVersion    = in.readByte();
        protocolMajorVersion   = in.readByte();
        protocolMinorVersion   = in.readByte();
        protocolSourceRevision = in.readInt();

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += out.put(programName, 25);
        counter += out.put(programMajorVersion);
        counter += out.put(programMinorVersion);
        counter += out.put(protocolMajorVersion);
        counter += out.put(protocolMinorVersion);
        counter += out.put(protocolSourceRevision);

        return counter;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = 1;
        result = prime * result + programMajorVersion;
        result = prime * result + programMinorVersion;
        result = prime * result
                + ((programName == null) ? 0 : programName.hashCode());
        result = prime * result + protocolMajorVersion;
        result = prime * result + protocolMinorVersion;
        result = prime * result + protocolSourceRevision;
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
        if (!(obj instanceof ProgramFragment)) {
            return false;
        }
        ProgramFragment other = (ProgramFragment) obj;
        if (programMajorVersion != other.programMajorVersion) {
            return false;
        }
        if (programMinorVersion != other.programMinorVersion) {
            return false;
        }
        if (programName == null) {
            if (other.programName != null) {
                return false;
            }
        } else if (!programName.equals(other.programName)) {
            return false;
        }
        if (protocolMajorVersion != other.protocolMajorVersion) {
            return false;
        }
        if (protocolMinorVersion != other.protocolMinorVersion) {
            return false;
        }
        if (protocolSourceRevision != other.protocolSourceRevision) {
            return false;
        }
        return true;
    }
}
