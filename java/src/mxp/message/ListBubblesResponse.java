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
 * A message providing a list of bubbles in response to a ListBubblesRequest,
 * as specified by the MXP protocol.
 */
public class ListBubblesResponse extends Message {
    /**
     * The bubbles listed.
     */
    private Vector<BubbleFragment> bubbles = new Vector<BubbleFragment>();

    /**
     * Constructor.
     */
    public ListBubblesResponse() {
        super(Message.Type.LIST_BUBBLES_RESPONSE);
    }

    /**
     * @return the bubbles
     */
    public Vector<BubbleFragment> getBubbles() {
        return bubbles;
    }

    /**
     * @param bubbles the bubbles to set
     */
    public void setBubbles(Vector<BubbleFragment> bubbles) {
        this.bubbles = bubbles;
    }

    @Override
    public int size() {
        if (bubbles == null) {
            return -1;
        }

        int size = 0;
        for (BubbleFragment b : bubbles) {
            size += b.size();
        }

        return size;
    }

    @Override
    public int deserialize(SerializationInputStream in, int length)
                                                        throws IOException {
        int counter    = in.counter();
        byte[] padding = new byte[60];

        bubbles.clear();
        while (in.counter() - counter < length) {
            BubbleFragment bf = new BubbleFragment();
            bf.deserialize(in, length - (in.counter() - counter));
            in.readBytes(padding);

            bubbles.add(bf);
        }

        return in.counter() - counter;
    }

    @Override
    public int serialize(SerializationOutputStream out) throws IOException {
        int    ret     = 0;
        byte[] padding = new byte[60];

        for (BubbleFragment bf : bubbles) {
            ret += bf.serialize(out);
            ret += out.put(padding);
        }

        return ret;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = super.hashCode();
        result = prime * result + ((bubbles == null) ? 0 : bubbles.hashCode());
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
        if (!(obj instanceof ListBubblesResponse)) {
            return false;
        }
        ListBubblesResponse other = (ListBubblesResponse) obj;
        if (bubbles == null) {
            if (other.bubbles != null) {
                return false;
            }
        } else if (!bubbles.equals(other.bubbles)) {
            return false;
        }
        return true;
    }
}
