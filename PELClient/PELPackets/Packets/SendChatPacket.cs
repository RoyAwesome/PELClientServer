using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PELPackets
{
    public class SendChatPacket : PELPacket
    {
        public string Username;
        public string Message;
        public DateTime when;
    }
}
