namespace PELClient
{
    partial class ChatMenu
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.chatTextBox = new System.Windows.Forms.RichTextBox();
            this.chatInput1 = new PELClient.ChatInput();
            this.playerList = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.whoisButton = new System.Windows.Forms.ToolStripMenuItem();
            this.statsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.reportButton = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.playerList);
            this.splitContainer1.Size = new System.Drawing.Size(942, 518);
            this.splitContainer1.SplitterDistance = 775;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.chatTextBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.chatInput1);
            this.splitContainer2.Size = new System.Drawing.Size(775, 518);
            this.splitContainer2.SplitterDistance = 489;
            this.splitContainer2.TabIndex = 0;
            // 
            // chatTextBox
            // 
            this.chatTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatTextBox.Location = new System.Drawing.Point(0, 0);
            this.chatTextBox.Name = "chatTextBox";
            this.chatTextBox.ReadOnly = true;
            this.chatTextBox.Size = new System.Drawing.Size(775, 489);
            this.chatTextBox.TabIndex = 0;
            this.chatTextBox.Text = "";
            // 
            // chatInput1
            // 
            this.chatInput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatInput1.Location = new System.Drawing.Point(0, 0);
            this.chatInput1.Name = "chatInput1";
            this.chatInput1.Size = new System.Drawing.Size(775, 25);
            this.chatInput1.TabIndex = 0;
            // 
            // playerList
            // 
            this.playerList.ContextMenuStrip = this.contextMenuStrip1;
            this.playerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playerList.FormattingEnabled = true;
            this.playerList.Location = new System.Drawing.Point(0, 0);
            this.playerList.Name = "playerList";
            this.playerList.Size = new System.Drawing.Size(163, 518);
            this.playerList.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.whoisButton,
            this.statsButton,
            this.reportButton});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(110, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // whoisButton
            // 
            this.whoisButton.Name = "whoisButton";
            this.whoisButton.Size = new System.Drawing.Size(152, 22);
            this.whoisButton.Text = "Whois";
            // 
            // statsButton
            // 
            this.statsButton.Name = "statsButton";
            this.statsButton.Size = new System.Drawing.Size(152, 22);
            this.statsButton.Text = "Stats";
            // 
            // reportButton
            // 
            this.reportButton.Name = "reportButton";
            this.reportButton.Size = new System.Drawing.Size(152, 22);
            this.reportButton.Text = "Report";
            // 
            // ChatMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ChatMenu";
            this.Size = new System.Drawing.Size(942, 518);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox playerList;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox chatTextBox;
        private ChatInput chatInput1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem whoisButton;
        private System.Windows.Forms.ToolStripMenuItem statsButton;
        private System.Windows.Forms.ToolStripMenuItem reportButton;
    }
}
