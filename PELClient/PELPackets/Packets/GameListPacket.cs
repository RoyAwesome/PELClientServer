using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PELPackets
{
    public class RequestNewGamePacket : PELPacket
    {
        public PELGame RequestedGame;
    }

    public class GameListPacket : PELPacket
    {

        public PELGame[] Games;

    }

    public class NewGamePacket : PELPacket
    {
        public PELGame Game;
    }


    public class PlayerStateChangedPacket : PELPacket
    {
        public enum PlayerState
        {
            Joined,
            Left,
            Kicked,
            Denied,
        }

        public int GameID;
        public string Username;
        public PlayerState State;
        public string Reason;
    }

    public class GameStateChangePacket : PELPacket
    {
        public int GameID;
        public PELGame.PELGameState GameState;
    }
}
