using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        static bool listening = false;
        static bool terminating = false;
        static bool accept = true;

        private const int BufferSize = 1024;
        public string Status = string.Empty;
        Thread thrAccept;

        public static RichTextBox monitor;

        static Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static List<Socket> socketList = new List<Socket>();
        public Form1()
        {           
            InitializeComponent();
            monitor = (RichTextBox) serverMonitor;
            
        }

        private void bt_StartServer_Click(object sender, EventArgs e)
        {
            int serverPort;
            //this port will be used by clients to connect
            TextBox PortNumber = (TextBox) tb_portNumber;
            serverPort = Convert.ToInt32(PortNumber.Text);



            try
            {
                server.Bind(new IPEndPoint(IPAddress.Any, serverPort));
                monitor.AppendText(Environment.NewLine + "Started listening for incoming connections.");
                

                server.Listen(3); //the parameter here is maximum length of the pending connections queue
                thrAccept = new Thread(new ThreadStart(Accept));
                thrAccept.Start();
                listening = true;
               
            }
            catch
            {
                monitor.AppendText(Environment.NewLine + "Cannot create a server with the specified port number\n Check the port number and try again.");
                monitor.AppendText(Environment.NewLine + "terminating...");
                //Console.WriteLine("Cannot create a server with the specified port number\n Check the port number and try again.");
                //Console.Write("terminating...");
            }
        }

        static private void Accept()
        {
            while (accept)
            {
                try
                {
                    socketList.Add(server.Accept());
                    monitor.AppendText(Environment.NewLine + "New client connected...");
                    Thread thrReceive;
                    thrReceive = new Thread(new ThreadStart(Receive));
                    thrReceive.Start();
                }
                catch
                {
                    if (terminating)
                        accept = false;
                    else
                    //Console.Write("Listening socket has stopped working...\n");
                    { monitor.AppendText(Environment.NewLine + "Listening socket has stopped working..."); }
                }
            }
        }


        //this function is used in ThrReceive
        //very similar to the client version
        //displays the received message in the console.
        static private void Receive()
        {
            bool connected = true;
            Socket n = socketList[socketList.Count - 1];

            while (connected)
            {
                try
                {
                    Byte[] buffer = new byte[BufferSize];
                    int rec = n.Receive(buffer);

                    if (rec <= 0)
                    {
                        throw new SocketException();
                    }

                    string newmessage = Encoding.Default.GetString(buffer);
                    newmessage = newmessage.Substring(0, newmessage.IndexOf("\0"));
                    Console.Write("Client: " + newmessage + "\r\n");


                    
                }
                catch
                {
                    if (!terminating)
                        Console.Write("Client has disconnected...\n");
                    n.Close();
                    socketList.Remove(n);
                    connected = false;
                }
            }

        }

        public void ReceiveTCP(int portN)
        {
            TcpListener Listener = null;
            try
            {
                Listener = new TcpListener(IPAddress.Any, portN);
                Listener.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            byte[] RecData = new byte[BufferSize];
            int RecBytes;

            for (;;)
            {
                TcpClient client = null;
                NetworkStream netstream = null;
                Status = string.Empty;
                try
                {
                    string message = "Accept the Incoming File ";
                    string caption = "Incoming Connection";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    if (Listener.Pending())
                    {
                        client = Listener.AcceptTcpClient();
                        netstream = client.GetStream();
                        Status = "Connected to a client\n";
                        result = MessageBox.Show(message, caption, buttons);

                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            string SaveFileName = string.Empty;
                            SaveFileDialog DialogSave = new SaveFileDialog();
                            DialogSave.Filter = "All files (*.*)|*.*";
                            DialogSave.RestoreDirectory = true;
                            DialogSave.Title = "Where do you want to save the file?";
                            DialogSave.InitialDirectory = @"C:/";
                            if (DialogSave.ShowDialog() == DialogResult.OK)
                                SaveFileName = DialogSave.FileName;
                            if (SaveFileName != string.Empty)
                            {
                                int totalrecbytes = 0;
                                FileStream Fs = new FileStream (SaveFileName, FileMode.OpenOrCreate, FileAccess.Write);
                                while ((RecBytes = netstream.Read (RecData, 0, RecData.Length)) > 0)
                                {
                                    Fs.Write(RecData, 0, RecBytes);
                                    totalrecbytes += RecBytes;
                                }
                                Fs.Close();
                            }
                            netstream.Close();
                            client.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //netstream.Close();
                }
            }
        }
    }



    
}
