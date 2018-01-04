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

import java.io.IOException;

/**
 * Base class for MXP messages.
 */
public abstract class Message {
    /**
     * A enumeration of possible message types.
     */
    public enum Type {
        /** The acknowledge message type. */
        ACKNOWLEDGE(1),
        /** The keepalive message type. */
        KEEPALIVE(2),
        /** The throttle message type. */
        THROTTLE(3),
        /** The challenge request message type. */
        CHALLENGE_REQUEST(4),
        /** The challenge response message type. */
        CHALLENGE_RESPONSE(5),
        /** The join request message type. */
        JOIN_REQUEST(10),
        /** The join response message type. */
        JOIN_RESPONSE(11),
        /** The leave request message type. */
        LEAVE_REQUEST(12),
        /** The leave response message type. */
        LEAVE_RESPONSE(13),
        /** The inject request message type. */
        INJECT_REQUEST(14),
        /** The inject response message type. */
        INJECT_RESPONSE(15),
        /** The modify request message type. */
        MODIFY_REQUEST(16),
        /** The modify response message type. */
        MODIFY_RESPONSE(17),
        /** The eject request message type. */
        EJECT_REQUEST(18),
        /** The eject response message type. */
        EJECT_RESPONSE(19),
        /** The interact request message type. */
        INTERACT_REQUEST(20),
        /** The interact response message type. */
        INTERACT_RESPONSE(21),
        /** The examine request message type. */
        EXAMINE_REQUEST(22),
        /** The examine response message type. */
        EXAMINE_RESPONSE(23),
        /** The list bubbles request message type. */
        LIST_BUBBLES_REQUEST(25),
        /** The list bubbles response message type. */
        LIST_BUBBLES_RESPONSE(26),
        /** The attach request message type. */
        ATTACH_REQUEST(30),
        /** The attach response message type. */
        ATTACH_RESPONSE(31),
        /** The detach request message type. */
        DETACH_REQUEST(32),
        /** The detach response message type. */
        DETACH_RESPONSE(33),
        /** The handover request message type. */
        HANDOVER_REQUEST(34),
        /** The handover response message type. */
        HANDOVER_RESPONSE(35),
        /** The perception event message type. */
        PERCEPTION_EVENT(40),
        /** The movement event message type. */
        MOVEMENT_EVENT(41),
        /** The disappearance event message type. */
        DISAPPEARANCE_EVENT(45),
        /** The handover event message type. */
        HANDOVER_EVENT(53),
        /** The action event message type. */
        ACTION_EVENT(60),
        /** The synchronization begin event message type. */
        SYNCHRONIZATION_BEGIN_EVENT(70),
        /** The synchronization end event message type. */
        SYNCHRONIZATION_END_EVENT(71);

        /**
         * The type id, as specified by the MXP protocol.
         */
        private int type;

        /**
         * Constructor.
         *
         * @param t the type id.
         */
        private Type(int t) {
            type = t;
        }

        /**
         * Return the type id.
         *
         * @return the type id denoted by this enum.
         */
        public int getType() {
            return type;
        }

        /**
         * Return a Type object based on a numeric value.
         *
         * @param typeId the numberic value of the type.
         * @return the corresponding type object.
         * @throws IllegalArgumentException if no type of the specified type
         *         exists.
         */
        public static Type forTypeId(int typeId)
                                            throws IllegalArgumentException {
            for (Type t : Type.values()) {
                if (t.type == typeId) {
                    return t;
                }
            }

            throw new IllegalArgumentException();
        }
    }

    /**
     * The type of the message, as specified in the MXP protocol.
     */
    private Type type;

    /**
     * Constructor.
     *
     * @param messageType the type of the message.
     */
    protected Message(Type messageType) {
        type = messageType;
    }

    /**
     * Return the type of this message.
     *
     * @return the type of this message.
     */
    public Type getType() {
        return type;
    }

    /**
     * Return the size of the message when serialized.
     *
     * @return the size of the message when serialized, or -1 if this cannot
     *         be determined at the moment.
     */
    public abstract int size();

    /**
     * Serialize the message.
     *
     * @param out the output stream to serialize into.
     * @return the number of bytes written into the serialization stream.
     * @throws IOException on I/O errors.
     */
    public abstract int
    serialize(SerializationOutputStream out) throws IOException;

    /**
     * Build a message by reading its serialized form from an input stream.
     *
     * @param in the input stream to read from.
     * @param length the maximum number of bytes to read from in.
     * @return the number of bytes read from the serialization stream.
     * @throws IOException on I/O errors.
     */
    public abstract int
    deserialize(SerializationInputStream in, int length) throws IOException;

    @Override
    public int hashCode() {
        return type.getType();
    }

    @Override
    public boolean equals(Object other) {
        if (other == null || !(other instanceof Message)) {
            return false;
        }

        return ((Message) other).type == type;
    }

    @Override
    public String toString() {
        return "Message [type=" + type + "]";
    }

    /**
     * Return a specific subclass of Message, corresponding to the type
     * specified.
     *
     * @param type the type of the message to return.
     * @return an empty message of the specified type.
     */
    public static Message forType(Type type) {
        switch (type) {
            case ACKNOWLEDGE:
                return new Acknowledge();
            case KEEPALIVE:
                return new Keepalive();
            case THROTTLE:
                return new Throttle();
            case CHALLENGE_REQUEST:
                return new ChallengeRequest();
            case CHALLENGE_RESPONSE:
                return new ChallengeResponse();
            case JOIN_REQUEST:
                return new JoinRequest();
            case JOIN_RESPONSE:
                return new JoinResponse();
            case LEAVE_REQUEST:
                return new LeaveRequest();
            case LEAVE_RESPONSE:
                return new LeaveResponse();
            case INJECT_REQUEST:
                return new InjectRequest();
            case INJECT_RESPONSE:
                return new InjectResponse();
            case MODIFY_REQUEST:
                return new ModifyRequest();
            case MODIFY_RESPONSE:
                return new ModifyResponse();
            case EJECT_REQUEST:
                return new EjectRequest();
            case EJECT_RESPONSE:
                return new EjectResponse();
            case INTERACT_REQUEST:
                return new InteractRequest();
            case INTERACT_RESPONSE:
                return new InteractResponse();
            case EXAMINE_REQUEST:
                return new ExamineRequest();
            case EXAMINE_RESPONSE:
                return new ExamineResponse();
            case LIST_BUBBLES_REQUEST:
                return new ListBubblesRequest();
            case LIST_BUBBLES_RESPONSE:
                return new ListBubblesResponse();
            case ATTACH_REQUEST:
                return new AttachRequest();
            case ATTACH_RESPONSE:
                return new AttachResponse();
            case DETACH_REQUEST:
                return new DetachRequest();
            case DETACH_RESPONSE:
                return new DetachResponse();
            case HANDOVER_REQUEST:
                return new HandoverRequest();
            case HANDOVER_RESPONSE:
                return new HandoverResponse();
            case PERCEPTION_EVENT:
                return new PerceptionEvent();
            case MOVEMENT_EVENT:
                return new MovementEvent();
            case DISAPPEARANCE_EVENT:
                return new DisappearanceEvent();
            case HANDOVER_EVENT:
                return new HandoverEvent();
            case ACTION_EVENT:
                return new ActionEvent();
            case SYNCHRONIZATION_BEGIN_EVENT:
                return new SynchronizationBeginEvent();
            case SYNCHRONIZATION_END_EVENT:
                return new SynchronizationEndEvent();
            default:
                throw new IllegalArgumentException();
        }
    }
}
