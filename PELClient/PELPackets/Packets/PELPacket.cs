using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PELPackets
{
    public class PELPacket
    {
        public string Type
        {
            get { return GetType().AssemblyQualifiedName; }
        }

    }
}
