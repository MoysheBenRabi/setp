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
import mxp.packet.PacketTest;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.util.Vector;

import junit.framework.TestCase;

/**
 * Test to verify the ServerConnection and Connection classes.
 */
public class ConnectionTest extends TestCase {
    /**
     * A class to handle receiving packets.
     */
    private class TestPacketHandler implements PacketHandler {
        /**
         * The list of packets received.
         */
        private Vector<Packet>   packets = new Vector<Packet>();

        /**
         * The address of clients that sent the packets.
         */
        private Vector<SocketAddress>   clients = new Vector<SocketAddress>();

        @Override
        public void packetReceived(Packet packet, SocketAddress client) {
            packets.add(packet);
            clients.add(client);
        }
    }

    /**
     * Test the server binding to a port, and the client connecting to it.
     *
     * @throws IOException on I/O errors
     */
    public void testConnection() throws IOException {
        TestPacketHandler   serverPacketHandler = new TestPacketHandler();
        InetSocketAddress   serverAddress =
                                    new InetSocketAddress("localhost", 5665);
        Connection    server = new Connection(serverAddress,
                                                          serverPacketHandler);

        TestPacketHandler   clientPacketHandler = new TestPacketHandler();
        Connection    client = new Connection(clientPacketHandler);

        // so far so good, we should be connceted now
        // otherwise, exceptions would have been thrown

        client.close();
        server.close();
    }

    /**
     * Test a bad client connection attempt.
     *
     * @throws IOException on I/O errors
     */
    public void testBadConnection() throws IOException {
        boolean gotException = false;
        try {
            TestPacketHandler   clientPacketHandler = new TestPacketHandler();
            InetSocketAddress   clientAddress =
                                     new InetSocketAddress("nonexistent", 123);
            Connection    client = new Connection(clientAddress,
                                                          clientPacketHandler);
            // this will not get executed by now
            client.close();
        } catch (IOException e) {
            gotException = true;
        }

        assertTrue(gotException);
    }

    /**
     * Test the a client connecting to a server, and sending a single packet,
     * and then the other way around.
     *
     * @throws IOException on I/O errors
     */
    public void testSinglePacketRoundtrip() throws IOException {
        TestPacketHandler   serverPacketHandler = new TestPacketHandler();
        InetSocketAddress   serverAddress =
                                    new InetSocketAddress("localhost", 5665);
        Connection    server = new Connection(serverAddress,
                                                          serverPacketHandler);

        TestPacketHandler   clientPacketHandler = new TestPacketHandler();
        Connection    client = new Connection(clientPacketHandler);

        // send a packet from the client to the server, and verify
        Packet packet = PacketTest.generatePacket();
        client.send(serverAddress, packet);

        try {
            Thread.yield();
            Thread.sleep(100);
        } catch (InterruptedException e) {
        }

        assertEquals(serverPacketHandler.packets.size(), 1);
        assertEquals(serverPacketHandler.packets.firstElement(), packet);
        assertEquals(serverPacketHandler.clients.size(), 1);

        // send a packet from the server to the client, and verify
        packet = PacketTest.generatePacket();
        server.send(serverPacketHandler.clients.firstElement(), packet);

        try {
            Thread.yield();
            Thread.sleep(100);
        } catch (InterruptedException e) {
        }

        assertEquals(clientPacketHandler.packets.size(), 1);
        assertEquals(clientPacketHandler.packets.firstElement(), packet);
        assertEquals(clientPacketHandler.clients.size(), 1);
        assertEquals(clientPacketHandler.clients.firstElement(), serverAddress);

        client.close();
        server.close();
    }

    /**
     * Test the a client connecting to a server, and sending a multiple packets,
     * and the other way around.
     *
     * @throws IOException on I/O errors
     */
    public void testMultiplePacketRoundtrip() throws IOException {
        TestPacketHandler   serverPacketHandler = new TestPacketHandler();
        InetSocketAddress   serverAddress =
                                    new InetSocketAddress("localhost", 5665);
        Connection    server = new Connection(serverAddress,
                                                          serverPacketHandler);

        TestPacketHandler   clientPacketHandler = new TestPacketHandler();
        Connection    client = new Connection(clientPacketHandler);

        // generate server to client packets
        Vector<Packet> toClientPackets = new Vector<Packet>();
        for (int i = 0; i < 5; ++i) {
            toClientPackets.add(PacketTest.generatePacket());
        }
        // generate client to server packets
        Vector<Packet> toServerPackets = new Vector<Packet>();
        for (int i = 0; i < 7; ++i) {
            toServerPackets.add(PacketTest.generatePacket());
        }

        // send the first packet to the server
        client.send(serverAddress, toServerPackets.get(0));

        try {
            Thread.yield();
            Thread.sleep(100);
        } catch (InterruptedException e) {
        }

        assertEquals(serverPacketHandler.packets.size(), 1);
        assertEquals(serverPacketHandler.packets.firstElement(),
                     toServerPackets.firstElement());
        assertEquals(serverPacketHandler.clients.size(), 1);

        // now send the rest of the packets interleaved
        SocketAddress clientAddress =
                                    serverPacketHandler.clients.firstElement();

        server.send(clientAddress, toClientPackets.get(0));
        client.send(serverAddress, toServerPackets.get(1));
        client.send(serverAddress, toServerPackets.get(2));
        server.send(clientAddress, toClientPackets.get(1));
        client.send(serverAddress, toServerPackets.get(3));
        server.send(clientAddress, toClientPackets.get(2));
        client.send(serverAddress, toServerPackets.get(4));
        server.send(clientAddress, toClientPackets.get(3));
        client.send(serverAddress, toServerPackets.get(5));
        client.send(serverAddress, toServerPackets.get(6));
        server.send(clientAddress, toClientPackets.get(4));

        try {
            Thread.yield();
            Thread.sleep(100);
        } catch (InterruptedException e) {
        }

        assertEquals(clientPacketHandler.packets.size(), 5);
        assertEquals(clientPacketHandler.packets, toClientPackets);
        assertEquals(clientPacketHandler.clients.size(), 5);

        assertEquals(serverPacketHandler.packets.size(), 7);
        assertEquals(serverPacketHandler.packets, toServerPackets);
        assertEquals(serverPacketHandler.clients.size(), 7);

        client.close();
        server.close();
    }
}
