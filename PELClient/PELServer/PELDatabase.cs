using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;

using System.Security.Cryptography;

using PELPackets;

namespace PELServer
{
    public class PELDatabase
    {

        public static string HashPassword(string password)
        {
            string passwordHash = ""; 

            using(var sha = SHA256.Create())
            {
                passwordHash = BitConverter.ToString(sha.ComputeHash(Encoding.ASCII.GetBytes(password))).Replace("-", "");
            }

            return passwordHash;            

        }

        public static void CreateRoy()
        {
            PELUser roy = new PELUser();

            roy.Username = "RoyAwesome";

             
            roy.PasswordHash = HashPassword("a295.a1");
            roy.Permissions = PELUser.UserPermissions.Admin;

            roy.MMR = 1000;

            var server = PELServer.Instance.DatabaseClient.GetServer();

            var users = server.GetDatabase("PEL").GetCollection<PELUser>("Users");

            users.Insert(roy);


        }

        public static void CreateBasicGameTypes()
        {
            PELGameMode gamemode = new PELGameMode();

            gamemode.Name = "Debug Gamemode";
            gamemode.Description = "You should never have this gamemode";
            gamemode.GamemodeID = 1;

            var gmtab = GetGamemodes();
            gmtab.Drop();
            gmtab.Insert(gamemode);


            PELGameType gameType = new PELGameType();
            gameType.GameTypeId = 1;
            gameType.GameName = "Base Cap";
            gameType.GameDescriptionURL = "NoURL";
            gameType.MaxPlayers = new int[]
            {                
                12,
                18,
                24,
                32,
                48,
            };
            gameType.Locations = new int[]
            {
                1,
            };

            var gametypetab = GetGameTypes();
            gametypetab.Drop();
            gametypetab.Insert(gameType);

            PELGameLocation location = new PELGameLocation();
            location.BaseName = "The Octagon";
            location.Continent = "Esamir";
            location.GameLocationID = 1;
            location.SetupURL = "NoURL";

            var gameloctab = GetGameLocations();
            gameloctab.Drop();
            gameloctab.Insert(location);

        }

        public static PELUser GetUser(string username, string password)
        {
           
           
            var users = GetUsersTable();

            string passwordHash = HashPassword(password);

            var query = Query<PELUser>.Where(x => x.PasswordHash == passwordHash && x.Username == username);

            PELUser user = users.FindOne(query);


            return user;

        }

        private static MongoCollection<T> GetTable<T>(string name)
        {
            var server = PELServer.Instance.DatabaseClient.GetServer();

            var collection = server.GetDatabase("PEL").GetCollection<T>(name);

            return collection;
        }

        public static MongoCollection<PELUser> GetUsersTable()
        {
            var users = GetTable<PELUser>("Users");

            return users;
        }

        public static MongoCollection<PELGameMode> GetGamemodes()
        {
            var Gamemodes = GetTable<PELGameMode>("Gamemodes");
            return Gamemodes;
        }

        public static MongoCollection<PELGameType> GetGameTypes()
        {
            var gametypes = GetTable<PELGameType>("Gametypes");
            return gametypes;
        }

        public static MongoCollection<PELGameLocation> GetGameLocations()
        {
            return GetTable<PELGameLocation>("GameLocations");
        }
    }
}
