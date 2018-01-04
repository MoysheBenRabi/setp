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
package mxp.net;

import mxp.packet.Packet;
import mxp.serialization.SerializationInputStream;
import mxp.serialization.SerializationOutputStream;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.SocketAddress;
import java.net.SocketException;

/**
 * An MXP-based network connection, which is able to send and receive
 * MXP packets.
 *
 * This class either binds to a specific interface / port, which is a typical
 * server-side usage, or client usage behind a firewall where only a number of
 * UDP ports are let through, or binds to any port on all local interfaces,
 * which is a generic usage for a client.
 *
 * This class makes sure that packets sent have globally unique ids by
 * maintaining a globally shared packet id counter.
 *
 * As MXP is based on UDP, packets are not guaranteed to reach their
 * destination.
 */
public class Connection {
    /**
     * Inner class waiting for incoming packets.
     */
    private class PacketReader implements Runnable {
        /**
         * Flag to signal if we're still supposed to be running.
         */
        private boolean shouldRun = true;

        /**
         * The thread main method - blocks on the UDP socket, and
         * fires if there's something received.
         */
        public void run() {
            while (shouldRun) {
                try {
                    byte []         buf = new byte[1500];
                    DatagramPacket  p = new DatagramPacket(buf, buf.length);

                    socket.receive(p);

                    notifyPacketHandler(p.getData(),
                                        p.getLength(),
                                        p.getSocketAddress());
                } catch (IOException e) {
                    // just don't care
                }
            }
        }
    }

    /**
     * The timeout used when blocking for incoming packets, in milliseconds.
     */
    private static final int LISTEN_TIMEOUT = 100;

    /**
     * The UDP socket this server connection uses.
     */
    private DatagramSocket socket;

    /**
     * The background object listening for incoming packets.
     */
    private PacketReader packetReader;

    /**
     * The background thread running the packetReader.
     */
    private Thread packetReaderThread;

    /**
     * The object that is going to handle the newly received packets.
     */
    private PacketHandler packetHandler;

    /**
     * Constructs a connection by binding to a specific port and
     * network address. This class will start a background thread to
     * listen for incoming communication. Thus the class has to be closed
     * using the close() call when not needed anymore.
     *
     * @param address the network address and port to bind to.
     * @param packetHandler the object that is going to handle newly received
     *        packets.
     * @throws SocketException on socket errors.
     */
    public Connection(SocketAddress address,
                      PacketHandler packetHandler)
                                                    throws SocketException {
        socket             = new DatagramSocket(address);
        this.packetHandler = packetHandler;

        socket.setSoTimeout(LISTEN_TIMEOUT);

        packetReader = new PacketReader();
        packetReaderThread = new Thread(packetReader);
        packetReaderThread.start();
    }

    /**
     * Constructs a connection by binding to a a random locally available port
     * an all local network address. This class will start a background thread
     * to listen for incoming communication. Thus the class has to be closed
     * using the close() call when not needed anymore.
     *
     * @param packetHandler the object that is going to handle newly received
     *        packets.
     * @throws SocketException on socket errors.
     */
    public Connection(PacketHandler packetHandler)
                                                    throws SocketException {
        socket             = new DatagramSocket();
        this.packetHandler = packetHandler;

        socket.setSoTimeout(LISTEN_TIMEOUT);

        packetReader = new PacketReader();
        packetReaderThread = new Thread(packetReader);
        packetReaderThread.start();
    }

    /**
     * Close this connection. This also stops the background thread listening
     * for incoming packets - thus no further new packet events are going
     * to be generated.
     */
    public void close() {
        packetReader.shouldRun = false;
        socket.close();
        try {
            packetReaderThread.join();
        } catch (InterruptedException e) {
            // just don't care
        }

        socket.close();
    }

    /**
     * Send a packet to a remote address.
     * The id field in the packet will be disregarded, and a unique id will
     * be assigned by this call.
     *
     * @param address the address to send the packet to.
     * @param packet the packet to send.
     * @return the packet id that was generated for this packet.
     * @throws IOException on I/O errors.
     */
    public int send(SocketAddress address, Packet packet) throws IOException {
        int packetId = PacketIdSequence.nextId();
        packet.setPacketId(packetId);

        ByteArrayOutputStream  baOut = new ByteArrayOutputStream(packet.size());
        SerializationOutputStream out = new SerializationOutputStream(baOut);

        packet.serialize(out);
        out.flush();

        byte[] buf = baOut.toByteArray();
        DatagramPacket udpPacket = new DatagramPacket(buf, buf.length, address);

        socket.send(udpPacket);

        return packetId;
    }

    /**
     * Notify the packet handler for the new packet received.
     *
     * @param data the contents of the UDP packet received.
     * @param length the number of bytes relevant from the data array
     * @param client the address of the client the UDP packet was received from.
     */
    private void notifyPacketHandler(byte[]         data,
                                     int            length,
                                     SocketAddress  client) {
        try {
            ByteArrayInputStream     baIn = new ByteArrayInputStream(data);
            SerializationInputStream in   = new SerializationInputStream(baIn);
            Packet                   packet = new Packet();

            packet.deserialize(in, length);

            packetHandler.packetReceived(packet, client);
        } catch (IOException e) {
            // oops, some problems with the packet - we can't really do
            // anything about it
        }
    }
}
