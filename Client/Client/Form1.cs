using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        //you dont have to declare the variables as static in the forms application
        static bool terminating = false;
        static Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        int serverPort;
        string serverIP;
        public static String fileName;
        public string SendingFilePath = string.Empty;
        public TextBox tb_File;
        public RichTextBox monitor;
        private const int BufferSize = 1024;

        public Form1()
        {
            InitializeComponent();
            tb_File = (TextBox)tb_FileName;
            monitor = (RichTextBox)clientMonitor;

        }

        private void bt_SendFile_Click(object sender, EventArgs e)
        {
            try {
                //this port will be used by clients to connect
                TextBox PortNumber = (TextBox)tb_ServerPort;
                serverPort = Convert.ToInt32(PortNumber.Text);

                TextBox IP = (TextBox)tb_ServerIp;
                serverIP = (IP.Text);

                client.Connect(serverIP, serverPort);
                
                fileName = tb_File.Text;
                if (fileName != "")
                {
                    SendFile(fileName);
                }
            }
            catch
            {

                monitor.AppendText(Environment.NewLine + "Cannot connected to the specified server\n");
                monitor.AppendText(Environment.NewLine + "terminating...");
                
            }


        }

        public void SendFile(String path)
        {
            byte[] SendingBuffer = null;
            FileStream Fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            try
            {
                int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(BufferSize)));
                int TotalLength = (int)Fs.Length;
                int CurrentPacketLength;
                int counter = 0;

                for (int i = 0; i< NoOfPackets; i++)
                {
                    if(TotalLength > BufferSize)
                    {
                        CurrentPacketLength = BufferSize;
                        TotalLength = TotalLength - CurrentPacketLength;
                    }
                    else
                    {
                        CurrentPacketLength = TotalLength;
                    }
                    SendingBuffer = new Byte[CurrentPacketLength];
                    Fs.Read(SendingBuffer, 0, CurrentPacketLength);
                    client.Send(SendingBuffer);
                }
                Fs.Close();
                
            }
            catch
            {

            }

        }


        static void SendMessage(string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message);

            //we can send a byte[] 
            client.Send(buffer);
            Console.Write("Your message has been sent.\n");
        }

        private void bt_Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dlg = new OpenFileDialog();
            Dlg.Filter = "All Files (*.*)|*.*";
            Dlg.CheckFileExists = true;
            Dlg.Title = "Choose a File";
            Dlg.InitialDirectory = @"C:\";
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                tb_File.Text = Dlg.FileName;
            }
        }
    }
}
