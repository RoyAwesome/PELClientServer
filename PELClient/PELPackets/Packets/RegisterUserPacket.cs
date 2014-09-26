using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PELPackets
{
    public class RegisterUserPacket : PELPacket
    {
        public string Username;
        public string Password;
        public string Email;
        public string UserId;
        public string BetaKey;
    }

    public class RegisterResponse : PELPacket
    {
        public enum ResponseType
        {
            Good,
            UserExists,
            CharacterExists,
            BadBetakey,
            Banned,
            Timeout,
        }

        public ResponseType Response;
    }
}
