#ifndef MXP_NET_SIMPLE_SESSION_STORE_H
#define MXP_NET_SIMPLE_SESSION_STORE_H

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
#include <map>
#include <boost/cstdint.hpp>
#include <boost/smart_ptr.hpp>
#include <boost/function.hpp>
#include <boost/bind.hpp>
#include <boost/iterator.hpp>
#include <boost/iterator/transform_iterator.hpp>
#include <boost/asio.hpp>

#include <mxp/net/simple_session.h>
#include <mxp/net/session_store.h>

namespace mxp {
namespace net {

using namespace boost;


/*! A pair of a remote address and a remote session id. */
typedef std::pair<asio::ip::udp::endpoint, uint32_t> address_id;

typedef std::map<uint32_t, shared_ptr<simple_session> > session_map_type;

typedef function<session_map_type::value_type::second_type
                    (const session_map_type::value_type &)>
                                                        session_map_get_value;

typedef transform_iterator<session_map_get_value,
                           session_map_type::const_iterator>
                                                              session_iterator;

/*! A session store to create and store session objects. This store acts
    both as a factory for session objects, and also as a lookup facility.
    This is an interface definitions - users must implement their own
    session stores.

    Sessions are considered to be unique for their local session id, and
    for a pair of remote address and remote session id.

    \tparam session the session object that is handled by this store
    \tparam iterator an iterator to the session objects
 */
class simple_session_store
        : public session_store<simple_session, session_iterator> {

private:
    /*! A map of sessions, keyed by the local session id. */
    std::map<uint32_t, shared_ptr<simple_session> >     _sessions;

    /*! A map of sessions, keyed by the remote address and remote session id. */
    std::map<address_id, shared_ptr<simple_session> >   _sessions_remote;

    /*! Generate a new, unique session id, in a thread-safe manner.

        \return a new, unique session id.
     */
    static uint32_t new_session_id();

public:
    /*! The iterator type returned by the begin() and end() functions. */
    typedef session_iterator iterator;

    /*! Constructor. */
    simple_session_store() {}

    /*! Virtual destructor. */
    virtual ~simple_session_store() {}

    /*! Create a new session.
        The newly created session will be maintained in the session store
        itself, and has to be remove by the remove() call after not needed.

        \return the local session id of the new session.
        \see #remove()
     */
    virtual uint32_t new_session() {
        shared_ptr<simple_session> session(new simple_session());

        session->_session_id = new_session_id();

        _sessions[session->_session_id] = session;

        return session->_session_id;
    }

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
                uint32_t                 remote_session_id) {

        session_map_type::iterator it = _sessions.find(session_id);
        if (it == _sessions.end()) {
            throw std::invalid_argument("no such session");
        }

        shared_ptr<simple_session> session = it->second;

        // remove the old address - id reference, if exists
        address_id                 ai(session->_address,
                                      session->_remote_session_id);
        std::map<address_id, shared_ptr<simple_session> >::iterator iit =
                                                    _sessions_remote.find(ai);
        if (iit != _sessions_remote.end()) {
            _sessions_remote.erase(iit);
        }

        // add the new address - id reference
        session->_address           = address;
        session->_remote_session_id = remote_session_id;
        address_id aai(address, remote_session_id);

        _sessions_remote[aai] = session;
    }

    /*! Remove all sessions from the session store. */
    virtual void clear() {
        _sessions.clear();
        _sessions_remote.clear();
    }

    /*! Tell if a session specified by a local session id exists in the store.

        \param session_id the local session id to check for
        \return true if a session by the specified local session id exists,
                false otherwise
     */
    virtual bool contains(uint32_t session_id) const {
        return _sessions.find(session_id) != _sessions.end();
    }

    /*! Tell if a session specified by a pair of remote address and remote
        session id exists in the store.

        \param address the remote address of the session
        \param remote_session_id the remote session id of the session
        \return true if a session by the specified local session id exists,
                false otherwise
     */
    virtual bool contains(asio::ip::udp::endpoint  address,
                          uint32_t                 remote_session_id) const {
        address_id ai(address, remote_session_id);

        return _sessions_remote.find(ai) != _sessions_remote.end();
    }

    /*! Return a pointer to a session based on a local session id.

        \param session_id the local session id of the session to look for
        \return a pointer to the session specified by the local session id
        \throws std::invalid_argument if no session by the specified session_id
                exists
     */
    virtual shared_ptr<simple_session> get(uint32_t session_id) const {
        session_map_type::const_iterator it = _sessions.find(session_id);
        if (it == _sessions.end()) {
            throw std::invalid_argument("no such session");
        }

        return it->second;
    }

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
    shared_ptr<simple_session> get(asio::ip::udp::endpoint  address,
                                   uint32_t                 remote_session_id)
                                                                        const {
        address_id ai(address, remote_session_id);

        std::map<address_id, shared_ptr<simple_session> >::const_iterator it =
                                                    _sessions_remote.find(ai);

        if (it == _sessions_remote.end()) {
            throw std::invalid_argument("no such session");
        }

        return it->second;
    }

    /*! Return an iterator that can walk through all sessions stored in this
        store. Any change to the store will invalidate this iterator.
        One can advance this iterator until it reaches end()

        \return an iterator that will walk through all sessions in the store,
                providing session_t & references.
        \see #end()
     */
    virtual
    iterator begin() const {
        session_map_get_value smgv =
                            bind(&session_map_type::value_type::second, _1);

        session_iterator it = make_transform_iterator(_sessions.begin(), smgv);

        return it;
    }

    /*! Return an iterator that is one beyond the last session for the iterator
        that was returned by begin().

        \return an iterator that is one beyond the last iterator for all
                sessions
        \see #begin()
     */
    virtual
    iterator end() const {
        session_map_get_value smgv =
                            bind(&session_map_type::value_type::second, _1);

        session_iterator it = make_transform_iterator(_sessions.end(), smgv);

        return it;
    }

    /*! Remove a session from the store.

        \param session_id the local session id of the session to remove.
        \throws std::invalid_argument if no session by the specified session_id
                exists
     */
    virtual void remove(uint32_t session_id) {
        session_map_type::iterator it = _sessions.find(session_id);
        if (it == _sessions.end()) {
            throw std::invalid_argument("no such session");
        }

        shared_ptr<simple_session> s = it->second;
        address_id                 ai(s->_address, s->_remote_session_id);

        std::map<address_id, shared_ptr<simple_session> >::iterator iit =
                                                    _sessions_remote.find(ai);
        if (iit != _sessions_remote.end()) {
            _sessions_remote.erase(iit);
        }

        _sessions.erase(it);
    }

    /*! The number of sessions in the session store.

        \return the number of items in the session store.
     */
    virtual size_t size() const {
        return _sessions.size();
    }

};

}
}

#endif
