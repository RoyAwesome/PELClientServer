using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using PELPackets;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Reflection;

namespace PELServer
{
    public class PELConnection
    {

        TcpClient Client;

        Thread thread;

        public int UserID;

        public string Username = "No Name";


        public PELUser User;

        StreamReader reader;
        StreamWriter writer;

        Queue<Action> Actions = new Queue<Action>();

        public PELConnection(TcpClient client)
        {
            Client = client;

            thread = new Thread(doThread);
            thread.Start();
        }


        private void doThread()
        {
            reader = new StreamReader(Client.GetStream());
            writer = new StreamWriter(Client.GetStream());


            Client.ReceiveTimeout = 10;

            while (true)
            {
                lock (Actions)
                {
                    while (Actions.Count > 0)
                    {
                        Actions.Dequeue()();
                    }
                }


                //Handle the packets
                try
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        //Kill the connection and return
                        PELServer.KillConnection(this);
                        Console.WriteLine("Lost " + Username);
                        return;
                    }

                    //Get the type of the packet
                    JObject jo = JObject.Parse(line);

                    string type = (string)jo["Type"];

                    Type t = Type.GetType(type);

                    PELPacket packet = JsonConvert.DeserializeObject(line, t) as PELPacket;

                    PELServer.HandlePacket(packet, this);

                }
                catch (IOException)
                {
                    //Ignore these, it's thrown when it times out
                }
                catch (Exception e)
                {
                    //Send the rest as errors to the client

                    if (User.Permissions == PELUser.UserPermissions.Admin) SendChat(e.ToString());
                    else SendChat("There was an error processing your request.  It's been logged");
                }


                if (!Client.Connected)
                {
                    Console.WriteLine("lost client " + Username);
                    PELServer.KillConnection(this);
                    return;
                }

            }


        }

        private void Send(string message)
        {
            writer.WriteLine(message);
            writer.Flush();
        }

        private void SendChat(string message)
        {
            SendChatPacket scp = new SendChatPacket();
            scp.when = DateTime.Now;
            scp.Username = "Server";
            scp.Message = message;

            SendPacket(scp);
        }

        private void KillConnection()
        {
            Client.Close();
        }


        public void Kill()
        {
            lock (Actions)
            {
                Actions.Enqueue(() =>
                    {
                        KillConnection();
                    });
            }
        }

        public void SendPacket(PELPacket packet)
        {
            lock (Actions)
            {
                Actions.Enqueue(() =>
                {
                    Send(JsonConvert.SerializeObject(packet));
                });
            }
        }


    }
}
