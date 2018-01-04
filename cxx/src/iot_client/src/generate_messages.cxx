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

#include "generate_messages.h"

using namespace boost;
using namespace mxp;

/*! The epoch used by MXP. */
static const posix_time::ptime epoch_2k(gregorian::date(2000, 1, 1));

/*! Initialize a program fragment.

    \param pf the program fragment to initialize.
*/
static void init_program_fragment(mxp::message::program_fragment & pf) {

    pf.program_name             = L"ClientProgram";
    pf.program_major_version    = 5;
    pf.program_minor_version    = 6;
    pf.protocol_major_version   = 0;
    pf.protocol_minor_version   = 5;
    pf.protocol_source_revision = 245;
}

/*! Initialize a object fragment.

    \param of the object fragment to initialize.
*/
void init_object_fragment(mxp::message::object_fragment & of) {

    of.object_id        = uuids::uuid("16fa3d12-525b-9f4c-9a09-ad7465208321");
    of.object_index     = 1;
    of.type_id          = uuids::uuid("9a891613-64b3-0f47-bb19-b030a131df4f");
    of.parent_object_id = uuids::uuid("39dcff04-519e-6d4a-8112-e6dd3498dbe9");
    of.object_name      = L"TestObjectName";
    of.type_name        = L"TestObjectType";
    of.owner_id         = uuids::uuid("96f6cb72-cb18-9146-960f-08175da6f492");

    array<float,  3> location             = {{ 2.0,  3.0,  4.0 }};
    array<float,  3> velocity             = {{ 5.0,  6.0,  7.0 }};
    array<float,  3> acceleration         = {{ 8.0,  9.0,  10.0 }};
    array<float,  4> orientation          = {{ 11.0, 12.0, 13.0, 14.0 }};
    array<float,  4> angular_velocity     = {{ 15.0, 16.0, 17.0, 18.0 }};
    array<float,  4> angular_acceleration = {{ 19.0, 20.0, 21.0, 22.0 }};
    uint8_t data[60] = {
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57 };

    of.location                         = location;
    of.velocity                         = velocity;
    of.acceleration                     = acceleration;
    of.orientation                      = orientation;
    of.angular_velocity                 = angular_velocity;
    of.angular_acceleration             = angular_acceleration;
    of.bounding_sphere_radius           = 23.0;
    of.mass                             = 24.0;
    of.extension_dialect                = L"TEST";
    of.extension_dialect_major_version  = 24;
    of.extension_dialect_minor_version  = 23;
    of.extension_data.reserve(60);
    for (int i = 0; i < 60; ++i) of.extension_data.push_back(data[i]);
}

/*! Initialize a interaction fragment.

    \param of the interaction fragment to initialize.
*/
static void init_interaction_fragment(mxp::message::interaction_fragment & iaf) {

    iaf.interaction_name = L"";
    iaf.source_participant_id =
            uuids::uuid("72cbf696-18cb-4691-960f-08175da6f492");
    iaf.source_object_id = uuids::uuid("00000000-0000-0000-0000-000000000000");
    iaf.target_participant_id =
        uuids::uuid("00000000-0000-0000-0000-000000000000");
    iaf.target_object_id = uuids::uuid("123dfa16-5b52-4c9f-9a09-ad7465208321");
    iaf.extension_dialect = L"TEDI";
    iaf.extension_dialect_major_version = 24;
    iaf.extension_dialect_minor_version = 25;

    uint8_t data[60] = {
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                     48, 49, 50, 51, 52, 53, 54, 55, 56, 57 };
    iaf.extension_data.reserve(60);
    for (int i = 0; i < 60; ++i) iaf.extension_data.push_back(data[i]);
}


/*! Generate a join request message.

    \return a reference join request message
 */
shared_ptr<mxp::message::join_rq> generate_join_rq() {
    shared_ptr<mxp::message::join_rq> jr(new mxp::message::join_rq());

    init_program_fragment(jr->client_program);

    jr->bubble_id = uuids::uuid("16fa9d53-525b-9f4c-9a09-ad746520873e");
    jr->avatar_id = uuids::uuid("16fa3d12-525b-9f4c-9a09-ad7465208321");
    jr->bubble_name            = L"";
    jr->location_name          = L"";
    jr->participant_identifier = L"TestParticipantName";
    jr->participant_secret     = L"TestParticipantPassphrase";
    jr->participant_realtime   = epoch_2k;
    jr->identity_provider_url  = L"http://test.identityprovider";

    return jr;
}

/*! Generate a inject request message.

    \return a reference inject request message
 */
shared_ptr<mxp::message::inject_rq> generate_inject_rq() {
    shared_ptr<mxp::message::inject_rq> ir(new mxp::message::inject_rq());

    init_object_fragment(ir->object_header);

    return ir;
}

/*! Generate a modify request message.

    \return a reference modify request message
 */
boost::shared_ptr<mxp::message::modify_rq>       generate_modify_rq() {
    shared_ptr<mxp::message::modify_rq> mr(new mxp::message::modify_rq());

    init_object_fragment(mr->object_header);

    array<float,  3> location  = {{ 20.0,  30.0,  40.0 }};
    mr->object_header.location = location;

    return mr;
}

/*! Generate an interact request message.

    \return a reference interact request message
 */
boost::shared_ptr<mxp::message::interact_rq>     generate_interact_rq() {
    shared_ptr<mxp::message::interact_rq> ir(new mxp::message::interact_rq());

    init_interaction_fragment(ir->request);

    return ir;
}

/*! Generate an examine request message.

    \return a reference examine request message
 */
boost::shared_ptr<mxp::message::examine_rq>      generate_examine_rq() {
    shared_ptr<mxp::message::examine_rq> er(new mxp::message::examine_rq());

    er->object_id    = uuids::uuid("00000000-0000-0000-0000-000000000000");
    er->object_index = 1;

    return er;
}

/*! Generate an eject request message.

    \return a reference eject request message
 */
boost::shared_ptr<mxp::message::eject_rq>        generate_eject_rq() {
    shared_ptr<mxp::message::eject_rq> er(new mxp::message::eject_rq());

    er->object_id = uuids::uuid("16fa3d12-525b-9f4c-9a09-ad7465208321");

    return er;
}

/*! Generate a leave request message.

    \return a reference leave request message
 */
boost::shared_ptr<mxp::message::leave_rq>        generate_leave_rq() {
    shared_ptr<mxp::message::leave_rq> lr(new mxp::message::leave_rq());

    return lr;
}
