using System;
using System.Collections.Generic;

using System.Text;
using MXP.Messages;
using MXP.Util;
using System.Diagnostics;

namespace MXP
{

    /// <summary>
    /// FrameEncoder encodes and decodes frame content to and from packet bytes.
    /// </summary>
    public class FrameEncoder
    {

		/// <summary>
		/// Encodes one message frame to packet bytes. 
		/// </summary>
		/// <param name="session">
		/// A session the message belongs to. <see cref="Session"/>
		/// </param>
		/// <param name="packetBytes">
		/// A byte array containing packet bytes. <see cref="System.Byte"/>
		/// </param>
		/// <param name="startIndex">
		/// The start index in packet byte array. <see cref="System.Int32"/>
		/// </param>
		/// <param name="frameQuaranteed">
		/// Output parameter which is true if frame is quaranteed. <see cref="System.Boolean"/>
		/// </param>
		/// <returns>
		/// Start index of next frame in packet byte array. <see cref="System.Int32"/>
		/// </returns>
        public static int EncodeFrame(Session session, byte[] packetBytes, int startIndex, ref bool frameQuaranteed)
        {
            MessageEntry messageEntry = session.GetPartialOutboundMessage();
            if (messageEntry == null)
            {
                return startIndex;
            }

            Message message = messageEntry.Message;

            frameQuaranteed = message.Quaranteed;

            int currentIndex = startIndex;
            UInt16 frameCount= message.FrameCount;
            byte frameSize = message.FrameDataSize(messageEntry.FramesCompleted);

            currentIndex = EncodeUtil.Encode(ref message.TypeCode, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref message.MessageId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref frameCount, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref messageEntry.FramesCompleted, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Encode(ref frameSize, packetBytes, currentIndex);

            try
            {
                currentIndex = message.EncodeFrameData(messageEntry.FramesCompleted, packetBytes, currentIndex);
                messageEntry.FramesCompleted++;
            }
            catch (Exception e)
            {
                LogUtil.Error("Error sending message: "+e.ToString());
                session.CompleteOutboundMessage(messageEntry);
            }

            if (messageEntry.FramesCompleted == message.FrameCount)
            {
                session.CompleteOutboundMessage(messageEntry);
            }

            return currentIndex;
        }

		/// <summary>
		/// Decodes one frame from packet bytes. 
		/// </summary>
		/// <param name="session">
		/// A session the message belongs to. <see cref="Session"/>
		/// </param>
		/// <param name="packetBytes">
		/// A byte array containing packet bytes. <see cref="System.Byte"/>
		/// </param>
		/// <param name="startIndex">
		/// The start index in packet byte array. <see cref="System.Int32"/>
		/// </param>
		/// <param name="frameQuaranteed">
		/// Output parameter which is true if frame is quaranteed. <see cref="System.Boolean"/>
		/// </param>
		/// <returns>
		/// Start index of next frame in packet byte array. <see cref="System.Int32"/>
		/// </returns>
        public static int DecodeFrame(Session session, byte[] packetBytes, int startIndex, ref bool frameQuaranteed)
        {
            byte messageType = 0;
            uint messageId = 0;
            int currentIndex = startIndex;
            UInt16 frameCount = 0;
            byte frameSize = 0;
            ushort frameIndex = 0;

            currentIndex = EncodeUtil.Decode(ref messageType, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref messageId, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref frameCount, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref frameIndex, packetBytes, currentIndex);
            currentIndex = EncodeUtil.Decode(ref frameSize, packetBytes, currentIndex);

            MessageEntry messageEntry=session.GetPartialInboundMessage(messageId, messageType, frameCount, frameIndex);
            
            if (messageEntry != null)
            {
                Message message = messageEntry.Message;
                frameQuaranteed = message.Quaranteed;
                currentIndex = message.DecodeFrameData(frameIndex, packetBytes, currentIndex, frameSize);
                messageEntry.FramesCompleted++;

                if (messageEntry.FramesCompleted == message.FrameCount)
                {
                    session.CompleteInboundMessage(message);
                }
            }
            else
            {
                // TODO Should these frames be stored for applying after the initial packet has arrived.
                // Should fix problems in situation where packet containing the initial packet is dropped or 
                // if the packet with later frame just happens to arrive first.
                LogUtil.Warn("Ignored frame which arrived before message initialization frame. Possible reconnect.");
            }

            return currentIndex;
        }

    }
}
