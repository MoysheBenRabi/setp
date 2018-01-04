#ifndef MXP_NET_H
#define MXP_NET_H

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

/** \namespace mxp::net

    The mxp::net namespace provides network communication facilities for the
    MXP implementation.

    The typical client user will create an mxp::net::communication
    object, that connects to a server, and send and receive messages from
    the mxp::message namespace, using this client object.

    \code
    void message_received(boost::shared_ptr<mxp::message::message> message,
                          boost::uint32_t                          session_id,
                          boost::asio::ip::udp::endpoint           endpoint) {
        // handle received messages here
    }

    // sample code to create a client communication object
    mxp::net::communication  client;

    // connect the event handler for received messages
    client.connect_message_handler(&message_received);

    // open a session towards a server at 'myserver' listening on port '3456'
    // resolve the hostname and the port to an endpoint
    asio::io_service                ioservice;
    asio::ip::udp::resolver         resolver(ioservice);
    asio::ip::udp::resolver::query  query(asio::ip::udp::v4(),
                                          "localhost", "3456");
    asio::ip::udp::endpoint         server_address = *resolver.resolve(query);

    // open the session using an MXP join request message
    shared_ptr<mxp::message::message> msg =  generate_join_rq();
    uint32_t    session_id = client.open_session(server_address, msg);

    // send an additional message
    msg = // generate some other message
    client.send(msg, session_id, true);

    // close the session
    client.close_session(session_id);
    \endcode

    The typical server user of this library will create an
    mxp::net::communication object if he wants to run a server,
    and use it send and receive messages from the mxp::message namespace.

    \code
    void message_received(boost::shared_ptr<mxp::message::message> message,
                          boost::uint32_t                          session_id,
                          boost::asio::ip::udp::endpoint           endpoint) {
        // handle received messages here
    }

    // sample code to bind to port 3456
    message_handler             server_message_handler;
    asio::ip::udp::endpoint     server_endpoint(asio::ip::udp::v4(), 3456);

    mxp::net::communication     server(server_endpoint);

    // connect the event handler for received messages
    server.connect_message_handler(&message_received);

    // TODO: session opening notification?

    // send a message to a client
    uint32_t                                    session_id;
    boost::shared_ptr<mxp::message::message>    message;
    // populate session and message, then send the message via the session
    // the client endpoint is contained in the session object

    server.send(message, session_id, true);
    \endcode

    \see communication
    \see communication
    \see mxp::message
  */

#include <mxp/net/connection.h>
#include <mxp/net/session.h>
#include <mxp/net/session_store.h>
#include <mxp/net/simple_session_store.h>
#include <mxp/net/communication.h>

#endif
