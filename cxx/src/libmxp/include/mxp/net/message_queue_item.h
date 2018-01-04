#ifndef MXP_NET_MESSAGE_QUEUE_ITEM_H
#define MXP_NET_MESSAGE_QUEUE_ITEM_H

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

#include <boost/smart_ptr.hpp>

#include <mxp/message.h>

namespace mxp {
namespace net {

using namespace boost;

/*! A class to represent a pair of a message, and a flag indicating if its
    a guaranteed message or not.
 */
class message_queue_item {
public:
    /*! Virtual destructor. */
    virtual ~message_queue_item() {}

    /*! Return the message related to the message queue item.

        \return the message related to this message queue item.
     */
    virtual shared_ptr<mxp::message::message> message() const = 0;

    /*! Tell if this message queue item is guaranteed or not.

        \return true if this message should be sent as a guaranteed message,
                false otherwise.
     */
    virtual bool guaranteed() const = 0;
};


}
}

#endif
