#ifndef MXP_NET_SESSION_H
#define MXP_NET_SESSION_H

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

#include <boost/cstdint.hpp>
#include <boost/date_time.hpp>
#include <boost/asio.hpp>

namespace mxp {
namespace net {

using namespace boost;
using namespace mxp;

/*! A session state between MXP protocol participants.
    A session has two unique identifiers: a local session id, which is unique
    locally, and a remote address - remote session id pair, which is unique
    among the sessions connected to us.

    Session and message ids of value 0 are treated as invalid ids.

    This is an interface class - subclasses of these are specific
    implementations.

    \tparam message_queue_item_list a container holding
            shared_ptr<message_queue_item> pointers
    \tparam packet_id_packet_map a map holding packet objects, keyed by
            their packet ids.
    \tparam packet_id_list a list of packet ids
    \tparam message_id_frame_map a map, with message ids as keys, and a list
            of message frames as values - those being the frames that make
            up the message when sent via MXP packets
 */
template<typename message_queue_item_list_t,
         typename packet_id_packet_map_t,
         typename packet_id_list_t,
         typename message_id_frame_map_t>
class session {
public:
    /*! A container holding shared_ptr<message_queue_item> pointers. */
    typedef message_queue_item_list_t message_queue_item_list;

    /*! A map holding packet objects, keyed by their packet ids. */
    typedef packet_id_packet_map_t packet_id_packet_map;

    /*! A list of packet ids. */
    typedef packet_id_list_t packet_id_list;

    /*! A map, with message ids as keys, and a list of message frames as values
         - those being the frames that make up the message when sent via MXP
         packets.
     */
    typedef message_id_frame_map_t message_id_frame_map;

    /*! Virtual destructor. */
    virtual ~session() {}

    /*! Return the sessions local session id.

        \return the local session id.
     */
    virtual uint32_t session_id() const = 0;

    /*! Return the remote connection endpoint.

        \return the remote connection endpoint.
     */
    virtual asio::ip::udp::endpoint address() const = 0;

    /*! Return the remote session id.

        \return the remote session id.
     */
    virtual uint32_t remote_session_id() const = 0;

    /*! Get the last used message id.

        \return the last used message id.
     */
    virtual uint32_t message_id() const = 0;

    /*! Get a new, unique message id.

        \return a new, unique message id.
     */
    virtual uint32_t next_message_id() = 0;

    /*! Return a reference to the container holding
        shared_ptr<message_queue_item> pointers, which are pending messages
        in the current session.

        \return a reference to a container holding
                shared_ptr<message_queue_item> pointers.
     */
    virtual message_queue_item_list & message_queue() = 0;

    /*! Return a map holding packets which were sent to a remote end, but are
        waiting for acknowledgements from the remote end.

        \return a map of packet objects, keyed by their packet ids.
     */
    virtual packet_id_packet_map & packets_pending_ack() = 0;

    /*! Return a list of packet ids, for which acknowledgement has to be sent.

        \return a list of packet id, for which acknowledgement has to be sent.
     */
    virtual packet_id_list & packets_to_ack() = 0;

    /*! Return a map, which is keyed by message ids, and which contain a
        list each as a value. The list is the list of message frames that
        make up the message when put into MXP packets. This is used to
        collect frames received from the other and, and are turned into
        messages as soon as all frames are received.

        \return a map of message ids mapped to a list of message frames.
     */
    virtual message_id_frame_map & message_frames() = 0;

    /*! Return the timestamp for the last acknowledge message sent.

        \return the timestamp of the last acknowledge message sent.
     */
    virtual posix_time::ptime last_ack_time() const = 0;

    /*! Set the timestamp for the last acknowledge message sent.

        \param timestamp the last acknowledge message timestamp.
     */
    virtual void last_ack_time(posix_time::ptime timestamp) = 0;

    /*! Return the timestamp for the last keep alive message sent.

        \return the timestamp of the last keep alive message sent.
     */
    virtual posix_time::ptime last_keepalive() const = 0;

    /*! Set the timestamp for the last keep alive message sent.

        \param timestamp the last keep alive message timestamp.
     */
    virtual void last_keepalive(posix_time::ptime timestamp) = 0;

    /*! Return the timestamp of the last message received from the remote end.

        \return the timestamp of the last message received from the remote end.
     */
    virtual posix_time::ptime last_message_time() const = 0;

    /*! Set the timestamp of the last message received from the remote end.

        \param timestamp of the last message received from the remote end.
     */
    virtual void last_message_time(posix_time::ptime timestamp) = 0;
};


}
}

#endif
