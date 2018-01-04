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

#include <boost/type.hpp>
#include <boost/smart_ptr.hpp>

#include <mxp/message.h>

#include "reference_messages.h"

namespace mxp {
namespace test {
namespace iot {

using namespace boost;
using namespace mxp;

/*! The epoch used by MXP. */
static const posix_time::ptime epoch_2k(gregorian::date(2000, 1, 1));

/*! Initialize a program fragment.

    \param pf the program fragment to initialize.
    \param base the base id to start the initialization with.
*/
static void init_program_fragment(mxp::message::program_fragment & pf,
                                  unsigned int                     base) {

    pf.program_name             = L"TestProgramName";
    pf.program_major_version    = base++;
    pf.program_minor_version    = base++;
    pf.protocol_major_version   = base++;
    pf.protocol_minor_version   = base++;
    pf.protocol_source_revision = base++;
}

/*! Initialize a response fragment.

    \param rf the response fragment to initialize.
*/
static void init_response_fragment(mxp::message::response_fragment & rf) {

    rf.request_message_id = 1u;
    rf.failure_code       = 2u;
}

/*! Initialize a object fragment.

    \param of the object fragment to initialize.
*/
static void init_object_fragment(mxp::message::object_fragment & of) {

    of.object_id        = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    of.object_index     = 1;
    of.type_id          = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    of.parent_object_id = uuids::uuid("00000000-0000-0000-0000-000000000000");
    of.object_name      = L"TestObjectName";
    of.type_name        = L"TestTypeName";
    of.owner_id         = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

    array<float,  3> location             = {{ 1.0,  2.0,  3.0 }};
    array<float,  3> velocity             = {{ 4.0,  5.0,  6.0 }};
    array<float,  3> acceleration         = {{ 7.0,  8.0,  9.0 }};
    array<float,  4> orientation          = {{ 10.0, 11.0, 12.0, 13.0 }};
    array<float,  4> angular_velocity     = {{ 14.0, 15.0, 16.0, 17.0 }};
    array<float,  4> angular_acceleration = {{ 18.0, 19.0, 20.0, 21.0 }};
    uint8_t data[40] = { 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48 };

    of.location                         = location;
    of.velocity                         = velocity;
    of.acceleration                     = acceleration;
    of.orientation                      = orientation;
    of.angular_velocity                 = angular_velocity;
    of.angular_acceleration             = angular_acceleration;
    of.bounding_sphere_radius           = 22.0;
    of.mass                             = 23.0;
    of.extension_dialect                = L"TEST";
    of.extension_dialect_major_version  = 0;
    of.extension_dialect_minor_version  = 0;
    of.extension_data.clear();
    of.extension_data.resize(40);
    std::copy(data, data + 40, of.extension_data.begin());
}

/*! Initialize a interaction fragment.

    \param of the interaction fragment to initialize.
*/
static void init_interaction_fragment(mxp::message::interaction_fragment & iaf) {

    iaf.interaction_name = L"TestInteractionName";
    iaf.source_participant_id =
        uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    iaf.source_object_id = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    iaf.target_participant_id =
        uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    iaf.target_object_id = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    iaf.extension_dialect = L"TEST";
    iaf.extension_dialect_major_version = 0;
    iaf.extension_dialect_minor_version = 0;
    iaf.extension_data.push_back(49);
    iaf.extension_data.push_back(50);

}

/*! Initialize a bubble fragment.

    \param of the bubble fragment to initialize.
*/
static void init_bubble_fragment(mxp::message::bubble_fragment & bf) {

    bf.bubble_id = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    bf.bubble_name = L"TestBubble1";
    bf.bubble_asset_cache_url = L"TestCloudUrl";
    bf.owner_id = uuids::uuid("00000000-0000-0000-0000-000000000000");
    bf.bubble_address = L"TestBubbleAddress";
    bf.bubble_port = 1;

    array<float, 3> center = {{ 2.0, 3.0, 4.0 }};
    bf.bubble_center = center;
    bf.bubble_range = 5.0;
    bf.bubble_perception_range = 6.0;
    bf.bubble_realtime = epoch_2k + posix_time::milliseconds(7);

}


/*! Generate a reference acknowledge message.

    \return a reference acknowledge message
 */
shared_ptr<mxp::message::acknowledge> generate_acknowledge() {
    shared_ptr<mxp::message::acknowledge> ack(new mxp::message::acknowledge());

    ack->packet_ids.push_back(1);
    ack->packet_ids.push_back(2);
    ack->packet_ids.push_back(3);
    ack->packet_ids.push_back(4);
    ack->packet_ids.push_back(5);

    return ack;
}

/*! Generate a reference keepalive message.

    \return a reference keepalive message
 */
shared_ptr<mxp::message::keepalive> generate_keepalive() {
    shared_ptr<mxp::message::keepalive> ka(new mxp::message::keepalive());

    return ka;
}

/*! Generate a reference throttle message.

    \return a reference throttle message
 */
shared_ptr<mxp::message::throttle> generate_throttle() {
    shared_ptr<mxp::message::throttle> th(new mxp::message::throttle());

    th->transfer_rate = 10000u;

    return th;
}

/*! Generate a challenge request message.

    \return a reference challenge request message
 */
shared_ptr<mxp::message::challenge_rq> generate_challenge_rq() {
    shared_ptr<mxp::message::challenge_rq> cr(new mxp::message::challenge_rq());

    for (unsigned int i = 0; i < 64; ++i) {
        cr->challenge_rq_data[i] = (uint8_t) i;
    }

    return cr;
}

/*! Generate a challenge response message.

    \return a reference challenge response message
 */
shared_ptr<mxp::message::challenge_rsp> generate_challenge_rsp() {
    shared_ptr<mxp::message::challenge_rsp>
                                        cr(new mxp::message::challenge_rsp());

    for (unsigned int i = 0; i < 64; ++i) {
        cr->challenge_rsp_data[i] = (uint8_t) i;
    }

    return cr;
}

/*! Generate a join request message.

    \return a reference join request message
 */
shared_ptr<mxp::message::join_rq> generate_join_rq() {
    shared_ptr<mxp::message::join_rq> jr(new mxp::message::join_rq());

    init_program_fragment(jr->client_program, 1u);

    jr->bubble_id = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    jr->avatar_id = uuids::uuid("00000000-0000-0000-0000-000000000000");
    jr->bubble_name            = L"TestBubbleName";
    jr->location_name          = L"TestLocation";
    jr->participant_identifier = L"TestParticipantName";
    jr->participant_secret     = L"TestParticipantPassphrase";
    jr->participant_realtime   = epoch_2k + posix_time::milliseconds(10);
    jr->identity_provider_url  = L"IdentityProviderUrl";

    return jr;
}

/*! Generate a join response message.

    \return a reference join response message
 */
shared_ptr<mxp::message::join_rsp> generate_join_rsp() {
    shared_ptr<mxp::message::join_rsp> jr(new mxp::message::join_rsp());

    init_response_fragment(jr->response_header);

    jr->bubble_id      = uuids::uuid("00000000-0000-0000-0000-000000000000");
    jr->participant_id = uuids::uuid("00000000-0000-0000-0000-000000000000");
    jr->avatar_id      = uuids::uuid("00000000-0000-0000-0000-000000000000");
    jr->bubble_name             = L"TestBubbleName";
    jr->bubble_asset_cache_url  = L"TestBubbleAssetCacheUrl";
    jr->bubble_range            = 3.f;
    jr->bubble_perception_range = 4.f;
    jr->bubble_realtime         = epoch_2k + posix_time::milliseconds(5);

    init_program_fragment(jr->server_program, 6);

    return jr;
}

/*! Generate a leave request message.

    \return a reference leave request message
 */
shared_ptr<mxp::message::leave_rq> generate_leave_rq() {
    shared_ptr<mxp::message::leave_rq> lr(new mxp::message::leave_rq());

    return lr;
}

/*! Generate a leave response message.

    \return a reference leave response message
 */
shared_ptr<mxp::message::leave_rsp> generate_leave_rsp() {
    shared_ptr<mxp::message::leave_rsp> lr(new mxp::message::leave_rsp());

    init_response_fragment(lr->response_header);

    return lr;
}

/*! Generate a inject request message.

    \return a reference inject request message
 */
shared_ptr<mxp::message::inject_rq> generate_inject_rq() {
    shared_ptr<mxp::message::inject_rq> ir(new mxp::message::inject_rq());

    init_object_fragment(ir->object_header);

    ir->object_header.extension_dialect_major_version = 24;
    ir->object_header.extension_dialect_minor_version = 25;
    uint8_t data[45] = {  49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53 };
    ir->object_header.extension_data.clear();
    ir->object_header.extension_data.resize(45);
    std::copy(data, data + 45, ir->object_header.extension_data.begin());

    return ir;
}

/*! Generate a inject response message.

    \return a reference inject response message
 */
shared_ptr<mxp::message::inject_rsp> generate_inject_rsp() {
    shared_ptr<mxp::message::inject_rsp> ir(new mxp::message::inject_rsp());

    init_response_fragment(ir->response_header);

    return ir;
}

/*! Generate a modify request message.

    \return a reference modify request message
 */
shared_ptr<mxp::message::modify_rq> generate_modify_rq() {
    shared_ptr<mxp::message::modify_rq> mr(new mxp::message::modify_rq());

    init_object_fragment(mr->object_header);

    uint8_t data[398] = { 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56 };

    mr->object_header.extension_data.clear();
    mr->object_header.extension_data.resize(398);
    std::copy(data, data + 398, mr->object_header.extension_data.begin());

    return mr;
}

/*! Generate a modify response message.

    \return a reference modify response message
 */
shared_ptr<mxp::message::modify_rsp> generate_modify_rsp() {
    shared_ptr<mxp::message::modify_rsp> mr(new mxp::message::modify_rsp());

    init_response_fragment(mr->response_header);

    return mr;
}

/*! Generate a eject request message.

    \return a reference eject request message
 */
shared_ptr<mxp::message::eject_rq> generate_eject_rq() {
    shared_ptr<mxp::message::eject_rq> er(new mxp::message::eject_rq());

    er->object_id = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

    return er;
}

/*! Generate a eject response message.

    \return a reference eject response message
 */
shared_ptr<mxp::message::eject_rsp> generate_eject_rsp() {
    shared_ptr<mxp::message::eject_rsp> er(new mxp::message::eject_rsp());

    init_response_fragment(er->response_header);

    return er;
}

/*! Generate a interact request message.

    \return a reference interact request message
 */
shared_ptr<mxp::message::interact_rq> generate_interact_rq() {
    shared_ptr<mxp::message::interact_rq> ir(new mxp::message::interact_rq());

    init_interaction_fragment(ir->request);

    uint8_t data[161] = { 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49 };

    ir->request.extension_data.clear();
    ir->request.extension_data.resize(161);
    std::copy(data, data + 161, ir->request.extension_data.begin());

    return ir;
}

/*! Generate a interact response message.

    \return a reference interact responce message
 */
shared_ptr<mxp::message::interact_rsp> generate_interact_rsp() {
   shared_ptr<mxp::message::interact_rsp> ir(new mxp::message::interact_rsp());

    init_response_fragment(ir->response_header);

    init_interaction_fragment(ir->response);

    uint8_t data[156] = { 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54 };

    ir->response.extension_data.clear();
    ir->response.extension_data.resize(156);
    std::copy(data, data + 156, ir->response.extension_data.begin());

    return ir;
}

/*! Generate a examine response message.

    \return a reference examine request message
 */
shared_ptr<mxp::message::examine_rq> generate_examine_rq() {
    shared_ptr<mxp::message::examine_rq> er(new mxp::message::examine_rq());

    er->object_id    = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    er->object_index = 1;

    return er;
}

/*! Generate a examine response message.

    \return a reference examine response message
 */
shared_ptr<mxp::message::examine_rsp> generate_examine_rsp() {
    shared_ptr<mxp::message::examine_rsp> er(new mxp::message::examine_rsp());

    init_response_fragment(er->response_header);
    init_object_fragment(er->object_header);

    return er;
}

/*! Generate a attach request message.

    \return a reference attach request message
 */
shared_ptr<mxp::message::attach_rq> generate_attach_rq() {
    shared_ptr<mxp::message::attach_rq> ar(new mxp::message::attach_rq());

    ar->target_bubble_id = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    init_bubble_fragment(ar->source_bubble);
    init_program_fragment(ar->source_bubble_server,1);

    return ar;
}

/*! Generate a attach response message.

    \return a reference attach response message
 */
shared_ptr<mxp::message::attach_rsp> generate_attach_rsp() {
    shared_ptr<mxp::message::attach_rsp> ar(new mxp::message::attach_rsp());

    init_response_fragment(ar->response_header);
    init_bubble_fragment(ar->target_bubble);
    init_program_fragment(ar->target_bubble_server,1);

    return ar;
}

/*! Generate a detach request message.

    \return a reference detach request message
 */
shared_ptr<mxp::message::detach_rq> generate_detach_rq() {
    shared_ptr<mxp::message::detach_rq> dr(new mxp::message::detach_rq());

    return dr;
}

/*! Generate a detach response message.

    \return a reference detach response message
 */
shared_ptr<mxp::message::detach_rsp> generate_detach_rsp() {
    shared_ptr<mxp::message::detach_rsp> dr(new mxp::message::detach_rsp());

    init_response_fragment(dr->response_header);

    return dr;
}

/*! Generate a handover request message.

    \return a reference handover request message
 */
shared_ptr<mxp::message::handover_rq> generate_handover_rq() {
    shared_ptr<mxp::message::handover_rq> hr(new mxp::message::handover_rq());

    hr->source_bubble_id = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    hr->target_bubble_id = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

    init_object_fragment(hr->object_header);

    uint8_t data[13] = {  49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51 };

    hr->object_header.extension_data.clear();
    hr->object_header.extension_data.resize(13);
    std::copy(data, data + 13, hr->object_header.extension_data.begin());

    return hr;
}

/*! Generate a handover response message.

    \return a reference handover response message
 */
shared_ptr<mxp::message::handover_rsp> generate_handover_rsp() {
    shared_ptr<mxp::message::handover_rsp> hr(new mxp::message::handover_rsp());

    init_response_fragment(hr->response_header);

    return hr;
}

/*! Generate a list bubbles request message.

    \return a reference list bubbles request message
 */
shared_ptr<mxp::message::list_bubbles_rq> generate_list_bubbles_rq() {
    shared_ptr<mxp::message::list_bubbles_rq> lbr(new mxp::message::list_bubbles_rq());

    lbr->list_type = mxp::message::list_bubbles_rq::LIST_TYPE_HOSTED;

    return lbr;
}

/*! Generate a list bubbles response message.

    \return a reference list bubbles response message
 */
shared_ptr<mxp::message::list_bubbles_rsp> generate_list_bubbles_rsp() {
    shared_ptr<mxp::message::list_bubbles_rsp>
        lbr(new mxp::message::list_bubbles_rsp());

    mxp::message::bubble_fragment bf;

    init_bubble_fragment(bf);

    bf.bubble_name = L"TestBubble1";
    lbr->bubble_fragments.push_back(bf);

    bf.bubble_name = L"TestBubble2";
    lbr->bubble_fragments.push_back(bf);

    bf.bubble_name = L"TestBubble3";
    lbr->bubble_fragments.push_back(bf);

    bf.bubble_name = L"TestBubble4";
    lbr->bubble_fragments.push_back(bf);

    bf.bubble_name = L"TestBubble5";
    lbr->bubble_fragments.push_back(bf);

    return lbr;
}

/*! Generate a perception event message.

    \return a reference perception event message
 */
shared_ptr<mxp::message::perception_event> generate_perception_event() {
    shared_ptr<mxp::message::perception_event>
        pe(new mxp::message::perception_event());

    init_object_fragment(pe->object_header);
    uint8_t data[45] = { 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53 };

    pe->object_header.extension_data.clear();
    pe->object_header.extension_data.resize(45);
    std::copy(data, data + 45, pe->object_header.extension_data.begin());

    return pe;
}

/*! Generate a movement event message.

    \return a reference movement event message
 */
shared_ptr<mxp::message::movement_event> generate_movement_event() {
    shared_ptr<mxp::message::movement_event>
        me(new mxp::message::movement_event());

    array<float, 3> location = {{ 1.0, 2.0, 3.0 }};
    array<float, 4> orientation =  {{ 10.0, 11.0, 12.0, 13.0 }};
    me->object_index = 1;
    me->location = location;
    me->orientation = orientation;

    return me;
}

/*! Generate a disappearance event message.

    \return a reference disappearance event message
 */
shared_ptr<mxp::message::disappearance_event> generate_disappearance_event() {
    shared_ptr<mxp::message::disappearance_event>
        de(new mxp::message::disappearance_event());

    de->object_index = 1;

    return de;
}

/*! Generate a action event message.

    \return a reference action event message
 */
shared_ptr<mxp::message::action_event> generate_action_event() {
    shared_ptr<mxp::message::action_event> ae(new mxp::message::action_event());

    uint8_t data[205] = { 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                      48, 49, 50, 51, 52, 53 };

    ae->action_name = L"TestInteractionName";
    ae->source_object_id = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    ae->observation_radius = 100.0;
    ae->extension_dialect = L"TEST";

    ae->extension_dialect_major_version = 1;
    ae->extension_dialect_minor_version = 2;

    ae->extension_data.clear();
    ae->extension_data.resize(205);
    std::copy(data, data + 205, ae->extension_data.begin());

    return ae;
}

/*! Generate a handover event message.

    \return a reference handover event message
 */
shared_ptr<mxp::message::handover_event> generate_handover_event() {
    shared_ptr<mxp::message::handover_event>
                                        he(new mxp::message::handover_event());

    he->source_bubble_id = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    he->target_bubble_id = uuids::uuid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

    init_object_fragment(he->object_header);

    uint8_t data[13] = { 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51 };

    he->object_header.extension_data.clear();
    he->object_header.extension_data.resize(13);
    std::copy(data, data + 13, he->object_header.extension_data.begin());

    return he;
}

/*! Generate a synchronization event message.

    \return a reference synchronization event message
 */
shared_ptr<mxp::message::synchronization_begin_event>
generate_synchronization_begin_event() {
    shared_ptr<mxp::message::synchronization_begin_event>
        se(new mxp::message::synchronization_begin_event());

    se->object_count = 1;

    return se;
}

/*! Generate a synchronization end event message.

    \return a reference synchronization event message
 */
shared_ptr<mxp::message::synchronization_end_event>
generate_synchronization_end_event() {
    shared_ptr<mxp::message::synchronization_end_event>
        se(new mxp::message::synchronization_end_event());

    return se;
}

}
}
}
