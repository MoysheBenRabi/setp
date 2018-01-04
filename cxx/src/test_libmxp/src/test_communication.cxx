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
#include <set>
#include <boost/cstdint.hpp>
#include <boost/smart_ptr.hpp>
#include <boost/bind.hpp>
#include <boost/thread.hpp>
#include <boost/asio.hpp>
#include <boost/test/unit_test.hpp>

#include <mxp/message.h>
#include <mxp/net.h>

#include "reference_messages.h"
#include "test_communication.h"

namespace mxp {
namespace test {
namespace net {
namespace communication {

using namespace boost;
using namespace mxp;
using namespace mxp::test::iot;

class message_handler {
public:
    std::set<uint32_t>                              open_sessions;

    std::vector<shared_ptr<mxp::message::message> > messages;
    std::vector<uint32_t>                           session_ids;
    std::vector<asio::ip::udp::endpoint>            endpoints;

    void new_session_handler(uint32_t                   session_id,
                             asio::ip::udp::endpoint    ep,
                             uint32_t                   remote_session_id) {

        open_sessions.insert(session_id);
    }

    void closed_session_handler(uint32_t                   session_id,
                                asio::ip::udp::endpoint    ep,
                                uint32_t                   remote_session_id) {

        open_sessions.erase(session_id);
    }

    void msg_handler(shared_ptr<mxp::message::message>  message,
                     uint32_t                           session_id,
                     asio::ip::udp::endpoint            ep) {

        messages.push_back(message);
        session_ids.push_back(session_id);
        endpoints.push_back(ep);
    }
};


void test_connection() {
    // create the server
    message_handler             server_message_handler;
    asio::ip::udp::endpoint     server_endpoint(asio::ip::udp::v4(), 5665);

    mxp::net::communication     server(server_endpoint);
    server.connect_message_handler(bind(&message_handler::msg_handler,
                                        &server_message_handler, _1, _2, _3));
    server.connect_new_session_handler(
            bind(&message_handler::new_session_handler,
                 &server_message_handler, _1, _2, _3));
    server.connect_closed_session_handler(
            bind(&message_handler::closed_session_handler,
                 &server_message_handler, _1, _2, _3));

    // create the client
    message_handler             client_message_handler;

    mxp::net::communication     client;
    client.connect_message_handler(bind(&message_handler::msg_handler,
                                        &client_message_handler, _1, _2, _3));

    asio::io_service                ioservice;
    asio::ip::udp::resolver         resolver(ioservice);
    asio::ip::udp::resolver::query  query(asio::ip::udp::v4(),
                                          "localhost", "5665");
    asio::ip::udp::endpoint         server_address = *resolver.resolve(query);


    // open a session by sending a join request message
    shared_ptr<mxp::message::message> msg =  generate_join_rq();
    uint32_t    session_id        = client.open_session(server_address, msg);
    uint32_t    remote_session_id = client.sessions()->
                                        get(session_id)->remote_session_id();

    BOOST_CHECK(client.sessions()->contains(session_id));
    BOOST_CHECK_EQUAL(server_message_handler.messages.size(), 1u);
    BOOST_CHECK_EQUAL(**server_message_handler.messages.begin(), *msg);
    BOOST_CHECK_EQUAL(server.sessions()->size(), 1u);
    BOOST_CHECK(server.sessions()->contains(remote_session_id));
    BOOST_CHECK(server_message_handler.open_sessions.find(remote_session_id)
                != server_message_handler.open_sessions.end());

    // now close the session
    client.close_session(session_id);

    BOOST_CHECK(!client.sessions()->contains(session_id));
}

void test_bad_connection() {
    // create the client
    message_handler             client_message_handler;

    mxp::net::communication     client;
    client.connect_message_handler(bind(&message_handler::msg_handler,
                                        &client_message_handler, _1, _2, _3));

    asio::io_service                ioservice;
    asio::ip::udp::resolver         resolver(ioservice);
    asio::ip::udp::resolver::query  query(asio::ip::udp::v4(),
                                          "localhost", "5665");
    asio::ip::udp::endpoint         server_address = *resolver.resolve(query);


    // try to open a session to a non-existent server
    shared_ptr<mxp::message::message> msg =  generate_join_rq();

    BOOST_CHECK_THROW(client.open_session(server_address, msg),
                      std::invalid_argument);
}

void test_single_message_roundtrip() {
    // create the server
    message_handler             server_message_handler;
    asio::ip::udp::endpoint     server_endpoint(asio::ip::udp::v4(), 5665);

    mxp::net::communication     server(server_endpoint);
    server.connect_message_handler(bind(&message_handler::msg_handler,
                                        &server_message_handler, _1, _2, _3));

    // create the client
    message_handler             client_message_handler;

    mxp::net::communication     client;
    client.connect_message_handler(bind(&message_handler::msg_handler,
                                        &client_message_handler, _1, _2, _3));

    asio::io_service                ioservice;
    asio::ip::udp::resolver         resolver(ioservice);
    asio::ip::udp::resolver::query  query(asio::ip::udp::v4(),
                                          "localhost", "5665");
    asio::ip::udp::endpoint         server_address = *resolver.resolve(query);


    // open a session by sending a join request message
    shared_ptr<mxp::message::message> msg =  generate_join_rq();
    uint32_t    session_id = client.open_session(server_address, msg);

    BOOST_CHECK(client.sessions()->contains(session_id));
    BOOST_CHECK_EQUAL(server_message_handler.messages.size(), 1u);
    BOOST_CHECK_EQUAL(**server_message_handler.messages.begin(), *msg);
    BOOST_CHECK_EQUAL(server.sessions()->size(), 1u);
    BOOST_CHECK(server.sessions()->contains(
                   client.sessions()->get(session_id)->remote_session_id()));


    // send a response from the server to the client
    shared_ptr<mxp::net::simple_session> client_session
                                        = client.sessions()->get(session_id);
    msg = generate_join_rsp();
    server.send(msg, client_session->remote_session_id(), false);

    this_thread::yield();
    this_thread::sleep(posix_time::milliseconds(100));

    BOOST_CHECK_EQUAL(client_message_handler.messages.size(), 1u);
    BOOST_CHECK_EQUAL(**client_message_handler.messages.begin(), *msg);

    // close the session
    client.close_session(session_id);

    BOOST_CHECK(!client.sessions()->contains(session_id));
}

void test_keepalive() {
    // create the server
    message_handler             server_message_handler;
    asio::ip::udp::endpoint     server_endpoint(asio::ip::udp::v4(), 5665);

    mxp::net::communication     server(server_endpoint);
    server.connect_message_handler(bind(&message_handler::msg_handler,
                                        &server_message_handler, _1, _2, _3));

    // create the client
    message_handler             client_message_handler;

    mxp::net::communication     client;
    client.connect_message_handler(bind(&message_handler::msg_handler,
                                        &client_message_handler, _1, _2, _3));

    asio::io_service                ioservice;
    asio::ip::udp::resolver         resolver(ioservice);
    asio::ip::udp::resolver::query  query(asio::ip::udp::v4(),
                                          "localhost", "5665");
    asio::ip::udp::endpoint         server_address = *resolver.resolve(query);


    // open a session by sending a join request message
    shared_ptr<mxp::message::message> msg =  generate_join_rq();
    uint32_t    session_id = client.open_session(server_address, msg);

    BOOST_CHECK(client.sessions()->contains(session_id));
    BOOST_CHECK_EQUAL(server_message_handler.messages.size(), 1u);
    BOOST_CHECK_EQUAL(**server_message_handler.messages.begin(), *msg);
    BOOST_CHECK_EQUAL(server.sessions()->size(), 1u);
    BOOST_CHECK(server.sessions()->contains(
                   client.sessions()->get(session_id)->remote_session_id()));


    // now wait and see that the session is kept alive, even if no
    // messages are actively sent
    shared_ptr<mxp::net::simple_session> client_session
                                        = client.sessions()->get(session_id);
    shared_ptr<mxp::net::simple_session> server_session
                 = server.sessions()->get(client_session->remote_session_id());

    posix_time::ptime client_last = client_session->last_message_time();
    posix_time::ptime server_last = server_session->last_message_time();

    for (unsigned int i = 0; i < 5; ++i) {
        this_thread::yield();
        this_thread::sleep(server.KEEPALIVE_INTERVAL * 2);

        BOOST_CHECK(client_last < client_session->last_message_time());
        BOOST_CHECK(server_last < server_session->last_message_time());

        client_last = client_session->last_message_time();
        server_last = server_session->last_message_time();
    }

    // now close the session from one side, and see it expire on the other
    // side too
    client.close_session(session_id);
    BOOST_CHECK(!client.sessions()->contains(session_id));
    BOOST_CHECK(server.sessions()->contains(server_session->session_id()));

    this_thread::yield();
    this_thread::sleep(server.KEEPALIVE_INTERVAL * 4);

    BOOST_CHECK(!server.sessions()->contains(server_session->session_id()));
}



}
}
}
}
