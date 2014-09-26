using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using System.IO;

using PELPackets;
using Newtonsoft.Json;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace PELClient
{
    public class ConnectionClient
    {
        public const string ConnectTo = "localhost";
        public const int Port = 21132;


        public static string Username;
        public static string Password;


        private static TcpClient clientConnection;

        private static StreamWriter writer;
        private static StreamReader reader;

        private static Thread clientThread;


        private static Queue<Action> MainThreadActions = new Queue<Action>();
        private static Queue<Action> ConnectionThreadActions = new Queue<Action>();

        static bool RunThread = true;

        public static void Dispatch(Action act)
        {
            lock (MainThreadActions)
            {
                MainThreadActions.Enqueue(act);
            }
        }

        public static void DoMainThreadActions()
        {
            lock (MainThreadActions)
            {
                while (MainThreadActions.Count > 0)
                {
                    MainThreadActions.Dequeue()();
                }
            }
        }

        public static void SendChat(string from, string message)
        {
            ConnectionClient.Dispatch(() =>
            {
                PELClientForm.Instance.chatMenu1.AddChatMessage(DateTime.Now, from, message);
            });
        }

        public static void Kill()
        {
            lock (ConnectionThreadActions)
            {
                ConnectionThreadActions.Enqueue(() =>
                    {
                        clientConnection.Close();
                        RunThread = false;
                    });
            }
        }

        public static string ConnectToServer()
        {
            clientConnection = new TcpClient(ConnectTo, Port);

            clientConnection.ReceiveTimeout = 30;

            writer = new StreamWriter(clientConnection.GetStream());
            reader = new StreamReader(clientConnection.GetStream());

            IntroduceClientPacket icp = new IntroduceClientPacket();
            icp.Username = Username;
            icp.Password = Password;

            writer.WriteLine(JsonConvert.SerializeObject(icp));
            writer.Flush();


            string resp = reader.ReadLine();

            AcceptClientPacket acp = JsonConvert.DeserializeObject<AcceptClientPacket>(resp);

            if (acp.Response == AcceptClientPacket.ResponseType.Good)
            {
                clientThread = new Thread(RunClient);
                clientThread.Start();

                if(OnConnectionComplete != null)
                {
                    OnConnectionComplete();
                }

                return "Good";
            }

            if (acp.Response == AcceptClientPacket.ResponseType.BadLogin)
            {
                clientConnection.Close();
                return "Bad Login";
            }
            if (acp.Response == AcceptClientPacket.ResponseType.Banned)
            {
                clientConnection.Close();
                return "Banned";
            }

            clientConnection.Close();
            return "Error";




        }

        public static event Action OnConnectionComplete;

        public static RegisterResponse RegisterClient(RegisterUserPacket rup)
        {
            clientConnection = new TcpClient(ConnectTo, Port);

            clientConnection.ReceiveTimeout = 5 * 1000;

            writer = new StreamWriter(clientConnection.GetStream());
            reader = new StreamReader(clientConnection.GetStream());

            writer.WriteLine(JsonConvert.SerializeObject(rup));
            writer.Flush();

            string response = reader.ReadLine();

            clientConnection.Close();

            RegisterResponse res = JsonConvert.DeserializeObject<RegisterResponse>(response);

            clientConnection = null;
            writer = null;
            reader = null;

            return res;

        }






        private static void RunClient()
        {



            while (RunThread)
            {
                lock (ConnectionThreadActions)
                {
                    while (ConnectionThreadActions.Count > 0)
                    {
                        ConnectionThreadActions.Dequeue()();
                    }
                }

                if(!clientConnection.Connected)
                {
                    MessageBox.Show("Server died");
                    Application.Exit();
                }

                //Handle the packets
                try
                {
                    string line = reader.ReadLine();
                    //Get the type of the packet

                    JObject jo = JObject.Parse(line);

                    string type = (string)jo["Type"];

                    Type t = Type.GetType(type);

                    PELPacket packet = JsonConvert.DeserializeObject(line, t) as PELPacket;

                    PacketHandlers.HandlePacket(packet);

                }
                catch (Exception e)
                {
                    if(!(e is IOException))
                        Dispatch(() =>
                        {
                            PELClientForm.Instance.chatMenu1.AddChatMessage(DateTime.Now, "Error: ", e.ToString());
                        });
                }

                
            }


        }

        public static void WritePacket(PELPacket packet)
        {
            lock (ConnectionThreadActions)
            {
                ConnectionThreadActions.Enqueue(() =>
                    {
                        writer.WriteLine(JsonConvert.SerializeObject(packet));
                        writer.Flush();
                    });
            }
        }

    }
}
