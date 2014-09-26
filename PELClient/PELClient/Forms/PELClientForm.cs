using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PELPackets;

namespace PELClient
{
    public partial class PELClientForm : Form
    {

        public List<PELPackets.PELGame> KnownGames = new List<PELPackets.PELGame>();

        public List<PELGameLocation> GameLocations = new List<PELGameLocation>();
        public List<PELGameMode> GameModes = new List<PELGameMode>();
        public List<PELGameType> GameTypes = new List<PELGameType>();

        public PELGame CurrentGame = null;
        public Forms.InGameForm InGameForm;


        public static PELClientForm Instance;

        public PELClientForm()
        {
            Instance = this;
            InitializeComponent();

            Application.Idle += Application_Idle;
            ConnectionClient.OnConnectionComplete += PacketHandlers.SuccessfulConnection;

        }

        public void ClearGames()
        {
            KnownGames.Clear();
            gameListBox.Items.Clear();
        }

        public void AddGame(PELPackets.PELGame game, bool announce = false)
        {
            KnownGames.Add(game);

            gameListBox.Items.Add(game);

            PELGameLocation location = GameLocations.FirstOrDefault(x => x.GameLocationID == game.GameLocation);
            PELGameType GameType = GameTypes.FirstOrDefault(x => x.GameTypeId == game.GameType);
            
            int players = game.MaxPlayers / 2;

            if(announce) ConnectionClient.SendChat("Server",  string.Format("{0} has started a new {1} vs {1} {2} at {3} on {4} (GameID: {5})", game.HostUsername, players, GameType.GameName, location.BaseName, location.Continent, game.ActiveGameID));
        }
    
        public void RemoveGame(int gameId)
        {
            PELGame game = KnownGames.FirstOrDefault(x => x.ActiveGameID == gameId);
            if (game == null) return;

            KnownGames.Remove(game);
            gameListBox.Items.Remove(game);

            ConnectionClient.SendChat("Server", game.HostUsername + "'s game has closed");
        }

        public void RefreshGame(PELGame game)
        {
            gameListBox.Items.Remove(game);
            gameListBox.Items.Add(game);
        }

        public void JoinGame(PELGame game)
        {
            CurrentGame = game;

            InGameForm = new Forms.InGameForm();
            InGameForm.Show();


        }

        public void RequestGameLeave()
        {
            RequestPacket req = new RequestPacket();
            req.Request = RequestPacket.RequestType.LeaveGame;
            req.Data = CurrentGame.ActiveGameID.ToString();

            ConnectionClient.WritePacket(req);
        }

        public void LeaveGame()
        {

            if (CurrentGame == null) return;

            CurrentGame = null;

            InGameForm.Close();
            InGameForm = null;

            
        }

        void Application_Idle(object sender, EventArgs e)
        {
            ConnectionClient.DoMainThreadActions();
        }

        private void PELClientForm_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            LoginForm login = new LoginForm();
            if(login.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Visible = true;
               
            }
        }

        private void PELClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConnectionClient.Kill();
        }

        private void PlayerList_Opening(object sender, CancelEventArgs e)
        {

        }

        private void hostGameButton_Click(object sender, EventArgs e)
        {
            PELClient.Forms.HostGameForm hostGame = new Forms.HostGameForm();

            hostGame.ShowDialog();
        }

        private void joinGameButton_Click(object sender, EventArgs e)
        {
            PELGame game = (gameListBox.SelectedItem as PELGame);

            if(game == null)
            {
                MessageBox.Show("Please select a game");
                return;
            }


            RequestPacket request = new RequestPacket();
            request.Request = RequestPacket.RequestType.JoinGame;
            request.Data = game.ActiveGameID.ToString();
            ConnectionClient.WritePacket(request);
        }
    }
}
