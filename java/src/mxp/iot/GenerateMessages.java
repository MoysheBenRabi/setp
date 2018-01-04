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

import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.List;
import java.util.Vector;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;

import mxp.message.Message;
import mxp.packet.Packet;
import mxp.packet.Packetizer;
import mxp.serialization.SerializationOutputStream;

/**
 * Class to generate reference message serializations, and save them
 * as a single zip file. This can be used to upload to the MXP
 * inter-operability service to verify that the implementation conforms
 * to the reference implementation.
 */
public final class GenerateMessages {

    /**
     * The timestamp that is used for all packets for testing.
     */
    private static final Date TIMESTAMP =
                    new GregorianCalendar(2009, 10, 5, 15, 33, 25).getTime();

    /**
     * Inaccessible constructor.
     */
    private GenerateMessages() {
    }

    /**
     * Add a message into the zip file, as a serialized packet.
     *
     * @param name the name of the serialized file to put into the zip file.
     * @param zos the zip file to create the new entry into
     * @param message the message to serialize
     * @param messageId the id of the message that is created.
     * @param guaranteed if the generated packets should be guaranteed packets.
     * @throws IOException on I/O errors
     */
    private static void addMessage(String           name,
                                   ZipOutputStream  zos,
                                   Message          message,
                                   int              messageId,
                                   boolean          guaranteed)
                                                        throws IOException {

        ZipEntry            entry    = new ZipEntry(name);
        List<Message>       messages = new Vector<Message>();
        List<Packet>        packets  = new Vector<Packet>();

        zos.putNextEntry(entry);
        messages.add(message);

        Packetizer.messagesToPackets(messages,
                                     messageId, // the base message id
                                     packets,
                                     1, // the session id
                                     1, // the base packet id
                                     TIMESTAMP,
                                     guaranteed ? (byte) 1 : (byte) 0);

        assert (packets.size() == 1);

        SerializationOutputStream sos = new SerializationOutputStream(zos);
        packets.get(0).serialize(sos);
        sos.flush();
        zos.closeEntry();
    }

    /**
     * Generate all the messages and put them into the appropriate zip file.
     *
     * @param os the output stream to generate the zip file into
     * @throws IOException on I/O errors.
     */
    static void generateMessages(OutputStream os) throws IOException {

        ZipOutputStream     zos = new ZipOutputStream(os);

        addMessage("messages/acknowledge.dat",
                zos,
                ReferenceMessage.generateAcknowledge(),
                1, false);
        addMessage("messages/attachrequest.dat",
                zos,
                ReferenceMessage.generateAttachRequest(),
                1, true);
        addMessage("messages/attachresponse.dat",
                zos,
                ReferenceMessage.generateAttachResponse(),
                1, true);
        addMessage("messages/challengerequest.dat",
                zos,
                ReferenceMessage.generateChallengeRequest(),
                1, true);
        addMessage("messages/challengeresponse.dat",
                zos,
                ReferenceMessage.generateChallengeResponse(),
                1, true);
        addMessage("messages/detachrequest.dat",
                zos,
                ReferenceMessage.generateDetachRequest(),
                1, true);
        addMessage("messages/detachresponse.dat",
                zos,
                ReferenceMessage.generateDetachResponse(),
                1, false);
        addMessage("messages/joinrequest.dat",
                zos,
                ReferenceMessage.generateJoinRequest(),
                1, true);
        addMessage("messages/joinresponse.dat",
                zos,
                ReferenceMessage.generateJoinResponse(),
                1, true);
        addMessage("messages/keepalive.dat",
                zos,
                ReferenceMessage.generateKeepalive(),
                1, true);
        addMessage("messages/leaverequest.dat",
                zos,
                ReferenceMessage.generateLeaveRequest(),
                1, true);
        addMessage("messages/leaveresponse.dat",
                zos,
                ReferenceMessage.generateLeaveResponse(),
                1, false);
        addMessage("messages/listbubblesrequest.dat",
                zos,
                ReferenceMessage.generateListBubblesRequest(),
                1, true);
        addMessage("messages/listbubblesresponse.dat",
                zos,
                ReferenceMessage.generateListBubblesResponse(),
                1, true);
        addMessage("messages/throttle.dat",
                zos,
                ReferenceMessage.generateThrottle(),
                1, true);
        addMessage("messages/ejectrequest.dat",
                zos,
                ReferenceMessage.generateEjectRequest(),
                1, true);
        addMessage("messages/ejectresponse.dat",
                zos,
                ReferenceMessage.generateEjectResponse(),
                1, true);
        addMessage("messages/examinerequest.dat",
                zos,
                ReferenceMessage.generateExamineRequest(),
                1, true);
        addMessage("messages/examineresponse.dat",
                zos,
                ReferenceMessage.generateExamineResponse(),
                1, true);
        addMessage("messages/handoverrequest.dat",
                zos,
                ReferenceMessage.generateHandoverRequest(),
                1, true);
        addMessage("messages/handoverresponse.dat",
                zos,
                ReferenceMessage.generateHandoverResponse(),
                1, true);
        addMessage("messages/injectrequest.dat",
                zos,
                ReferenceMessage.generateInjectRequest(),
                1, true);
        addMessage("messages/injectresponse.dat",
                zos,
                ReferenceMessage.generateInjectResponse(),
                1, true);
        addMessage("messages/interactrequest.dat",
                zos,
                ReferenceMessage.generateInteractRequest(),
                1, true);
        addMessage("messages/interactresponse.dat",
                zos,
                ReferenceMessage.generateInteractResponse(),
                1, true);
        addMessage("messages/modifyrequest.dat",
                zos,
                ReferenceMessage.generateModifyRequest(),
                1, true);
        addMessage("messages/modifyresponse.dat",
                zos,
                ReferenceMessage.generateModifyResponse(),
                1, true);
        addMessage("messages/actionevent.dat",
                zos,
                ReferenceMessage.generateActionEvent(),
                1, true);
        addMessage("messages/disappearanceevent.dat",
                zos,
                ReferenceMessage.generateDisappearanceEvent(),
                1, true);
        addMessage("messages/handoverevent.dat",
                zos,
                ReferenceMessage.generateHandoverEvent(),
                1, true);
        addMessage("messages/movementevent.dat",
                zos,
                ReferenceMessage.generateMovementEvent(),
                1, false);
        addMessage("messages/perceptionevent.dat",
                zos,
                ReferenceMessage.generatePerceptionEvent(),
                1, true);
        addMessage("messages/synchronizationbeginevent.dat",
                zos,
                ReferenceMessage.generateSynchronizationBeginEvent(),
                1, true);
        addMessage("messages/synchronizationendevent.dat",
                zos,
                ReferenceMessage.generateSynchronizationEndEvent(),
                1, true);

        zos.finish();
    }

    /**
     * Program entry point. Specify a filename to save the generated
     * messages into.
     *
     * @param args command line arguments - a single filename argument is
     *        expected.
     * @throws IOException on I/O errors.
     */
    public static void main(String[] args) throws IOException {
        if (args.length == 0) {
            System.out.println("Please specify a filename to save into.");

            System.exit(-1);
        }

        FileOutputStream    fos = new FileOutputStream(args[0]);

        generateMessages(fos);

        fos.close();
    }

}
