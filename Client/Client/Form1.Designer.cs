namespace Client
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
            this.components = new System.ComponentModel.Container();
            this.tb_ServerIp = new System.Windows.Forms.TextBox();
            this.tb_ServerPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bt_SendFile = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.bt_Browse = new System.Windows.Forms.Button();
            this.tb_FileName = new System.Windows.Forms.TextBox();
            this.Files = new System.Windows.Forms.Label();
            this.clientMonitor = new System.Windows.Forms.RichTextBox();
            this.tb_UserName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.connectBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_Disconnect = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_ServerIp
            // 
            this.tb_ServerIp.Location = new System.Drawing.Point(31, 39);
            this.tb_ServerIp.Name = "tb_ServerIp";
            this.tb_ServerIp.Size = new System.Drawing.Size(310, 22);
            this.tb_ServerIp.TabIndex = 0;
            // 
            // tb_ServerPort
            // 
            this.tb_ServerPort.Location = new System.Drawing.Point(31, 84);
            this.tb_ServerPort.Name = "tb_ServerPort";
            this.tb_ServerPort.Size = new System.Drawing.Size(100, 22);
            this.tb_ServerPort.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server\'s IP Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port";
            // 
            // bt_SendFile
            // 
            this.bt_SendFile.Location = new System.Drawing.Point(31, 134);
            this.bt_SendFile.Name = "bt_SendFile";
            this.bt_SendFile.Size = new System.Drawing.Size(161, 28);
            this.bt_SendFile.TabIndex = 4;
            this.bt_SendFile.Text = "Send File";
            this.bt_SendFile.UseVisualStyleBackColor = true;
            this.bt_SendFile.Click += new System.EventHandler(this.bt_SendFile_Click);
            // 
            // bt_Browse
            // 
            this.bt_Browse.Location = new System.Drawing.Point(31, 183);
            this.bt_Browse.Name = "bt_Browse";
            this.bt_Browse.Size = new System.Drawing.Size(161, 28);
            this.bt_Browse.TabIndex = 5;
            this.bt_Browse.Text = "Browse";
            this.bt_Browse.UseVisualStyleBackColor = true;
            this.bt_Browse.Click += new System.EventHandler(this.bt_Browse_Click);
            // 
            // tb_FileName
            // 
            this.tb_FileName.Location = new System.Drawing.Point(92, 223);
            this.tb_FileName.Name = "tb_FileName";
            this.tb_FileName.Size = new System.Drawing.Size(236, 22);
            this.tb_FileName.TabIndex = 6;
            // 
            // Files
            // 
            this.Files.AutoSize = true;
            this.Files.Location = new System.Drawing.Point(28, 223);
            this.Files.Name = "Files";
            this.Files.Size = new System.Drawing.Size(37, 17);
            this.Files.TabIndex = 7;
            this.Files.Text = "Files";
            // 
            // clientMonitor
            // 
            this.clientMonitor.Location = new System.Drawing.Point(369, 104);
            this.clientMonitor.Name = "clientMonitor";
            this.clientMonitor.Size = new System.Drawing.Size(379, 158);
            this.clientMonitor.TabIndex = 8;
            this.clientMonitor.Text = "";
            // 
            // tb_UserName
            // 
            this.tb_UserName.Location = new System.Drawing.Point(369, 39);
            this.tb_UserName.Name = "tb_UserName";
            this.tb_UserName.Size = new System.Drawing.Size(196, 22);
            this.tb_UserName.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(366, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Client User Name";
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(150, 84);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(75, 23);
            this.connectBtn.TabIndex = 11;
            this.connectBtn.Text = "Connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(366, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Client Monitor";
            // 
            // btn_Disconnect
            // 
            this.btn_Disconnect.Location = new System.Drawing.Point(241, 84);
            this.btn_Disconnect.Name = "btn_Disconnect";
            this.btn_Disconnect.Size = new System.Drawing.Size(100, 23);
            this.btn_Disconnect.TabIndex = 13;
            this.btn_Disconnect.Text = "Disconnect";
            this.btn_Disconnect.UseVisualStyleBackColor = true;
            this.btn_Disconnect.Click += new System.EventHandler(this.btn_Disconnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 304);
            this.Controls.Add(this.btn_Disconnect);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.connectBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_UserName);
            this.Controls.Add(this.clientMonitor);
            this.Controls.Add(this.Files);
            this.Controls.Add(this.tb_FileName);
            this.Controls.Add(this.bt_Browse);
            this.Controls.Add(this.bt_SendFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_ServerPort);
            this.Controls.Add(this.tb_ServerIp);
            this.Name = "Form1";
            this.Text = "Client";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_ServerIp;
        private System.Windows.Forms.TextBox tb_ServerPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bt_SendFile;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button bt_Browse;
        private System.Windows.Forms.TextBox tb_FileName;
        private System.Windows.Forms.Label Files;
        private System.Windows.Forms.RichTextBox clientMonitor;
        private System.Windows.Forms.TextBox tb_UserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_Disconnect;
    }
}

