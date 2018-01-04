#ifndef MXP_NET_SIMPLE_SESSION_H
#define MXP_NET_SIMPLE_SESSION_H

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

#include <vector>
#include <map>

#include <mxp/packet.h>
#include <mxp/net/simple_message_queue_item.h>
#include <mxp/net/session.h>

namespace mxp {
namespace net {

using namespace boost;

class simple_session_store;

typedef std::vector<simple_message_queue_item> simple_message_queue_list;

typedef std::map<uint32_t, mxp::packet::packet> simple_packet_id_packet_map;

typedef std::vector<uint32_t> simple_packet_id_list;

typedef std::map<uint32_t, std::vector<mxp::packet::message_frame> >
                                                simple_message_id_frame_map;


/*! A simple session implementation. */
class simple_session : public session<simple_message_queue_list,
                                      simple_packet_id_packet_map,
                                      simple_packet_id_list,
                                      simple_message_id_frame_map> {

    friend class simple_session_store;

private:
    /*! The local session id. */
    uint32_t                    _session_id;

    /*! The remote end's address. */
    asio::ip::udp::endpoint     _address;

    /*! The remote session id. */
    uint32_t                    _remote_session_id;

    /*! The last used message id. */
    uint32_t                    _message_id;

    /*! A vector holding simple message queue item objects. */
    simple_message_queue_list   _message_queue;

    /*! Packets pending an ack message in a map, holding packets, keyed by
        their packet ids. */
    simple_packet_id_packet_map _packets_pending_ack;

    /*! A list of packet ids to be acknowledged still. */
    simple_packet_id_list       _packets_to_ack;

    /*! A map, keyed by message ids, mapped to a vector of message frames,
        which make up the messages itself.
     */
    simple_message_id_frame_map _message_frames;

    /*! Timestamp of the last acknowledge message sent. */
    posix_time::ptime           _last_ack_time;

    /*! Timestamp of the last keep alive message sent. */
    posix_time::ptime           _last_keepalive_time;

    /*! Timestamp of the last message received. */
    posix_time::ptime           _last_message_time;

    /*! Generate a new, unique message id. This call is thread safe.

        \return a new, unqiue message id.
     */
    static uint32_t new_message_id();

public:
    /*! Constructor. */
    simple_session() : _session_id(0u),
                       _remote_session_id(0u),
                       _message_id(0u),
            _last_ack_time(posix_time::microsec_clock::universal_time()),
            _last_keepalive_time(posix_time::microsec_clock::universal_time()),
            _last_message_time(posix_time::microsec_clock::universal_time()) {
    }

    /*! Virtual destructor. */
    virtual ~simple_session() {}

    virtual uint32_t session_id() const {
        return _session_id;
    }

    virtual asio::ip::udp::endpoint address() const {
        return _address;
    }

    virtual uint32_t remote_session_id() const {
        return _remote_session_id;
    }

    virtual uint32_t message_id() const {
        return _message_id;
    }

    virtual uint32_t next_message_id() {
        uint32_t id = new_message_id();

        _message_id = id;

        return id;
    }

    virtual simple_message_queue_list & message_queue() {
        return _message_queue;
    }

    virtual simple_packet_id_packet_map & packets_pending_ack() {
        return _packets_pending_ack;
    }

    virtual simple_packet_id_list & packets_to_ack() {
        return _packets_to_ack;
    }

    virtual simple_message_id_frame_map & message_frames() {
        return _message_frames;
    }

    virtual posix_time::ptime last_ack_time() const {
        return _last_ack_time;
    }

    virtual void last_ack_time(posix_time::ptime timestamp) {
        _last_ack_time = timestamp;
    }

    virtual posix_time::ptime last_keepalive() const {
        return _last_keepalive_time;
    }

    virtual void last_keepalive(posix_time::ptime timestamp) {
        _last_keepalive_time = timestamp;
    }

    virtual posix_time::ptime last_message_time() const {
        return _last_message_time;
    }

    virtual void last_message_time(posix_time::ptime timestamp) {
        _last_message_time = timestamp;
    }
};

}
}

#endif
