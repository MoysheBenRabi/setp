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
#include <boost/lexical_cast.hpp>
#include <boost/bind.hpp>
#include <boost/thread.hpp>
#include <boost/date_time.hpp>
#include <boost/signals2.hpp>
#include <boost/asio.hpp>
#include <boost/test/unit_test.hpp>

#include <mxp/packet.h>
#include <mxp/net.h>

#include "test_packet.h"
#include "test_connection.h"

namespace mxp {
namespace test {
namespace net {
namespace connection {

using namespace boost;
using namespace mxp;

class packet_handler {
public:
    std::vector<mxp::packet::packet>       packets;
    std::vector<asio::ip::udp::endpoint>   endpoints;

    void handler(mxp::packet::packet       p,
                 asio::ip::udp::endpoint   ep) {

        packets.push_back(p);
        endpoints.push_back(ep);
    }
};


void test_bad_server_connection() {
    asio::io_service io_service;

    asio::ip::udp::endpoint ep(asio::ip::udp::v4(), 5665);

    // bind to a port to make sure it's already taken
    asio::ip::udp::socket  socket(io_service, ep);

    // now let's try to bind to the same port using a server_connection object
    BOOST_CHECK_THROW(mxp::net::connection server(ep),
                      boost::system::system_error);
}

void test_single_packet_roundtrip() {
    packet_handler                  server_packet_handler;
    asio::ip::udp::endpoint         server_ep(asio::ip::udp::v4(), 5665);
    mxp::net::connection            server(server_ep);

    packet_handler                  client_packet_handler;
    mxp::net::connection            client;

    asio::io_service                ioservice;
    asio::ip::udp::resolver         resolver(ioservice);
    asio::ip::udp::resolver::query  query(asio::ip::udp::v4(),
                                          "localhost", "5665");
    asio::ip::udp::endpoint         server_address = *resolver.resolve(query);

    server.connect_packet_handler(bind(&packet_handler::handler,
                                       &server_packet_handler, _1, _2));
    client.connect_packet_handler(bind(&packet_handler::handler,
                                       &client_packet_handler, _1, _2));

    // send a packet from the client to the server
    mxp::packet::packet p = mxp::test::packet::packet::generate_test_packet();
    client.send(server_address, p);

    // wait and see that this arrives at the server
    boost::this_thread::yield();
    boost::this_thread::sleep(posix_time::milliseconds(100));

    BOOST_CHECK_EQUAL(1u, server_packet_handler.packets.size());
    BOOST_CHECK_EQUAL(p, server_packet_handler.packets[0]);
    BOOST_CHECK_EQUAL(1u, server_packet_handler.endpoints.size());
    BOOST_CHECK_EQUAL("127.0.0.1",
                      server_packet_handler.endpoints[0].address().to_string());

    // now send the same packet back, and see if it arrives
    server.send(server_packet_handler.endpoints[0], p);

    // wait and see that this arrives at the client
    boost::this_thread::yield();
    boost::this_thread::sleep(posix_time::milliseconds(100));

    BOOST_CHECK_EQUAL(1u, client_packet_handler.packets.size());
    BOOST_CHECK_EQUAL(p, client_packet_handler.packets[0]);
    BOOST_CHECK_EQUAL(1u, client_packet_handler.endpoints.size());
    BOOST_CHECK_EQUAL("127.0.0.1",
                      client_packet_handler.endpoints[0].address().to_string());

    BOOST_CHECK_EQUAL(5665u,
                      client_packet_handler.endpoints[0].port());
}

void test_multiple_packet_roundtrip() {
    packet_handler                  server_packet_handler;
    asio::ip::udp::endpoint         server_endpoint(asio::ip::udp::v4(), 5665);
    mxp::net::connection            server(server_endpoint);

    packet_handler                  client_packet_handler;
    mxp::net::connection            client;

    server.connect_packet_handler(bind(&packet_handler::handler,
                                       &server_packet_handler, _1, _2));
    client.connect_packet_handler(bind(&packet_handler::handler,
                                       &client_packet_handler, _1, _2));


    // generate some packets
    std::vector<mxp::packet::packet> to_client_packets;
    for (unsigned int i = 0; i < 5; ++i) {
        mxp::packet::packet p =
                            mxp::test::packet::packet::generate_test_packet();
        p.packet_id = i;

        to_client_packets.push_back(p);
    }

    std::vector<mxp::packet::packet> to_server_packets;
    for (unsigned int i = 0; i < 7; ++i) {
        mxp::packet::packet p =
                            mxp::test::packet::packet::generate_test_packet();
        p.packet_id = i;

        to_server_packets.push_back(p);
    }

    asio::io_service                ioservice;
    asio::ip::udp::resolver         resolver(ioservice);
    asio::ip::udp::resolver::query  query(asio::ip::udp::v4(),
                                          "localhost", "5665");
    asio::ip::udp::endpoint         server_address = *resolver.resolve(query);


    // send the first packet to the server
    client.send(server_address, to_server_packets[0]);

    // wait and see that this arrives at the server
    boost::this_thread::yield();
    boost::this_thread::sleep(posix_time::milliseconds(100));

    BOOST_CHECK_EQUAL(1u, server_packet_handler.packets.size());
    BOOST_CHECK_EQUAL(to_server_packets[0], server_packet_handler.packets[0]);
    BOOST_CHECK_EQUAL(1u, server_packet_handler.endpoints.size());
    BOOST_CHECK_EQUAL("127.0.0.1",
                      server_packet_handler.endpoints[0].address().to_string());

    asio::ip::udp::endpoint client_endpoint(server_packet_handler.endpoints[0]);

    // now send the rest of the packets interleaved
    server.send(client_endpoint, to_client_packets[0]);
    client.send(server_address, to_server_packets[1]);
    client.send(server_address, to_server_packets[2]);
    server.send(client_endpoint, to_client_packets[1]);
    client.send(server_address, to_server_packets[3]);
    server.send(client_endpoint, to_client_packets[2]);
    client.send(server_address, to_server_packets[4]);
    client.send(server_address, to_server_packets[5]);
    server.send(client_endpoint, to_client_packets[3]);
    client.send(server_address, to_server_packets[6]);
    server.send(client_endpoint, to_client_packets[4]);

    // wait and see that this arrives at the client
    boost::this_thread::yield();
    boost::this_thread::sleep(posix_time::milliseconds(100));

    BOOST_CHECK_EQUAL(5u, client_packet_handler.packets.size());
    for (unsigned int i = 0; i < 5; ++i) {
        BOOST_CHECK_EQUAL(to_client_packets[i],
                          client_packet_handler.packets[i]);
        BOOST_CHECK_EQUAL("127.0.0.1",
                     client_packet_handler.endpoints[i].address().to_string());
        BOOST_CHECK_EQUAL(5665u,
                          client_packet_handler.endpoints[i].port());
    }

    BOOST_CHECK_EQUAL(7u, server_packet_handler.endpoints.size());
    for (unsigned int i = 0; i < 7; ++i) {
        BOOST_CHECK_EQUAL(to_server_packets[i],
                          server_packet_handler.packets[i]);
        BOOST_CHECK_EQUAL("127.0.0.1",
                     server_packet_handler.endpoints[i].address().to_string());
    }
}


}
}
}
}
