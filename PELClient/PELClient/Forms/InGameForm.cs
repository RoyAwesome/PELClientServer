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
    public partial class InGameForm : Form
    {
        public InGameForm()
        {
            InitializeComponent();
        }

        public PELGame CurrentGame;

        public PELGameLocation GameLocation;
        public PELGameType GameType;

        private void InGameForm_Load(object sender, EventArgs e)
        {
            CurrentGame = PELClientForm.Instance.CurrentGame;

            GameLocation = PELClientForm.Instance.GameLocations.FirstOrDefault(x => x.GameLocationID == CurrentGame.GameLocation);
            GameType = PELClientForm.Instance.GameTypes.FirstOrDefault(x => x.GameTypeId == CurrentGame.GameType);

            linkLabel1.Text = GameType.GameName;
            linkLabel2.Text = GameLocation.BaseName;


            RefreshPlayers();

        }

        public void RefreshPlayers()
        {
            playerListBox.Items.Clear();
            playerListBox.Items.AddRange(CurrentGame.Players);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(GameType.GameDescriptionURL);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(GameLocation.SetupURL);        
        }

        private void leaveGameButton_Click(object sender, EventArgs e)
        {
            PELClientForm.Instance.RequestGameLeave();

            this.Close();
        }

        private void InGameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                PELClientForm.Instance.RequestGameLeave();
                                
            }
        }
    }
}
