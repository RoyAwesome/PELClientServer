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

namespace PELClient.Forms
{
    public partial class HostGameForm : Form
    {
        public HostGameForm()
        {
            InitializeComponent();
        }

        private void HostGameForm_Load(object sender, EventArgs e)
        {
            gameTypeBox.Items.AddRange(PELClientForm.Instance.GameTypes.ToArray());
            gameTypeBox.SelectedIndex = 0;

        }

        private void gameModeBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //Vind a list of game types and player counts for that game type

            PELGameType gametype = gameTypeBox.SelectedItem as PELGameType;


            IEnumerable<PELGameLocation> locations = PELClientForm.Instance.GameLocations.Where(x => gametype.Locations.Contains(x.GameLocationID));
            gameLocationBox.Items.AddRange(locations.ToArray());
            gameLocationBox.SelectedIndex = 0;

            maxPlayersBox.Items.AddRange(gametype.MaxPlayers.Select(x => x.ToString()).ToArray());
            maxPlayersBox.SelectedIndex = 0;

            
        }

        private void hostGameButton_Click(object sender, EventArgs e)
        {
            PELGame game = new PELGame();
            game.HostUsername = ConnectionClient.Username;
            game.GameMode = 1;

            game.GameLocation = (gameLocationBox.SelectedItem as PELGameLocation).GameLocationID;
            game.GameType = (gameTypeBox.SelectedItem as PELGameType).GameTypeId;

            game.MaxPlayers = (int.Parse(maxPlayersBox.SelectedItem.ToString()));

            game.RequiredMMR = 0;

            

            game.BannedPlayers = new string[0];

            RequestNewGamePacket rngp = new RequestNewGamePacket();
            rngp.RequestedGame = game;

            ConnectionClient.WritePacket(rngp);

            Close();

        }
    }
}
