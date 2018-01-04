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
#include <stdexcept>
#include <iostream>
#include <boost/smart_ptr.hpp>
#include <boost/format.hpp>
#include <boost/thread.hpp>
#include <boost/asio.hpp>

#include <mxp.h>

#include "generate_messages.h"

using namespace boost;
using namespace mxp;


/*! A very simple message handler class, that just stored the message
    details as they are received.
 */
class message_handler {
public:
    std::vector<shared_ptr<message::message> > messages;
    std::vector<uint32_t>                      session_ids;
    std::vector<asio::ip::udp::endpoint>       endpoints;

    void handler(shared_ptr<message::message>  message,
                 uint32_t                      session_id,
                 asio::ip::udp::endpoint       ep) {

        messages.push_back(message);
        session_ids.push_back(session_id);
        endpoints.push_back(ep);
    }
};


/*! Wait for a specific type of message to be received.

    \param handler the message handler to check for received messages
    \param response_type the type of message to wait for
    \param timeout the maximum amount of time to wait for
    \throws std::invalid_argument if the timeout is reached, and the
            requested session type is not received.
 */
static shared_ptr<message::message>
wait_for_message(message_handler          & handler,
                 message::type              response_type,
                 posix_time::time_duration  timeout) {

    this_thread::yield();
    this_thread::sleep(timeout);

    unsigned int limit = handler.messages.size();
    for (unsigned int i = 0; i < limit; ++i) {
        if (handler.messages[i]->get_type() == response_type) {
            return handler.messages[i];
        }
    }

    throw std::invalid_argument(
                   str(format("timeout exceeded waiting for message type %d")
                               % response_type));
}

/*! Exchange reference messages

    \param hostname the name of the IOT server to connect to
    \param port the port at the IOT server to connect to
    \throws std::invalid_argument on issues
*/
static void exchange_reference_messages(const char *hostname,
                                        const char *port) {

    asio::io_service                ioservice;
    asio::ip::udp::resolver         resolver(ioservice);
    asio::ip::udp::resolver::query  query(asio::ip::udp::v4(),
                                          hostname, port);
    asio::ip::udp::endpoint         server_address = *resolver.resolve(query);


    message_handler                 client_message_handler;
    shared_ptr<message::message>    response;
    shared_ptr<message::join_rsp>   join_response;
    uint32_t                        session_id;
    message::response_fragment    * rh;
    message::object_fragment      * of;
    message::object_fragment        off;

    const posix_time::time_duration     timeout = posix_time::milliseconds(200);

    mxp::net::communication     client;
    client.connect_message_handler(bind(&message_handler::handler,
                                        &client_message_handler, _1, _2, _3));


    // connect using a join request message
    shared_ptr<message::message> msg =  generate_join_rq();
    session_id    = client.open_session(server_address, msg);
    response = wait_for_message(client_message_handler,
                                message::JOIN_RSP,
                                timeout);
    join_response = static_pointer_cast<message::join_rsp>(response);


    // send an inject request message, and anticipate an inject response
    client.send(generate_inject_rq(), session_id, true);
    response = wait_for_message(client_message_handler,
                                message::INJECT_RSP,
                                timeout);
    rh = &(static_pointer_cast<message::inject_rsp>(response)->response_header);
    if (rh->failure_code != 0) {
        throw std::invalid_argument("failure code does not match");
    }

    // wait for a sync begin event
    response = wait_for_message(client_message_handler,
                                message::SYNCHRONIZATION_BEGIN_EVENT,
                                timeout * 2);
    if (static_pointer_cast<message::synchronization_begin_event>(response)
                                                        ->object_count != 1u) {
        throw std::invalid_argument("object count does not match");
    }

    // wait for a perception event
    response = wait_for_message(client_message_handler,
                                message::PERCEPTION_EVENT,
                                timeout);
    of = &(static_pointer_cast<message::perception_event>(response)
                                                              ->object_header);
    init_object_fragment(off);
    off.owner_id = join_response->participant_id;
    if (!(*of == off)) {
        throw std::invalid_argument("object fragments does not match");
    }

    // wait for a sync end event
    response = wait_for_message(client_message_handler,
                                message::SYNCHRONIZATION_END_EVENT,
                                timeout);


    // send a modify request and expect a modify response
    client.send(generate_modify_rq(), session_id, true);
    response = wait_for_message(client_message_handler,
                                message::MODIFY_RSP,
                                timeout);
    rh = &(static_pointer_cast<message::modify_rsp>(response)->response_header);
    if (rh->failure_code != 0) {
        throw std::invalid_argument("failure code does not match");
    }

    // send an interaction request message
    client.send(generate_interact_rq(), session_id, true);
    // interact response is not implemented on the test server...

    // send an examine request message
    client.send(generate_examine_rq(), session_id, true);
    // examine response is not implemented on the test server...

    // send an eject request message, and wait for a response
    client.send(generate_eject_rq(), session_id, true);
    response = wait_for_message(client_message_handler,
                                message::EJECT_RSP,
                                timeout);
    rh = &(static_pointer_cast<message::modify_rsp>(response)->response_header);
    if (rh->failure_code != 0) {
        throw std::invalid_argument("failure code does not match");
    }

    // after an eject, see that a disappearance event is sent
    response = wait_for_message(client_message_handler,
                                message::DISAPPEARANCE_EVENT,
                                timeout);
    if (static_pointer_cast<message::disappearance_event>(response)
                                                        ->object_index != 1u) {
        throw std::invalid_argument("disappearance object id does not match");
    }

    // send a leave request message, and wait for a response
    client.send(generate_leave_rq(), session_id, true);
    response = wait_for_message(client_message_handler,
                                message::LEAVE_RSP,
                                timeout);
    rh = &(static_pointer_cast<message::modify_rsp>(response)->response_header);
    if (rh->failure_code != 0) {
        throw std::invalid_argument("failure code does not match");
    }

    // that's all, folks!

    client.close_session(session_id);
}


/*! Program entry point

  \param argc the number of command line arguments
  \param argv the command line arguments. two arguments are expected: the
         IOT server host name and port to connect to.
  \return 0 on successful execution, <0 on failures
*/
int main(int argc, char * argv[]) {
    if (argc != 3) {
        std::cerr << "Usage: " << argv[0] << " host port" << std::endl
                << std::endl;

        return -1;
    }

    try {
        exchange_reference_messages(argv[1], argv[2]);
    } catch (const std::invalid_argument & e) {
        std::cerr << e.what() << std::endl;

        return -2;
    }

    return 0;
}
