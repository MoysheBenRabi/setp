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
#include <boost/bind.hpp>
#include <boost/asio.hpp>
#include <boost/uuid/uuid.hpp>

#include <mxp.h>

#include "server.h"

using namespace boost;
using namespace mxp;


/*! Flag to signal if we should be running. */
static bool should_run = true;

/*! A UUID generator. */
uuids::uuid_generator   id_generator;



/*! Run the simple server.

    \param port the port of the server to bind to
    \throws std::invalid_argument on issues
*/
static void run_server(const char *port) {

    // create the server
    server  s(lexical_cast<int>(port));

    // create a single bubble, and add it to the server
    array<float, 3>   center = {{ 0.0, 0.0, 0.0 }};
    uuids::uuid       id = uuids::uuid("16fa9d53-525b-9f4c-9a09-ad746520873e");

    shared_ptr<bubble>  b(new bubble(id, L"Test Bubble", center, 100.0, 50.0));

    s.add(b);

    std::cout << "server default bubble: " << s.default_bubble()->id()
              << std::endl;

    // run until interrupted here
    while (should_run) {
        this_thread::sleep(posix_time::milliseconds(100));
    }
}


/*! Program entry point

  \param argc the number of command line arguments
  \param argv the command line arguments. two arguments are expected: the
         IOT server host name and port to connect to.
  \return 0 on successful execution, <0 on failures
*/
int main(int argc, char * argv[]) {
    if (argc != 2) {
        std::cerr << "Usage: " << argv[0] << " port" << std::endl
                << std::endl;

        return -1;
    }

    // Block all signals for background thread.
    sigset_t new_mask;
    sigfillset(&new_mask);
    sigset_t old_mask;
    pthread_sigmask(SIG_BLOCK, &new_mask, &old_mask);

    // Run server in background thread.
    should_run = true;
    boost::thread t(boost::bind(&run_server, argv[1]));

    // Restore previous signals.
    pthread_sigmask(SIG_SETMASK, &old_mask, 0);

    // Wait for signal indicating time to shut down.
    sigset_t wait_mask;
    sigemptyset(&wait_mask);
    sigaddset(&wait_mask, SIGINT);
    sigaddset(&wait_mask, SIGQUIT);
    sigaddset(&wait_mask, SIGTERM);
    pthread_sigmask(SIG_BLOCK, &wait_mask, 0);
    int sig = 0;
    sigwait(&wait_mask, &sig);

    // make the serving thread stop after a signal is reached
    should_run = false;
    t.join();

    return 0;
}
