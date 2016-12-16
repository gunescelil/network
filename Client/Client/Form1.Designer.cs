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
            this.btn_RequestFiles = new System.Windows.Forms.Button();
            this.btn_Download = new System.Windows.Forms.Button();
            this.btn_ChangeFileName = new System.Windows.Forms.Button();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.tb_OldFileName = new System.Windows.Forms.TextBox();
            this.tb_FileToDownload = new System.Windows.Forms.TextBox();
            this.tb_FileToDelete = new System.Windows.Forms.TextBox();
            this.tb_NewFileName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_ServerIp
            // 
            this.tb_ServerIp.Location = new System.Drawing.Point(31, 39);
            this.tb_ServerIp.Name = "tb_ServerIp";
            this.tb_ServerIp.Size = new System.Drawing.Size(298, 22);
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
            this.bt_SendFile.Location = new System.Drawing.Point(194, 127);
            this.bt_SendFile.Name = "bt_SendFile";
            this.bt_SendFile.Size = new System.Drawing.Size(135, 28);
            this.bt_SendFile.TabIndex = 4;
            this.bt_SendFile.Text = "Send File";
            this.bt_SendFile.UseVisualStyleBackColor = true;
            this.bt_SendFile.Click += new System.EventHandler(this.bt_SendFile_Click);
            // 
            // bt_Browse
            // 
            this.bt_Browse.Location = new System.Drawing.Point(31, 127);
            this.bt_Browse.Name = "bt_Browse";
            this.bt_Browse.Size = new System.Drawing.Size(136, 28);
            this.bt_Browse.TabIndex = 5;
            this.bt_Browse.Text = "Browse";
            this.bt_Browse.UseVisualStyleBackColor = true;
            this.bt_Browse.Click += new System.EventHandler(this.bt_Browse_Click);
            // 
            // tb_FileName
            // 
            this.tb_FileName.Location = new System.Drawing.Point(93, 170);
            this.tb_FileName.Name = "tb_FileName";
            this.tb_FileName.Size = new System.Drawing.Size(236, 22);
            this.tb_FileName.TabIndex = 6;
            // 
            // Files
            // 
            this.Files.AutoSize = true;
            this.Files.Location = new System.Drawing.Point(29, 170);
            this.Files.Name = "Files";
            this.Files.Size = new System.Drawing.Size(37, 17);
            this.Files.TabIndex = 7;
            this.Files.Text = "Files";
            // 
            // clientMonitor
            // 
            this.clientMonitor.Location = new System.Drawing.Point(448, 84);
            this.clientMonitor.Name = "clientMonitor";
            this.clientMonitor.Size = new System.Drawing.Size(379, 158);
            this.clientMonitor.TabIndex = 8;
            this.clientMonitor.Text = "";
            // 
            // tb_UserName
            // 
            this.tb_UserName.Location = new System.Drawing.Point(450, 39);
            this.tb_UserName.Name = "tb_UserName";
            this.tb_UserName.Size = new System.Drawing.Size(196, 22);
            this.tb_UserName.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(445, 19);
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
            this.label4.Location = new System.Drawing.Point(447, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Client Monitor";
            // 
            // btn_Disconnect
            // 
            this.btn_Disconnect.Location = new System.Drawing.Point(241, 84);
            this.btn_Disconnect.Name = "btn_Disconnect";
            this.btn_Disconnect.Size = new System.Drawing.Size(88, 23);
            this.btn_Disconnect.TabIndex = 13;
            this.btn_Disconnect.Text = "Disconnect";
            this.btn_Disconnect.UseVisualStyleBackColor = true;
            this.btn_Disconnect.Click += new System.EventHandler(this.btn_Disconnect_Click);
            // 
            // btn_RequestFiles
            // 
            this.btn_RequestFiles.Location = new System.Drawing.Point(32, 234);
            this.btn_RequestFiles.Name = "btn_RequestFiles";
            this.btn_RequestFiles.Size = new System.Drawing.Size(161, 28);
            this.btn_RequestFiles.TabIndex = 14;
            this.btn_RequestFiles.Text = "Request File List";
            this.btn_RequestFiles.UseVisualStyleBackColor = true;
            this.btn_RequestFiles.Click += new System.EventHandler(this.btn_RequestFiles_Click);
            // 
            // btn_Download
            // 
            this.btn_Download.Location = new System.Drawing.Point(31, 334);
            this.btn_Download.Name = "btn_Download";
            this.btn_Download.Size = new System.Drawing.Size(161, 28);
            this.btn_Download.TabIndex = 15;
            this.btn_Download.Text = "Download";
            this.btn_Download.UseVisualStyleBackColor = true;
            // 
            // btn_ChangeFileName
            // 
            this.btn_ChangeFileName.Location = new System.Drawing.Point(32, 283);
            this.btn_ChangeFileName.Name = "btn_ChangeFileName";
            this.btn_ChangeFileName.Size = new System.Drawing.Size(161, 28);
            this.btn_ChangeFileName.TabIndex = 16;
            this.btn_ChangeFileName.Text = "Change File Name";
            this.btn_ChangeFileName.UseVisualStyleBackColor = true;
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(32, 389);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(161, 28);
            this.btn_Delete.TabIndex = 17;
            this.btn_Delete.Text = "Delete File";
            this.btn_Delete.UseVisualStyleBackColor = true;
            // 
            // tb_OldFileName
            // 
            this.tb_OldFileName.Location = new System.Drawing.Point(224, 289);
            this.tb_OldFileName.Name = "tb_OldFileName";
            this.tb_OldFileName.Size = new System.Drawing.Size(194, 22);
            this.tb_OldFileName.TabIndex = 18;
            // 
            // tb_FileToDownload
            // 
            this.tb_FileToDownload.Location = new System.Drawing.Point(224, 340);
            this.tb_FileToDownload.Name = "tb_FileToDownload";
            this.tb_FileToDownload.Size = new System.Drawing.Size(194, 22);
            this.tb_FileToDownload.TabIndex = 19;
            // 
            // tb_FileToDelete
            // 
            this.tb_FileToDelete.Location = new System.Drawing.Point(224, 389);
            this.tb_FileToDelete.Name = "tb_FileToDelete";
            this.tb_FileToDelete.Size = new System.Drawing.Size(194, 22);
            this.tb_FileToDelete.TabIndex = 20;
            // 
            // tb_NewFileName
            // 
            this.tb_NewFileName.Location = new System.Drawing.Point(450, 289);
            this.tb_NewFileName.Name = "tb_NewFileName";
            this.tb_NewFileName.Size = new System.Drawing.Size(194, 22);
            this.tb_NewFileName.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(221, 258);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 17);
            this.label5.TabIndex = 22;
            this.label5.Text = "Old File Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(447, 258);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 17);
            this.label6.TabIndex = 23;
            this.label6.Text = "New File Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(221, 314);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 17);
            this.label7.TabIndex = 24;
            this.label7.Text = "File To Download";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(221, 365);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 17);
            this.label8.TabIndex = 25;
            this.label8.Text = "File To Delete";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 449);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_NewFileName);
            this.Controls.Add(this.tb_FileToDelete);
            this.Controls.Add(this.tb_FileToDownload);
            this.Controls.Add(this.tb_OldFileName);
            this.Controls.Add(this.btn_Delete);
            this.Controls.Add(this.btn_ChangeFileName);
            this.Controls.Add(this.btn_Download);
            this.Controls.Add(this.btn_RequestFiles);
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
        private System.Windows.Forms.Button btn_RequestFiles;
        private System.Windows.Forms.Button btn_Download;
        private System.Windows.Forms.Button btn_ChangeFileName;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.TextBox tb_OldFileName;
        private System.Windows.Forms.TextBox tb_FileToDownload;
        private System.Windows.Forms.TextBox tb_FileToDelete;
        private System.Windows.Forms.TextBox tb_NewFileName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}

