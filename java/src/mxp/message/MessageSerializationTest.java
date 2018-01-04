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

import mxp.serialization.SerializationInputStream;
import mxp.serialization.SerializationOutputStream;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;

import junit.framework.TestCase;

/**
 * Unit test to check for correct serialization of messages.
 */
public class MessageSerializationTest extends TestCase {
    /**
     * Check serialization of the Acknowledge message.
     *
     * @throws IOException on I/O errors
     */
    public void testAcknowledge() throws IOException {
        Acknowledge                ack   = MessageTest.generateAcknowledge();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = ack.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        Acknowledge                 aack = new Acknowledge();

        aack.deserialize(in, counter);

        assertEquals(ack, aack);
    }

    /**
     * Check serialization of the Keepalive message.
     *
     * @throws IOException on I/O errors
     */
    public void testKeepalive() throws IOException {
        Keepalive                  ka    = MessageTest.generateKeepalive();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = ka.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        Keepalive                   kka = new Keepalive();

        kka.deserialize(in, counter);

        assertEquals(ka, kka);
    }

    /**
     * Check serialization of the Throttle message.
     *
     * @throws IOException on I/O errors
     */
    public void testThrottle() throws IOException {
        Throttle                   throttle = MessageTest.generateThrottle();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = throttle.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        Throttle                    tthrottle = new Throttle();

        tthrottle.deserialize(in, counter);

        assertEquals(throttle, tthrottle);
    }

    /**
     * Check serialization of the Challenge request message.
     *
     * @throws IOException on I/O errors
     */
    public void testChallengeRequest() throws IOException {
        ChallengeRequest           cr = MessageTest.generateChallengeRequest();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = cr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        ChallengeRequest            ccr = new ChallengeRequest();

        ccr.deserialize(in, counter);

        assertEquals(cr, ccr);
    }

    /**
     * Check serialization of the Challenge response message.
     *
     * @throws IOException on I/O errors
     */
    public void testChallengeResponse() throws IOException {
        ChallengeResponse          cr = MessageTest.generateChallengeResponse();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = cr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        ChallengeResponse           ccr = new ChallengeResponse();

        ccr.deserialize(in, counter);

        assertEquals(cr, ccr);
    }

    /**
     * Check serialization of a message program fragment.
     *
     * @throws IOException on I/O errors
     */
    public void testProgramFragment() throws IOException {
        ProgramFragment            pf = MessageTest.generateProgramFragment();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = pf.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        ProgramFragment             ppf = new ProgramFragment();

        ppf.deserialize(in, counter);

        assertEquals(pf, ppf);
    }

    /**
     * Check serialization of a join request message.
     *
     * @throws IOException on I/O errors
     */
    public void testJoinRequest() throws IOException {
        JoinRequest                jr = MessageTest.generateJoinRequest();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = jr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        JoinRequest                 jjr = new JoinRequest();

        jjr.deserialize(in, counter);

        assertEquals(jr, jjr);
    }

    /**
     * Check serialization of a join response message.
     *
     * @throws IOException on I/O errors
     */
    public void testJoinResponse() throws IOException {
        JoinResponse               jr = MessageTest.generateJoinResponse();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = jr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        JoinResponse                jjr = new JoinResponse();

        jjr.deserialize(in, counter);

        assertEquals(jr, jjr);
    }

    /**
     * Check serialization of the Leave Request message.
     *
     * @throws IOException on I/O errors
     */
    public void testLeaveRequest() throws IOException {
        LeaveRequest               lr = MessageTest.generateLeaveRequest();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = lr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        LeaveRequest                llr = new LeaveRequest();

        llr.deserialize(in, counter);

        assertEquals(lr, llr);
    }

    /**
     * Check serialization of the Leave Response message.
     *
     * @throws IOException on I/O errors
     */
    public void testLeaveResponse() throws IOException {
        LeaveResponse              lr = MessageTest.generateLeaveResponse();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = lr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        LeaveResponse               llr = new LeaveResponse();

        llr.deserialize(in, counter);

        assertEquals(lr, llr);
    }

    /**
     * Check serialization of a message object fragment.
     *
     * @throws IOException on I/O errors
     */
    public void testObjectFragment() throws IOException {
        ObjectFragment              of = MessageTest.generateObjectFragment();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = of.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        ObjectFragment              oof = new ObjectFragment();

        oof.deserialize(in, counter);

        assertEquals(of, oof);
    }

    /**
     * Check serialization of an inject request message.
     *
     * @throws IOException on I/O errors
     */
    public void testInjectRequest() throws IOException {
        InjectRequest              ir = MessageTest.generateInjectRequest();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = ir.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        InjectRequest               iir = new InjectRequest();

        iir.deserialize(in, counter);

        assertEquals(ir, iir);
    }

    /**
     * Check serialization of the Inject Response message.
     *
     * @throws IOException on I/O errors
     */
    public void testInjectResponse() throws IOException {
        InjectResponse             ir = MessageTest.generateInjectResponse();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = ir.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        InjectResponse              iir = new InjectResponse();

        iir.deserialize(in, counter);

        assertEquals(ir, iir);
    }

    /**
     * Check serialization of a modify request message.
     *
     * @throws IOException on I/O errors
     */
    public void testModifyRequest() throws IOException {
        ModifyRequest              mr = MessageTest.generateModifyRequest();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = mr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        ModifyRequest               mmr = new ModifyRequest();

        mmr.deserialize(in, counter);

        assertEquals(mr, mmr);
    }

    /**
     * Check serialization of the Modify Response message.
     *
     * @throws IOException on I/O errors
     */
    public void testModifyResponse() throws IOException {
        ModifyResponse             mr = MessageTest.generateModifyResponse();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = mr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        ModifyResponse              mmr = new ModifyResponse();

        mmr.deserialize(in, counter);

        assertEquals(mr, mmr);
    }

    /**
     * Check serialization of an eject request message.
     *
     * @throws IOException on I/O errors
     */
    public void testEjectRequest() throws IOException {
        EjectRequest               er = MessageTest.generateEjectRequest();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = er.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        EjectRequest                eer = new EjectRequest();

        eer.deserialize(in, counter);

        assertEquals(er, eer);
    }

    /**
     * Check serialization of the Eject Response message.
     *
     * @throws IOException on I/O errors
     */
    public void testEjectResponse() throws IOException {
        EjectResponse              er = MessageTest.generateEjectResponse();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = er.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        EjectResponse               eer = new EjectResponse();

        eer.deserialize(in, counter);

        assertEquals(er, eer);
    }

    /**
     * Check serialization of a message interaction fragment.
     *
     * @throws IOException on I/O errors
     */
    public void testInteractionFragment() throws IOException {
        InteractionFragment        iaf =
                                      MessageTest.generateInteractionFragment();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = iaf.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        InteractionFragment         iiaf = new InteractionFragment();

        iiaf.deserialize(in, counter);

        assertEquals(iaf, iiaf);
    }

    /**
     * Check serialization of an interaction request message.
     *
     * @throws IOException on I/O errors
     */
    public void testInteractRequest() throws IOException {
        InteractRequest            ir = MessageTest.generateInteractRequest();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = ir.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        InteractRequest             iir = new InteractRequest();

        iir.deserialize(in, counter);

        assertEquals(ir, iir);
    }

    /**
     * Check serialization of an interaction response message.
     *
     * @throws IOException on I/O errors
     */
    public void testInteractResponse() throws IOException {
        InteractResponse           ir = MessageTest.generateInteractResponse();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = ir.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        InteractResponse            iir = new InteractResponse();

        iir.deserialize(in, counter);

        assertEquals(ir, iir);
    }

    /**
     * Check serialization of an examine request message.
     *
     * @throws IOException on I/O errors
     */
    public void testExamineRequest() throws IOException {
        ExamineRequest             er = MessageTest.generateExamineRequest();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = er.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        ExamineRequest              eer = new ExamineRequest();

        eer.deserialize(in, counter);

        assertEquals(er, eer);
    }

    /**
     * Check serialization of an examine response message.
     *
     * @throws IOException on I/O errors
     */
    public void testExamineResponse() throws IOException {
        ExamineResponse            er = MessageTest.generateExamineResponse();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = er.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        ExamineResponse             eer = new ExamineResponse();

        eer.deserialize(in, counter);

        assertEquals(er, eer);
    }

    /**
     * Check serialization of a message bubble fragment.
     *
     * @throws IOException on I/O errors
     */
    public void testBubbleFragment() throws IOException {
        BubbleFragment             bf = MessageTest.generateBubbleFragment();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = bf.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        BubbleFragment              bbf = new BubbleFragment();

        bbf.deserialize(in, counter);

        assertEquals(bf, bbf);
    }

    /**
     * Check serialization of a bubble attach request.
     *
     * @throws IOException on I/O errors
     */
    public void testAttachRequest() throws IOException {
        AttachRequest              ar = MessageTest.generateAttachRequest();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = ar.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        AttachRequest               aar = new AttachRequest();

        aar.deserialize(in, counter);

        assertEquals(ar, aar);
    }

    /**
     * Check serialization of a bubble attach response.
     *
     * @throws IOException on I/O errors
     */
    public void testAttachResponse() throws IOException {
        AttachResponse             ar = MessageTest.generateAttachResponse();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = ar.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        AttachResponse              aar = new AttachResponse();

        aar.deserialize(in, counter);

        assertEquals(ar, aar);
    }

    /**
     * Check serialization of a bubble detach request.
     *
     * @throws IOException on I/O errors
     */
    public void testDetachRequest() throws IOException {
        DetachRequest              dr = MessageTest.generateDetachRequest();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = dr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        DetachRequest               ddr = new DetachRequest();

        ddr.deserialize(in, counter);

        assertEquals(dr, ddr);
    }

    /**
     * Check serialization of a bubble detach response.
     *
     * @throws IOException on I/O errors
     */
    public void testDetachResponse() throws IOException {
        DetachResponse             dr = MessageTest.generateDetachResponse();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = dr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        DetachResponse              ddr = new DetachResponse();

        ddr.deserialize(in, counter);

        assertEquals(dr, ddr);
    }

    /**
     * Check serialization of an object handover request from bubble to bubble.
     *
     * @throws IOException on I/O errors
     */
    public void testHandoverRequest() throws IOException {
        HandoverRequest            hr = MessageTest.generateHandoverRequest();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = hr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        HandoverRequest             hhr = new HandoverRequest();

        hhr.deserialize(in, counter);

        assertEquals(hr, hhr);
    }

    /**
     * Check serialization of a bubble to bubble object handover response.
     *
     * @throws IOException on I/O errors
     */
    public void testHandoverResponse() throws IOException {
        HandoverResponse           hr = MessageTest.generateHandoverResponse();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = hr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        HandoverResponse            hhr = new HandoverResponse();

        hhr.deserialize(in, counter);

        assertEquals(hr, hhr);
    }

    /**
     * Check serialization of a bubble list request message.
     *
     * @throws IOException on I/O errors
     */
    public void testListBubbleRequest() throws IOException {
        ListBubblesRequest         lbr =
                                       MessageTest.generateListBubblesRequest();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = lbr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        ListBubblesRequest          llbr = new ListBubblesRequest();

        llbr.deserialize(in, counter);

        assertEquals(lbr, llbr);
    }

    /**
     * Check serialization of a bubble list response message.
     *
     * @throws IOException on I/O errors
     */
    public void testListBubbleResponse() throws IOException {
        ListBubblesResponse        lbr =
                                    MessageTest.generateListBubblesResponse();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = lbr.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        ListBubblesResponse         llbr = new ListBubblesResponse();

        llbr.deserialize(in, counter);

        assertEquals(lbr, llbr);
    }

    /**
     * Check serialization of a perception event message.
     *
     * @throws IOException on I/O errors
     */
    public void testPerceptionEvent() throws IOException {
        PerceptionEvent            pe = MessageTest.generatePerceptionEvent();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = pe.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        PerceptionEvent             ppe = new PerceptionEvent();

        ppe.deserialize(in, counter);

        assertEquals(pe, ppe);
    }

    /**
     * Check serialization of a disappearance event message.
     *
     * @throws IOException on I/O errors
     */
    public void testDisappearanceEvent() throws IOException {
        DisappearanceEvent         de =
                                    MessageTest.generateDisappearanceEvent();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = de.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        DisappearanceEvent          dde = new DisappearanceEvent();

        dde.deserialize(in, counter);

        assertEquals(de, dde);
    }

    /**
     * Check serialization of a movement event message.
     *
     * @throws IOException on I/O errors
     */
    public void testMovementEvent() throws IOException {
        MovementEvent              me = MessageTest.generateMovementEvent();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = me.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        MovementEvent               mme = new MovementEvent();

        mme.deserialize(in, counter);

        assertEquals(me, mme);
    }

    /**
     * Check serialization of an action event message.
     *
     * @throws IOException on I/O errors
     */
    public void testActionEvent() throws IOException {
        ActionEvent                ae = MessageTest.generateActionEvent();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = ae.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        ActionEvent                 aae = new ActionEvent();

        aae.deserialize(in, counter);

        assertEquals(ae, aae);
    }

    /**
     * Check serialization of an object handover event from bubble to bubble.
     * This is the same message as the handover request.
     *
     * @throws IOException on I/O errors
     */
    public void testHandoverEvent() throws IOException {
        HandoverEvent              he = MessageTest.generateHandoverEvent();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = he.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        HandoverEvent               hhe = new HandoverEvent();

        hhe.deserialize(in, counter);

        assertEquals(he, hhe);
    }

    /**
     * Check serialization of a synchronization begin event message.
     *
     * @throws IOException on I/O errors
     */
    public void testSynchronizationBeginEvent() throws IOException {
        SynchronizationBeginEvent  sbe =
                                MessageTest.generateSynchronizationBeginEvent();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = sbe.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        SynchronizationBeginEvent   ssbe = new SynchronizationBeginEvent();

        ssbe.deserialize(in, counter);

        assertEquals(sbe, ssbe);
    }

    /**
     * Check serialization of a synchronization end event message.
     *
     * @throws IOException on I/O errors
     */
    public void testSynchronizationEndEvent() throws IOException {
        SynchronizationEndEvent    see =
                                MessageTest.generateSynchronizationEndEvent();
        ByteArrayOutputStream      baOut = new ByteArrayOutputStream();
        SerializationOutputStream  out   = new SerializationOutputStream(baOut);
        int                        counter = 0;

        counter = see.serialize(out);
        out.flush();

        ByteArrayInputStream        baIn = new ByteArrayInputStream(
                                                         baOut.toByteArray());
        SerializationInputStream    in = new SerializationInputStream(baIn);
        SynchronizationEndEvent     ssee = new SynchronizationEndEvent();

        ssee.deserialize(in, counter);

        assertEquals(see, ssee);
    }

}
