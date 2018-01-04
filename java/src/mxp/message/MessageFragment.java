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
 * A re-usable part of an MXP message.
 */
public abstract class MessageFragment {
    /**
     * Constructor.
     */
    protected MessageFragment() {
    }

    /**
     * Return the size of the message when serialized.
     *
     * @return the size of the message when serialized, or -1 if this cannot
     *         be determined at the moment.
     */
    public abstract int size();

    /**
     * Serialize the message fragment.
     *
     * @param out the output stream to serialize into.
     * @return the number of bytes written into the serialization stream.
     * @throws IOException on I/O errors.
     */
    public abstract int
    serialize(SerializationOutputStream out) throws IOException;

    /**
     * Build a message fragment by reading its serialized form from an
     * input stream.
     *
     * @param in the input stream to read from.
     * @param length the maximum number of bytes to read from in.
     * @return the number of bytes read from the serialization stream.
     * @throws IOException on I/O errors.
     */
    public abstract int
    deserialize(SerializationInputStream in, int length) throws IOException;
}
