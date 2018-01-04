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
package mxp.message;

import java.util.Date;
import java.util.UUID;
import java.util.Vector;

import junit.framework.TestCase;

/**
 * Generic helper functions to test messages.
 */
public class MessageTest extends TestCase {
    /**
     * Create a bogus test to make jUnit happy about this being a test case.
     */
    public void testBogus() {
    }

    /**
     * Generate a test Acknowledge message.
     *
     * @return an acknowledge message.
     */
    public static Acknowledge generateAcknowledge() {
        Acknowledge                ack   = new Acknowledge();

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

        throttle.setMaxTransferRate(220);

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
     * @return a Program fragment
     */
    public static ProgramFragment generateProgramFragment() {
        ProgramFragment            pf = new ProgramFragment();

        pf.setProgramName("foo bar program");
        pf.setProgramMajorVersion((byte) 0);
        pf.setProgramMinorVersion((byte) 1);
        pf.setProtocolMajorVersion((byte) 2);
        pf.setProtocolMinorVersion((byte) 3);
        pf.setProtocolSourceRevision(4);

        return pf;
    }

    /**
     * Generate a test Join request message.
     *
     * @return a Join request message
     */
    public static JoinRequest generateJoinRequest() {
        ProgramFragment            pf = generateProgramFragment();

        JoinRequest                jr = new JoinRequest();

        jr.setBubbleId(UUID.randomUUID());
        jr.setAvatarId(UUID.randomUUID());
        jr.setBubbleName("bubble name");
        jr.setLocationName("location name");
        jr.setParticipantId("participant id");
        jr.setParticipantSecret("participant pw");
        jr.setParticipantRealtime(new Date());
        jr.setIdentityProviderUrl("http://my.open.id/provider/");
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
        rf.setRequestMessageId(4332);
        rf.setFailureCode((byte) 0x00);

        return rf;
    }

    /**
     * Generate a test Join response message.
     *
     * @return a Join response message
     */
    public static JoinResponse generateJoinResponse() {
        ResponseFragment           rf = generateResponseFragment();
        ProgramFragment            pf = generateProgramFragment();

        JoinResponse               jr = new JoinResponse();

        jr.setResponseHeader(rf);
        jr.setBubbleId(UUID.randomUUID());
        jr.setParticipantId(UUID.randomUUID());
        jr.setAvatarId(UUID.randomUUID());
        jr.setBubbleName("bubble name");
        jr.setBubbleAssetCacheUrl("http://get.assets.from.here/");
        jr.setBubbleRange(220.0f);
        jr.setBubblePerceptionRange(100.0f);
        jr.setBubbleRealtime(new Date());
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

        of.setObjectId(UUID.randomUUID());
        of.setObjectIndex(123);
        of.setTypeId(UUID.randomUUID());
        of.setParentObjectId(UUID.randomUUID());
        of.setObjectName("object name");
        of.setTypeName("type name");
        of.setOwnerId(UUID.randomUUID());
        of.setLocation(new float[] { 0.1f, 0.2f, 0.3f });
        of.setVelocity(new float[] { 0.1f, 0.2f, 0.3f });
        of.setAcceleration(new float[] { 0.1f, 0.2f, 0.3f });
        of.setOrientation(new float[] { 0.1f, 0.2f, 0.3f, 0.4f });
        of.setAngularVelocity(new float[] { 0.1f, 0.2f, 0.3f, 0.4f });
        of.setAngularAcceleration(new float[] { 0.1f, 0.2f, 0.3f, 0.4f });
        of.setBoundingSphereRadius(23.4f);
        of.setMass(100.f);
        of.setExtensionDialect("ize");
        of.setExtensionDialectMajorVersion((byte) 1);
        of.setExtensionDialectMinorVersion((byte) 0);
        of.setExtensionData(new byte[] { 0x01, 0x02 });

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
        ObjectFragment              of = generateObjectFragment();
        ModifyRequest              mr = new ModifyRequest();

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

        er.setObjectId(UUID.randomUUID());

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

        iaf.setInteractionName("handshake");
        iaf.setSourceParticipantId(UUID.randomUUID());
        iaf.setSourceObjectId(UUID.randomUUID());
        iaf.setTargetParticipantId(UUID.randomUUID());
        iaf.setTargetObjectId(UUID.randomUUID());
        iaf.setExtensionDialect("ize");
        iaf.setExtensionDialectMajorVersion((byte) 1);
        iaf.setExtensionDialectMinorVersion((byte) 0);
        iaf.setExtensionData(new byte[] { 0x01, 0x02 });

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

        er.setObjectId(UUID.randomUUID());
        er.setObjectIndex(1234);

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

        bf.setBubbleId(UUID.randomUUID());
        bf.setBubbleName("bubble name");
        bf.setBubbleAssetCacheUrl("http://bubble.asset/cache");
        bf.setOwnerId(UUID.randomUUID());
        bf.setBubbleAddress("bubble.address");
        bf.setBubblePort(12345);
        bf.setBubbleCenter(new float[] { 12.3f, 23.4f, 34.5f });
        bf.setBubbleRange(4567f);
        bf.setBubblePerceptionRange(1234f);
        bf.setBubbleRealtime(new Date());

        return bf;
    }

    /**
     * Generate an Attach request message.
     *
     * @return a test Attach request.
     */
    public static AttachRequest generateAttachRequest() {
        BubbleFragment              bf = generateBubbleFragment();
        ProgramFragment             pf = generateProgramFragment();
        AttachRequest               ar = new AttachRequest();

        ar.setTargetBubbleId(UUID.randomUUID());
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
        ProgramFragment             pf = generateProgramFragment();
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

        hr.setSourceBubbleId(UUID.randomUUID());
        hr.setTargetBubbleId(UUID.randomUUID());
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

        lbr.setListType((byte) 1);

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

        bubbles.add(generateBubbleFragment());
        bubbles.add(generateBubbleFragment());

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

        de.setObjectIndex(12345);

        return de;
    }

    /**
     * Generate an Movement event message.
     *
     * @return a test Movement event.
     */
    public static MovementEvent generateMovementEvent() {
        MovementEvent               me = new MovementEvent();

        me.setObjectIndex(12345);
        me.setLocation(new float[] { 12.3f, 34.5f, 45.6f });
        me.setOrientation(new float[] { 12.3f, 34.5f, 45.6f, 56.7f });

        return me;
    }

    /**
     * Generate an Action event message.
     *
     * @return a test Action event.
     */
    public static ActionEvent generateActionEvent() {
        ActionEvent                 ae = new ActionEvent();

        ae.setActionName("action!");
        ae.setSourceObjectId(UUID.randomUUID());
        ae.setObservationRadius(123.4f);
        ae.setExtensionDialect("ize");
        ae.setExtensionDialectMajorVersion((byte) 1);
        ae.setExtensionDialectMinorVersion((byte) 0);
        ae.setExtensionData(new byte[] { 0x01, 0x02 });

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

        he.setSourceBubbleId(UUID.randomUUID());
        he.setTargetBubbleId(UUID.randomUUID());
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

        sbe.setObjectCount(1234);

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
