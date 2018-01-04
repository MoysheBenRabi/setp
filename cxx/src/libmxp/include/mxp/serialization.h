#ifndef MXP_SERIALIZATION_H
#define MXP_SERIALIZATION_H

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

/** \namespace mxp::serialization
    The mxp::serialization namespace provides implementation for the basic
    data types used in MXP messages and packets. Functions of this namespace
    are not usually called by users of the MXP protocol.

    Serialization is abstracted into byte_reader and byte_writer classes,
    which in turn can be based on iterators. This abstraction is needed
    to allow the use of a double dispatch pattern, and thus allow
    virtual functions to use iterator-templated byte_writer and byte_reader
    classes. see http://en.wikipedia.org/wiki/Double_dispatch for details
    on double-dispatch.

    The generic semantics for serializing basic types is the following:

    \code
    unsigned int len = write_it<T>(it, value);
    \endcode

    where T is the type of value to be written,
    it is a forward iterator that can be incremented to contain
    the bytes written, and the return value is the number of bytes
    written.
    upon return, it will point just beyond the serialized value.

    To serialize unicode strings into UTF-8, use one of the following two forms:

    \code
    unsigned int len = write_it<std::wstring>(it, str);
    unsigned int len = write_it<std::wstring>(it, str, len);
    \endcode

    where str is the string to serialize, and len is the space to serialize
    it into. the space will be padded with 0 characters of the serialized
    form of the string is shorter.

    To serialize an range of bytes, use:

    \code
    unsigned int len = write_range_it<uint8_t>(it, data, len);
    \endcode

    where data is an iterator pointing to the range of bytes, and len is
    the number of bytes to write.

    The generic semantics for de-serializing basic types is the following:

    \code
    T value = read_it<T>(it);
    \endcode

    where it is a forward iterator providing the raw bytes to be de-serialized.
    upon return, it will point just beyond the deserialized value.

    To deserialize from UTF-8 to unicode string, use one of the following two
    forms:

    \code
    std::wstring str = read_it<std::wstring>(it);
    std::wstring str = read_it<std::wstring>(it, len);
    \endcode

    where it points to a UTF-8 string, and str will contain the corresponding
    unicode character points. len is the amount of bytes to read. if the
    UTF-8 string is shorter, it is expected that the rest of the values are
    all 0 characters. all space until len is consumed from the iterator.

    To deserialize an range of bytes, use:

    \code
    read_range_it<uint8_t>(it, data, len);
    \endcode

    where data is an iterator that will contain the deserialized range
    of bytes, and len is the number of bytes to read.


    The implementations in this namespace are part of a work-around, as we
    intend to use partially specialized function templates, which are not
    supported in C++. thus, we define partially specialized structures with
    static functions, and call them from a template function. see
http://bytes.com/topic/c/answers/61482-partial-specialization-function-template
    for the core idea.

    Thus, basically disregard the set of structs defined in this namespace,
    and just use the write() and read() functions.
  */

#include <mxp/serialization/serialize.h>
#include <mxp/serialization/deserialize.h>

#endif
