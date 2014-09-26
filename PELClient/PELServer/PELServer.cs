#define DEV
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using System.Dynamic;
using System.Security.Cryptography;


namespace PELServer
{


    public delegate void HandlePacket(PELPackets.PELPacket packet, PELConnection connection);
    class PELServer
    {
        public static PELServer Instance;

        public const int Port = 21132;


        public MongoClient DatabaseClient;

        public PELGames GameList = new PELGames();

#if DEV
        public const string MongoConnectString = "mongodb://192.168.1.106";
#else
        public const string MongoConnectString = "mongodb://localhost";
#endif

        public PELServer()
        {
            Instance = this;

            DatabaseClient = new MongoClient(MongoConnectString);

            Console.WriteLine("Server online");

            PELDatabase.CreateBasicGameTypes();
        }


        public static void HandlePacket(PELPackets.PELPacket packet, PELConnection connection)
        {

            QueueAction(() =>
                {
                    Instance.DealWithPacket(packet, connection);
                });

        }

        public static void Broadcast(PELPackets.PELPacket packet)
        {

            QueueAction(() =>
            {
                Instance.BroadcastPacket(packet);
            });

        }

        public static void KillConnection(PELConnection connection)
        {
            QueueAction(() => { Instance.KillPELConnection(connection); });
        }


        private static void QueueAction(Action act)
        {
            lock (Instance.MainThreadActions)
            {
                Instance.MainThreadActions.Enqueue(act);
            }
        }

        TcpListener serverSocket;

        Queue<Action> MainThreadActions = new Queue<Action>();

        Dictionary<Type, HandlePacket> PacketHandlers = new Dictionary<Type, HandlePacket>()
            {
                { typeof(PELPackets.IntroduceClientPacket), PELPacketHandler.HandleIntroPacket },
                { typeof(PELPackets.SendChatPacket), PELPacketHandler.HandleChatPacket },
                { typeof(PELPackets.RequestPacket), PELPacketHandler.HandleRequestPacket },
                { typeof(PELPackets.RegisterUserPacket), PELPacketHandler.HandleRegisterPacket },
                { typeof(PELPackets.RequestNewGamePacket), PELPacketHandler.RequestNewGamePacket }
            };

        public List<PELConnection> Connections = new List<PELConnection>();


        public void Run()
        {

            serverSocket = new TcpListener(IPAddress.Any, Port);

            serverSocket.Start();
            while (true)
            {
                if (serverSocket.Pending())
                {

                    TcpClient connection = serverSocket.AcceptTcpClient();
                    Console.WriteLine("Got a connection");

                    Connections.Add(new PELConnection(connection));

                }

                lock (MainThreadActions)
                {
                    while (MainThreadActions.Count > 0)
                    {
                        Action act = MainThreadActions.Dequeue();
                        act();
                    }
                }

            }


        }

        private void DealWithPacket(PELPackets.PELPacket packet, PELConnection connection)
        {
            if(!PacketHandlers.ContainsKey(packet.GetType()))
            {
                Logger.Error("Unable to handle packet: " + packet.GetType());
                return;
            }
            PacketHandlers[packet.GetType()](packet, connection);
        }

        private void BroadcastPacket(PELPackets.PELPacket packet)
        {
            foreach (PELConnection conn in Connections)
            {
                conn.SendPacket(packet);
            }
        }

        public void KillPELConnection(PELConnection connection)
        {

            Connections.Remove(connection);
            PELPackets.UserStateChange usc = new PELPackets.UserStateChange();
            usc.State = PELPackets.UserStateChange.StateChanges.Part;
            usc.Username = connection.Username;

            GameList.UserDisconnected(connection.Username);

            BroadcastPacket(usc);

        }

        public IEnumerable<PELConnection> GetConnections()
        {
            return Connections.AsEnumerable();
        }




    }
}
