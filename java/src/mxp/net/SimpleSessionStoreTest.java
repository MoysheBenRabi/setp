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

import java.net.InetSocketAddress;

import junit.framework.TestCase;

/**
 * Test case to test the SimpleSessionStore object.
 */
public class SimpleSessionStoreTest extends TestCase {
    /**
     * A very basic simple test.
     */
    public void testSimple() {
        SessionStore ss        = new SimpleSessionStore();
        int          sessionId = ss.newSession();

        assertEquals(ss.size(), 1);
        assertTrue(ss.contains(sessionId));

        Session session = ss.get(sessionId);
        assertEquals(session.getSessionId(), sessionId);

        ss.updateRemoteSession(sessionId,
                               new InetSocketAddress("foo.bar", 4567),
                               2);

        assertTrue(ss.contains(new InetSocketAddress("foo.bar", 4567), 2));
        assertEquals(ss.get(new InetSocketAddress("foo.bar", 4567), 2),
                            session);

        ss.remove(session.getSessionId());

        assertEquals(ss.size(), 0);
        assertFalse(ss.contains(session.getSessionId()));
        assertFalse(ss.contains(new InetSocketAddress("foo.bar", 4567), 2));
    }

    /**
     * Test for updating sessions by changing the remote address properties,
     * to see if there are memory leaks.
     */
    public void testUpdate() {
        SessionStore ss        = new SimpleSessionStore();
        int          sessionId = ss.newSession();

        assertEquals(ss.size(), 1);
        assertTrue(ss.contains(sessionId));

        Session session = ss.get(sessionId);
        assertEquals(session.getSessionId(), sessionId);

        ss.updateRemoteSession(sessionId,
                               new InetSocketAddress("foo.bar", 4567),
                               2);

        assertTrue(ss.contains(new InetSocketAddress("foo.bar", 4567), 2));
        assertEquals(ss.get(new InetSocketAddress("foo.bar", 4567), 2),
                            session);

        assertEquals(ss.size(), 1);
        assertTrue(ss.contains(session.getSessionId()));
        assertEquals(ss.get(session.getSessionId()), session);
        assertTrue(ss.contains(new InetSocketAddress("foo.bar", 4567), 2));
        assertEquals(ss.get(new InetSocketAddress("foo.bar", 4567), 2),
                     session);

        ss.updateRemoteSession(sessionId,
                               new InetSocketAddress("foo.bar", 5678),
                               3);

        assertEquals(ss.size(), 1);
        assertTrue(ss.contains(session.getSessionId()));
        assertEquals(ss.get(session.getSessionId()), session);
        assertFalse(ss.contains(new InetSocketAddress("foo.bar", 4567), 2));
        assertTrue(ss.contains(new InetSocketAddress("foo.bar", 5678), 3));
        assertEquals(ss.get(new InetSocketAddress("foo.bar", 5678), 3),
                     session);
    }
}
