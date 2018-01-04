#ifndef MXP_SERIALIZATION_DESERIALIZE
#define MXP_SERIALIZATION_DESERIALIZE

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

#include <iterator>
#include <vector>
#include <string>
#include <istream>
#include <boost/cstdint.hpp>
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

/*! Base byte reader class. This is used as part of a double dispatch
    pattern, to allow for virtual serialize functions to be called with
    any iterator type defined by a template.

    \see byte_reader
 */
class byte_reader_base {
public:
    /*! Constructor, */
    byte_reader_base() {}

    /*! Virtual destructor. */
    virtual ~byte_reader_base() {}

    /*! Read a byte from the underlying storage.

        \return the next byte from the underlying storage
     */
    virtual uint8_t read() = 0;

    /*! Tell the number of bytes read during the lifetime of this reader.

        \return the number of bytes read during the lifetime of this reader.
     */
    virtual unsigned int count() = 0;
};

/*! Byte reader class for any iterator that is able to de-reference itself
    as an uint8_t
 */
template<typename iterator>
class byte_reader : public byte_reader_base {
private:
    /*! The underlying iterator to read from. */
    iterator & _it;

    /*! The initial position of the iterator, when the reader object was
        created.
     */
    iterator   _start;

public:
    /*! Constructor.

        \param it the iterator to read the bytes from.
     */
    explicit byte_reader(iterator &it) : _it(it), _start(it) {}

    /*! Virtual destructor. */
    virtual ~byte_reader() {}

    virtual uint8_t read() { return *_it++; }
    virtual unsigned int count() { return _it - _start; }
};


/// \cond INTERNAL

/*! Base class to mimic partially specialized functions using partially
    specialized classes. Won't compile in itself.

    \tparam T the type to de-serialize
 */
template<typename T>
struct do_read {
    /*! Perform the de-serialization.
        Always throws an exception.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static T apply(byte_reader_base & reader) {
        throw std::exception("unsupported de-serialization type");
    }

    /*! Perform the de-serialization.
        Always throws an exception.

        \param reader the reader to get the serialized data from
        \param len the maximum number of bytes to read.
        \return the de-serialized data.
     */
    static T apply(byte_reader_base & reader, unsigned int len) {
        throw std::exception("unsupported de-serialization type");
    }
};

/*! Base class to mimic partially specialized functions using partially
    specialized classes. Won't compile in itself.

    \tparam T the type of the range to deserialize, this is the value type
            of the data_iterator
    \tparam data_iterator the iterator that will store the deserialized data
 */
template<typename T, typename data_iterator>
struct do_read_range {
    /*! Deserialize a range by reading the data from an iterator.

        \param reader the reader to get the serialized data from
        \param data store the deserialized data here
        \param len the number of bytes to read
     */
    static void apply(byte_reader_base & reader,
                      data_iterator data,
                      unsigned int len) {
        throw std::exception("unsupported de-serialization type");
    }
};


/*! De-serialize a signed 8 bit integer.
 */
template<>
struct do_read<int8_t> {
    /*! De-serialize a signed 8 bit integer.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static int8_t apply(byte_reader_base & reader) {
        int8_t value = 0;

        value |= reader.read();

        return value;
    }
};

/*! De-serialize an unsigned 8 bit integer.
 */
template<>
struct do_read<uint8_t> {
    /*! De-serialize an unsigned 8 bit integer.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static uint8_t apply(byte_reader_base & reader) {
        uint8_t value = 0;

        value |= reader.read();

        return value;
    }
};

/*! De-serialize a signed 16 bit integer.
 */
template<>
struct do_read<int16_t> {
    /*! De-serialize a signed 16 bit integer.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static int16_t apply(byte_reader_base & reader) {
        int16_t value = 0;

        value |= reader.read();
        value |= reader.read() << 8;

        return value;
    }
};

/*! De-serialize an unsigned 16 bit integer.
 */
template<>
struct do_read<uint16_t> {
    /*! De-serialize an unsigned 16 bit integer.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static uint16_t apply(byte_reader_base & reader) {
        uint16_t value = 0;

        value |= reader.read();
        value |= reader.read() << 8;

        return value;
    }
};

/*! De-serialize a signed 32 bit integer.
 */
template<>
struct do_read<int32_t> {
    /*! De-serialize a signed 32 bit integer.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static int32_t apply(byte_reader_base & reader) {
        int32_t value = 0;

        value |= reader.read();
        value |= reader.read() << 8;
        value |= reader.read() << 16;
        value |= reader.read() << 24;

        return value;
    }
};

/*! De-serialize an unsigned 32 bit integer.
 */
template<>
struct do_read<uint32_t> {
    /*! De-serialize an unsigned 32 bit integer.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static uint32_t apply(byte_reader_base & reader) {
        uint32_t value = 0;

        value |= reader.read();
        value |= reader.read() << 8;
        value |= reader.read() << 16;
        value |= reader.read() << 24;

        return value;
    }
};

/*! De-serialize a signed 64 bit integer.
 */
template<>
struct do_read<int64_t> {
    /*! De-serialize a signed 64 bit integer.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static int64_t apply(byte_reader_base & reader) {
        int64_t value = 0;

        value |= ((int64_t) reader.read());
        value |= ((int64_t) reader.read()) << 8;
        value |= ((int64_t) reader.read()) << 16;
        value |= ((int64_t) reader.read()) << 24;
        value |= ((int64_t) reader.read()) << 32;
        value |= ((int64_t) reader.read()) << 40;
        value |= ((int64_t) reader.read()) << 48;
        value |= ((int64_t) reader.read()) << 56;

        return value;
    }
};

/*! De-serialize an unsigned 64 bit integer.
 */
template<>
struct do_read<uint64_t> {
    /*! De-serialize an unsigned 64 bit integer.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static uint64_t apply(byte_reader_base & reader) {
        uint64_t value = 0;

        value |= ((uint64_t) reader.read());
        value |= ((uint64_t) reader.read()) << 8;
        value |= ((uint64_t) reader.read()) << 16;
        value |= ((uint64_t) reader.read()) << 24;
        value |= ((uint64_t) reader.read()) << 32;
        value |= ((uint64_t) reader.read()) << 40;
        value |= ((uint64_t) reader.read()) << 48;
        value |= ((uint64_t) reader.read()) << 56;

        return value;
    }
};

/*! De-serialize a single precision IEC 559 / IEEE 754 floating point value.
 */
template<>
struct do_read<float> {
    /*! A union that gives access to a 32 bit int and a float (also 32 bits) in
        the same memory space.
     */
    union float_uint32 {
        float       f;
        uint32_t    i;
    };

    /*! De-serialize a single precision IEC 559 / IEEE 754 floating point value.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static float apply(byte_reader_base & reader) {
        // this code only works on platforms that are
        // IEC 559 / IEEE 754 compliant
        BOOST_STATIC_ASSERT(std::numeric_limits<float>::is_iec559);
        BOOST_STATIC_ASSERT(sizeof(float) == 4u);

        float_uint32    value;
        value.i = do_read<uint32_t>::apply(reader);

        return value.f;
    }
};

/*! De-serialize a double precision IEC 559 / IEEE 754 floating point value.
 */
template<>
struct do_read<double> {
    /*! A union that gives access to a 64 bit int and a double (also 64 bits) in
        the same memory space.
     */
    union double_uint64 {
        double      d;
        uint64_t    i;
    };

    /*! De-serialize a double precision IEC 559 / IEEE 754 floating point value.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static double apply(byte_reader_base & reader) {
        // this code only works on platforms that are
        // IEC 559 / IEEE 754 compliant
        BOOST_STATIC_ASSERT(std::numeric_limits<double>::is_iec559);
        BOOST_STATIC_ASSERT(sizeof(double) == 8u);

        double_uint64   value;
        value.i = do_read<uint64_t>::apply(reader);

        return value.d;
    }
};


/*! De-serialize a fixed size array.
 */
template<typename T, std::size_t N>
struct do_read<array<T, N> > {
    /*! De-serialize a fixed size array.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static array<T, N> apply(byte_reader_base & reader) {
        array<T, N>  value;
        for (unsigned int i = 0; i < N; ++i) {
            value[i] = do_read<T>::apply(reader);
        }

        return value;
    }
};

/*! De-serialize a point in time - serialized as the number of
    milliseconds since January 1st 2000.
 */
template<>
struct do_read<posix_time::ptime> {
    /*! De-serialize aa point in time - serialized as the number of
        milliseconds since January 1st 2000.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static posix_time::ptime apply(byte_reader_base & reader) {
        static const gregorian::date  sinceEpoch(2000, 1, 1);

        int64_t offset = do_read<int64_t>::apply(reader);
        posix_time::ptime value(sinceEpoch,
                                posix_time::milliseconds(offset));

        return value;
    }
};

/*! Deserialize a range of bytes by reading the data from an iterator.

    \tparam data_iterator the iterator that will store the deserialized data.
 */
template<typename data_iterator>
struct do_read_range<uint8_t, data_iterator> {
    /*! Serialize a range of bytes by writing the data to an iterator.

        \param reader the reader to get the serialized data from
        \param data store the deserialized data here
        \param len the number of bytes to read
     */
    static void apply(byte_reader_base & reader,
                      data_iterator data,
                      unsigned int len) {
        unsigned int  i = len;
        while (i--) {
            *data++ = reader.read();
        }
    }
};

/*! De-serialize a UUID from a 16 byte value.
 */
template<>
struct do_read<uuids::uuid> {
    /*! De-serialize a UUID from a 16 byte value.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static uuids::uuid apply(byte_reader_base & reader) {
        uint8_t bytes[16];

        do_read_range<uint8_t, uint8_t *>::apply(reader, bytes, 16);
        uuids::uuid value(bytes, bytes + 16);

        return value;
    }
};

/*! De-serialize a UTF-8 encoded string into a series of UCS-4 character
    points. The implementation of std::wstring will determine if it can
    only store UCS-2 values, or can store UCS-4 values as well.
    The input is expected to be a 0-terminated UTF-8 string, and it is read up
    until the 0-termination.
 */
template<>
struct do_read<std::wstring> {
    /*! De-serialize a UTF-8 encoded string into a series of UCS-4 character
        points. The implementation of std::wstring will determine if it can
        only store UCS-2 values, or can store UCS-4 values as well.
        The input is expected to be a 0-terminated UTF-8 string, and it is read
        up until the 0-termination.

        \param reader the reader to get the serialized data from
        \return the de-serialized data.
     */
    static std::wstring apply(byte_reader_base & reader) {
        std::wstring            ret;
        std::vector<uint8_t>    bytes;

        // read up until the terminating 0 character, and then convert it
        // into unicode character points
        uint8_t     byte;
        while ((byte = reader.read()) != 0x00) {
            bytes.push_back(byte);
        }
        try {
            std::back_insert_iterator<std::wstring> it =
                                                    std::back_inserter(ret);
            utf8::utf8to32<std::wstring::value_type>(bytes.begin(),
                                                     bytes.end(),
                                                     it);
        } catch (utf8::invalid_utf8 &) {
            // don't do anything - return the string as far as it was processed
        }

        return ret;
    }

    /*! De-serialize a UTF-8 encoded string into a series of UCS-4 character
        points. The implementation of std::wstring will determine if it can
        only store UCS-2 values, or can store UCS-4 values as well.
        The input is expected to be a 0-terminated UTF-8 string, or the whole
        range has to contain a UTF-8 string. If the string is shorter,
        the rest should be padded with 0 characters. The whole range is
        consumed.

        \param reader the reader to get the serialized data from
        \param len the number of bytes to read
        \return the de-serialized data.
     */
    static std::wstring apply(byte_reader_base & reader, unsigned int len) {
        std::wstring            ret;
        std::vector<uint8_t>    bytes;

        // read up until the terminating 0 character, and then convert it
        // into unicode character points
        const unsigned int  start = reader.count();
        uint8_t             byte;
        while ((reader.count() - start) < len
            && (byte = reader.read()) != 0x00) {

            bytes.push_back(byte);
        }
        try {
            std::back_insert_iterator<std::wstring> it =
                                                    std::back_inserter(ret);
            utf8::utf8to32<std::wstring::value_type>(bytes.begin(),
                                                     bytes.end(),
                                                     it);
        } catch (utf8::invalid_utf8 &) {
            // don't do anything - return the string as far as it was processed
        }

        // consume the remaining bytes from the reader
        while ((reader.count() - start) < len) {
            reader.read();
        }

        return ret;
    }
};


/// \endcond INTERNAL


/*! De-serialize a value of a specific type by reading the serialized
    data from an iterator.

    \tparam T the type to de-serialize
    \tparam iterator the iterator, which is expected to contain the serialized
            form of the value, as a sequence of bytes.
    \param it the iterator to get the serialized data from. on return, it
           will point to the first byte after the data that was read.
    \return the de-serialized data.
 */
template<typename T, typename iterator>
T read_it(iterator & it) {
    byte_reader<iterator>   reader(it);
    return do_read<T>::apply(reader);
}

/*! De-serialize a value of a specific type by reading the serialized
    data from an iterator.

    \tparam T the type to de-serialize
    \param reader the reader to get the serialized data from.
    \return the de-serialized data.
 */
template<typename T>
T read(byte_reader_base & reader) {
    return do_read<T>::apply(reader);
}

/*! De-serialize a value of a specific type by reading the serialized
    data from an iterator.

    \tparam T the type to de-serialize
    \tparam iterator the iterator, which is expected to contain the serialized
            form of the value, as a sequence of bytes.
    \param it the iterator to get the serialized data from. on return, it
           will point to the first byte after the data that was read.
    \param len read up to this many bytes from the reader
    \return the de-serialized data.
 */
template<typename T, typename iterator>
T read_it(iterator & it, unsigned int len) {
    byte_reader<iterator>   reader(it);
    return do_read<T>::apply(reader, len);
}

/*! De-serialize a value of a specific type by reading the serialized
    data from an iterator.

    \tparam T the type to de-serialize
    \param reader the reader to get the serialized data from.
    \param len read up to this many bytes from the reader
    \return the de-serialized data.
 */
template<typename T>
T read(byte_reader_base & reader, unsigned int len) {
    return do_read<T>::apply(reader, len);
}

/*! Deserialize a range by reading the data from an iterator.

    \tparam T the type of the range to deserialize, this is the value type
            of the data_iterator
    \tparam iterator the iterator, which is expected to contain the serialized
            form of the value, as a sequence of bytes.
    \tparam data_iterator the iterator that will store the deserialized data.
    \param it the iterator to get the serialized data from. on return, it
           will point to the first byte after the data that was read.
    \param data store the deserialized data here
    \param len the number of bytes to read
 */
template<typename T, typename iterator, typename data_iterator>
void read_range_it(iterator    & it,
                   data_iterator data,
                   unsigned int  len) {
    byte_reader<iterator>   reader(it);
    do_read_range<T, data_iterator>::apply(reader, data, len);
}

/*! Deserialize a range by reading the data from an iterator.

    \tparam T the type of the range to deserialize, this is the value type
            of the data_iterator
    \tparam data_iterator the iterator that will store the deserialized data.
    \param reader the reader to get the serialized data from.
    \param data store the deserialized data here
    \param len the number of bytes to read
 */
template<typename T, typename data_iterator>
void read_range(byte_reader_base & reader,
                data_iterator      data,
                unsigned int       len) {
    do_read_range<T, data_iterator>::apply(reader, data, len);
}



}
}

#endif
