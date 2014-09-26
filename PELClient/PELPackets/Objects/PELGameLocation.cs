using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PELPackets
{
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class PELGameLocation
    {

        public int GameLocationID;
        public string BaseName;
        public string Continent;
        public string SetupURL;

        public override string ToString()
        {
            return string.Format("{0} on {1}", BaseName, Continent);
        }
    }
}
