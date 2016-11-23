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
            this.bt_StartServer = new System.Windows.Forms.Button();
            this.tb_portNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // serverMonitor
            // 
            this.serverMonitor.Location = new System.Drawing.Point(52, 70);
            this.serverMonitor.Name = "serverMonitor";
            this.serverMonitor.Size = new System.Drawing.Size(446, 311);
            this.serverMonitor.TabIndex = 0;
            this.serverMonitor.Text = "";
            // 
            // bt_StartServer
            // 
            this.bt_StartServer.Location = new System.Drawing.Point(543, 179);
            this.bt_StartServer.Name = "bt_StartServer";
            this.bt_StartServer.Size = new System.Drawing.Size(183, 23);
            this.bt_StartServer.TabIndex = 1;
            this.bt_StartServer.Text = "Start Server";
            this.bt_StartServer.UseVisualStyleBackColor = true;
            this.bt_StartServer.Click += new System.EventHandler(this.bt_StartServer_Click);
            // 
            // tb_portNumber
            // 
            this.tb_portNumber.Location = new System.Drawing.Point(543, 103);
            this.tb_portNumber.Name = "tb_portNumber";
            this.tb_portNumber.Size = new System.Drawing.Size(173, 22);
            this.tb_portNumber.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(540, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Port Number";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 455);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_portNumber);
            this.Controls.Add(this.bt_StartServer);
            this.Controls.Add(this.serverMonitor);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox serverMonitor;
        private System.Windows.Forms.Button bt_StartServer;
        private System.Windows.Forms.TextBox tb_portNumber;
        private System.Windows.Forms.Label label1;

        public class fileServer {




        }






    }
}

