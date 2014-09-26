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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        private void Error(string reason)
        {
            errorLabel.Text = "Cannot Login.  Reason: " + reason;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            ConnectionClient.Username = usernameBox.Text;
            ConnectionClient.Password = passwordBox.Text;

            string resp = ConnectionClient.ConnectToServer();
            if(resp == "Good")
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                Error(resp);
            }
           
        }

        private void cancelbutton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterForm registerform = new RegisterForm();


            registerform.ShowDialog();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
