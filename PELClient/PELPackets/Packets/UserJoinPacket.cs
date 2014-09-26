using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PELPackets
{
    public class UserStateChange : PELPacket
    {
        public enum StateChanges
        {
            None,
            Join,
            Part,
        }

        public string Username;
        public StateChanges State;
    }
}
