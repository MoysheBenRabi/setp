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

#include "mxp/serialization/serialize.h"
#include "mxp/serialization/deserialize.h"

#include "test_two_way_serialization.h"

namespace mxp {
namespace test {
namespace serialization {
namespace twserialization {

using namespace boost;
using namespace mxp;

void test_byte() {
    int8_t expected = -11;

    uint8_t        bytes[1];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<int8_t>(b, expected);

    BOOST_CHECK_EQUAL(b - bytes, 1);
    BOOST_CHECK_EQUAL(len, 1u);

    b = bytes;
    int8_t value = mxp::serialization::read_it<int8_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 1);
    BOOST_CHECK_EQUAL(expected, value);
}

void test_ubyte() {
    uint8_t expected = 11;

    uint8_t        bytes[1];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<uint8_t>(b, expected);

    BOOST_CHECK_EQUAL(b - bytes, 1);
    BOOST_CHECK_EQUAL(len, 1u);

    b = bytes;
    uint8_t value = mxp::serialization::read_it<uint8_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 1);
    BOOST_CHECK_EQUAL(expected, value);
}

void test_short() {
    int16_t expected = -1111;

    uint8_t        bytes[2];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<int16_t>(b, expected);

    BOOST_CHECK_EQUAL(b - bytes, 2);
    BOOST_CHECK_EQUAL(len, 2u);

    b = bytes;
    int16_t value = mxp::serialization::read_it<int16_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 2);
    BOOST_CHECK_EQUAL(expected, value);
}

void test_ushort() {
    uint16_t expected = 11111;

    uint8_t        bytes[2];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<uint16_t>(b, expected);

    BOOST_CHECK_EQUAL(b - bytes, 2);
    BOOST_CHECK_EQUAL(len, 2u);

    b = bytes;
    uint16_t value = mxp::serialization::read_it<uint16_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 2);
    BOOST_CHECK_EQUAL(expected, value);
}

void test_int() {
    int32_t expected = -1111111111;

    uint8_t        bytes[4];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<int32_t>(b, expected);

    BOOST_CHECK_EQUAL(b - bytes, 4);
    BOOST_CHECK_EQUAL(len, 4u);

    b = bytes;
    int32_t value = mxp::serialization::read_it<int32_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 4);
    BOOST_CHECK_EQUAL(expected, value);
}

void test_uint() {
    uint32_t expected = 1111111111u;

    uint8_t        bytes[4];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<uint32_t>(b, expected);

    BOOST_CHECK_EQUAL(b - bytes, 4);
    BOOST_CHECK_EQUAL(len, 4u);

    b = bytes;
    uint32_t value = mxp::serialization::read_it<uint32_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 4);
    BOOST_CHECK_EQUAL(expected, value);
}

void test_long() {
    int64_t expected = -1111111111111111111ll;

    uint8_t        bytes[8];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<int64_t>(b, expected);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(len, 8u);

    b = bytes;
    int64_t value = mxp::serialization::read_it<int64_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(expected, value);
}

void test_ulong() {
    uint64_t expected = 1111111111111111111ull;

    uint8_t        bytes[8];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<uint64_t>(b, expected);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(len, 8u);

    b = bytes;
    uint64_t value = mxp::serialization::read_it<uint64_t>(b);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(expected, value);
}

void test_float() {
    float expected = 0.1111111111111111f;

    uint8_t        bytes[4];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<float>(b, expected);

    BOOST_CHECK_EQUAL(b - bytes, 4);
    BOOST_CHECK_EQUAL(len, 4u);

    b = bytes;
    float value = mxp::serialization::read_it<float>(b);

    BOOST_CHECK_EQUAL(b - bytes, 4);
    BOOST_CHECK_EQUAL(expected, value);
}

void test_double() {
    double expected = 0.1111111111111111111;

    uint8_t        bytes[8];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<double>(b, expected);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(len, 8u);

    b = bytes;
    double value = mxp::serialization::read_it<double>(b);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(expected, value);
}

void test_array() {
    array<float, 3> expected;
    expected[0] = 0.1111111111111111f;
    expected[1] = 0.2222222222222222f;
    expected[2] = 0.3333333333333333f;

    uint8_t        bytes[12];
    uint8_t      * b = bytes;

    unsigned int len =
                    mxp::serialization::write_it<array<float, 3> >(b, expected);

    BOOST_CHECK_EQUAL(b - bytes, 12);
    BOOST_CHECK_EQUAL(len, 12u);

    b = bytes;
    array<float, 3> value = mxp::serialization::read_it<array<float, 3> >(b);

    BOOST_CHECK_EQUAL(b - bytes, 12);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected.begin(), expected.end(),
                                  value.begin(), value.end());
}

void test_date() {
    posix_time::ptime expected(gregorian::date(2009, 1, 1));

    uint8_t        bytes[8];
    uint8_t      * b = bytes;

    unsigned int len =
                    mxp::serialization::write_it<posix_time::ptime>(b,expected);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(len, 8u);

    b = bytes;
    posix_time::ptime value = mxp::serialization::read_it<posix_time::ptime>(b);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(expected, value);
}

void test_uuid() {
    uuids::uuid expected("d436efa8-91be-48f8-aad4-9b85bde1c603");

    uint8_t        bytes[16];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<uuids::uuid>(b, expected);

    BOOST_CHECK_EQUAL(b - bytes, 16);
    BOOST_CHECK_EQUAL(len, 16u);

    b = bytes;
    uuids::uuid value = mxp::serialization::read_it<uuids::uuid>(b);

    BOOST_CHECK_EQUAL(b - bytes, 16);
    BOOST_CHECK_EQUAL(expected, value);
}

void test_bytes() {
    static uint8_t expected[] = { 0x01, 0x02, 0x03, 0x04 };

    uint8_t        bytes[4];
    uint8_t      * b = bytes;

    mxp::serialization::write_range_it<uint8_t>(b, expected, 4);

    BOOST_CHECK_EQUAL(b - bytes, 4);

    uint8_t       value[4];
    b = bytes;
    mxp::serialization::read_range_it<uint8_t>(b, value, 4);

    BOOST_CHECK_EQUAL(b - bytes, 4);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 4, value, value + 4);
}

void test_string() {
    std::wstring expected(L"Hell\xf3!");

    uint8_t        bytes[8];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<std::wstring>(b,expected);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(len, 8u);

    b = bytes;
    std::wstring value = mxp::serialization::read_it<std::wstring>(b);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(expected.size(), value.size());
    BOOST_CHECK(!expected.compare(value));
}

void test_string_fixed() {
    std::wstring expected(L"Hell\xf3!");

    uint8_t        bytes[10];
    uint8_t      * b = bytes;

    unsigned int len =
        mxp::serialization::write_it<std::wstring>(b, expected, 10u);

    BOOST_CHECK_EQUAL(b - bytes, 10);
    BOOST_CHECK_EQUAL(len, 10u);

    b = bytes;
    std::wstring value = mxp::serialization::read_it<std::wstring>(b, 10u);

    BOOST_CHECK_EQUAL(b - bytes, 10);
    BOOST_CHECK_EQUAL(expected.size(), value.size());
    BOOST_CHECK(!expected.compare(value));
}


}
}
}
}

