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
import java.util.Vector;

/**
 * An Acknowledge message, as specified by the MXP protocol.
 * This message is capable of holding up to 64 message acknowledgements.
 */
public class Acknowledge extends Message {
    /**
     * The maximum number of packet ids an acknowledge message may contain.
     */
    public static final int MAX_PACKET_IDS = 64;

    /**
     * The packet ids that are acknowledged to be received by this package.
     */
    private Vector<Integer>     packetIds = new Vector<Integer>();

    /**
     * Constructor.
     */
    public Acknowledge() {
        super(Message.Type.ACKNOWLEDGE);
    }

    /**
     * @return the packetIds
     */
    public Vector<Integer> getPacketIds() {
        return packetIds;
    }

    /**
     * @param packetIds the packetIds to set
     */
    public void setPacketIds(Vector<Integer> packetIds) {
        if (packetIds.size() > MAX_PACKET_IDS) {
            throw new IllegalArgumentException();
        }

        this.packetIds = packetIds;
    }

    @Override
    public int size() {
        return packetIds.size() * 4;
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        int counter = in.counter();

        packetIds.clear();
        while (in.counter() - counter < length) {
            packetIds.add(in.readInt());
        }

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int ret = 0;

        for (Integer i : packetIds) {
            ret += out.put(i);
        }

        return ret;
    }

    @Override
    public int hashCode() {
        return packetIds.hashCode();
    }

    @Override
    public boolean equals(Object other) {
        if (other == null || !(other instanceof Acknowledge)) {
            return false;
        }

        Acknowledge ack = (Acknowledge) other;

        if (!packetIds.equals(ack.packetIds)) {
            return false;
        }

        return true;
    }
}
