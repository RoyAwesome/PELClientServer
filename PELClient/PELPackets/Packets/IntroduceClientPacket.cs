using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PELPackets
{
    public class IntroduceClientPacket : PELPacket
    {
        public string Username;
        public string Password;
        
    }

    public class AcceptClientPacket : PELPacket
    {
        public enum ResponseType
        {
            Good,
            BadLogin,
            Banned,
            Error,
        }

        public ResponseType Response;
        public string Username;
        public int ClientID;
        //TODO: Player Info
        PELUser User;
    }
}
