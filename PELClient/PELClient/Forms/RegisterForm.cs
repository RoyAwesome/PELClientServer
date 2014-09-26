using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PELClient
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void Error(string reason)
        {
            errorText.Text = reason;
        }

        private void Status(string status)
        {
            statusLabel.Text = status;
        }

        public string Username
        {
            get { return usernameBox.Text; }
        }

        public string Password
        {
            get { return passwordBox.Text; }
        }

        public string Email
        {
            get { return emailBox.Text; }
        }

        public string CharacterID
        {
            get { return characterIdBox.Text; }
        }
        public string BetaKey
        {
            get { return betaKeyBox.Text; }
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            //Validate the form

            if(usernameBox.Text == "")
            {
                Error("Please enter a Username");
                return;
            }
            if(passwordBox.Text == "")
            {
                Error("Please enter a Password");
                return;
            }

            if(confirmPasswordBox.Text == "")
            {
                Error("Please confirm your password");
                return;
            }
            if(passwordBox.Text != confirmPasswordBox.Text)
            {
                Error("Your passwords are different");
                return;
            }

            if(emailBox.Text == "" || !emailBox.Text.Contains("@") || !emailBox.Text.Contains("."))
            {
                Error("Please enter a valid email");
                return;
            }

            if(characterIdBox.Text == "")
            {
                Error("Please input your character ID");
                return;
            }
            /* TODO: Add a rules page
            if(rulesConfirmBox.Text != "Im not a shitter")
            {
                Error("Read the damn rules you mongrel");
                return;
            }
             * */

            if(betaKeyBox.Text != "")
            {
                Error("Please enter a beta key");
                return;
            }

            Status("Connecting to server");

            PELPackets.RegisterUserPacket rup = new PELPackets.RegisterUserPacket();

            rup.Username = Username;
            rup.Password = Password;
            rup.UserId = CharacterID;
            rup.BetaKey = BetaKey;
            rup.Email = Email;

            PELPackets.RegisterResponse resp = ConnectionClient.RegisterClient(rup);

            Status("Got Response");

            if(resp == null)
            {
                Error("Server connection failed!  Try again later");
                return;
            }
            if(resp.Response == PELPackets.RegisterResponse.ResponseType.Good)
            {
                //If we hit this point we are good
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            if (resp.Response == PELPackets.RegisterResponse.ResponseType.CharacterExists)
            {
                Error("That character already is in use");
                return;
            }
            if (resp.Response == PELPackets.RegisterResponse.ResponseType.UserExists)
            {
                Error("That username already exists");
            }
            if (resp.Response == PELPackets.RegisterResponse.ResponseType.Banned)
            {
                Error("You are banned.  Go away");
            }
            if(resp.Response == PELPackets.RegisterResponse.ResponseType.BadBetakey)
            {
                Error("That beta key is invalid");
            }

           
            
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }


    }
}
