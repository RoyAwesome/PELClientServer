using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PELPackets
{
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class PELGameType
    {
        public int GameTypeId;
        public string GameName;
        public int[] MaxPlayers;
        public string GameDescriptionURL;
        public int[] Locations;

        public override string ToString()
        {
            return GameName;
        }
    }
}
