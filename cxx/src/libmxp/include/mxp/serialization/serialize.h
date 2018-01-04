#ifndef MXP_SERIALIZATION_SERIALIZE
#define MXP_SERIALIZATION_SERIALIZE

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

#include <limits>
#include <istream>
#include <string>
#include <boost/static_assert.hpp>
#include <boost/cstdint.hpp>
#include <boost/foreach.hpp>
#include <boost/array.hpp>
#include <boost/date_time.hpp>
#include <boost/uuid/uuid.hpp>

#include "utf8.h"

namespace mxp {

using boost::int8_t;
using boost::uint8_t;
using boost::int16_t;
using boost::uint16_t;
using boost::int32_t;
using boost::uint32_t;
using boost::int64_t;
using boost::uint64_t;

namespace serialization {

using namespace boost;

/*! Base byte writer class. This is used as part of a double dispatch
    pattern, to allow for virtual serialize functions to be called with
    any iterator type defined by a template.

    \see byte_writer
    \see unsigned int write(byte_writer_base & writer, const T & value)
 */
class byte_writer_base {
public:
    /*! Constructor. */
    byte_writer_base() {}
    /*! Virtual destructor. */
    virtual ~byte_writer_base() {}

    /*! Write a byte into the underlying storage.

        \param b the byte to write
     */
    virtual void write(uint8_t b) = 0;

    /*! Tell the number of bytes written during the lifetime of this writer.

        \return the number of bytes written during the lifetime of this writer.
     */
    virtual unsigned int count() = 0;
};

/*! Byte writer class for any iterator that is able to de-reference itself
    as an lvalue for uint8_t
 */
template<typename iterator>
class byte_writer : public byte_writer_base {
private:
    /*! The underlying iterator to write to. */

    iterator & _it;

    /*! The number of advances this iterator writer has made since its
        creation.
     */
    unsigned int _counter;

public:
    /*! Constructor.

        \param it the iterator to delegate the written bytes to
     */
    explicit byte_writer(iterator &it) : _it(it), _counter(0) {}

    /*! Virtual destructor. */
    virtual ~byte_writer() {}

    virtual void write(uint8_t b) { *_it++ = b; ++_counter; }
    virtual unsigned int count() { return _counter; }
};


/// \cond INTERNAL

/*! Base class to mimic partially specialized functions using partially
    specialized classes. Won't compile in itself.

    \tparam T the type to serialize
 */
template<typename T>
struct do_write {
    /*! Perform the serialization.
        Always throws an exception.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer, const T & value) {
        throw std::exception("unsupported de-serialization type");
    }

    /*! Perform the serialization, with a specified maximum number of bytes
        to write.
        Always throws an exception.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \param len the maximum number of bytes to write
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              const T & value,
                              unsigned int len) {
        throw std::exception("unsupported de-serialization type");
    }
};

/*! Base class to mimic partially specialized functions using partially
    specialized classes. Won't compile in itself.

    \tparam T the type of the range to serialize, this is the value type
            of the data_iterator
    \tparam data_iterator the iterator that will provide the data to serialize
 */
template<typename T, typename data_iterator>
struct do_write_range {
    /*! Serialize a range by writing the data to an iterator.

        \param writer the writer to write the serialized data to
        \param data the data to write to the target iterator
        \param len the number of bytes to write
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & it,
                              data_iterator data,
                              unsigned int len) {
        throw std::exception("unsupported de-serialization type");
    }
};

/*! Serialize a signed 8 bit integer.
*/
template<>
struct do_write<int8_t> {
    /*! Serialize a signed 8 bit integer.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer, const int8_t & value) {
        writer.write(value);

        return 1;
    }
};

/*! Serialize an unsigned 8 bit integer.
 */
template<>
struct do_write<uint8_t> {
    /*! Serialize an unsigned 8 bit integer.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              const uint8_t & value) {
        writer.write(value);

        return 1;
    }
};

/*! Serialize a signed 16 bit integer.
 */
template<>
struct do_write<int16_t> {
    /*! Serialize a signed 16 bit integer.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              const int16_t & value) {
        writer.write(value & 0xff);
        writer.write((value >> 8) & 0xff);

        return 2;
    }
};

/*! Serialize an unsigned 16 bit integer.
 */
template<>
struct do_write<uint16_t> {
    /*! Serialize an unsigned 16 bit integer.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              const uint16_t & value) {
        writer.write(value & 0xff);
        writer.write((value >> 8) & 0xff);

        return 2;
    }
};

/*! Serialize a signed 32 bit integer.
 */
template<>
struct do_write<int32_t> {
    /*! Serialize a signed 32 bit integer.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              const int32_t & value) {
        writer.write(value & 0xff);
        writer.write((value >> 8) & 0xff);
        writer.write((value >> 16) & 0xff);
        writer.write((value >> 24) & 0xff);

        return 4;
    }
};

/*! Serialize an unsigned 32 bit integer.
 */
template<>
struct do_write<uint32_t> {
    /*! Serialize an unsigned 32 bit integer.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              const uint32_t & value) {
        writer.write(value & 0xff);
        writer.write((value >> 8) & 0xff);
        writer.write((value >> 16) & 0xff);
        writer.write((value >> 24) & 0xff);

        return 4;
    }
};

/*! Serialize a signed 64 bit integer.
 */
template<>
struct do_write<int64_t> {
    /*! Serialize a signed 64 bit integer.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              const int64_t & value) {
        writer.write(value & 0xff);
        writer.write((value >> 8) & 0xff);
        writer.write((value >> 16) & 0xff);
        writer.write((value >> 24) & 0xff);
        writer.write((value >> 32) & 0xff);
        writer.write((value >> 40) & 0xff);
        writer.write((value >> 48) & 0xff);
        writer.write((value >> 56) & 0xff);

        return 8;
    }
};

/*! Serialize an unsigned 64 bit integer.
 */
template<>
struct do_write<uint64_t> {
    /*! Serialize an unsigned 64 bit integer.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              const uint64_t & value) {
        writer.write(value & 0xff);
        writer.write((value >> 8) & 0xff);
        writer.write((value >> 16) & 0xff);
        writer.write((value >> 24) & 0xff);
        writer.write((value >> 32) & 0xff);
        writer.write((value >> 40) & 0xff);
        writer.write((value >> 48) & 0xff);
        writer.write((value >> 56) & 0xff);

        return 8;
    }
};

/*! Serialize a single precision IEC 559 / IEEE 754 floating point value.
 */
template<>
struct do_write<float> {
    /*! A union that gives access to a 32 bit int and a float (also 32 bits) in
        the same memory space.
     */
    union float_uint32 {
        float       f;
        uint32_t    i;
    };

    /*! Serialize a single precision IEC 559 / IEEE 754 floating point value.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              const float & value) {
        // this code only works on platforms that store float according
        // to IEC 559 / IEEE 754, let's assert for this
        BOOST_STATIC_ASSERT(std::numeric_limits<float>::is_iec559);
        BOOST_STATIC_ASSERT(sizeof(float) == 4u);

        float_uint32    v;
        v.f = value;

        return do_write<uint32_t>::apply(writer, v.i);
    }
};

/*! Serialize a double precision IEC 559 / IEEE 754 floating point value.
 */
template<>
struct do_write<double> {
    /*! A union that gives access to a 64 bit int and a double (also 64 bits) in
        the same memory space.
     */
    union double_uint64 {
        double      d;
        uint64_t    i;
    };

    /*! Serialize a double precision IEC 559 / IEEE 754 floating point value.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              const double & value) {
        // this code only works on platforms that store float according
        // to IEC 559 / IEEE 754, let's assert for this, see above
        BOOST_STATIC_ASSERT(std::numeric_limits<double>::is_iec559);
        BOOST_STATIC_ASSERT(sizeof(double) == 8u);

        double_uint64   v;
        v.d = value;

        return do_write<uint64_t>::apply(writer, v.i);
    }
};

/*! Serialize a fixed array of values.
 */
template<typename T, std::size_t N>
struct do_write<array<T, N> > {
    /*! Serialize a fixed size array of values.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              const array<T, N> & value) {
        unsigned int count = 0;
        BOOST_FOREACH(const T & t, value) {
            count += do_write<T>::apply(writer, t);
        }

        return count;
    }
};

/*! Serialize a point in time - serialized as the number of
    milliseconds since January 1st 2000.

    \tparam iterator the iterator to write the serialized data to
 */
template<>
struct do_write<posix_time::ptime> {
    /*! Serialize a point in time - serialized as the number of
        milliseconds since January 1st 2000.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              const posix_time::ptime & value) {
        static const posix_time::ptime sinceEpoch(gregorian::date(2000, 1, 1));

        int64_t diff = (value - sinceEpoch).total_milliseconds();

        return do_write<int64_t>::apply(writer, diff);
    }
};

/*! Serialize a UUID as a 16 byte value.

    \tparam iterator the iterator to write the serialized data to
 */
template<>
struct do_write<uuids::uuid> {
    /*! Serialize a UUID as a 16 byte value.

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              const uuids::uuid & value) {

        unsigned int                start = writer.count();
        uuids::uuid::const_iterator it    = value.begin();
        while (it != value.end()) {
            writer.write(*it++);
        }

        return writer.count() - start;
    }
};

/*! Serialize a range of bytes by writing the data to an iterator.

    \tparam data_iterator the iterator that will provide the data to serialize
 */
template<typename data_iterator>
struct do_write_range<uint8_t, data_iterator> {
    /*! Serialize a range of bytes by writing the data to an iterator.

        \param writer the writer to write the serialized data to.
        \param data the data to write to the target iterator
        \param len the number of bytes to write
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base & writer,
                              data_iterator data,
                              unsigned int len) {
        unsigned int  i = len;
        while (i--) {
            writer.write(*data++);
        }

        return len;
    }
};

/*! Serialize an UCS-2 or UCS-4 string, that is, a string made up of a series
    of Unicode character points. If we're treating UCS-2 or UCS-4, that
    depends on the size of the wchar_t type. Usually UCS-2 is enough
    for everyone.
    The string is going to be represented as a 0-terminated UTF-8 sequence.
 */
template<>
struct do_write<std::wstring> {
    /*! Serialize an UCS-2 or UCS-4 string, that is, a string made up of a
        series of Unicode character points

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base   & writer,
                              const std::wstring & value) {
        // encode the characters one by one, and write them out
        unsigned int counter = 0;
        uint8_t      bytes[6];
        BOOST_FOREACH(std::wstring::value_type ch, value) {
            uint8_t *end = utf8::append(ch, bytes);
            counter += do_write_range<uint8_t, uint8_t*>::apply(writer,
                                                                bytes,
                                                                end - bytes);
        }
        // append a 0-terminator
        writer.write(0);
        ++counter;

        return counter;
    }

    /*! Serialize an UCS-2 or UCS-4 string, that is, a string made up of a
        series of Unicode character points

        \param writer the writer to write the serialized data to.
        \param value the value to serialize
        \param len the number of bytes to write
        \return the number of advances made on the supplied iterator
     */
    static unsigned int apply(byte_writer_base   & writer,
                              const std::wstring & value,
                              unsigned int         len) {
        // encode the characters one by one, and write them out
        unsigned int counter = 0;
        uint8_t      bytes[6];
        BOOST_FOREACH(std::wstring::value_type ch, value) {
            uint8_t *end = utf8::append(ch, bytes);
            unsigned int l = end - bytes;
            if (counter + l <= len) {
                counter += do_write_range<uint8_t, uint8_t*>::apply(writer,
                                                                bytes, l);
            } else {
                break;
            }
        }
        // fill up the remaining space with zeroes
        while (counter < len) {
            writer.write(0);
            ++counter;
        }

        return counter;
    }
};


/// \endcond INTERNAL


/*! Serialize a value of a specific type by writing the serialized
    data to an iterator.

    \tparam T the type to deserialize
    \tparam iterator the iterator, which will be written, and will contain
            the serialized data as a series of bytes
    \param it the iterator to write the serialized data to. on function return,
           this will point to one past the written serialized data.
    \param value the value to serialize
    \return the number of advances made on the supplied iterator
 */
template<typename T, typename iterator>
unsigned int write_it(iterator & it, const T & value) {
    byte_writer<iterator>   writer(it);
    return do_write<T>::apply(writer, value);
}

/*! Serialize a value of a specific type by writing the serialized
    data to a byte_writer.

    \tparam T the type to deserialize
    \param writer the writer to write the serialized data to.
    \param value the value to serialize
    \return the number of advances made on the supplied iterator
 */
template<typename T>
unsigned int write(byte_writer_base & writer, const T & value) {
    return do_write<T>::apply(writer, value);
}

/*! Serialize a value of a specific type by writing the serialized
    data to an iterator.

    \tparam T the type to deserialize
    \tparam iterator the iterator, which will be written, and will contain
            the serialized data as a series of bytes
    \param it the iterator to write the serialized data to. on function return,
           this will point to one past the written serialized data.
    \param len the maximum number of advances to make on the iterator
    \param value the value to serialize
    \return the number of advances made on the supplied iterator
 */
template<typename T, typename iterator>
unsigned int write_it(iterator & it, const T & value, unsigned int len) {
    byte_writer<iterator>   writer(it);
    return do_write<T>::apply(writer, value, len);
}

/*! Serialize a value of a specific type by writing the serialized
    data to a byte_writer.

    \tparam T the type to deserialize
    \param writer the writer to write the serialized data to.
    \param value the value to serialize
    \param len the maximum number of bytes to write to writer
    \return the number of advances made on the supplied iterator
 */
template<typename T>
unsigned int write(byte_writer_base & writer,
                   const T          & value,
                   unsigned int       len) {
    return do_write<T>::apply(writer, value, len);
}

/*! Serialize a range by writing the data to an iterator.

    \tparam T the type of the range to serialize, this is the value type
            of the data_iterator
    \tparam iterator the iterator, which will be written, and will contain
            the serialized data as a series of bytes
    \tparam data_iterator the iterator that will provide the data to serialize
    \param it the iterator to write the serialized data to. on function return,
           this will point to one past the written serialized data.
    \param data the data to write to the target iterator
    \param len the number of bytes to write
    \return the number of advances made on the supplied iterator
 */
template<typename T, typename iterator, typename data_iterator>
unsigned int write_range_it(iterator    & it,
                            data_iterator data,
                            unsigned int  len) {
    byte_writer<iterator>   writer(it);
    return do_write_range<T, data_iterator>::apply(writer, data, len);
}

/*! Serialize a range by writing the data to an iterator.

    \tparam T the type of the range to serialize, this is the value type
            of the data_iterator
    \tparam data_iterator the iterator that will provide the data to serialize
    \param writer the writer to write the serialized data to.
    \param data the data to write to the target iterator
    \param len the number of bytes to write
    \return the number of advances made on the supplied iterator
 */
template<typename T, typename data_iterator>
unsigned int write_range(byte_writer_base & writer,
                         data_iterator      data,
                         unsigned int       len) {
    return do_write_range<T, data_iterator>::apply(writer, data, len);
}


}
}

#endif
