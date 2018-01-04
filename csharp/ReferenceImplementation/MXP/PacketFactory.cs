using System;
using System.Collections.Generic;

using System.Text;

namespace MXP
{

    /// <summary>
    /// PacketFactory is used to reserve and release packets from recycling pool.
    /// Recycling helps to lower the carbage collection load on real time simulation applications.
    /// </summary>
    public class PacketFactory
    {
        public static PacketFactory Current
        {
            get
            {
                lock (typeof(PacketFactory))
                {
                    if (theCurrent == null)
                    {
                        theCurrent = new PacketFactory();
                    }
                    return theCurrent;
                }
            }
        }

        public static PacketFactory theCurrent = null;
        private int packetsReserved = 0;
        private int packetsReleased = 0;

        public int PacketsReserved
        {
            get
            {
                return packetsReserved;
            }
        }

        public int PacketsReleased
        {
            get
            {
                return packetsReleased;
            }
        }

        public Queue<Packet> packets = new Queue<Packet>();

        public PacketFactory()
        {

        }

        public Packet ReservePacket()
        {
            lock (packets)
            {
                packetsReserved++;
                if (packets.Count > 0)
                {
                    return packets.Dequeue();
                }
                else
                {
                    return new Packet();
                }
            }
        }

        public void ReleasePacket(Packet packet)
        {
            lock (packets)
            {
                packetsReleased++;
                packet.Clear();
                packets.Enqueue(packet);
            }
        }

        public override string ToString()
        {
            return "PacketFactory {pool="+packets.Count+",reserved="+packetsReserved+",released="+packetsReleased+"}";
        }

    }
}
