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
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;
import java.util.Vector;

import mxp.packet.MessageFrame;
import mxp.packet.Packet;

/**
 * A basic, simple session store.
 */
public class SimpleSessionStore implements SessionStore {
    /**
     * A map of sessions, keyed by the session id.
     */
    private Map<Integer, Session>   sessions;

    /**
     * A map of sessions, keyed by a combination of an address and a session id.
     */
    private Map<AddressId, Session>   sessionsRemote;

    /**
     * Constructor.
     */
    public SimpleSessionStore() {
        sessions       = new TreeMap<Integer, Session>();
        sessionsRemote = new HashMap<AddressId, Session>();
    }

    @Override
    public int newSession() {
        SimpleSession session = new SimpleSession();

        session.setSessionId(SessionIdSequence.nextId());
        session.setAddress(null);
        session.setRemoteSessionId(0);
        session.setLastKeepalive(new Date());
        session.setMessageId(0);
        session.setMessageQueue(new Vector<MessageQueueItem>());
        session.setPacketsPendingAck(new TreeMap<Integer, Packet>());
        session.setPacketsToAck(new Vector<Integer>());
        session.setMessageFrames(new TreeMap<Integer, List<MessageFrame>>());

        sessions.put(session.getSessionId(), session);

        return session.getSessionId();
    }

    @Override
    public void updateRemoteSession(int             sessionId,
                                    SocketAddress   address,
                                    int             remoteSessionId)
                                            throws IllegalArgumentException {
        if (!contains(sessionId)) {
            throw new IllegalArgumentException();
        }

        SimpleSession session = (SimpleSession) sessions.get(sessionId);

        AddressId ai = new AddressId(session.getAddress(),
                                     session.getRemoteSessionId());

        if (sessionsRemote.containsKey(ai)) {
            sessionsRemote.remove(ai);
        }

        session.setAddress(address);
        session.setRemoteSessionId(remoteSessionId);

        ai = new AddressId(address, remoteSessionId);
        sessionsRemote.put(ai, session);
    }

    @Override
    public void clear() {
        sessions.clear();
        sessionsRemote.clear();
    }

    @Override
    public boolean contains(int sessionId) {
        return sessions.containsKey(sessionId);
    }

    @Override
    public boolean contains(SocketAddress address, int sessionId)
                                            throws IllegalArgumentException {
        AddressId as = new AddressId(address, sessionId);

        return sessionsRemote.containsKey(as);
    }

    @Override
    public Session get(int sessionId) throws IllegalArgumentException {
        if (!sessions.containsKey(sessionId)) {
            throw new IllegalArgumentException();
        }

        return sessions.get(sessionId);
    }

    @Override
    public Session get(SocketAddress address, int sessionId)
                                            throws IllegalArgumentException {

        AddressId as = new AddressId(address, sessionId);

        if (!sessionsRemote.containsKey(as)) {
            throw new IllegalArgumentException();
        }

        return sessionsRemote.get(as);
    }

    @Override
    public Iterator<Session> iterator() {
        return sessions.values().iterator();
    }

    @Override
    public void remove(int sessionId) throws IllegalArgumentException {
        if (!sessions.containsKey(sessionId)) {
            throw new IllegalArgumentException();
        }

        Session session = sessions.get(sessionId);
        AddressId as = new AddressId(session.getAddress(),
                                     session.getRemoteSessionId());

        sessions.remove(sessionId);
        if (sessionsRemote.containsKey(as)) {
            sessionsRemote.remove(as);
        }
    }

    @Override
    public int size() {
        return sessions.size();
    }

}
