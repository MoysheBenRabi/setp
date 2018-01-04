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
/**
 * This package contains the network communication related functionality of
 * a Java implementation of the Metaverse eXchange Protocol (MXP) draft
 * version 0.5.
 * <p/>
 * A typical user of this package creates a Communication object to
 * establish communication with an MXP server. He then would send
 * messages over this object, and react on messages received from the server.
 * <p/>
 * Clients can chose to maintain information like session info in relation to
 * this communication by implementing the SessionStore interface.
 * <p/>
 * For server implementations, the same Communication object is created,
 * with a specified network interface and port to bind to.
 * <p/>
 * This project is distributed under the GNU Affero General Public License
 * (GNU AGPL).
 *
 * @see mxp.net.Communication
 * @see mxp.message
 */
package mxp.net;
