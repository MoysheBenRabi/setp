#ifndef MXP_MESSAGE_LIST_BUBBLES_RSP
#define MXP_MESSAGE_LIST_BUBBLES_RSP

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
#include "mxp/serialization/serialize.h"
#include "mxp/serialization/deserialize.h"
#include "mxp/message/message.h"
#include "bubble_fragment.h"


namespace mxp {
namespace message {

using namespace mxp;

/*! A List bubbles responce message, as specified by MXP.
 */
class list_bubbles_rsp : public message {
public:

    /*! List of recived bubble descriptions */
    std::vector<bubble_fragment> bubble_fragments;

    /*! Constructor. */
    list_bubbles_rsp() : message(LIST_BUBBLES_RSP) {}

    /*! Virtual destructor. */
    virtual ~list_bubbles_rsp() {}

    /*! Copy constructor.

        \param lbs_rsp the object to base this copy on.
     */
    list_bubbles_rsp(const list_bubbles_rsp & lbs_rsp) : message(LIST_BUBBLES_RSP),
        bubble_fragments(lbs_rsp.bubble_fragments) {
    }

    /*! Assignment operator.

        \param lbs_rsp the object to base this object on.
        \return a reference to this object, after the assignemt has been made.
     */
    list_bubbles_rsp& operator=(const list_bubbles_rsp & lbs_rsp) {
        if (this == &lbs_rsp) {
            return *this;
        }
        bubble_fragments = lbs_rsp.bubble_fragments;
        return *this;
    }

    /*! equals.

        \param other the object to equal with this one.
        \return true if objects is equal and false afterwards.
     */
    virtual bool equals(const message & other) const {
        return (this == &other) || ((get_type() == other.get_type())&&
            std::equal(bubble_fragments.begin(),bubble_fragments.end(),
            ((list_bubbles_rsp&)other).bubble_fragments.begin()));
    }

    virtual unsigned int size() const {
        return 255*bubble_fragments.size();
    }

    /*! Serialize this message.

        \param writer the byte writer to serialize this message into, as a
               sequence of bytes values added to the iterator.
        \return the number of advances made on the iterator during serialization
     */
    virtual unsigned int
    serialize(byte_writer_base & writer) const {        
        unsigned int counter = 0;
        const uint8_t pb = 0;
        uint32_t i;
        uint8_t j;
        for (i = 0; i < bubble_fragments.size(); ++i) {
            counter += bubble_fragments[i].serialize(writer);
            for (j = 0; j < 60; ++j) write<uint8_t>(writer,pb);
            counter += 60;
        }
        return counter;
    }

    /*! De-serialize this message. The contents of this message will be filled
        with the de-serialized values read from the supplied byte reader.

        \param reader the byte reader to serialize this message from by reading
               a sequence of bytes from it
        \param len the maximum number of reads to make on the supplied
               reader
     */
    virtual void
    deserialize(byte_reader_base & reader, unsigned int len) {               
        bubble_fragment bubble;       
        uint32_t begin = reader.count();
        bubble_fragments.reserve(len/255);
        while (reader.count() - begin < len) {
            // read bubble_fragment            
            bubble.deserialize(reader,len);
            bubble_fragments.push_back(bubble);
            // read padding bytes;
            for (uint8_t i = 0; i < 60; ++i) read<uint8_t>(reader);
        }
    }

};


/*! Write a human-readable representation of a message to an output stream.

    \param os the output stream to write to.
    \param lbs_rsp the message to write.
    \return the output stream after the message has been written to it.
 */
inline
std::ostream& operator<<(std::ostream &os, const list_bubbles_rsp & lbs_rsp) {
    os << "list_bubbles_rsp[type: " << lbs_rsp.get_type();
    for (uint32_t i = 0; i < lbs_rsp.bubble_fragments.size(); ++i) 
        os << lbs_rsp.bubble_fragments[i];
    os << "]";
    return os;
}

}
}

#endif

