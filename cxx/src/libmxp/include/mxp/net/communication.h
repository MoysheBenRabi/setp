#ifndef MXP_NET_COMMUNICATION_H
#define MXP_NET_COMMUNICATION_H

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
#include <vector>
#include <boost/cstdint.hpp>
#include <boost/smart_ptr.hpp>
#include <boost/foreach.hpp>
#include <boost/bind.hpp>
#include <boost/date_time.hpp>
#include <boost/thread.hpp>
#include <boost/signals2.hpp>

#include <mxp/message.h>
#include <mxp/packet.h>
#include <mxp/net/session.h>
#include <mxp/net/session_store.h>
#include <mxp/net/simple_session_store.h>
#include <mxp/net/simple_message_queue_item.h>
#include <mxp/net/connection.h>

namespace mxp {
namespace net {

using namespace boost;
using namespace mxp;

/*! A client communication object, which connects to a server and listend for
    incoming messages. It will notify the interested parties when
    receiving any incomming messages, and can also send messages to the server.

    This class either binds to a specific interface / port, which is a typical
    server-side usage, or client usage behind a firewall where only a number of
    UDP ports are let through, or binds to any port on all local interfaces,
    which is a generic usage for a client.

    The class also maintains a list of packets to be acknowledged via an
    acknowledge message, and sends such ack messages automatically. It will
    also silently consume & process received acknowledge messages, and resend
    packets which have not been acknowledged. This is all done because
    semantically speaking, package acknowledgements should belong to the
    packet layer, even though in MXP it's done by a message - a design flaw,
    in the opinion of this implementor.

    The class also takes care of initial hand-shaking - establishing mutual
    session ids on both sides of a communication session. In MXP, this means
    that after an initial connection message is sent, the corresponding
    acknowledge message has to be caught, and the session id on the remote side
    has to be stored and used from there. In MXP, for each session there are two
    unique ids, one from each side of the communication.

    The class sends keep-alive messages for each open session on a regular
    basis, and silently consumes keep-alive messages for each session.

    If needed, this class does not store & maintain resources - in this mode,
    all resources are expected to be maintained by the caller.

    \tparam session_store_t a specific sublcass of the session_store class, or
            an equivalent class, to use as a session store. defaults to
            simple_session_store
    \tparam session_t a specific subclass of the session class, or an
            equivalent class, that act as sessions during the communication.
            defaults to simple_session
 */
template<typename session_store_t, typename session_t>
class basic_communication {
public:
    /*! Signal type for new session events, opened by a remote party.
        The signature for the event handler is:

        \code
        void handler(boost::uint32_t                  session_id,
                     boost::asio::ip::udp::endpoint   endpoint
                     boost::uint32_t                  remote_session_id);
        \endcode

        Where session_id is the local session id of the new opened message,
        endpoint is the address of the remote host which opened the message,
        and remote_session_id is the session id at the remote end.
     */
    typedef signals2::signal<void (uint32_t,
                                   asio::ip::udp::endpoint,
                                   uint32_t)>
                                                            new_session_signal;

    /*! Signal type for session closing events, for sesions closed by the
        remote party.
        The signature for the event handler is:

        \code
        void handler(boost::uint32_t                  session_id,
                     boost::asio::ip::udp::endpoint   endpoint
                     boost::uint32_t                  remote_session_id);
        \endcode

        Where session_id is the local session id of the closed message,
        endpoint is the address of the remote host which closed the message,
        and remote_session_id is the session id at the remote end.
     */
    typedef signals2::signal<void (uint32_t,
                                   asio::ip::udp::endpoint,
                                   uint32_t)>
                                                        closed_session_signal;

    /*! Signal type for message received events.
        The signature for the event handler is:

        \code
        void message_handler(boost::shared_ptr<mxp::message::message> message,
                             boost::uint32_t                         session_id,
                             boost::asio::ip::udp::endpoint          endpoint);
        \endcode

        Where message is a pointer to the message just received, session_id is
        the id of the session that sent the message, and endpoint is the
        other side which sent the message.
     */
    typedef signals2::signal<void (shared_ptr<message::message>,
                                   uint32_t,
                                   asio::ip::udp::endpoint)>
                                                                message_signal;

private:
    /*! An address - id pair. */
    typedef std::pair<asio::ip::udp::endpoint, uint32_t>    address_id;


    /*! The underlying connection. */
    connection _connection;

    /*! The session store, storing all the sessions. */
    shared_ptr<session_store_t> _sessions;

    /*! The signal for sending notifications on newly opened sessions. */
    new_session_signal _new_session_signal;

    /*! The signal for sending notifications on closed sessions. */
    closed_session_signal _closed_session_signal;

    /*! The signal for sending notifications on receiving messages. */
    message_signal _message_signal;

    /*! Flag to signal if the background threads for this communication
        session should be running. */
    bool _should_run;

    /*! The ticker background thread, performing all actions in the
        background. */
    thread _ticker_thread;

    /*! The mutex for the communication object, ensuring thread-safety. */
    recursive_mutex _mutex;

    /*! The conditional variable helping for synchronizing tasks related to
        opening sessions. */
    condition_variable_any _opening_session_condition;


    /*! A map of sessions which are in the 'opening' phase - that is, they
        have a local session id, but we're still waiting for the remote
        session id to be received from the remote end.
        The key in this map contains the remote end, and the packet id
        we're waiting an acknowledgement for. the value in the map is
        the local session id.
     */
    std::map<address_id, uint32_t>  _opening_sessions;


    /*! Event handler for when a new packet is received by the underlying
        connection. Turn the packet into one or more messages, and
        notify all interested parties about the messages.

        \param packet the packet just received
        \param endpoint the other end, who sent the packet
    */
    void packet_received(packet::packet             packet,
                         asio::ip::udp::endpoint    endpoint) {

        // guard with a try-catch block, so as to avoid this thread
        // from exiting via an uncaught exception

        try {
            handle_packet(packet, endpoint);

        } catch (const std::exception & ex) {
            std::cerr << "issue with received packet "
                      << ex.what() << std::endl;
        } catch (...) {
            std::cerr << "issue with received packet" << std::endl;
        }
    }

    /*! Process the reception of a freshly received packet.
        This includes opening new sessions, processing keep-alive
        and acknowledge messages, and notifying interested parties
        as new messages are constructed from the packet.

        \param packet the packet just received
        \param endpoint the other end, who sent the packet
    */
    void handle_packet(packet::packet             packet,
                       asio::ip::udp::endpoint  & endpoint) {

        typedef std::vector<shared_ptr<message::message> > container;

        lock_guard<recursive_mutex> lock(_mutex);

        if (!_sessions->contains(endpoint, packet.session_id)) {
            handle_opening_packet(packet, endpoint);
        }
        if (!_sessions->contains(endpoint, packet.session_id)) {
            return;
        }

        shared_ptr<session_t> s = _sessions->get(endpoint, packet.session_id);

        if (packet.guaranteed != 0) {
            // if we received a guaranteed packet, add it to the ack list
            s->packets_to_ack().push_back(packet.packet_id);
        }


        // turn the packet into messages
        std::vector<packet::packet> packets;
        packets.push_back(packet);

        container messages;
        std::back_insert_iterator<container> mit = std::back_inserter(messages);

        packet::packets_to_messages(packets.begin(),
                                    packets.end(),
                                    mit,
                                    s->message_frames());

        if (messages.empty()) {
            return;
        }

        s->last_message_time(posix_time::microsec_clock::universal_time());


        // get the ack messages from the queue, and process them locally
        // also silently consume keepalive messages
        container non_ack_messages;
        non_ack_messages.reserve(messages.size());

        BOOST_FOREACH(shared_ptr<message::message> m, messages) {
            if (m->get_type() == message::ACKNOWLEDGE) {
                process_acknowledge(m, endpoint, packet.session_id);
            } else if (m->get_type() == message::KEEPALIVE) {
                // silently ignore keepalive messages
            } else {
                non_ack_messages.push_back(m);
            }
        }

        // notify all interested parties about relevant messages
        BOOST_FOREACH(shared_ptr<message::message> & m, non_ack_messages) {
            _message_signal(m, packet.session_id, endpoint);
        }
    }

    /*! Handle the receipt of a session opening packet - a first packet
        from a particular endpoint with a particular session id.
        This might be a response to a session we initiated, or be a session
        initiation itself by the other end.

        \param packet the packet just received
        \param endpoint the other end, who sent the packet
    */
    void handle_opening_packet(mxp::packet::packet        packet,
                               asio::ip::udp::endpoint    endpoint) {

        typedef std::vector<shared_ptr<message::message> >  message_container;

        typedef std::map<uint32_t, std::vector<mxp::packet::message_frame> >
                                                          message_id_frame_map;

        lock_guard<recursive_mutex> lock(_mutex);

        // turn the packet into messages
        std::vector<packet::packet> packets;
        packets.push_back(packet);

        message_container       messages;
        std::back_insert_iterator<message_container> mit
                                                = std::back_inserter(messages);
        message_id_frame_map    message_frames;

        packet::packets_to_messages(packets.begin(),
                                    packets.end(),
                                    mit,
                                    message_frames);

        if (messages.empty()) {
            return;
        }

        shared_ptr<message::message> m = *messages.begin();


        // now process the message

        // don't start sessions on keep alive messages
        if (m->get_type() == message::KEEPALIVE) {
            return;
        }

        // if this is a new session initiated by the other end
        if (m->get_type() != message::ACKNOWLEDGE) {
            uint32_t session_id = _sessions->new_session();
            _sessions->update(session_id, endpoint, packet.session_id);

            // notify interested parties
            _new_session_signal(session_id, endpoint, packet.session_id);

            return;
        }

        // this is the first ack for a session we initiated
        shared_ptr<message::acknowledge> ack
                                = static_pointer_cast<message::acknowledge>(m);

        BOOST_FOREACH(uint32_t packet_id, ack->packet_ids) {
            address_id ai(endpoint, packet_id);

            typename std::map<address_id, uint32_t>::iterator it
                                                = _opening_sessions.find(ai);

            if (it == _opening_sessions.end()) {
                continue;
            }

            uint32_t session_id = it->second;

            // ok, so now we know the remote address and remote session id
            // for this session that we opened
            _sessions->update(session_id, endpoint, packet.session_id);

            _opening_sessions.erase(it);
            _opening_session_condition.notify_all();
        }
    }

    /*! Process a received acknowledge message by removing acknowledged
        packets from the resend queue.

        \param m the acknowledge message
        \param address the remote endpoint which sent the message
        \param session_id the remote session the message is part of
     */
    void process_acknowledge(shared_ptr<mxp::message::message>  m,
                             asio::ip::udp::endpoint            address,
                             uint32_t                           session_id) {

        lock_guard<recursive_mutex> lock(_mutex);

        shared_ptr<message::acknowledge> ack
                                = static_pointer_cast<message::acknowledge>(m);

        BOOST_FOREACH(uint32_t p_id, ack->packet_ids) {
            if (_sessions->contains(address, session_id)) {
                shared_ptr<session_t> s = _sessions->get(address, session_id);
                s->packets_pending_ack().erase(p_id);
            }
        }
    }

    /*! The periodic execution at each tick - will housekeeping, like
        sending ack messages, resending packets in need, kicking dormant
        sessions, and most importantly, send pending messages.
     */
    void ticker() {
        posix_time::ptime sleep_until =
                  posix_time::microsec_clock::universal_time() + TICK_INTERVAL;

        while (_should_run) {
            this_thread::sleep(sleep_until);

            try {
                lock_guard<recursive_mutex> lock(_mutex);

                kick_dormant_sessions();

                BOOST_FOREACH(shared_ptr<session_t> s, *_sessions) {
                    generate_keepalive_messages(s);
                    generate_acknowledge_messages(s);
                    send_pending_messages(s);
                    resend_packets(s);
                }
            } catch (...) {
                // make sure the thread is not exited on any exceptions
            }

            sleep_until += TICK_INTERVAL;
        }
    }

    /*! Terminate sessions which were inactive for a too long time. */
    void kick_dormant_sessions() {
        lock_guard<recursive_mutex> lock(_mutex);

        const posix_time::ptime kick_if_before =
                                    posix_time::microsec_clock::universal_time()
                                  - (KEEPALIVE_INTERVAL * 3);
        std::vector<uint32_t> close_session_ids;

        BOOST_FOREACH(shared_ptr<session_t> s, *_sessions) {
            if (s->last_message_time() < kick_if_before) {
                close_session_ids.push_back(s->session_id());
            }
        }

        BOOST_FOREACH(uint32_t session_id, close_session_ids) {
            close_session(session_id);
        }
    }

    /*! Generate keep alive messages for active sessions, if needed.

        \param session the session to generate the keepalive messages for
     */
    void generate_keepalive_messages(shared_ptr<session_t> session) {
        lock_guard<recursive_mutex> lock(_mutex);

        const posix_time::ptime send_if_before =
                                posix_time::microsec_clock::universal_time()
                              - KEEPALIVE_INTERVAL;

        if (session->last_keepalive() < send_if_before) {
            shared_ptr<message::message> ka(new message::keepalive());

            send(ka, session->session_id(), true);
            session->last_keepalive(
                                posix_time::microsec_clock::universal_time());
        }
    }

    /*! Generate acknowledge messages for guaranteed packets received.

        \param session the session to generate the acknowledge messages for.
     */
    void generate_acknowledge_messages(shared_ptr<session_t> session) {

        lock_guard<recursive_mutex> lock(_mutex);

        const posix_time::ptime ack_if_before =
                    posix_time::microsec_clock::universal_time() - ACK_INTERVAL;
        typename session_t::packet_id_list & packets_to_ack
                                                    = session->packets_to_ack();

        if (packets_to_ack.empty()) {
            return;
        }

        if (session->last_ack_time() < ack_if_before) {
            std::vector<shared_ptr<message::message> >  acks;
            shared_ptr<message::acknowledge>    ack(new message::acknowledge());
            unsigned int                        ix = 0u;
            typename session_t::packet_id_list::iterator it
                                                      = packets_to_ack.begin();

            while (it != packets_to_ack.end()) {
                unsigned int size =
                  (std::min)((unsigned int) (packets_to_ack.size() - ix),
                         (unsigned int) message::acknowledge::MAX_PACKET_IDS);

                ack->packet_ids.resize(size);
                std::copy(it, it + size, ack->packet_ids.begin());

                acks.push_back(ack);

                ack.reset(new message::acknowledge());
                it += size;
                ix += size;
            }

            send(acks.begin(), acks.end(), session->session_id(), false);
            packets_to_ack.clear();
            session->last_ack_time(
                                  posix_time::microsec_clock::universal_time());
        }
    }

    /*! Send pending messages to the remote end.

        \param session the sesssion to send messages for.
     */
    void send_pending_messages(shared_ptr<session_t> session) {

        lock_guard<recursive_mutex> lock(_mutex);

        std::vector<packet::packet>                     packets;
        std::vector<packet::packet>                     guaranteed_packets;
        std::vector<shared_ptr<message::message> >      messages;

        typename session_t::packet_id_packet_map & packets_pending_ack
                                               = session->packets_pending_ack();

        BOOST_FOREACH(message_queue_item & mqi, session->message_queue()) {
            messages.clear();
            messages.push_back(mqi.message());

            if (mqi.guaranteed()) {
                std::back_insert_iterator<std::vector<packet::packet> > pit =
                                       std::back_inserter(guaranteed_packets);

                messages_to_packets(messages.begin(),
                                    messages.end(),
                                    session->next_message_id(),
                                    pit,
                                    session->session_id(),
                                    0, // packet id, set by send()
                                    posix_time::second_clock::universal_time(),
                                    (uint8_t) 0x01u);
            } else {
                std::back_insert_iterator<std::vector<packet::packet> > pit =
                                                    std::back_inserter(packets);

                messages_to_packets(messages.begin(),
                                    messages.end(),
                                    session->next_message_id(),
                                    pit,
                                    session->session_id(),
                                    0, // packet id, set by send()
                                    posix_time::second_clock::universal_time(),
                                    (uint8_t) 0x00u);
            }
        }

        session->message_queue().clear();

        BOOST_FOREACH(packet::packet & p, packets) {
            _connection.send(session->address(), p);
        }
        BOOST_FOREACH(packet::packet & p, guaranteed_packets) {
            _connection.send(session->address(), p);
            packets_pending_ack[p.packet_id] = p;
        }
    }

    /*! Resend packets for which no acknowledgements were received in time.

        \param session the session to resend the packets for.
     */
    void resend_packets(shared_ptr<session_t> session) {

        lock_guard<recursive_mutex> lock(_mutex);

        const posix_time::ptime now =
                                posix_time::microsec_clock::universal_time();
        typename session_t::packet_id_packet_map & packets_pending_ack
                                              = session->packets_pending_ack();

        BOOST_FOREACH(typename session_t::packet_id_packet_map::value_type &
                                                                        entry,
                      packets_pending_ack) {

            packet::packet & p = entry.second;

            if (p.first_send_time
              < now - (ACK_TIMEOUT * (p.resend_count + 1u))) {

                if (p.resend_count > MAX_PACKET_RESEND_COUNT) {
                    packets_pending_ack.erase(p.packet_id);
                } else {
                    p.resend_count++;
                    _connection.send(session->address(), p);
                }
            }
        }
    }


public:
    /*! The frequency of sending packets. */
    const posix_time::time_duration TICK_INTERVAL;

    /*! The frequency of sending keep alive messages. */
    const posix_time::time_duration KEEPALIVE_INTERVAL;

    /*! The frequency of sending acknowledgement messages. */
    const posix_time::time_duration ACK_INTERVAL;

    /*! The amount of time to wait before resending a packet. */
    const posix_time::time_duration ACK_TIMEOUT;

    /*! The maximum number of resends of a packet, after which the packet is
        simply silently discarded.
     */
    const unsigned int MAX_PACKET_RESEND_COUNT;


    /*! Create a communication object that binds to a specific network
        interface, and uses a supplied session store.

        \param endpoint the network endpoint to bind to.
        \param sessions the session store to use for storing sessions.
        \throws boost::system::system_error on errors, like the specified port
                is already in use
     */
    basic_communication(ip::udp::endpoint             endpoint,
                        shared_ptr<session_store_t>   sessions)
            : _connection(endpoint),
              _sessions(sessions),
              _should_run(false),
              TICK_INTERVAL(posix_time::milliseconds(10)),
              KEEPALIVE_INTERVAL(posix_time::milliseconds(200)),
              ACK_INTERVAL(posix_time::milliseconds(200)),
              ACK_TIMEOUT(posix_time::milliseconds(1000)),
              MAX_PACKET_RESEND_COUNT(3u) {

        _connection.connect_packet_handler(
                bind(&basic_communication::packet_received, this, _1, _2));

        _should_run = true;
        // add bind around the function call as a workaround for a Sun CC
        // internal compiler bug, see
        // http://bugs.sun.com/bugdatabase/view_bug.do?bug_id=6855014
        thread t(bind(&basic_communication::ticker, this));
        _ticker_thread.swap(t);
    }

    /*! Create a communication object that binds to a specific network
        interface, and uses a default implementation of the session store.

        \param endpoint the network endpoint to bind to.
        \throws boost::system::system_error on errors, like the specified port
                is already in use
     */
    basic_communication(ip::udp::endpoint            endpoint)
            : _connection(endpoint),
              _sessions(new session_store_t()),
              _should_run(false),
              TICK_INTERVAL(posix_time::milliseconds(10)),
              KEEPALIVE_INTERVAL(posix_time::milliseconds(200)),
              ACK_INTERVAL(posix_time::milliseconds(200)),
              ACK_TIMEOUT(posix_time::milliseconds(1000)),
              MAX_PACKET_RESEND_COUNT(3u) {

        _connection.connect_packet_handler(
                bind(&basic_communication::packet_received, this, _1, _2));

        _should_run = true;
        // add bind around the function call as a workaround for a Sun CC
        // internal compiler bug, see
        // http://bugs.sun.com/bugdatabase/view_bug.do?bug_id=6855014
        thread t(bind(&basic_communication::ticker, this));
        _ticker_thread.swap(t);
    }

    /*! Create a communication object that binds to all local network
        interfaces on a random port, and uses a supplied session store.

        \param sessions the session store to use for storing sessions.
        \throws boost::system::system_error on errors, like the specified port
                is already in use
     */
    basic_communication(shared_ptr<session_store_t>     sessions)
            : _connection(),
              _sessions(sessions),
              _should_run(false),
              TICK_INTERVAL(posix_time::milliseconds(10)),
              KEEPALIVE_INTERVAL(posix_time::milliseconds(200)),
              ACK_INTERVAL(posix_time::milliseconds(200)),
              ACK_TIMEOUT(posix_time::milliseconds(1000)),
              MAX_PACKET_RESEND_COUNT(3u) {

        _connection.connect_packet_handler(
                bind(&basic_communication::packet_received, this, _1, _2));

        _should_run = true;
        // add bind around the function call as a workaround for a Sun CC
        // internal compiler bug, see
        // http://bugs.sun.com/bugdatabase/view_bug.do?bug_id=6855014
        thread t(bind(&basic_communication::ticker, this));
        _ticker_thread.swap(t);
    }

    /*! Create a communication object that binds to all local network
        interfaces on a random port, and uses a default implementation of
        the session store.

        \throws boost::system::system_error on errors, like the specified port
                is already in use
     */
    basic_communication()
            : _connection(),
              _sessions(new session_store_t()),
              _should_run(false),
              TICK_INTERVAL(posix_time::milliseconds(10)),
              KEEPALIVE_INTERVAL(posix_time::milliseconds(200)),
              ACK_INTERVAL(posix_time::milliseconds(200)),
              ACK_TIMEOUT(posix_time::milliseconds(1000)),
              MAX_PACKET_RESEND_COUNT(3u) {

        _connection.connect_packet_handler(
                bind(&basic_communication::packet_received, this, _1, _2));

        _should_run = true;
        // add bind around the function call as a workaround for a Sun CC
        // internal compiler bug, see
        // http://bugs.sun.com/bugdatabase/view_bug.do?bug_id=6855014
        thread t(bind(&basic_communication::ticker, this));
        _ticker_thread.swap(t);
    }

    /*! Destructor. Stops & joins the background thread.
     */
    ~basic_communication() {
        _should_run = false;
        _ticker_thread.join();
    }

    /*! Connect a handler for new session events, opened by a remote party.
        The signature for the event handler is:

        \code
        void handler(boost::uint32_t                  session_id,
                     boost::asio::ip::udp::endpoint   endpoint
                     boost::uint32_t                  remote_session_id);
        \endcode

        Where session_id is the local session id of the new opened message,
        endpoint is the address of the remote host which opened the message,
        and remote_session_id is the session id at the remote end.

        One should disconnect the handler before it expires by calling
        disconnect() on the returned connection object.

        \param handler the event handler.
        \return the connection object, can be use to disconnect from this
                event handler by calling disconnect() on the returned object.
     */
    signals2::connection
    connect_new_session_handler(const new_session_signal::slot_type & handler) {
        return _new_session_signal.connect(handler);
    }

    /*! Connect a handler for session closing events, for sesions closed by the
        remote party.
        The signature for the event handler is:

        \code
        void handler(boost::uint32_t                  session_id,
                     boost::asio::ip::udp::endpoint   endpoint
                     boost::uint32_t                  remote_session_id);
        \endcode

        Where session_id is the local session id of the closed message,
        endpoint is the address of the remote host which closed the message,
        and remote_session_id is the session id at the remote end.

        One should disconnect the handler before it expires by calling
        disconnect() on the returned connection object.

        \param handler the event handler.
        \return the connection object, can be use to disconnect from this
                event handler by calling disconnect() on the returned object.
     */
    signals2::connection
    connect_closed_session_handler(
                            const new_session_signal::slot_type & handler) {
        return _closed_session_signal.connect(handler);
    }

    /*! Connect a handler to the events when messages are received.
        The signature for the event handler is:

        \code
        void packet_handler(boost::shared_ptr<mxp::message::message> message,
                            boost::uint32_t                          session_id,
                            boost::asio::ip::udp::endpoint           endpoint);
        \endcode

        Where message is a pointer to the message just received, session_id is
        the id of the session that sent the message, and endpoint is the
        other side which sent the message.

        One should disconnect the handler before it expires by calling
        disconnect() on the returned connection object.

        \param handler the event handler.
        \return the connection object, can be use to disconnect from this
                event handler by calling disconnect() on the returned object.
     */
    signals2::connection
    connect_message_handler(const message_signal::slot_type & handler) {
        return _message_signal.connect(handler);
    }

    /*! Access the session store that is used by this communication object.

        \return the session store used by this communication object.
     */
    shared_ptr<session_store_t> sessions() {
        return _sessions;
    }

    /*! Start a session by sending an initial message. This call blocks until
        an aknowledgement is received from the other side, and thus a session
        is established.
        Make sure to close the ssession after it is no longer needed.

        \param endpoint the remote endpoint to send the initial message to
        \param message the message to send, which is usually a join request
        \return the local session id of the session just started
        \throws std::invalid_argument on issues with the supplied message
        \throws boost::system::system_error on sending errors
        \see #close_session
     */
    uint32_t open_session(asio::ip::udp::endpoint           endpoint,
                          shared_ptr<mxp::message::message> message) {

        unique_lock<recursive_mutex>     lock(_mutex);

        // first convert the message into packets, and send the packets over
        typedef std::vector<shared_ptr<message::message> >  message_container;
        typedef std::vector<packet::packet>                 packet_container;

        message_container               messages;
        packet_container                packets;
        std::back_insert_iterator<packet_container> pit =
                                                std::back_inserter(packets);

        messages.push_back(message);
        uint32_t              session_id    = _sessions->new_session();
        shared_ptr<session_t> session       = _sessions->get(session_id);

        messages_to_packets(messages.begin(),
                            messages.end(),
                            session->next_message_id(),
                            pit,
                            session->session_id(),
                            0, // packet id, will be set by the send() call
                            posix_time::second_clock::universal_time(),
                            (uint8_t) 0x01u);

        if (packets.empty()) {
            throw std::invalid_argument(
                                    "message cannot be turned into packets");
        }

        // and now send all the packets we have
        BOOST_FOREACH(packet::packet & p, packets) {
            _connection.send(endpoint, p);
        }

        // store the opening sesion
        address_id ai(endpoint, packets[0].packet_id);
        _opening_sessions[ai] = session_id;

        // so far, so good. now wait for an acknowledgement from the remote
        // endpoint to come, for our first packet sent
        // TODO: resend?
        posix_time::ptime wait_until =
                    posix_time::microsec_clock::universal_time() + ACK_TIMEOUT;
        do {
            _opening_session_condition.timed_wait(lock, wait_until);

            // if we received an ack while sleeping, then our entry would
            // have been removed from the opening sessions map
            if (_opening_sessions.find(ai) == _opening_sessions.end()) {
                return session_id;
            }
        } while (posix_time::microsec_clock::universal_time() < wait_until);

        throw std::invalid_argument("session could not be opened in time");
    }

    /*! Close a session.

        \param session_id the local session id of the session to close.
        \throws std::invalid_argument if no session by the specified session_id
                exists
     */
    void close_session(uint32_t session_id) {
        lock_guard<recursive_mutex>     lock(_mutex);

        shared_ptr<session_t>     s = _sessions->get(session_id);

        _sessions->remove(session_id);

        // notify interested parties
        _closed_session_signal(session_id,
                               s->address(),
                               s->remote_session_id());
    }

    /*! Put a single message into the message queue, so that it is sent to the
        other end in the next iteration.

        \param message the message to send
        \param session_id the local session id, of which this message is part of
        \param guaranteed flag to indicate if the message is to be sent in a
               guaranteed way.
        \throws std::invalid_argument if no session by the specified session_id
                exists
     */
    void send(shared_ptr<mxp::message::message> message,
              uint32_t                          session_id,
              bool                              guaranteed) {

        lock_guard<recursive_mutex>     lock(_mutex);

        shared_ptr<session_t>     s = _sessions->get(session_id);
        simple_message_queue_item mqi(message, guaranteed);

        s->message_queue().push_back(mqi);
    }

    /*! Put a list of messages into the message queue, so that it is sent to the
        other end in the next iteration.

        \tparam message_iterator an iterator on
                shared_ptr<mxp::message::message> pointers
        \param messages_begin an iterator to the first message to send
        \param messages_end an iterator one beyond the last message to send
        \param session_id the local session id, of which this message is part of
        \param guaranteed flag to indicate if the message is to be sent in a
               guaranteed way.
        \throws std::invalid_argument if no session by the specified session_id
                exists
     */
    template<typename message_iterator>
    void send(message_iterator          messages_begin,
              message_iterator          messages_end,
              uint32_t                  session_id,
              bool                      guaranteed) {

        lock_guard<recursive_mutex>     lock(_mutex);

        shared_ptr<session_t> s = _sessions->get(session_id);

        for (message_iterator it = messages_begin; it != messages_end; ++it) {
            simple_message_queue_item mqi(*it, guaranteed);

            s->message_queue().push_back(mqi);
        }
    }
};



/*! A communication object that uses a simple session store as its session
    store, which stores simple_session objects.
 */
typedef basic_communication<simple_session_store, simple_session>
                                                                communication;


}
}

#endif
