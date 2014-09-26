using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PELPackets
{
    public class PELGame
    {
        public enum PELGameState
        {
            Open,
            Pregame,
            Started,
            Postgame,
            Closing,
            Closed,
        }


        public int ActiveGameID;
        public string HostUsername;
        public int MaxPlayers;
        
        public int GameMode;
        public int GameType;
        public int GameLocation;

        public PELGameState GameState;
        public int RequiredMMR;

        public string[] Players;
        public string[] BannedPlayers;

        public override string ToString()
        {
            return string.Format("<{0}> Host: {1}  ({2}/{3})", ActiveGameID, HostUsername, Players.Length, MaxPlayers);
        }

        public void AddPlayer(string username)
        {
            string[] old = Players;
            Players = new string[old.Length + 1];
            Array.Copy(old, Players, old.Length);
            Players[Players.Length - 1] = username;
        }

        public void RemovePlayer(string username)
        {
            List<string> players = new List<string>(Players);
            players.Remove(username);
            Players = players.ToArray();            
        }
    }
}
