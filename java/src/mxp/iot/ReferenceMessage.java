/*
 * Copyright (c) 2009-2010 Tyrell Corporation.
 *
 * The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 *
 * The Original Code is an implementation of the Metaverse eXchange Protocol.
 *
 * The Initial Developer of the Original Code is Akos Maroy.
 * All Rights Reserved.
 *
 * Contributor(s): Akos Maroy.
 *
 * Alternatively, the contents of this file may be used under the terms
 * of the Affero General Public License (the  "AGPL"), in which case the
 * provisions of the AGPL are applicable instead of those
 * above. If you wish to allow use of your version of this file only
 * under the terms of the AGPL and not to allow others to use
 * your version of this file under the MPL, indicate your decision by
 * deleting the provisions above and replace them with the notice and
 * other provisions required by the AGPL. If you do not delete
 * the provisions above, a recipient may use your version of this file
 * under either the MPL or the AGPL.
 */
package mxp.iot;

import java.util.Date;
import java.util.GregorianCalendar;
import java.util.UUID;
import java.util.Vector;

import mxp.message.Acknowledge;
import mxp.message.ActionEvent;
import mxp.message.AttachRequest;
import mxp.message.AttachResponse;
import mxp.message.BubbleFragment;
import mxp.message.ChallengeRequest;
import mxp.message.ChallengeResponse;
import mxp.message.DetachRequest;
import mxp.message.DetachResponse;
import mxp.message.DisappearanceEvent;
import mxp.message.EjectRequest;
import mxp.message.EjectResponse;
import mxp.message.ExamineRequest;
import mxp.message.ExamineResponse;
import mxp.message.HandoverEvent;
import mxp.message.HandoverRequest;
import mxp.message.HandoverResponse;
import mxp.message.InjectRequest;
import mxp.message.InjectResponse;
import mxp.message.InteractRequest;
import mxp.message.InteractResponse;
import mxp.message.InteractionFragment;
import mxp.message.JoinRequest;
import mxp.message.JoinResponse;
import mxp.message.Keepalive;
import mxp.message.LeaveRequest;
import mxp.message.LeaveResponse;
import mxp.message.ListBubblesRequest;
import mxp.message.ListBubblesResponse;
import mxp.message.ModifyRequest;
import mxp.message.ModifyResponse;
import mxp.message.MovementEvent;
import mxp.message.ObjectFragment;
import mxp.message.PerceptionEvent;
import mxp.message.ProgramFragment;
import mxp.message.ResponseFragment;
import mxp.message.SynchronizationBeginEvent;
import mxp.message.SynchronizationEndEvent;
import mxp.message.Throttle;

/**
 * Create reference messages, that are used in the inter-operability testing.
 *
 * @see <a href="http://iot.bubblecloud.org/reference_messages.aspx">
 *      reference messages</a>
 */
public final class ReferenceMessage {

    /**
     * The number of milliseconds at 1st January 2000 elapsed since
     * the usual UNIX epoch of 1st January 1970.
     */
     private static final long EPOCH2K =
                new GregorianCalendar(2000, 0, 1, 0, 0, 0).getTime().getTime();

     /**
      * In-accessible constructor.
      */
     private ReferenceMessage() {
     }

    /**
     * Generate a test Acknowledge message.
     *
     * @return an acknowledge message.
     */
    public static Acknowledge generateAcknowledge() {
        Acknowledge                ack   = new Acknowledge();

        Vector<Integer> packetIds = new Vector<Integer>(5);
        packetIds.add(1);
        packetIds.add(2);
        packetIds.add(3);
        packetIds.add(4);
        packetIds.add(5);

        ack.setPacketIds(packetIds);

        return ack;
    }

    /**
     * Generate a test Keepalive message.
     *
     * @return a Keepalive message.
     */
    public static Keepalive generateKeepalive() {
        Keepalive   ka = new Keepalive();

        return ka;
    }

    /**
     * Generate a test Throttle message.
     *
     * @return an throttle message.
     */
    public static Throttle generateThrottle() {
        Throttle                   throttle = new Throttle();

        throttle.setMaxTransferRate(10000);

        return throttle;
    }

    /**
     * Generate a test Challenege request message.
     *
     * @return a Challenge request message.
     */
    public static ChallengeRequest generateChallengeRequest() {
        ChallengeRequest           cr = new ChallengeRequest();
        byte[]                     data = new byte[64];
        for (int i = 0; i < 64; ++i) {
            data[i] = (byte) i;
        }

        cr.setChallengeRequestData(data);

        return cr;
    }

    /**
     * Generate a test Challenege response message.
     *
     * @return a Challenge request message.
     */
    public static ChallengeResponse generateChallengeResponse() {
        ChallengeResponse          cr = new ChallengeResponse();
        byte[]                     data = new byte[64];
        for (int i = 0; i < 64; ++i) {
            data[i] = (byte) i;
        }
        cr.setChallengeResponseData(data);

        return cr;
    }

    /**
     * Generate a test Program fragment.
     *
     * @param base start to fill numeric parameters with this value, and
     *        then increse the value by one for each subsequent parameter
     * @return a Program fragment
     */
    public static ProgramFragment generateProgramFragment(int base) {
        ProgramFragment            pf = new ProgramFragment();

        pf.setProgramName("TestProgramName");
        pf.setProgramMajorVersion((byte) base++);
        pf.setProgramMinorVersion((byte) base++);
        pf.setProtocolMajorVersion((byte) base++);
        pf.setProtocolMinorVersion((byte) base++);
        pf.setProtocolSourceRevision(base++);

        return pf;
    }

    /**
     * Generate a test Join request message.
     *
     * @return a Join request message
     */
    public static JoinRequest generateJoinRequest() {
        ProgramFragment            pf = generateProgramFragment(1);

        JoinRequest                jr = new JoinRequest();

        jr.setBubbleId(new UUID(0xaaaaaaaabbbbccccL,
                                0xddddeeeeeeeeeeeeL));
        jr.setAvatarId(new UUID(0x0000000000000000L,
                                0x0000000000000000L));
        jr.setBubbleName("TestBubbleName");
        jr.setLocationName("TestLocation");
        jr.setParticipantId("TestParticipantName");
        jr.setParticipantSecret("TestParticipantPassphrase");
        jr.setParticipantRealtime(new Date(EPOCH2K + 10));
        jr.setIdentityProviderUrl("IdentityProviderUrl");
        jr.setClientProgram(pf);

        return jr;
    }

    /**
     * Generate a test Response fragment.
     *
     * @return a Response fragment
     */
    public static ResponseFragment generateResponseFragment() {
        ResponseFragment           rf = new ResponseFragment();
        rf.setRequestMessageId(1);
        rf.setFailureCode((byte) 0x02);

        return rf;
    }

    /**
     * Generate a test Join response message.
     *
     * @return a Join response message
     */
    public static JoinResponse generateJoinResponse() {
        ResponseFragment           rf = generateResponseFragment();
        ProgramFragment            pf = generateProgramFragment(6);

        JoinResponse               jr = new JoinResponse();

        jr.setResponseHeader(rf);
        jr.setBubbleId(new UUID(0x0000000000000000L,
                                0x0000000000000000L));
        jr.setParticipantId(new UUID(0x0000000000000000L,
                                     0x0000000000000000L));
        jr.setAvatarId(new UUID(0x0000000000000000L,
                                0x0000000000000000L));
        jr.setBubbleName("TestBubbleName");
        jr.setBubbleAssetCacheUrl("TestBubbleAssetCacheUrl");
        jr.setBubbleRange(3f);
        jr.setBubblePerceptionRange(4f);
        jr.setBubbleRealtime(new Date(EPOCH2K + 5));
        jr.setServerProgram(pf);

        return jr;
    }

    /**
     * Generate a test Leave request message.
     *
     * @return a Leave request message
     */
    public static LeaveRequest generateLeaveRequest() {
        LeaveRequest               lr = new LeaveRequest();

        return lr;
    }

    /**
     * Generate a test Leave response message.
     *
     * @return a Leave response message
     */
    public static LeaveResponse generateLeaveResponse() {
        LeaveResponse              lr = new LeaveResponse();
        ResponseFragment           rf = generateResponseFragment();
        lr.setResponseHeader(rf);

        return lr;
    }

    /**
     * Generate a test Object fragment.
     *
     * @return a test Object fragment.
     */
    public static ObjectFragment generateObjectFragment() {
        ObjectFragment              of = new ObjectFragment();

        of.setObjectId(new UUID(0xaaaaaaaabbbbccccL,
                                0xddddeeeeeeeeeeeeL));
        of.setObjectIndex(1);
        of.setTypeId(new UUID(0xaaaaaaaabbbbccccL,
                              0xddddeeeeeeeeeeeeL));
        of.setParentObjectId(new UUID(0x0000000000000000L,
                                      0x0000000000000000L));
        of.setObjectName("TestObjectName");
        of.setTypeName("TestTypeName");
        of.setOwnerId(new UUID(0xaaaaaaaabbbbccccL,
                               0xddddeeeeeeeeeeeeL));
        of.setLocation(new float[] { 1f, 2f, 3f });
        of.setVelocity(new float[] { 4f, 5f, 6f });
        of.setAcceleration(new float[] { 7f, 8f, 9f });
        of.setOrientation(new float[] { 10f, 11f, 12f, 13f });
        of.setAngularVelocity(new float[] { 14f, 15f, 16f, 17f });
        of.setAngularAcceleration(new float[] { 18f, 19f, 20f, 21f });
        of.setBoundingSphereRadius(22f);
        of.setMass(23f);
        of.setExtensionDialect("TEST");
        of.setExtensionDialectMajorVersion((byte) 0);
        of.setExtensionDialectMinorVersion((byte) 0);
        of.setExtensionData(new byte[] {
                 49, 50, 51, 52, 53, 54, 55, 56, 57,
             48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
             48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
             48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
             48 });

        return of;
    }

    /**
     * Generate an Inject request message.
     *
     * @return a test Inject request.
     */
    public static InjectRequest generateInjectRequest() {
        ObjectFragment              of = generateObjectFragment();
        InjectRequest               ir = new InjectRequest();

        of.setExtensionDialectMajorVersion((byte) 24);
        of.setExtensionDialectMinorVersion((byte) 25);
        of.setExtensionData(new byte[] {
                    49, 50, 51, 52, 53, 54, 55, 56, 57,
                48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
                48, 49, 50, 51, 52, 53 });
        ir.setObject(of);

        return ir;
    }

    /**
     * Generate an Inject response message.
     *
     * @return a test Inject response.
     */
    public static InjectResponse generateInjectResponse() {
        InjectResponse             ir = new InjectResponse();
        ResponseFragment           rf = generateResponseFragment();

        ir.setResponseHeader(rf);

        return ir;
    }

    /**
     * Generate a Modify request message.
     *
     * @return a test Modify request.
     */
    public static ModifyRequest generateModifyRequest() {
        ObjectFragment             of = generateObjectFragment();
        ModifyRequest              mr = new ModifyRequest();

        of.setExtensionData(new byte[] {
                    49, 50, 51, 52, 53, 54, 55, 56, 57,
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
                48, 49, 50, 51, 52, 53, 54, 55, 56 });

        mr.setObject(of);

        return mr;
    }

    /**
     * Generate a Modify response message.
     *
     * @return a test Modify response.
     */
    public static ModifyResponse generateModifyResponse() {
        ModifyResponse             mr = new ModifyResponse();
        ResponseFragment           rf = generateResponseFragment();

        mr.setResponseHeader(rf);

        return mr;
    }

    /**
     * Generate an Eject request message.
     *
     * @return a test Eject request.
     */
    public static EjectRequest generateEjectRequest() {
        EjectRequest               er = new EjectRequest();

        er.setObjectId(new UUID(0xaaaaaaaabbbbccccL,
                                0xddddeeeeeeeeeeeeL));

        return er;
    }

    /**
     * Generate an Eject response message.
     *
     * @return a test Eject response.
     */
    public static EjectResponse generateEjectResponse() {
        EjectResponse              er = new EjectResponse();
        ResponseFragment           rf = generateResponseFragment();

        er.setResponseHeader(rf);

        return er;
    }

    /**
     * Generate an a test Interaction Fragment.
     *
     * @return a test Interaction fragment.
     */
    public static InteractionFragment generateInteractionFragment() {
        InteractionFragment         iaf = new InteractionFragment();

        iaf.setInteractionName("TestInteractionName");
        iaf.setSourceParticipantId(new UUID(0xaaaaaaaabbbbccccL,
                                            0xddddeeeeeeeeeeeeL));
        iaf.setSourceObjectId(new UUID(0xaaaaaaaabbbbccccL,
                                       0xddddeeeeeeeeeeeeL));
        iaf.setTargetParticipantId(new UUID(0xaaaaaaaabbbbccccL,
                                            0xddddeeeeeeeeeeeeL));
        iaf.setTargetObjectId(new UUID(0xaaaaaaaabbbbccccL,
                                       0xddddeeeeeeeeeeeeL));
        iaf.setExtensionDialect("TEST");
        iaf.setExtensionDialectMajorVersion((byte) 0);
        iaf.setExtensionDialectMinorVersion((byte) 0);
        iaf.setExtensionData(new byte[] { 49, 50 });

        return iaf;
    }

    /**
     * Generate an Interact request message.
     *
     * @return a test Interact request.
     */
    public static InteractRequest generateInteractRequest() {
        InteractionFragment         iaf = generateInteractionFragment();
        InteractRequest             ir = new InteractRequest();

        iaf.setExtensionData(new byte[] {
                    49, 50, 51, 52, 53, 54, 55, 56, 57,
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
                48, 49 });

        ir.setRequest(iaf);

        return ir;
    }

    /**
     * Generate an Interact response message.
     *
     * @return a test Interact response.
     */
    public static InteractResponse generateInteractResponse() {
        ResponseFragment            rf = generateResponseFragment();
        InteractionFragment         iaf = generateInteractionFragment();
        InteractResponse            ir = new InteractResponse();

        ir.setResponseHeader(rf);

        iaf.setExtensionData(new byte[] {
                  49, 50, 51, 52, 53, 54, 55, 56, 57,
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
              48, 49, 50, 51, 52, 53, 54 });

        ir.setResponse(iaf);

        return ir;
    }

    /**
     * Generate an Examine request message.
     *
     * @return a test Examine request.
     */
    public static ExamineRequest generateExamineRequest() {
        ExamineRequest             er = new ExamineRequest();

        er.setObjectId(new UUID(0xaaaaaaaabbbbccccL,
                                0xddddeeeeeeeeeeeeL));
        er.setObjectIndex(1);

        return er;
    }

    /**
     * Generate an Examine response message.
     *
     * @return a test Examine response.
     */
    public static ExamineResponse generateExamineResponse() {
        ResponseFragment            rf = generateResponseFragment();
        ObjectFragment              of = generateObjectFragment();
        ExamineResponse             er = new ExamineResponse();

        er.setResponseHeader(rf);
        er.setObject(of);

        return er;
    }

    /**
     * Generate a Bubble fragment.
     *
     * @return a test Bubble fragment.
     */
    public static BubbleFragment generateBubbleFragment() {
        BubbleFragment              bf = new BubbleFragment();

        bf.setBubbleId(new UUID(0xaaaaaaaabbbbccccL,
                                0xddddeeeeeeeeeeeeL));
        bf.setBubbleName("TestBubble1");
        bf.setBubbleAssetCacheUrl("TestCloudUrl");
        bf.setOwnerId(new UUID(0x0000000000000000L,
                               0x0000000000000000L));
        bf.setBubbleAddress("TestBubbleAddress");
        bf.setBubblePort(1);
        bf.setBubbleCenter(new float[] { 2f, 3f, 4f });
        bf.setBubbleRange(5f);
        bf.setBubblePerceptionRange(6f);
        bf.setBubbleRealtime(new Date(EPOCH2K + 7));

        return bf;
    }

    /**
     * Generate an Attach request message.
     *
     * @return a test Attach request.
     */
    public static AttachRequest generateAttachRequest() {
        BubbleFragment              bf = generateBubbleFragment();
        ProgramFragment             pf = generateProgramFragment(1);
        AttachRequest               ar = new AttachRequest();

        ar.setTargetBubbleId(new UUID(0xaaaaaaaabbbbccccL,
                                      0xddddeeeeeeeeeeeeL));
        ar.setSourceBubble(bf);
        ar.setSourceBubbleServer(pf);

        return ar;
    }

    /**
     * Generate an Attach response message.
     *
     * @return a test Attach response.
     */
    public static AttachResponse generateAttachResponse() {
        ResponseFragment            rf = generateResponseFragment();
        BubbleFragment              bf = generateBubbleFragment();
        ProgramFragment             pf = generateProgramFragment(1);
        AttachResponse              ar = new AttachResponse();

        ar.setResponseHeader(rf);
        ar.setTargetBubble(bf);
        ar.setTargetBubbleServer(pf);

        return ar;
    }

    /**
     * Generate an Detach request message.
     *
     * @return a test Detach request.
     */
    public static DetachRequest generateDetachRequest() {
        DetachRequest               dr = new DetachRequest();

        return dr;
    }

    /**
     * Generate an Detach response message.
     *
     * @return a test Detach response.
     */
    public static DetachResponse generateDetachResponse() {
        ResponseFragment            rf = generateResponseFragment();
        DetachResponse              dr = new DetachResponse();

        dr.setResponseHeader(rf);

        return dr;
    }

    /**
     * Generate an Handover request message.
     *
     * @return a test Handover request.
     */
    public static HandoverRequest generateHandoverRequest() {
        ObjectFragment              of = generateObjectFragment();
        HandoverRequest             hr = new HandoverRequest();

        hr.setSourceBubbleId(new UUID(0xaaaaaaaabbbbccccL,
                                      0xddddeeeeeeeeeeeeL));
        hr.setTargetBubbleId(new UUID(0xaaaaaaaabbbbccccL,
                                      0xddddeeeeeeeeeeeeL));
        of.setExtensionData(new byte[] { 49, 50, 51, 52, 53,
                                         54, 55, 56, 57,
                                         48, 49, 50, 51 });
        hr.setObject(of);

        return hr;
    }

    /**
     * Generate an Handover response message.
     *
     * @return a test Handover response.
     */
    public static HandoverResponse generateHandoverResponse() {
        ResponseFragment           rf = generateResponseFragment();
        HandoverResponse           hr = new HandoverResponse();

        hr.setResponseHeader(rf);

        return hr;
    }

    /**
     * Generate an List bubbles request message.
     *
     * @return a test List bubbles request.
     */
    public static ListBubblesRequest generateListBubblesRequest() {
        ListBubblesRequest          lbr = new ListBubblesRequest();

        lbr.setListType((byte) 0);

        return lbr;
    }

    /**
     * Generate an List bubbles response message.
     *
     * @return a test List response request.
     */
    public static ListBubblesResponse generateListBubblesResponse() {
        ListBubblesResponse         lbr = new ListBubblesResponse();
        Vector<BubbleFragment>      bubbles = lbr.getBubbles();
        BubbleFragment              bf;

        bf = generateBubbleFragment();
        bf.setBubbleName("TestBubble1");
        bubbles.add(bf);

        bf = generateBubbleFragment();
        bf.setBubbleName("TestBubble2");
        bubbles.add(bf);

        bf = generateBubbleFragment();
        bf.setBubbleName("TestBubble3");
        bubbles.add(bf);

        bf = generateBubbleFragment();
        bf.setBubbleName("TestBubble4");
        bubbles.add(bf);

        bf = generateBubbleFragment();
        bf.setBubbleName("TestBubble5");
        bubbles.add(bf);


        lbr.setBubbles(bubbles);

        return lbr;
    }

    /**
     * Generate an Perception event message.
     *
     * @return a test Perception event.
     */
    public static PerceptionEvent generatePerceptionEvent() {
        ObjectFragment              of = generateObjectFragment();
        PerceptionEvent             pe = new PerceptionEvent();

        of.setExtensionData(new byte[] {
                49, 50, 51, 52, 53, 54, 55, 56, 57,
            48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
            48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
            48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
            48, 49, 50, 51, 52, 53 });

        pe.setObject(of);

        return pe;
    }

    /**
     * Generate an Disappearance event message.
     *
     * @return a test Disappearance event.
     */
    public static DisappearanceEvent generateDisappearanceEvent() {
        DisappearanceEvent          de = new DisappearanceEvent();

        de.setObjectIndex(1);

        return de;
    }

    /**
     * Generate an Movement event message.
     *
     * @return a test Movement event.
     */
    public static MovementEvent generateMovementEvent() {
        MovementEvent               me = new MovementEvent();

        me.setObjectIndex(1);
        me.setLocation(new float[] { 1f, 2f, 3f });
        me.setOrientation(new float[] { 10f, 11f, 12f, 13f });

        return me;
    }

    /**
     * Generate an Action event message.
     *
     * @return a test Action event.
     */
    public static ActionEvent generateActionEvent() {
        ActionEvent                 ae = new ActionEvent();

        ae.setActionName("TestInteractionName");
        ae.setSourceObjectId(new UUID(0xaaaaaaaabbbbccccL,
                                      0xddddeeeeeeeeeeeeL));
        ae.setObservationRadius(100f);
        ae.setExtensionDialect("TEST");
        ae.setExtensionDialectMajorVersion((byte) 1);
        ae.setExtensionDialectMinorVersion((byte) 2);
        ae.setExtensionData(new byte[] {
                49, 50, 51, 52, 53, 54, 55, 56, 57,
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
            48, 49, 50, 51, 52, 53 });

        return ae;
    }

    /**
     * Generate a Handover event message.
     *
     * @return a test Handover event.
     */
    public static HandoverEvent generateHandoverEvent() {
        ObjectFragment              of = generateObjectFragment();
        HandoverEvent               he = new HandoverEvent();

        he.setSourceBubbleId(new UUID(0xaaaaaaaabbbbccccL,
                                      0xddddeeeeeeeeeeeeL));
        he.setTargetBubbleId(new UUID(0xaaaaaaaabbbbccccL,
                                      0xddddeeeeeeeeeeeeL));

        of.setExtensionData(new byte[] {
                49, 50, 51, 52, 53, 54, 55, 56, 57,
            48, 49, 50, 51 });

        he.setObject(of);

        return he;
    }

    /**
     * Generate a Synchronization begin event message.
     *
     * @return a test Synchronization begin event.
     */
    public static
    SynchronizationBeginEvent generateSynchronizationBeginEvent() {
        SynchronizationBeginEvent   sbe = new SynchronizationBeginEvent();

        sbe.setObjectCount(1);

        return sbe;
    }

    /**
     * Generate a Synchronization end event message.
     *
     * @return a test Synchronization end event.
     */
    public static
    SynchronizationEndEvent generateSynchronizationEndEvent() {
        SynchronizationEndEvent     see = new SynchronizationEndEvent();

        return see;
    }

}
