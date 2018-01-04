#ifndef MXP_SERVER_H
#define MXP_SERVER_H

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

#include <map>
#include <stdexcept>

#include <boost/smart_ptr.hpp>
#include <boost/function.hpp>
#include <boost/bind.hpp>
#include <boost/iterator.hpp>
#include <boost/iterator/transform_iterator.hpp>
#include <boost/asio.hpp>
#include <boost/uuid/uuid.hpp>

#include <mxp.h>

namespace mxp {

using namespace boost;
using namespace mxp::services;

/*! A class to represent an MXP server - that is, an entity that accepts
    MXP connections, and keeps track of entities injected into bubbles.
    The class provides updates to clients, and expected clients to send update
    messages to it.
 */
class server {

private:
    typedef std::map<uuids::uuid, shared_ptr<bubble> > bubble_map_type;

    typedef function<bubble_map_type::value_type::second_type
                        (const bubble_map_type::value_type &)>
                                                        bubble_map_get_value;

    typedef transform_iterator<bubble_map_get_value,
                               bubble_map_type::const_iterator>
                                                            bubble_iterator;

    /*! The MXP communication object, doing all the communication with
        other parties.
     */
    mxp::net::communication                         _server;

    /*! A map of MXP bubbles, keyed by the bubbles unique id. */
    std::map<uuids::uuid, shared_ptr<bubble> >      _bubbles;

    /*! The id of the default bubble in the server. */
    uuids::uuid                                     _default_bubble;

    /*! Handler for new session opening events.

        \param session_id the local id of the new session
        \param ep the remote endpoint associated with this session
        \param remote_session_id the sesion id of the session at the remote
               endpoint.
     */
    void new_session_handler(uint32_t                   session_id,
                             asio::ip::udp::endpoint    ep,
                             uint32_t                   remote_session_id) {
        std::cerr << "new session " << session_id << " from " << ep
                  << std::endl;
    }

    /*! Handler for session closing events.

        \param session_id the local id of the closed session
        \param ep the remote endpoint associated with this session
        \param remote_session_id the sesion id of the session at the remote
               endpoint.
     */
    void closed_session_handler(uint32_t                   session_id,
                                asio::ip::udp::endpoint    ep,
                                uint32_t                   remote_session_id) {
        std::cerr << "closed session " << session_id << " from " << ep
                  << std::endl;
    }

    /*! Handler for newly received messages.

        \param message a pointer to the received message.
        \param session_id the id of the session the message is part of
        \param ep the remote endpoint which sent the message
     */
    void msg_handler(shared_ptr<mxp::message::message>  message,
                     uint32_t                           session_id,
                     asio::ip::udp::endpoint            ep) {
        std::cerr << "received message " << *message
                  << " for session " << session_id << " from " << ep
                  << std::endl;
    }


public:
    /*! Constructor.

        \param port the port for the bubble to bind to.
     */
    server(unsigned int port)
        : _server(asio::ip::udp::endpoint(asio::ip::udp::v4(), port)),
          _default_bubble(uuids::uuid()) {

        // connect the event handlers to the _server
        _server.connect_message_handler(
                bind(&server::msg_handler, this, _1, _2, _3));
        _server.connect_new_session_handler(
                bind(&server::new_session_handler, this, _1, _2, _3));
        _server.connect_closed_session_handler(
                bind(&server::closed_session_handler, this, _1, _2, _3));
    }

    /*! Return the number of bubbles in this server.

        \return the number of bubbles in this server.
     */
    unsigned int size() const {
        return _bubbles.size();
    }

    /*! Tell if a bubble exists in the server.

        \param id the id of the bubble to check for.
        \return true if the bubble is contained in the server, false otherwise.
     */
    bool contains(const uuids::uuid & id) const {
        return _bubbles.find(id) != _bubbles.end();
    }

    /*! Get a specific bubble from the server.

        \param id the id of the bubble to get
        \return a reference to the bubble in question.
        \throws std::invalid_argument if the bubble with the specified id
                was not found.
     */
    shared_ptr<const bubble> get(const uuids::uuid & id) const {
        bubble_map_type::const_iterator it = _bubbles.find(id);

        if (it == _bubbles.end()) {
            throw std::invalid_argument("no such bubble id");
        }

        return it->second;
    }

    /*! Add an MXP bubble to the server. The server will start to maintain
        objects in the bubble after it is added. If a bubble with the same
        id exists, it is replaced by the new bubble.

        If this would be the only bubble, it is made the default bubble.

        \param b the MXP bubble to add to the server.
     */
    void add(shared_ptr<bubble>  b) {
        _bubbles[b->id()] = b;

        if (size() == 1u) {
            _default_bubble = b->id();
        }
    }

    /*! Remove an MXP bubble from the server.
        If this was the default bubble, a random bubble is chosen to be
        the default. If there will be no more bubbles, the default bubble
        is set to a UUID of all zeros.

        \param id the id of the bubble to remove.
     */
    void remove(const uuids::uuid & id) {
        if (contains(id)) {
            _bubbles.erase(id);
        }

        if (_default_bubble == id) {
            _default_bubble = _bubbles.size() > 0u
                            ? _bubbles.begin()->second->id()
                            : uuids::uuid();
        }
    }

    /*! Set the default bubble.

        \param id the new default bubble.
        \throws std::invalid_argument if the bubble with the specified id
                was not found.
     */
    void default_bubble(const uuids::uuid & id) {
        if (contains(id)) {
            _default_bubble = id;

            return;
        }

        throw std::invalid_argument("no such bubble");
    }

    /*! Get the default bubble.

        \return the default bubble
        \throws std::invalid_argument if there are no bubbles in the server
     */
    shared_ptr<const bubble> default_bubble() const {
        if (_bubbles.size() > 0u) {
            return get(_default_bubble);
        }

        throw std::invalid_argument("no such bubble");
    }

    /*! Return a begin iterator for the MXP bubbles contained in the server.

        \return an iterator of shared_ptr<bubble> pointers,
                pointing to the first MXP bubble in the server.
        \see #bubbles_end()
     */
    bubble_iterator bubbles_begin() const {
        bubble_map_get_value bmgv =
                            bind(&bubble_map_type::value_type::second, _1);

        return make_transform_iterator(_bubbles.begin(), bmgv);
    }

    /*! Return an end iterator for the MXP bubble contained in the server.

        \return an iterator of shared_ptr<bubble> pointers,
                pointing to one beyond the last MXP bubble in the server.
        \see #bubbles_begin()
     */
    bubble_iterator objects_end() const {
        bubble_map_get_value bmgv =
                            bind(&bubble_map_type::value_type::second, _1);

        return make_transform_iterator(_bubbles.end(), bmgv);
    }

};

}

#endif
