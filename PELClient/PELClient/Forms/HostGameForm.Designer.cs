namespace PELClient.Forms
{
    partial class HostGameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gameTypeBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gameLocationBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.maxPlayersBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.hostGameButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gameTypeBox
            // 
            this.gameTypeBox.FormattingEnabled = true;
            this.gameTypeBox.Location = new System.Drawing.Point(114, 93);
            this.gameTypeBox.Name = "gameTypeBox";
            this.gameTypeBox.Size = new System.Drawing.Size(158, 21);
            this.gameTypeBox.TabIndex = 0;
            this.gameTypeBox.SelectedValueChanged += new System.EventHandler(this.gameModeBox_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Game Type:";
            // 
            // gameLocationBox
            // 
            this.gameLocationBox.FormattingEnabled = true;
            this.gameLocationBox.Location = new System.Drawing.Point(114, 155);
            this.gameLocationBox.Name = "gameLocationBox";
            this.gameLocationBox.Size = new System.Drawing.Size(158, 21);
            this.gameLocationBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Game Location: ";
            // 
            // maxPlayersBox
            // 
            this.maxPlayersBox.FormattingEnabled = true;
            this.maxPlayersBox.Location = new System.Drawing.Point(114, 217);
            this.maxPlayersBox.Name = "maxPlayersBox";
            this.maxPlayersBox.Size = new System.Drawing.Size(158, 21);
            this.maxPlayersBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 220);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Max Players:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(114, 307);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(157, 20);
            this.textBox1.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 310);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Minimum MMR:";
            // 
            // hostGameButton
            // 
            this.hostGameButton.Location = new System.Drawing.Point(197, 417);
            this.hostGameButton.Name = "hostGameButton";
            this.hostGameButton.Size = new System.Drawing.Size(75, 23);
            this.hostGameButton.TabIndex = 8;
            this.hostGameButton.Text = "Host";
            this.hostGameButton.UseVisualStyleBackColor = true;
            this.hostGameButton.Click += new System.EventHandler(this.hostGameButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(15, 417);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // HostGameForm
            // 
            this.AcceptButton = this.hostGameButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(284, 452);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.hostGameButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.maxPlayersBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gameLocationBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gameTypeBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "HostGameForm";
            this.Text = "HostGameForm";
            this.Load += new System.EventHandler(this.HostGameForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox gameTypeBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox gameLocationBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox maxPlayersBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button hostGameButton;
        private System.Windows.Forms.Button cancelButton;
    }
}