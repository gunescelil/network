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
using System.Threading;


namespace Client
{
    public partial class Form1 : Form
    {
        //you dont have to declare the variables as static in the forms application
        static bool terminating = false;
        static Socket client; //= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        int serverPort;
        string serverIP;
        public static String fileName;
        public string SendingFilePath = string.Empty;
        public TextBox tb_File;
        public static RichTextBox monitor;
        private const int BufferSize = 1024;
        public static String username;
        public TextBox nameUserTb;
        Button connectButton, disconnectButton;
        TextBox PortNumber;
        TextBox IP;


        Thread thrReceive;

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            tb_File = (TextBox)tb_FileName;
            monitor = (RichTextBox)clientMonitor;
            nameUserTb = (TextBox)tb_UserName;
            connectButton = (Button)connectBtn;
            disconnectButton = (Button)btn_Disconnect;
            PortNumber = (TextBox)tb_ServerPort;
            IP = (TextBox)tb_ServerIp;

        }

        static void SendMessage(string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message);

            //we can send a byte[] 
            client.Send(buffer);
            //Console.Write("Your message has been sent.\n");
        }

        private void bt_SendFile_Click(object sender, EventArgs e)
        {
            try
            {

                fileName = tb_File.Text;
                if (fileName != "")
                {
                    String[] filePathArray = fileName.Split('\\');
                    monitor.AppendText("Trying to send the file named " + filePathArray[filePathArray.Length - 1] + "\n");
                    SendFile(fileName);
                }
                else
                {
                    monitor.AppendText(Environment.NewLine + "Please choose a file\n");
                }
                
            }
            catch
            {

                //monitor.AppendText("Cannot connected to the specified server\n");
                //monitor.AppendText("terminating...\n");

            }



        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            //this port will be used by clients to connect
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverPort = Convert.ToInt32(PortNumber.Text);

            // Changing UI
            connectButton.Enabled = false;
            nameUserTb.Enabled = false;
            PortNumber.Enabled = false;
            IP.Enabled = false;


            serverIP = (IP.Text);
            try
            {
                client.Connect(serverIP, serverPort);
            }
            catch
            {
                monitor.AppendText("Something went wrong while connecting.\n");
            }


            thrReceive = new Thread(new ThreadStart(Receive));
            thrReceive.Start();
            username = nameUserTb.Text;

        }


        //this function will be used in the thread
        static private void Receive()
        {
            bool connected = true;
            //Console.WriteLine("Connected to the server.");
            byte[] bufferName = Encoding.Default.GetBytes(username);
            client.Send(bufferName);


            while (connected)
            {
                try
                {
                    byte[] buffer = new byte[64];

                    int rec = client.Receive(buffer);

                    if (rec <= 0)
                    {
                        throw new SocketException();
                    }

                    string newmessage = Encoding.Default.GetString(buffer);
                    newmessage = newmessage.Substring(0, newmessage.IndexOf("\0"));

                    if (newmessage.Equals("usernameExists"))
                    {
                        connected = false;
                        //button
                        
                        client.Close();
                        monitor.AppendText("Username is already connected to the server.\n");

                    }
                    else
                    {
                        monitor.AppendText("Connected to server\n");


                    }


                    //Console.Write("Server: " + newmessage + "\r\n");
                }
                catch
                {
                    if (!terminating)
                    {
                        monitor.AppendText("Connection has been terminated...\n");
                        //Console.Write("Connection has been terminated...\n");

                    }
                    connected = false;

                }
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


                String[] nameArray = fileName.Split('\\');
                String fileToSend = nameArray[nameArray.Length - 1];

                monitor.AppendText("Started sending the file named " + fileToSend + "\n");

                SendingBuffer = Encoding.Default.GetBytes(fileToSend);
                client.Send(SendingBuffer);

                SendingBuffer = Encoding.Default.GetBytes(NoOfPackets.ToString());
                client.Send(SendingBuffer);

                for (int i = 0; i < NoOfPackets; i++)
                {
                    if (TotalLength > BufferSize)
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

                monitor.AppendText("Sent the file named " + fileToSend + "\n");
                Fs.Close();

            }
            catch
            {

            }

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

        private void btn_Disconnect_Click(object sender, EventArgs e)
        {
            connectButton.Enabled = true;
            nameUserTb.Enabled = true;
            PortNumber.Enabled = true;
            IP.Enabled = true;

            SendMessage("close");

            client.Close();
            monitor.AppendText(Environment.NewLine + "Disconnected from server " + "");
        }
    }
}