#ifndef UTF8_FOR_CPP_CORE_H_2675DCD0_9480_4c0c_B92A_CC14C027B731
#define UTF8_FOR_CPP_CORE_H_2675DCD0_9480_4c0c_B92A_CC14C027B731

// Copyright 2006 Nemanja Trifunovic

/*
Permission is hereby granted, free of charge, to any person or organization
obtaining a copy of the software and accompanying documentation covered by
this license (the "Software") to use, reproduce, display, distribute,
execute, and transmit the Software, and to prepare derivative works of the
Software, and to permit third-parties to whom the Software is furnished to
do so, all subject to the following:

The copyright notices in the Software and this entire statement, including
the above license grant, this restriction and the following disclaimer,
must be included in all copies of the Software, in whole or in part, and
all derivative works of the Software, unless such copies or derivative
works are solely in the form of machine-executable object code generated by
a source language processor.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
DEALINGS IN THE SOFTWARE.
*/


#include <iterator>
#include <boost/type.hpp>

namespace utf8
{
    using boost::uint8_t;
    using boost::uint16_t;
    using boost::uint32_t;

// Helper code - not intended to be directly called by the library users. May be changed at any time
namespace internal
{
    // Unicode constants
    // Leading (high) surrogates: 0xd800 - 0xdbff
    // Trailing (low) surrogates: 0xdc00 - 0xdfff
    const uint16_t LEAD_SURROGATE_MIN  = 0xd800u;
    const uint16_t LEAD_SURROGATE_MAX  = 0xdbffu;
    const uint16_t TRAIL_SURROGATE_MIN = 0xdc00u;
    const uint16_t TRAIL_SURROGATE_MAX = 0xdfffu;
    const uint16_t LEAD_OFFSET         = LEAD_SURROGATE_MIN - (0x10000 >> 10);
    const uint32_t SURROGATE_OFFSET    = 0x10000u - (LEAD_SURROGATE_MIN << 10) - TRAIL_SURROGATE_MIN;

    // Maximum valid value for a Unicode code point
    const uint32_t CODE_POINT_MAX      = 0x0010ffffu;

    template<typename octet_type>
    inline uint8_t mask8(octet_type oc)
    {
        return static_cast<uint8_t>(0xff & oc);
    }
    template<typename u16_type>
    inline uint16_t mask16(u16_type oc)
    {
        return static_cast<uint16_t>(0xffff & oc);
    }
    template<typename octet_type>
    inline bool is_trail(octet_type oc)
    {
        return ((mask8(oc) >> 6) == 0x2);
    }

    template <typename u16>
    inline bool is_lead_surrogate(u16 cp)
    {
        return (cp >= LEAD_SURROGATE_MIN && cp <= LEAD_SURROGATE_MAX);
    }

    template <typename u16>
    inline bool is_trail_surrogate(u16 cp)
    {
        return (cp >= TRAIL_SURROGATE_MIN && cp <= TRAIL_SURROGATE_MAX);
    }

    template <typename u16>
    inline bool is_surrogate(u16 cp)
    {
        return (cp >= LEAD_SURROGATE_MIN && cp <= TRAIL_SURROGATE_MAX);
    }

    template <typename u32>
    inline bool is_code_point_valid(u32 cp)
    {
        return (cp <= CODE_POINT_MAX && !is_surrogate(cp) && cp != 0xfffe && cp != 0xffff);
    }

    template <typename octet_iterator>
    inline typename std::iterator_traits<octet_iterator>::difference_type
    sequence_length(octet_iterator lead_it)
    {
        uint8_t lead = mask8(*lead_it);
        if (lead < 0x80)
            return 1;
        else if ((lead >> 5) == 0x6)
            return 2;
        else if ((lead >> 4) == 0xe)
            return 3;
        else if ((lead >> 3) == 0x1e)
            return 4;
        else
            return 0;
    }

    inline bool is_overlong_sequence(uint32_t cp, int length)
    {
        if (cp < 0x80) {
            if (length != 1) 
                return true;
        }
        else if (cp < 0x800) {
            if (length != 2) 
                return true;
        }
        else if (cp < 0x10000) {
            if (length != 3) 
                return true;
        }

        return false;
    }

    enum utf_error {UTF8_OK, NOT_ENOUGH_ROOM, INVALID_LEAD, INCOMPLETE_SEQUENCE, OVERLONG_SEQUENCE, INVALID_CODE_POINT};

    /// get_sequence_x functions decode utf-8 sequences of the length x

    template <typename octet_iterator>
    utf_error get_sequence_1(octet_iterator& it, octet_iterator end, uint32_t* code_point)
    {
        if (it != end) {
            if (code_point)
                *code_point = mask8(*it);
            return UTF8_OK;
        }
        return NOT_ENOUGH_ROOM;
    }

    template <typename octet_iterator>
    utf_error get_sequence_2(octet_iterator& it, octet_iterator end, uint32_t* code_point)
    {
        utf_error ret_code = NOT_ENOUGH_ROOM;

        if (it != end) {
            uint32_t cp = mask8(*it);
            if (++it != end) {
                if (is_trail(*it)) {
                    cp = ((cp << 6) & 0x7ff) + ((*it) & 0x3f);

                    if (code_point)
                        *code_point = cp;
                    ret_code = UTF8_OK;
                }
                else
                    ret_code = INCOMPLETE_SEQUENCE;
            }
            else
                ret_code = NOT_ENOUGH_ROOM;
        }

        return ret_code;
    }

    template <typename octet_iterator>
    utf_error get_sequence_3(octet_iterator& it, octet_iterator end, uint32_t* code_point)
    {
        utf_error ret_code = NOT_ENOUGH_ROOM;

        if (it != end) {
            uint32_t cp = mask8(*it);
            if (++it != end) {
                if (is_trail(*it)) {
                    cp = ((cp << 12) & 0xffff) + ((mask8(*it) << 6) & 0xfff);
                    if (++it != end) {
                        if (is_trail(*it)) {
                            cp += (*it) & 0x3f;

                            if (code_point)
                                *code_point = cp;
                            ret_code = UTF8_OK;
                        }
                        else 
                            ret_code = INCOMPLETE_SEQUENCE;
                    }
                    else
                        ret_code = NOT_ENOUGH_ROOM;
                }
                else
                    ret_code = INCOMPLETE_SEQUENCE;
            }
            else
                ret_code = NOT_ENOUGH_ROOM;
        }

        return ret_code;
    }

    template <typename octet_iterator>
    utf_error get_sequence_4(octet_iterator& it, octet_iterator end, uint32_t* code_point)
    {
        utf_error ret_code = NOT_ENOUGH_ROOM;

        if (it != end) {
            uint32_t cp = mask8(*it);
            if (++it != end) {
                if (is_trail(*it)) {
                    cp = ((cp << 18) & 0x1fffff) + ((mask8(*it) << 12) & 0x3ffff);
                    if (++it != end) {
                        if (is_trail(*it)) {
                            cp += (mask8(*it) << 6) & 0xfff;
                            if (++it != end) {
                                if (is_trail(*it)) {
                                    cp += (*it) & 0x3f;

                                    if (code_point)
                                        *code_point = cp;
                                    ret_code = UTF8_OK;
                                }
                                else
                                    ret_code = INCOMPLETE_SEQUENCE;
                            }
                            else
                                ret_code = NOT_ENOUGH_ROOM;
                        }
                        else
                            ret_code = INCOMPLETE_SEQUENCE;
                    }
                    else
                        ret_code = NOT_ENOUGH_ROOM;
                }
                else 
                    ret_code = INCOMPLETE_SEQUENCE;
            }
            else
                ret_code = NOT_ENOUGH_ROOM;
        }

        return ret_code;
    }

    template <typename octet_iterator>
    utf_error validate_next(octet_iterator& it, octet_iterator end, uint32_t* code_point)
    {
        // Save the original value of it so we can go back in case of failure
        // Of course, it does not make much sense with i.e. stream iterators
        octet_iterator original_it = it;

        uint32_t cp = 0;
        // Determine the sequence length based on the lead octet
        typedef typename std::iterator_traits<octet_iterator>::difference_type octet_difference_type;
        octet_difference_type length = sequence_length(it);
        if (length == 0)
            return INVALID_LEAD;

        // Now that we have a valid sequence length, get trail octets and calculate the code point
        utf_error err = UTF8_OK;
        switch (length) {
            case 1:
                err = get_sequence_1(it, end, &cp);
                break;
            case 2:
                err = get_sequence_2(it, end, &cp);
            break;
            case 3:
                err = get_sequence_3(it, end, &cp);
            break;
            case 4:
                err = get_sequence_4(it, end, &cp);
            break;
        }

        if (err == UTF8_OK) {
            // Decoding succeeded. Now, security checks...
            if (is_code_point_valid(cp)) {
                if (!is_overlong_sequence(cp, length)){
                    // Passed! Return here.
                    if (code_point)
                        *code_point = cp;
                    ++it;
                    return UTF8_OK;
                }
                else
                    err = OVERLONG_SEQUENCE;
            }
            else 
                err = INVALID_CODE_POINT;
        }

        // Failure branch - restore the original value of the iterator
        it = original_it;
        return err;
    }

    template <typename octet_iterator>
    inline utf_error validate_next(octet_iterator& it, octet_iterator end) {
        return validate_next(it, end, 0);
    }

} // namespace internal

    /// The library API - functions intended to be called by the users

    // Byte order mark
    const uint8_t bom[] = {0xef, 0xbb, 0xbf};

    template <typename octet_iterator>
    octet_iterator find_invalid(octet_iterator start, octet_iterator end)
    {
        octet_iterator result = start;
        while (result != end) {
            internal::utf_error err_code = internal::validate_next(result, end);
            if (err_code != internal::UTF8_OK)
                return result;
        }
        return result;
    }

    template <typename octet_iterator>
    inline bool is_valid(octet_iterator start, octet_iterator end)
    {
        return (find_invalid(start, end) == end);
    }

    template <typename octet_iterator>
    inline bool is_bom (octet_iterator it)
    {
        return (
            (internal::mask8(*it++)) == bom[0] &&
            (internal::mask8(*it++)) == bom[1] &&
            (internal::mask8(*it))   == bom[2]
           );
    }
} // namespace utf8

#endif

