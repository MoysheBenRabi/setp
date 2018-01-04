#ifndef MXP_NET_SESSION_STORE_H
#define MXP_NET_SESSION_STORE_H

/* Copyright (c) 2009-2010 Tyrell Corporation & Moyshe Ben Rabi.

   The contents of this file are subject to the Mozilla Public License
   Version 1.1 (the "License"); you may not use this file except in
   compliance with the License. You may obtain a copy of the License at
   http://www.mozilla.org/MPL/

   Software distributed under the License is distributed on an "AS IS"
   basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
   License for the specific language governing rights and limitations
   under the License.

   The Original Code is an implementation of the Metaverse eXchange Protocol.

   The Initial Developer of the Original Code is Akos Maroy and Moyshe Ben Rabi.
   All Rights Reserved.

   Contributor(s): Akos Maroy and Moyshe Ben Rabi.

   Alternatively, the contents of this file may be used under the terms
   of the Affero General Public License (the  "AGPL"), in which case the
   provisions of the AGPL are applicable instead of those
   above. If you wish to allow use of your version of this file only
   under the terms of the AGPL and not to allow others to use
   your version of this file under the MPL, indicate your decision by
   deleting the provisions above and replace them with the notice and
   other provisions required by the AGPL. If you do not delete
   the provisions above, a recipient may use your version of this file
   under either the MPL or the AGPL.
*/

#include <stdexcept>
#include <boost/cstdint.hpp>
#include <boost/smart_ptr.hpp>
#include <boost/asio.hpp>

namespace mxp {
namespace net {

using namespace boost;

/*! A session store to create and store session objects. This store acts
    both as a factory for session objects, and also as a lookup facility.
    This is an interface definitions - users must implement their own
    session stores.

    Sessions are considered to be unique for their local session id, and
    for a pair of remote address and remote session id.

    \tparam session the session object that is handled by this store
    \tparam iterator an iterator to the session objects
 */
template<typename session, typename iterator>
class session_store {

public:
    /*! A const_iterator typedef to make BOOST_FOREACH happy. */
    typedef const iterator const_iterator;

    /*! Virtual destructor. */
    virtual ~session_store() {}

    /*! Create a new session.
        The newly created session will be maintained in the session store
        itself, and has to be remove by the remove() call after not needed.

        \return the local session id of the new session.
        \see #remove()
     */
    virtual uint32_t new_session() = 0;

    /*! Update an existing session with a remote address and session id.

        \param session_id the local session id of the session to update
        \param address the remote address this session is associated with
        \param remote_session_id the session id at the remote end
        \throws std::invalid_argument if no session by the specified session_id
                exists
     */
    virtual
    void update(uint32_t                 session_id,
                asio::ip::udp::endpoint  address,
                uint32_t                 remote_session_id) = 0;

    /*! Remove all sessions from the session store. */
    virtual void clear() = 0;

    /*! Tell if a session specified by a local session id exists in the store.

        \param session_id the local session id to check for
        \return true if a session by the specified local session id exists,
                false otherwise
     */
    virtual bool contains(uint32_t session_id) const = 0;

    /*! Tell if a session specified by a pair of remote address and remote
        session id exists in the store.

        \param address the remote address of the session
        \param remote_session_id the remote session id of the session
        \return true if a session by the specified local session id exists,
                false otherwise
     */
    virtual bool contains(asio::ip::udp::endpoint  address,
                          uint32_t                 remote_session_id) const = 0;

    /*! Return a pointer to a session based on a local session id.

        \param session_id the local session id of the session to look for
        \return a pointer to the session specified by the local session id
        \throws std::invalid_argument if no session by the specified session_id
                exists
     */
    virtual shared_ptr<session> get(uint32_t session_id) const = 0;

    /*! Return a pointer to a session based on a remote address and a remote
        session id.

        \param address the remote address of the session
        \param remote_session_id the remote session id of the session
        \return a pointer to the session specified by the remote address and
                remote session id
        \throws std::invalid_argument if no session by the specified session_id
                exists
     */
    virtual
    shared_ptr<session> get(asio::ip::udp::endpoint  address,
                            uint32_t                 remote_session_id)
                                                                    const = 0;

    /*! Return an iterator that can walk through all sessions stored in this
        store. Any change to the store will invalidate this iterator.
        One can advance this iterator until it reaches end()

        \return an iterator that will walk through all sessions in the store,
                and which resolves to shared_ptr<session> pointers.
        \see #end()
     */
    virtual
    iterator begin() const = 0;

    /*! Return an iterator that is one beyond the last session for the iterator
        that was returned by begin().

        \return an iterator that is one beyond the last iterator for all
                sessions
        \see #begin()
     */
    virtual
    iterator end() const = 0;

    /*! Remove a session from the store.

        \param session_id the local session id of the session to remove.
        \throws std::invalid_argument if no session by the specified session_id
                exists
     */
    virtual void remove(uint32_t session_id) = 0;

    /*! The number of sessions in the session store.

        \return the number of items in the session store.
     */
    virtual size_t size() const = 0;

};

}
}

#endif
