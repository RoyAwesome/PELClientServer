using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PELPackets;

namespace PELServer
{
    public class PELGames
    {
        public IEnumerable<PELGame> AllGames
        {
            get { return GameList; }
        }


        List<PELGame> GameList = new List<PELGame>();

        int CurrentID = 1;


        public void StartNewGame(PELGame game, PELConnection connection)
        {
            PELGame userInGame = AllGames.FirstOrDefault(x => x.Players.Contains(connection.Username));
            if (userInGame != null)
            {
                PlayerStateChangedPacket pscp = new PlayerStateChangedPacket();
                pscp.State = PlayerStateChangedPacket.PlayerState.Denied;
                pscp.Reason = "You are already in a game";

                connection.SendPacket(pscp);
                return;
            }


            game.GameState = PELGame.PELGameState.Open;
            game.ActiveGameID = CurrentID;
            CurrentID++;

            game.Players = new string[0];




            GameList.Add(game);

            NewGamePacket ngp = new NewGamePacket();
            ngp.Game = game;

            PELServer.Broadcast(ngp);
            Logger.Log(connection.Username + " started " + game.ActiveGameID);


            UserJoinGame(game.ActiveGameID, connection);

        }

        public void UserJoinGame(int gameID, PELConnection connection)
        {

            //Check if the user is in a game

            PELGame userInGame = AllGames.FirstOrDefault(x => x.Players.Contains(connection.Username));
            if(userInGame != null)
            {
                PlayerStateChangedPacket pscp = new PlayerStateChangedPacket();
                pscp.State = PlayerStateChangedPacket.PlayerState.Denied;
                pscp.Reason = "You are already in a game";

                connection.SendPacket(pscp);
                return;
            }



            PELGame game = AllGames.FirstOrDefault(x => x.ActiveGameID == gameID);

            if (game == null)
            {
                PlayerStateChangedPacket pscp = new PlayerStateChangedPacket();
                pscp.State = PlayerStateChangedPacket.PlayerState.Denied;
                pscp.Reason = "Game not found";

                connection.SendPacket(pscp);
                return;
            }

            if (game.GameState != PELGame.PELGameState.Open)
            {
                //Don't put the player in the game if it's not open
                PlayerStateChangedPacket pscp = new PlayerStateChangedPacket();
                pscp.State = PlayerStateChangedPacket.PlayerState.Denied;
                pscp.Reason = "Game is not open";

                connection.SendPacket(pscp);
                return;
            }

            if(game.BannedPlayers != null && game.BannedPlayers.Contains(connection.Username))
            {
                PlayerStateChangedPacket pscp = new PlayerStateChangedPacket();
                pscp.State = PlayerStateChangedPacket.PlayerState.Denied;
                pscp.Reason = "You are banned from this game";

                connection.SendPacket(pscp);
                return;
            }

            if(game.Players.Length == game.MaxPlayers)
            {
                PlayerStateChangedPacket pscp = new PlayerStateChangedPacket();
                pscp.State = PlayerStateChangedPacket.PlayerState.Denied;
                pscp.Reason = "Full";

                connection.SendPacket(pscp);
                return;
            }


            Logger.Log("User " + connection.Username + " joined game " + game.ActiveGameID);

            game.AddPlayer(connection.Username);


            if (game.HostUsername == connection.Username) return; //Don't send the player join packet to people if the host has joined the game         


            PlayerStateChangedPacket pscpp = new PlayerStateChangedPacket();
            pscpp.Username = connection.Username;
            pscpp.GameID = gameID;
            pscpp.State = PlayerStateChangedPacket.PlayerState.Joined;

            PELServer.Broadcast(pscpp);

        }

        public void UserDisconnected(string username)
        {
            //Check to see if he is in any game

            PELGame game = AllGames.FirstOrDefault(x => x.Players.Contains(username));

            if (game == null) return; //Do nothing

            RemoveUserFromGame(game.ActiveGameID, username);


        }

        public void RemoveUserFromGame(int gameId, string user)
        {
            PELGame game = AllGames.FirstOrDefault(x => x.ActiveGameID == gameId);
            if (game == null) return; //Game died, so dont bother trying to remove the user from that game.

            game.RemovePlayer(user);

           

            Logger.Log(user + " has left game " + gameId);

            if(user == game.HostUsername)
            {
                //Close the game
                CloseGame(game);
                return;
            }


            PlayerStateChangedPacket pscp = new PlayerStateChangedPacket();
            pscp.GameID = gameId;
            pscp.Username = user;
            pscp.State = PlayerStateChangedPacket.PlayerState.Left;

            PELServer.Broadcast(pscp);
        }


        public void CloseGame(PELGame game)
        {
            game.GameState = PELGame.PELGameState.Closed;

            GameStateChangePacket gscp = new GameStateChangePacket();
            gscp.GameID = game.ActiveGameID;
            gscp.GameState = PELGame.PELGameState.Closed;

            PELServer.Broadcast(gscp);

            GameList.Remove(game);

            Logger.Log(game.ActiveGameID + " closed");
        }

    }
}
