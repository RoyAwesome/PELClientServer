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

using PELPackets;

namespace PELServer
{
    class PELPacketHandler
    {

        public static void HandleIntroPacket(PELPacket packet, PELConnection connection)
        {
            IntroduceClientPacket icp = packet as IntroduceClientPacket;

            //TODO: Validate user
            Console.WriteLine("Acceping connection for " + icp.Username);

            PELUser user = PELDatabase.GetUser(icp.Username, icp.Password);

            AcceptClientPacket acp = new AcceptClientPacket();
            
            acp.Username = icp.Username;

            if(user == null) //Fail the user login
            {
                acp.Response = AcceptClientPacket.ResponseType.BadLogin;
               
            }
            else if(user.Permissions.HasFlag(PELUser.UserPermissions.Banned))
            {
                acp.Response = AcceptClientPacket.ResponseType.Banned;
            }
            else
            {
               //we are successful
                acp.Response = AcceptClientPacket.ResponseType.Good;
            }



            connection.Username = icp.Username;
            connection.SendPacket(acp);


            if(acp.Response == AcceptClientPacket.ResponseType.Good)
            {
                UserStateChange usc = new UserStateChange();
                usc.Username = acp.Username;
                usc.State = UserStateChange.StateChanges.Join;

                connection.User = user;

                PELServer.Broadcast(usc);
            }
            else
            {
                connection.Kill();
            }
         

            
        }

        public static void HandleChatPacket(PELPacket packet, PELConnection connection)
        {
            SendChatPacket scp = packet as SendChatPacket;
            scp.when = DateTime.Now;

            Console.WriteLine(scp.Username + ": " + scp.Message);

            PELServer.Broadcast(scp);
        }

        public static void HandleRequestPacket(PELPacket packet, PELConnection connection)
        {
            RequestPacket rq = packet as RequestPacket;

            if(rq.Request == RequestPacket.RequestType.UserList)
            {
                //Get the usernames 
                string[] Users = PELServer.Instance.GetConnections().Select(x => x.Username).Where(x => x != connection.Username).ToArray();

                PlayerListPacket plp = new PlayerListPacket();
                plp.Usernames = Users;

                connection.SendPacket(plp);
            }         
  
            if(rq.Request == RequestPacket.RequestType.GameList)
            {
                PELGame[] games = PELServer.Instance.GameList.AllGames.ToArray();

                GameListPacket glp = new GameListPacket();
                glp.Games = games;

                connection.SendPacket(glp);

            }

            if(rq.Request == RequestPacket.RequestType.StaticData)
            {
                //Gather all of the static data

                PELGameType[] gametypes = PELDatabase.GetGameTypes().FindAll().ToArray();
                PELGameLocation[] locations = PELDatabase.GetGameLocations().FindAll().ToArray();
                PELGameMode[] gamemodes = PELDatabase.GetGamemodes().FindAll().ToArray();


                StaticDataPacket sdp = new StaticDataPacket();
                sdp.GameModes = gamemodes;
                sdp.Locations = locations;
                sdp.GameTypes = gametypes;

                connection.SendPacket(sdp);

            }

            if(rq.Request == RequestPacket.RequestType.JoinGame)
            {
                int gameid = int.Parse(rq.Data);
                Logger.Log(connection.Username + " wants to join gameid " + gameid);
                PELServer.Instance.GameList.UserJoinGame(gameid, connection);
            }

            if(rq.Request == RequestPacket.RequestType.LeaveGame)
            {
                int gameid = int.Parse(rq.Data);
                Logger.Log(connection.Username + " is leaving gameid: " + gameid);

                PELServer.Instance.GameList.RemoveUserFromGame(gameid, connection.Username);
            }

        }

        public static void HandleRegisterPacket(PELPacket packet, PELConnection connection)
        {
            RegisterUserPacket rup = packet as RegisterUserPacket;

            RegisterResponse resp = new RegisterResponse();

            //check to see if this user exists
            var Users = PELDatabase.GetUsersTable();

            var query = Query<PELUser>.Where(x => x.Username == rup.Username);

            PELUser user = Users.FindOne(query);
            if(user != null)
            {
                resp.Response = RegisterResponse.ResponseType.UserExists;
                connection.SendPacket(resp);
                return;
            }

            query = Query<PELUser>.Where(x => x.LiveChar == rup.UserId);

            if (Users.FindOne(query) != null)
            {
                resp.Response = RegisterResponse.ResponseType.UserExists;
                connection.SendPacket(resp);
                return;
            }
            
            //If we've made it here, we win and can create the user

            PELUser newUser = new PELUser();

            newUser.Username = rup.Username;
            newUser.PasswordHash = PELDatabase.HashPassword(rup.Password);
            newUser.Email = rup.Email;
            newUser.LiveChar = rup.UserId;
            newUser.MMR = 1000;

            Users.Insert(newUser);

            Console.WriteLine("Registered " + newUser.Username + " email: " + newUser.Email);

            resp.Response = RegisterResponse.ResponseType.Good;
            connection.SendPacket(resp);
            return;
            
        }


        public static void RequestNewGamePacket(PELPacket packet, PELConnection connection)
        {
            RequestNewGamePacket rngp = packet as RequestNewGamePacket;

            PELServer.Instance.GameList.StartNewGame(rngp.RequestedGame, connection);
        }
    }
}
