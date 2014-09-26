using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PELPackets;

namespace PELClient
{
    public delegate void HandlePacket(PELPacket packet);

    public static class PacketHandlers
    {
        private static Dictionary<Type, HandlePacket> Handlers = new Dictionary<Type, HandlePacket>()
        {           
            {typeof(SendChatPacket), HandleChatPacket},
            {typeof(UserStateChange), HandlePlayerStateChange},
            {typeof(PlayerListPacket), HandlePlayerlistPacket},
            {typeof(StaticDataPacket), HandleStaticDataPacket},
            {typeof(GameListPacket), HandleGameListPacket},
            {typeof(PlayerStateChangedPacket), HandlePlayerGameStatePacket},
            {typeof(NewGamePacket), HandleNewGamePacket},
            {typeof(GameStateChangePacket), HandleGameStatePacket},
        };
        public static void HandlePacket(PELPacket packet)
        {
            if(! Handlers.ContainsKey(packet.GetType()))
            {
                ConnectionClient.SendChat("CLIENT_ERROR", "Unable to handle packet: " + packet.GetType());
                return;
            }
            Handlers[packet.GetType()](packet);
        }


     

        public static void HandleChatPacket(PELPacket packet)
        {
            SendChatPacket scp = packet as SendChatPacket;
            ConnectionClient.Dispatch(() =>
                {
                    PELClientForm.Instance.chatMenu1.AddChatMessage(scp.when, scp.Username, scp.Message);
                });

        }

        public static void HandlePlayerStateChange(PELPacket packet)
        {
            UserStateChange usc = packet as UserStateChange;

            if(usc.State == UserStateChange.StateChanges.Join)
            {
                ConnectionClient.Dispatch(() =>
                    {
                        PELClientForm.Instance.chatMenu1.AddChatMessage(DateTime.Now, "Server", usc.Username + " has joined!");
                        PELClientForm.Instance.chatMenu1.AddUser(usc.Username);
                    });
            }
            if(usc.State == UserStateChange.StateChanges.Part)
            {
                ConnectionClient.Dispatch(() =>
                {
                    PELClientForm.Instance.chatMenu1.AddChatMessage(DateTime.Now, "Server", usc.Username + " has left!");
                    PELClientForm.Instance.chatMenu1.RemoveUser(usc.Username);
                });
            }

        }

        public static void HandlePlayerlistPacket(PELPacket packet)
        {
            PlayerListPacket plp = packet as PlayerListPacket;

            ConnectionClient.Dispatch(() =>
                {
                    foreach (String s in plp.Usernames)
                    {
                        PELClientForm.Instance.chatMenu1.AddUser(s);
                    }
                });
           

        }


        public static void HandleGameListPacket(PELPacket packet)
        {
            GameListPacket glp = packet as GameListPacket;


            ConnectionClient.Dispatch(() =>
                {
                    PELClientForm.Instance.ClearGames();

                    foreach(PELGame game in glp.Games)
                    {
                        PELClientForm.Instance.AddGame(game);
                    }

                });

        }

        public static void HandleNewGamePacket(PELPacket packet)
        {
            NewGamePacket ngp = packet as NewGamePacket;

            ConnectionClient.Dispatch(() =>
                {
                    PELClientForm.Instance.AddGame(ngp.Game, true);
                    if(ngp.Game.HostUsername == ConnectionClient.Username)
                    {
                        //That's us!
                        PELClientForm.Instance.JoinGame(ngp.Game);
                    }
                });

        }

        public static void HandleStaticDataPacket(PELPacket packet)
        {
            StaticDataPacket staticData = packet as StaticDataPacket;

            ConnectionClient.Dispatch(() =>
                {
                    PELClientForm.Instance.GameLocations.Clear();
                    PELClientForm.Instance.GameLocations.AddRange(staticData.Locations);

                    PELClientForm.Instance.GameModes.Clear();
                    PELClientForm.Instance.GameModes.AddRange(staticData.GameModes);

                    PELClientForm.Instance.GameTypes.Clear();
                    PELClientForm.Instance.GameTypes.AddRange(staticData.GameTypes);
                });

        }

        public static void HandleGameStatePacket(PELPacket packet)
        {
            GameStateChangePacket gscp = packet as GameStateChangePacket;

            if(gscp.GameState == PELGame.PELGameState.Closed)
            {
                ConnectionClient.Dispatch(() =>
                    {
                        //TODO: Remove the game
                        PELClientForm.Instance.RemoveGame(gscp.GameID);
                    });
            }

        }

        public static void HandlePlayerGameStatePacket(PELPacket packet)
        {
            PlayerStateChangedPacket pscp = packet as PlayerStateChangedPacket;

            if(pscp.State == PlayerStateChangedPacket.PlayerState.Denied)
            {
                ConnectionClient.Dispatch(() =>
                    {
                        PELClientForm.Instance.chatMenu1.AddChatMessage(DateTime.Now, "Server", "You cannot join that game. Reason: " + pscp.Reason); 

                    });

                return;
            }

            if(pscp.State == PlayerStateChangedPacket.PlayerState.Joined)
            {
                
                ConnectionClient.Dispatch(() =>
                {

                    PELClientForm.Instance.chatMenu1.AddChatMessage(DateTime.Now, "Server", string.Format("{0} has joined game {1}", pscp.Username, pscp.GameID));
                    PELGame game = PELClientForm.Instance.KnownGames.FirstOrDefault(x => x.ActiveGameID == pscp.GameID);
                    game.AddPlayer(pscp.Username);
                    PELClientForm.Instance.RefreshGame(game);

                    if(pscp.Username == ConnectionClient.Username)
                    {
                        //That's us!
                        PELClientForm.Instance.JoinGame(game);
                    }
                    if(game == PELClientForm.Instance.CurrentGame)
                    {
                        PELClientForm.Instance.InGameForm.RefreshPlayers();
                    }
                    
                });

                return;
            }

            if (pscp.State == PlayerStateChangedPacket.PlayerState.Left)
            {
                ConnectionClient.Dispatch(() =>
                {
                    PELClientForm.Instance.chatMenu1.AddChatMessage(DateTime.Now, "Server", string.Format("{0} has left game {1}", pscp.Username, pscp.GameID));
                    PELGame game = PELClientForm.Instance.KnownGames.FirstOrDefault(x => x.ActiveGameID == pscp.GameID);
                    game.RemovePlayer(pscp.Username);

                    PELClientForm.Instance.RefreshGame(game);
                    if (pscp.Username == ConnectionClient.Username)
                    {
                        //That's us!
                        PELClientForm.Instance.LeaveGame();
                    }
                    if (game == PELClientForm.Instance.CurrentGame)
                    {
                        PELClientForm.Instance.InGameForm.RefreshPlayers();
                    }

                });

                return;
            }

            if (pscp.State == PlayerStateChangedPacket.PlayerState.Kicked)
            {
                ConnectionClient.Dispatch(() =>
                {
                    PELClientForm.Instance.chatMenu1.AddChatMessage(DateTime.Now, "Server", string.Format("{0} has been kicked from {1}", pscp.Username, pscp.GameID));
                    PELGame game = PELClientForm.Instance.KnownGames.FirstOrDefault(x => x.ActiveGameID == pscp.GameID);
                    game.RemovePlayer(pscp.Username);
                    PELClientForm.Instance.RefreshGame(game);

                    if (pscp.Username == ConnectionClient.Username)
                    {
                        //That's us!
                        PELClientForm.Instance.LeaveGame();
                    }
                    if (game == PELClientForm.Instance.CurrentGame)
                    {
                        PELClientForm.Instance.InGameForm.RefreshPlayers();
                    }

                });

                return;
            }



        }

        public static void SuccessfulConnection()
        {
            //We are successfully connected?  Great!  Lets get a bunch of data
            RequestPacket request = new RequestPacket();
            request.Request = RequestPacket.RequestType.GameList;
            ConnectionClient.WritePacket(request);



            RequestPacket rcp = new RequestPacket();
            rcp.Request = RequestPacket.RequestType.UserList;
            rcp.Data = ConnectionClient.Username;
            ConnectionClient.WritePacket(rcp);


            rcp = new RequestPacket();
            rcp.Request = RequestPacket.RequestType.StaticData;
            ConnectionClient.WritePacket(rcp);
        }

    }
}
