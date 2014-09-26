using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PELClient
{
    public partial class ChatMenu : UserControl
    {
        public ChatMenu()
        {
            InitializeComponent();
            chatInput1.sendButton.Click += sendButton_Click;
        }

        void sendButton_Click(object sender, EventArgs e)
        {
            PELPackets.SendChatPacket scp = new PELPackets.SendChatPacket();
            scp.Message = chatInput1.chatBox.Text;
            scp.Username = ConnectionClient.Username;
            scp.when = DateTime.Now;

            ConnectionClient.WritePacket(scp);

            chatInput1.chatBox.Text = "";
        }

        public void AddUser(string username)
        {
            this.playerList.Items.Add(username);
        }

        public void RemoveUser(string username)
        {
            this.playerList.Items.Remove(username);
        }

        public void AddChatMessage(DateTime when, string user, string message)
        {
            chatTextBox.Text += "\n" + string.Format("<{0}> {1}", user, message);
        }


        private void chatInput1_Load(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
