#ifndef MXP_NET_CONNECTION_H
#define MXP_NET_CONNECTION_H

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
#include <boost/cstdint.hpp>
#include <boost/lexical_cast.hpp>
#include <boost/array.hpp>
#include <boost/bind.hpp>
#include <boost/thread.hpp>
#include <boost/signals2.hpp>
#include <boost/asio.hpp>

#include <mxp/packet.h>

namespace mxp {
namespace net {

using namespace boost;
using namespace boost::asio;
using namespace mxp;

/*! A connection object, which can send and receive packets to and from
    several sessions. It will notify the interested parties when
    receiving any incomming packets.

    A typical client usage of this class would be to instantiate it without
    a specific interface to bind to - so that the implementation selects
    a random port on all local network interfaces. A server user of this
    class would want to bind to a specific port, so that incoming clients
    have somewhere known to connect to.

    As MXP is a UDP based protocol, packet sending and reception is not
    guaranteed.
 */
class connection {
public:
    /*! Signal type for packet received events.
        The signature for the event handler is:

        \code
        void packet_handler(mxp::packet::packet             packet,
                            boost::asio::ip::udp::endpoint  endpoint);
        \endcode

        Where packet is the packet just received, and endpoint is the
        other side which sent the packet.
     */
    typedef signals2::signal<void (packet::packet, ip::udp::endpoint)>
                                                                packet_signal;

private:
    /*! An io_service, handling all the network events. */
    io_service _io_service;

    /*! The socket this server is bound to. We're listening for
        incoming packets on this socket.
     */
    ip::udp::socket   _socket;

    /*! The data buffer we're expecting data into. */
    array<uint8_t, 1500> _data;

    /*! This will contain the remote endpoint, as packets are received. */
    ip::udp::endpoint _remote_endpoint;

    /*! The signal for sending notifications on receiving packets. */
    packet_signal _packet_signal;

    /*! Flag to signal if our thread function should be running. */
    bool _should_run;

    /*! The thread object, representing the background thread doing all
        the network stuff.
     */
    thread _thread;

    /*! A mutex, making the unique packet counter thread safe. */
    recursive_mutex _mutex;

    /*! The last packet id used. */
    uint32_t _last_packet_id;

    /*! The thread function for the background thread, keeping the enclosed
        io_service alive.
     */
    void thread_func() {
        while (_should_run) {
            _io_service.run();

            // if we exit, reset communication related structures
            _io_service.reset();
        }
    }

    /*! Start to receive data packets on the socket bound. */
    void start_to_receive() {
        _socket.async_receive_from(buffer(_data),
                                   _remote_endpoint,
                                   bind(&connection::receive_handler,
                                        this,
                                        placeholders::error,
                                        placeholders::bytes_transferred));
    }

    /*! Handle the event of us having received a data packet from a client.

        \param error the network error in relation to listening for incoming
               traffic
        \param len the length of the data received
     */
    void receive_handler(const boost::system::error_code & error,
                         std::size_t len) {
        if (error) {
            // well, can't really do much in case on a reception error
            return;
        }

        // make a copy of all relevant data
        std::vector<uint8_t> data(_data.begin(), _data.begin() + len);
        ip::udp::endpoint    ep(_remote_endpoint);

        // start to receive again, as fast as possible
        start_to_receive();

        // process the data & notify whoever is interested
        notify_packet_handler(data, ep);
    }

    /*! Notify all interested parties of a new packet received.

        \param data the data that was received from someone
        \param endpoint the network details of the other party
     */
    void notify_packet_handler(const std::vector<uint8_t> & data,
                               const ip::udp::endpoint    & endpoint) {
        packet::packet p;
        std::vector<uint8_t>::const_iterator begin = data.begin();
        p.deserialize(begin, data.size());

        // send a signal to all interested parties with the new packet
        _packet_signal(p, endpoint);
    }

public:
    /*! Create a connection object with a specific local interface to bind to.

        \param endpoint the network interface to bind to.
        \throws boost::system::system_error on errors, like the endpoint
                cannot be connected to
     */
    connection(ip::udp::endpoint & endpoint)
            : _io_service(),
              _socket(_io_service, endpoint),
              _should_run(false),
              _last_packet_id(0u) {

        _should_run = true;
        // add bind around the function call as a workaround for a Sun CC
        // internal compiler bug, see
        // http://bugs.sun.com/bugdatabase/view_bug.do?bug_id=6855014
        thread t(bind(&connection::thread_func, this));
        _thread.swap(t);

        start_to_receive();
    }

    /*! Create a connection object which binds to all local network interfaces,
        on a random port number.

        \throws boost::system::system_error on errors, like the specified
                hostname does not exist, or it cannot be connected to
     */
    connection() : _io_service(),
                   _socket(_io_service, ip::udp::endpoint(ip::udp::v4(), 0)),
                   _should_run(false),
                   _last_packet_id(0u) {

        _should_run = true;
        // add bind around the function call as a workaround for a Sun CC
        // internal compiler bug, see
        // http://bugs.sun.com/bugdatabase/view_bug.do?bug_id=6855014
        thread t(bind(&connection::thread_func, this));
        _thread.swap(t);

        start_to_receive();
    }

    /*! Destructor. Stops & joins the background thread, which is doing the
        network I/O.
     */
    ~connection() {
        _should_run = false;
        _io_service.stop();
        _thread.join();
    }

    /*! Provide access to the socket we're listening on.

        \return the socket we're listening on.
     */
    const ip::udp::socket & socket() const {
        return _socket;
    }

    /*! Connect a handler to the events when packets are received.
        The signature for the event handler is:

        \code
        void packet_handler(packet::packet packet, ip::udp::endpoint endpoint);
        \endcode

        Where packet is the packet just received, and endpoint is the
        other side which sent the packet.

        One should disconnect the handler before it expires by calling
        disconnect() on the returned connection object.

        \param handler the event handler.
        \return the connection object, can be use to disconnect from this
                event handler by calling disconnect() on the returned object.
     */
    signals2::connection
    connect_packet_handler(const packet_signal::slot_type & handler) {
        return _packet_signal.connect(handler);
    }

    /*! Send a packet to a remote party.
        The id field of the supplied message is going to be discarded, and
        a unique id will be set for the packet.

        \param endpoint the network endpoint to send the packet to.
        \param packet the packet to send
        \throws boost::system::system_error on sending errors
     */
    void send(ip::udp::endpoint         endpoint,
              packet::packet          & packet) {

        {
            lock_guard<recursive_mutex>     lock(_mutex);

            packet.packet_id = ++_last_packet_id;
        }

        unsigned int len = packet.size();
        uint8_t data[packet::packet::max_size];
        uint8_t *p = data;
        packet.serialize(p);

        if (p - data == (signed int) len) {
            // this is sync for the time being
            // could turn async, but then the created buffer has to be stored
            // until the send callback is called

            _socket.send_to(buffer(data, len), endpoint);
        }
    }
};


}
}

#endif
