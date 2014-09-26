using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PELPackets
{
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class PELGameMode
    {
        public int GamemodeID;
        public string Name;
        public string Description;

        public override string ToString()
        {
            return Name;
        }
    }
}
