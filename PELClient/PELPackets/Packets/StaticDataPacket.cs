using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PELPackets
{
    public class StaticDataPacket : PELPacket
    {
        public PELGameType[] GameTypes;
                
        public PELGameLocation[] Locations;

        public PELGameMode[] GameModes;

    }
}
