namespace Server
{
    partial class Form1
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
            this.serverMonitor = new System.Windows.Forms.RichTextBox();
            this.tb_portNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btStartServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // serverMonitor
            // 
            this.serverMonitor.Location = new System.Drawing.Point(42, 113);
            this.serverMonitor.Name = "serverMonitor";
            this.serverMonitor.Size = new System.Drawing.Size(440, 211);
            this.serverMonitor.TabIndex = 0;
            this.serverMonitor.Text = "";
            // 
            // tb_portNumber
            // 
            this.tb_portNumber.Location = new System.Drawing.Point(539, 143);
            this.tb_portNumber.Name = "tb_portNumber";
            this.tb_portNumber.Size = new System.Drawing.Size(100, 22);
            this.tb_portNumber.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(536, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port Number";
            // 
            // btStartServer
            // 
            this.btStartServer.Location = new System.Drawing.Point(539, 231);
            this.btStartServer.Name = "btStartServer";
            this.btStartServer.Size = new System.Drawing.Size(176, 23);
            this.btStartServer.TabIndex = 3;
            this.btStartServer.Text = "Start Server";
            this.btStartServer.UseVisualStyleBackColor = true;
            this.btStartServer.Click += new System.EventHandler(this.bt_StartServer_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 455);
            this.Controls.Add(this.btStartServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_portNumber);
            this.Controls.Add(this.serverMonitor);
            this.Name = "Form1";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox serverMonitor;
        private System.Windows.Forms.TextBox tb_portNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btStartServer;
    }
}

