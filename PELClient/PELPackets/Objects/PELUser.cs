using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PELPackets
{
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
    public class PELUser
    {
        [Flags]
        public enum UserPermissions
        {
            None            = 0x0000,
            Banned          = 0x0001,
            CantJoinGame    = 0x0002,
            

            CanKick         = 0x0100,
            CanWarn         = 0x0200,
            CanTerminateGame = 0x0400,
            CanChangeStats  = 0x0800,

            Admin           = 0xFF00,
        }

        public string Username;

        [Newtonsoft.Json.JsonIgnore]
        public string PasswordHash;

        public int MMR;
        public UserPermissions Permissions;
        public string LiveChar;
        public string Email;

    }
}
