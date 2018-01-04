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

#include <string>
#include <boost/cstdint.hpp>
#include <boost/date_time.hpp>
#include <boost/uuid/uuid.hpp>
#include <boost/array.hpp>
#include <boost/test/unit_test.hpp>

#include "mxp/serialization/deserialize.h"

#include "test_deserialization.h"

namespace mxp {
namespace test {
namespace serialization {
namespace deserialization {

using namespace boost;
using namespace mxp;

void test_byte() {
    static uint8_t bytes[] = { 0xf5 };
    uint8_t  *     b       = bytes;

    int8_t value = mxp::serialization::read_it<int8_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 1);
    BOOST_CHECK_EQUAL(value, -11);
}

void test_ubyte() {
    static uint8_t bytes[] = { 0x0b };
    uint8_t  *     b       = bytes;

    uint8_t value = mxp::serialization::read_it<uint8_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 1);
    BOOST_CHECK_EQUAL(value, 11);
}

void test_short() {
    static uint8_t bytes[] = { 0xa9, 0xfb };
    uint8_t  *     b       = bytes;

    int16_t value = mxp::serialization::read_it<int16_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 2);
    BOOST_CHECK_EQUAL(value, -1111);
}

void test_ushort() {
    static uint8_t bytes[] = { 0x67, 0x2b };
    uint8_t  *     b       = bytes;

    uint16_t value = mxp::serialization::read_it<uint16_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 2);
    BOOST_CHECK_EQUAL(value, 11111);
}

void test_int() {
    static uint8_t bytes[] = { 0x39, 0xca, 0xc5, 0xbd };
    uint8_t  *     b       = bytes;

    int32_t value = mxp::serialization::read_it<int32_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 4);
    BOOST_CHECK_EQUAL(value, -1111111111);
}

void test_uint() {
    static uint8_t bytes[] = { 0xc7, 0x35, 0x3a, 0x42 };
    uint8_t  *     b       = bytes;

    uint32_t value = mxp::serialization::read_it<uint32_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 4);
    BOOST_CHECK_EQUAL(value, 1111111111u);
}

void test_long() {
    static uint8_t bytes[] = { 0x39, 0x8e, 0x3b, 0xd4,
                               0x54, 0x8a, 0x94, 0xf0 };
    uint8_t  *     b       = bytes;

    int64_t value = mxp::serialization::read_it<int64_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(value, -1111111111111111111ll);
}

void test_ulong() {
    static uint8_t bytes[] = { 0xc7, 0x71, 0xc4, 0x2b,
                               0xab, 0x75, 0x6b, 0x0f };
    uint8_t  *     b       = bytes;

    uint64_t value = mxp::serialization::read_it<uint64_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(value, 1111111111111111111ull);
}

void test_float() {
    static uint8_t bytes[] = { 0x39, 0x8e, 0xe3, 0x3d };
    uint8_t  *     b       = bytes;

    float value = mxp::serialization::read_it<float>(b);

    BOOST_CHECK_EQUAL(b - bytes, 4);
    BOOST_CHECK_EQUAL(value, 0.1111111111111111f);
}

void test_double() {
    static uint8_t bytes[] = { 0x1c, 0xc7, 0x71, 0x1c,
                               0xc7, 0x71, 0xbc, 0x3f };
    uint8_t  *     b       = bytes;

    double value = mxp::serialization::read_it<double>(b);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(value, 0.1111111111111111111);
}

void test_array() {
    static uint8_t bytes[] = { 0x39, 0x8e, 0xe3, 0x3d,
                               0x39, 0x8e, 0x63, 0x3e,
                               0xab, 0xaa, 0xaa, 0x3e };
    uint8_t  *     b       = bytes;

    array<float, 3> value = mxp::serialization::read_it<array<float, 3> >(b);

    BOOST_CHECK_EQUAL(b - bytes, 12);
    BOOST_CHECK_EQUAL(value.size(), 3u);
    BOOST_CHECK_EQUAL(value[0], 0.1111111111111111f);
    BOOST_CHECK_EQUAL(value[1], 0.2222222222222222f);
    BOOST_CHECK_EQUAL(value[2], 0.3333333333333333f);
}

void test_date() {
    static uint8_t bytes[] = { 0x00, 0xa0, 0xad, 0x24,
                               0x42, 0x00, 0x00, 0x00 };
    uint8_t  *     b       = bytes;

    posix_time::ptime value = mxp::serialization::read_it<posix_time::ptime>(b);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(value,
                      posix_time::ptime(gregorian::date(2009, 1, 1)));
}

void test_uuid() {
    static uint8_t bytes[] = { 0xd4, 0x36, 0xef, 0xa8, 0x91, 0xbe, 0x48,
                               0xf8, 0xaa, 0xd4, 0x9b, 0x85, 0xbd, 0xe1,
                               0xc6, 0x03 };
    uint8_t  *     b       = bytes;

    uuids::uuid value = mxp::serialization::read_it<uuids::uuid>(b);

    BOOST_CHECK_EQUAL(b - bytes, 16);
    BOOST_CHECK_EQUAL(value,
                      uuids::uuid("d436efa8-91be-48f8-aad4-9b85bde1c603"));
}

void test_bytes() {
    static uint8_t bytes[] = { 0x01, 0x02, 0x03, 0x04 };
    uint8_t        value[2];
    uint8_t      * b = bytes;
    uint8_t      * v = value;

    mxp::serialization::read_range_it<uint8_t>(b, v, 2);

    BOOST_CHECK_EQUAL(b - bytes, 2);
    BOOST_CHECK_EQUAL_COLLECTIONS(value, value + 2, bytes, bytes + 2);
}

void test_string() {
    static uint8_t bytes[] = { 'H', 'e', 'l', 'l', 0xc3, 0xb3, '!', 0x00 };
    uint8_t  *     b       = bytes;

    std::wstring value = mxp::serialization::read_it<std::wstring>(b);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(6u, value.size());
    BOOST_CHECK(!value.compare(L"Hell\xf3!"));
}

void test_string_fixed() {
    static uint8_t bytes[] = { 'H', 'e', 'l', 'l', 0xc3, 0xb3, '!', 0x00,
                               0x00, 0x00 };
    uint8_t  *     b       = bytes;

    std::wstring value = mxp::serialization::read_it<std::wstring>(b, 10u);

    BOOST_CHECK_EQUAL(b - bytes, 10);
    BOOST_CHECK_EQUAL(6u, value.size());
    BOOST_CHECK(!value.compare(L"Hell\xf3!"));
}

void test_string_bad_utf8() {
    // let's put the UTF-8 sequences in the wrong order
    static uint8_t bytes[] = { 'H', 'e', 'l', 'l', 0xb3, 0xc3, '!', 0x00 };
    uint8_t  *     b       = bytes;

    std::wstring value = mxp::serialization::read_it<std::wstring>(b);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(4u, value.size());
    BOOST_CHECK(!value.compare(L"Hell"));

    // now try again with the fixed-size read call
    value = mxp::serialization::read_it<std::wstring>((b = bytes), 8u);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(4u, value.size());
    BOOST_CHECK(!value.compare(L"Hell"));
}


}
}
}
}

