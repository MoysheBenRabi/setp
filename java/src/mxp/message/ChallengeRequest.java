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
 * A ChallengeRequest message, as specified by the MXP protocol.
 */
public class ChallengeRequest extends Message {
    /**
     * The challenge request data.
     */
    private byte[] challengeRequestData = new byte[64];

    /**
     * Constructor.
     */
    public ChallengeRequest() {
        super(Message.Type.CHALLENGE_REQUEST);
    }

    /**
     * @return the challengeRequestData
     */
    public byte[] getChallengeRequestData() {
        return challengeRequestData;
    }

    /**
     * Set the challenge request data. only arrays of 64 bytes are
     * accepted.
     *
     * @param newChallengeRequestData the new ChallengeRequestData to set
     */
    public void setChallengeRequestData(byte[] newChallengeRequestData) {
        if (newChallengeRequestData.length != 64) {
            throw new IllegalArgumentException();
        }

        challengeRequestData = newChallengeRequestData;
    }

    @Override
    public int size() {
        return 64;
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        if (length < size()) {
            throw new IOException();
        }

        int counter = in.counter();
        challengeRequestData = new byte[64];

        if (in.read(challengeRequestData) != 64) {
            throw new IOException();
        }

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int counter = 0;

        counter += out.put(challengeRequestData);

        return counter;
    }

    @Override
    public int hashCode() {
        return Arrays.hashCode(challengeRequestData);
    }

    @Override
    public boolean equals(Object other) {
        if (other == null || !(other instanceof ChallengeRequest)) {
            return false;
        }

        ChallengeRequest cr = (ChallengeRequest) other;

        return Arrays.equals(challengeRequestData, cr.challengeRequestData);
    }
}
