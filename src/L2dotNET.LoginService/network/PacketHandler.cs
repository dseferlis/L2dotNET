﻿using System;
using System.Collections.Concurrent;
using log4net;
using L2dotNET.LoginService.GSCommunication;
using L2dotNET.LoginService.Network.InnerNetwork.ClientPackets;
using L2dotNET.Network;

namespace L2dotNET.LoginService.Network
{
    public class PacketHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PacketHandler));

        private static readonly ConcurrentDictionary<byte, Type> ClientPackets = new ConcurrentDictionary<byte, Type>();
        private static readonly ConcurrentDictionary<byte, Type> ClientPacketsServ = new ConcurrentDictionary<byte, Type>();

        static PacketHandler()
        {
            ClientPackets.TryAdd(0x00, typeof(RequestAuthLogin));
            ClientPackets.TryAdd(0x02, typeof(RequestServerLogin));
            ClientPackets.TryAdd(0x05, typeof(RequestServerList));
            ClientPackets.TryAdd(0x07, typeof(AuthGameGuard));

            ClientPacketsServ.TryAdd(0xA0, typeof(RequestLoginServPing));
            ClientPacketsServ.TryAdd(0xA1, typeof(RequestLoginAuth));
            ClientPacketsServ.TryAdd(0xA2, typeof(RequestPlayerInGame));
            ClientPacketsServ.TryAdd(0xA3, typeof(RequestPlayersOnline));
        }

        public static void Handle(Packet packet, LoginClient client)
        {
            Log.Info($"Received packet with Opcode:{packet.FirstOpcode.ToString("X2")}");

            if (!ClientPackets.ContainsKey(packet.FirstOpcode))
                return;

            PacketBase incPacket = (PacketBase)Activator.CreateInstance(ClientPackets[packet.FirstOpcode], packet, client);
            incPacket?.RunImpl();
        }

        public static void Handle(Packet packet, ServerThread client)
        {
            Log.Info($"Received packet with Opcode:{packet.FirstOpcode.ToString("X2")}");

            if (!ClientPacketsServ.ContainsKey(packet.FirstOpcode))
                return;

            PacketBase incPacket = (PacketBase)Activator.CreateInstance(ClientPacketsServ[packet.FirstOpcode], packet, client);
            incPacket?.RunImpl();
        }
    }
}