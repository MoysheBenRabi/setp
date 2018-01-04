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
package mxp.net;

import java.net.SocketAddress;
import java.util.Iterator;

/**
 * A session store to store session objects. Sessions are considered unique
 * by their local id, and also by their remote address / remote id combination.
 */
public interface SessionStore extends Iterable<Session> {
    /**
     * Create a new session, with a unique session id. The new session will be
     * put in the store immediately. The new session will not have its
     * remote address and remote session id set. After the session is not
     * needed anymore, remove / destroy it by calling remove().
     *
     * @return the local session id of the newly created session.
     */
    int newSession();

    /**
     * Update the remote address and remote session id of an existing
     * session.
     *
     * @param sessionId the local session of the session to update.
     * @param address the address of the remote end of the session
     * @param remoteSessionId the session id at the remote end of the session.
     * @throws IllegalArgumentException if no session with the specified
     *         session id exists.
     */
    void updateRemoteSession(int            sessionId,
                             SocketAddress  address,
                             int            remoteSessionId)
                                              throws IllegalArgumentException;

    /**
     * Clear all sessions from the store.
     */
    void clear();

    /**
     * Check if a session is in the store, based on a local session id.
     *
     * @param sessionId the local session id.
     * @return true of the store contain a session with the local session id,
     *         false otherwise.
     */
    boolean contains(int sessionId);

    /**
     * Check if a session is in the store, based on the remote address and
     * remote session id.
     *
     * @param address the remote address of the session
     * @param sessionId the remote session id of the session.
     * @return true of the store contain a session with the supplied remote
     *         address and session id, false otherwise.
     */
    boolean contains(SocketAddress address, int sessionId);

    /**
     * Return a session based on a local session id.
     *
     * @param sessionId the local session id to return the session for
     * @return a session with the specified local session id.
     * @throws IllegalArgumentException in there is no session in the store
     *         with the specified session id.
     */
    Session get(int sessionId) throws IllegalArgumentException;

    /**
     * Return a session based on a remote address and remote session id.
     *
     * @param address the remote address of the session
     * @param sessionId the remote session id of the session.
     * @return a session with the specified remote address and remote session
     *         id.
     * @throws IllegalArgumentException in there is no session in the store
     *         with the specified remote address and session id.
     */
    Session get(SocketAddress address, int sessionId)
                                            throws IllegalArgumentException;

    /**
     * Return an iterator an all sessions stored in the session id. The iterator
     * may be invalidated by any changes to the session store.
     *
     * @return an iterator an all sessions stored in the session id.
     */
    Iterator<Session> iterator();

    /**
     * Remove a session from the store. After this call, any activities on
     * the previously stored reference to the session is question is
     * undefined.
     *
     * @param sessionId the local id of the session to remove.
     * @throws IllegalArgumentException if no session with the specified local
     *         id exists.
     */
    void remove(int sessionId) throws IllegalArgumentException;

    /**
     * Return the number of sessions in the store.
     *
     * @return the number of sessions in the store.
     */
    int size();

}
