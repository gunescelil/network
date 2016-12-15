using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Threading;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {

        static Socket possibleClient;

        static bool listening = false;
        static bool terminating = false;
        static bool accept = true;

        private const int BufferSize = 1024;
        public string Status = string.Empty;
        Thread thrAccept;
        Thread thrMessage;

        public static RichTextBox monitor;

        static Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static List<Socket> socketList = new List<Socket>();
        static List<string> nameList = new List<string>();
        Button serveringButton;

        public static string userNameString;
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            monitor = (RichTextBox)serverMonitor;
            serveringButton = (Button)btStartServer;

        }

        private void bt_StartServer_Click(object sender, EventArgs e)
        {
            int serverPort;
            //this port will be used by clients to connect
            TextBox PortNumber = (TextBox)tb_portNumber;
            serverPort = Convert.ToInt32(PortNumber.Text);
            serveringButton.Enabled = false;


            try
            {
                server.Bind(new IPEndPoint(IPAddress.Any, serverPort));
                monitor.AppendText("Started listening for incoming connections.\n");


                server.Listen(3); //the parameter here is maximum length of the pending connections queue
                thrAccept = new Thread(new ThreadStart(Accept));
                thrAccept.Start();
                listening = true;

            }
            catch
            {
                monitor.AppendText("Cannot create a server with the specified port number\n Check the port number and try again.\n");
                monitor.AppendText("terminating...\n");
                //Console.WriteLine("Cannot create a server with the specified port number\n Check the port number and try again.");
                //Console.Write("terminating...");
            }
        }

        private void Accept()
        {
            while (accept)
            {
                try
                {
                    possibleClient = server.Accept();
                    if (TakeUserNameExists(possibleClient))
                    {
                        byte[] buffer = Encoding.ASCII.GetBytes("usernameExists");
                        possibleClient.Send(buffer);
                        possibleClient.Close();
                    }
                    else
                    {
                        byte[] buffer = Encoding.ASCII.GetBytes("Welcome");
                        possibleClient.Send(buffer);

                        Thread thrReceive;
                        thrReceive = new Thread(new ThreadStart(Receive));
                        thrReceive.Start();
                        //Thread thrMessage;
                        //thrMessage = new Thread(new ThreadStart(ReceiveMessage));
                        //thrMessage.Start();
                    }
                }
                catch
                {
                    possibleClient.Close();
                    if (terminating)
                        accept = false;
                    else
                    //Console.Write("Listening socket has stopped working...\n");
                    { monitor.AppendText("Listening socket has stopped working...\n"); }
                }
            }
        }


        private bool TakeUserNameExists(Socket possibleClient)
        {
            byte[] userNameBuffer = new byte[64];
            int recCharNum = possibleClient.Receive(userNameBuffer);
            userNameString = System.Text.Encoding.UTF8.GetString(userNameBuffer, 0, recCharNum);
            monitor.AppendText("A user named " + userNameString + " is trying to connect\n");
            for (int i = 0; i < nameList.Count; i++)
            {
                if (userNameString.Equals(nameList[i]))
                {
                    monitor.AppendText("User " + userNameString + " is already connected\n");
                    return true;
                }
            }

            monitor.AppendText("User " + userNameString + " is connected\n");
            nameList.Add(userNameString);
            socketList.Add(possibleClient);

            return false;
        }

        /* This function is to receive text messages from client.
         * But it is not used and implemented
         * */
        static private void ReceiveMessage()
        {
            bool connected = true;
            Socket n = socketList[socketList.Count - 1];

            while (connected)
            {
                try
                {
                    Byte[] buffer = new byte[64];
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




        //this function is used in ThrReceive
        //very similar to the client version
        //displays the received message in the console.
        static private void Receive()
        {

            bool connected = true;
            while (connected)
            {
                Socket n = socketList[socketList.Count - 1];
                byte[] namebuffer = new byte[64];
                int receivedNameCharCount = n.Receive(namebuffer);
                if (System.Text.Encoding.UTF8.GetString(namebuffer, 0, receivedNameCharCount).Equals("close"))
                {
                    connected = false;
                    monitor.AppendText("kapa");
                    socketList.Remove(n);
                    n.Close();
                }
                else
                {
                    // String filename = Convert.ToString(namebuffer);
                    String filename = System.Text.Encoding.UTF8.GetString(namebuffer, 0, receivedNameCharCount);
                    //filename.Substring(0, filename.IndexOf('0'));

                    byte[] countBuffer = new byte[64];
                    int numberOfPacketsByteNumber = n.Receive(countBuffer); // The number of packets
                    String numberOfPacketsString = System.Text.Encoding.UTF8.GetString(countBuffer, 0, numberOfPacketsByteNumber);
                    int packetNumberToReceive = Int32.Parse(numberOfPacketsString);


                    //String myDir = "C:\\Users\\asus\\Desktop\\Server\\" + nameList[nameList.Count-1];
                    String myDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Server\\" + nameList[nameList.Count - 1];
                    System.IO.Directory.CreateDirectory(myDir);
                    Stream fileStream = File.OpenWrite(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Server\\" + userNameString + "\\" + filename);

                    monitor.AppendText(userNameString + " is uploading a file named" + filename + "\n");

                    // TODO: 

                    int i = 0;
                    while (i < packetNumberToReceive)
                    {
                        try
                        {


                            Byte[] buffer = new byte[BufferSize];
                            int rec = n.Receive(buffer);

                            if (rec <= 0)
                            {
                                throw new SocketException();
                                monitor.AppendText("Something went wrong while reading the buffer\n");
                            }

                            //string newmessage = Encoding.Default.GetString(buffer);
                            //newmessage = newmessage.Substring(0, newmessage.IndexOf("\0"));
                            fileStream.Write(buffer, 0, rec);
                            //Console.Write("Client: " + newmessage + "\r\n");



                        }
                        catch
                        {
                            if (!terminating)
                            {
                                Console.Write("Client has disconnected...\n");
                            }
                            n.Close();
                            socketList.Remove(n);
                            connected = false;


                        }
                        i++;
                    }
                    monitor.AppendText(userNameString + " finished uploading " + filename + "\n");
                    fileStream.Close();


                }
            }
        }


    }

}




