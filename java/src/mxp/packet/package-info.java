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
 * This package contains implementation for the message packet layer
 * for the Metaverse eXchange Protocol (MXP) draft version 0.5.
 * It contains implementations for the message packet and message
 * frame concepts, and implementations to process an MXP message
 * into frames and then put them into packets. It also contains
 * code that is able serialize all this.
 * <p/>
 * Generic MXP users are usually not interested in using this package
 * directly. Please see the mxp.net package for network
 * oriented communication, and the mxp.message package
 * for the MXP messages themselves.
 * <p/>
 * This project is distrubted under the GNU Affero General Public License
 * (GNU AGPL).
 *
 * @see mxp.message
 * @see mxp net
 */
package mxp.packet;
