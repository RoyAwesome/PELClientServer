using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PELPackets
{
    public class RequestPacket : PELPacket
    {
        public enum RequestType
        {            
            UserList,
            GameList,
            Stats,
            GameTypes,
            GameModes,
            GameLocation,
            StaticData,
            JoinGame,
            LeaveGame,
        }

        public RequestType Request;
        public string Data;

    }
}
