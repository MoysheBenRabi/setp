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

#include "test_serialization.h"

namespace mxp {
namespace test {
namespace serialization {
namespace serialization {

using namespace boost;
using namespace mxp;

void test_byte() {
    static uint8_t expected[] = { 0xf5 };
    uint8_t        bytes[1];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<int8_t>(b, -11);

    BOOST_CHECK_EQUAL(b - bytes, 1);
    BOOST_CHECK_EQUAL(len, 1u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 1, bytes, bytes + 1);
}

void test_ubyte() {
    static uint8_t expected[] = { 0x0b };
    uint8_t        bytes[1];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<uint8_t>(b, 11);

    BOOST_CHECK_EQUAL(b - bytes, 1);
    BOOST_CHECK_EQUAL(len, 1u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 1, bytes, bytes + 1);
}

void test_short() {
    static uint8_t expected[] = { 0xa9, 0xfb };
    uint8_t        bytes[2];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<int16_t>(b, -1111);

    BOOST_CHECK_EQUAL(b - bytes, 2);
    BOOST_CHECK_EQUAL(len, 2u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 2, bytes, bytes + 2);
}

void test_ushort() {
    static uint8_t expected[] = { 0x67, 0x2b };
    uint8_t        bytes[2];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<uint16_t>(b, 11111);

    BOOST_CHECK_EQUAL(b - bytes, 2);
    BOOST_CHECK_EQUAL(len, 2u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 2, bytes, bytes + 2);
}

void test_int() {
    static uint8_t expected[] = { 0x39, 0xca, 0xc5, 0xbd };
    uint8_t        bytes[4];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<int32_t>(b, -1111111111);

    BOOST_CHECK_EQUAL(b - bytes, 4);
    BOOST_CHECK_EQUAL(len, 4u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 4, bytes, bytes + 4);
}

void test_uint() {
    static uint8_t expected[] = { 0xc7, 0x35, 0x3a, 0x42 };
    uint8_t        bytes[4];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<uint32_t>(b, 1111111111u);

    BOOST_CHECK_EQUAL(b - bytes, 4);
    BOOST_CHECK_EQUAL(len, 4u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 4, bytes, bytes + 4);
}

void test_long() {
    static uint8_t expected[] = { 0x39, 0x8e, 0x3b, 0xd4,
                                  0x54, 0x8a, 0x94, 0xf0 };
    uint8_t        bytes[8];
    uint8_t      * b = bytes;

    unsigned int len =
            mxp::serialization::write_it<int64_t>(b, -1111111111111111111ll);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(len, 8u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 8, bytes, bytes + 8);
}

void test_ulong() {
    static uint8_t expected[] = { 0xc7, 0x71, 0xc4, 0x2b,
                                  0xab, 0x75, 0x6b, 0x0f };
    uint8_t        bytes[8];
    uint8_t      * b = bytes;

    unsigned int len =
            mxp::serialization::write_it<uint64_t>(b, 1111111111111111111ull);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(len, 8u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 8, bytes, bytes + 8);
}

void test_float() {
    static uint8_t expected[] = { 0x39, 0x8e, 0xe3, 0x3d };
    uint8_t        bytes[4];
    uint8_t      * b = bytes;

    unsigned int len =
                mxp::serialization::write_it<float>(b, 0.1111111111111111f);

    BOOST_CHECK_EQUAL(b - bytes, 4);
    BOOST_CHECK_EQUAL(len, 4u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 4, bytes, bytes + 4);
}

void test_double() {
    static uint8_t expected[] = { 0x1c, 0xc7, 0x71, 0x1c,
                                  0xc7, 0x71, 0xbc, 0x3f };
    uint8_t        bytes[8];
    uint8_t      * b = bytes;

    unsigned int len =
                mxp::serialization::write_it<double>(b, 0.1111111111111111111);

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(len, 8u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 8, bytes, bytes + 8);
}

void test_array() {
    static uint8_t expected[] = { 0x39, 0x8e, 0xe3, 0x3d,
                                  0x39, 0x8e, 0x63, 0x3e,
                                  0xab, 0xaa, 0xaa, 0x3e };
    uint8_t        bytes[12];
    uint8_t      * b = bytes;

    array<float, 3> value;
    value[0] = 0.1111111111111111f;
    value[1] = 0.2222222222222222f;
    value[2] = 0.3333333333333333f;

    unsigned int len =
                    mxp::serialization::write_it<array<float, 3> >(b, value);

    BOOST_CHECK_EQUAL(b - bytes, 12);
    BOOST_CHECK_EQUAL(len, 12u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 12, bytes, bytes + 12);
}

void test_date() {
    static uint8_t expected[] = { 0x00, 0xa0, 0xad, 0x24,
                                  0x42, 0x00, 0x00, 0x00 };
    uint8_t        bytes[8];
    uint8_t      * b = bytes;

    unsigned int len =
            mxp::serialization::write_it<posix_time::ptime>(b,
                               posix_time::ptime(gregorian::date(2009, 1, 1)));

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(len, 8u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 8, bytes, bytes + 8);
}

void test_uuid() {
    static uint8_t expected[] = { 0xd4, 0x36, 0xef, 0xa8, 0x91, 0xbe, 0x48,
                                  0xf8, 0xaa, 0xd4, 0x9b, 0x85, 0xbd, 0xe1,
                                  0xc6, 0x03 };
    uint8_t        bytes[16];
    uint8_t      * b = bytes;

    unsigned int len = mxp::serialization::write_it<uuids::uuid>(b,
                    uuids::uuid("d436efa8-91be-48f8-aad4-9b85bde1c603"));

    BOOST_CHECK_EQUAL(b - bytes, 16);
    BOOST_CHECK_EQUAL(len, 16u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 16, bytes, bytes + 16);
}

void test_bytes() {
    static uint8_t expected[] = { 0x01, 0x02, 0x03, 0x04 };
    uint8_t        bytes[2];
    uint8_t      * b = bytes;

    unsigned int len =
                    mxp::serialization::write_range_it<uint8_t>(b, expected, 2);

    BOOST_CHECK_EQUAL(b - bytes, 2);
    BOOST_CHECK_EQUAL(len, 2u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 2, bytes, bytes + 2);
}

void test_string() {
    static uint8_t expected[] = { 'H', 'e', 'l', 'l', 0xc3, 0xb3, '!', '\0' };
    uint8_t        bytes[8];
    uint8_t      * b = bytes;

    unsigned int len =
            mxp::serialization::write_it<std::wstring>(b, L"Hell\xf3!");

    BOOST_CHECK_EQUAL(b - bytes, 8);
    BOOST_CHECK_EQUAL(len, 8u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 8, bytes, bytes + 8);
}

/*! Test string serialization into a fixed space. */
void test_string_fixed() {
    static uint8_t expected[] = { 'H', 'e', 'l', 'l', 0xc3, 0xb3, '!',
                                  0, 0, 0 };
    uint8_t        bytes[10];
    uint8_t      * b = bytes;

    unsigned int len =
            mxp::serialization::write_it<std::wstring>(b, L"Hell\xf3!", 10u);

    BOOST_CHECK_EQUAL(b - bytes, 10);
    BOOST_CHECK_EQUAL(len, 10u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 10, bytes, bytes + 10);
}

/*! Test string serialization into a fixed space, so that the output does
    not fit.
 */
void test_string_fixed_truncate() {
    static uint8_t expected[] = { 'H', 'e', 'l', 'l', 0xc3, 0xb3, };
    uint8_t        bytes[6];
    uint8_t      * b = bytes;

    unsigned int len =
            mxp::serialization::write_it<std::wstring>(b, L"Hell\xf3!", 6u);

    BOOST_CHECK_EQUAL(b - bytes, 6);
    BOOST_CHECK_EQUAL(len, 6u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 6, bytes, bytes + 6);
}

/*! Test string serialization into a fixed space, so that the output does
    not fit, and the last UTF-8 sequence would fit only partially, and is
    thus removed.
 */
void test_string_fixed_truncate_partial() {
    static uint8_t expected[] = { 'H', 'e', 'l', 'l', 0x00, };
    uint8_t        bytes[5];
    uint8_t      * b = bytes;

    unsigned int len =
            mxp::serialization::write_it<std::wstring>(b, L"Hell\xf3!", 5u);

    BOOST_CHECK_EQUAL(b - bytes, 5);
    BOOST_CHECK_EQUAL(len, 5u);
    BOOST_CHECK_EQUAL_COLLECTIONS(expected, expected + 5, bytes, bytes + 5);
}


}
}
}
}

