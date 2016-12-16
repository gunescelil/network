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
        TextBox oldFile_tb;
        TextBox newFile_tb;
        TextBox deleteFile_tb;
        TextBox downloadFile_tb;

        String fileToSend;

        int NoOfPackets;
        int NoOfPacketsDownload;

        Thread thrReceive;
        Thread thrMessage;

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
            oldFile_tb = (TextBox)tb_OldFileName;
            newFile_tb = (TextBox)tb_NewFileName;
            downloadFile_tb = (TextBox)tb_FileToDownload;
            deleteFile_tb = (TextBox)tb_FileToDelete;


        }

        static void SendMessage(string message)
        {
            byte[] buffer = Encoding.Default.GetBytes(message);
            client.Send(buffer);
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
                    //SendFile(fileName);
                    SendFile();
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

        private void SendFile()
        {
            SendMessage("file");            
            handleMessage();
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




            //thrReceive = new Thread(new ThreadStart(Receive));
            //thrReceive.Start();

            SendMessage("unme");
            handleMessage();

            username = nameUserTb.Text;
            byte[] bufferName = Encoding.Default.GetBytes(username);
            client.Send(bufferName);


            thrMessage = new Thread(new ThreadStart(receiveMessage));
            thrMessage.Start();

        }


        private void receiveMessage()
        {
            bool connected = true;
            while (connected)
            {
                handleMessage();
            }
        }

        static private void receiveFile()
        {

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


                SendingBuffer = Encoding.Default.GetBytes("PNP" + username + "\\" + fileToSend + "\\" + NoOfPackets.ToString() + "\\");
                client.Send(SendingBuffer);

                // String num_packets = "PN" + NoOfPackets.ToString();
                // SendingBuffer = Encoding.Default.GetBytes(num_packets);
                // client.Send(SendingBuffer);

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

        private void btn_RequestFiles_Click(object sender, EventArgs e)
        {
            SendMessage("list");           
        }

         public void handleMessage()
        {
            byte[] response = new byte[4];
            client.Receive(response);
            string message = Encoding.Default.GetString(response);

            message = message.ToLower();
            switch(message)
            {
                case "liok": // The server is sending the file list
                    SendMessage("lico");
                    receiveFileList();
                    break;
                case "welc":
                    monitor.AppendText("Connected to server\n");
                    break;

                case "unok":
                    monitor.AppendText("Server wants my name\n");
                    SendMessage("unam"); // now sending the real user name
                    break;
                case "usno":
                    //button
                    client.Close();
                    monitor.AppendText("Username is already connected to the server.\n");
                    break;
                case "down": // send request to download the file
                    break;
                
                case "dlte": // send request to delete the file
                    break;
                case "fiok": // Server send ack to receive data 
                    SendMessage("fnme"); // Send info to of sending file name
                    handleMessage();
                    sendFileName();
                    handleMessage(); // Wait ack message
                    break ;
                case "fnok":
                    sendFileName();
                    handleMessage();
                    break;
                case "fnco":
                    //SendMessage("data"); // send the real data of the file
                    SendMessage("pckn");
                    handleMessage();
                    //sendNumberOfPackets();
                    //sendFileData();
                    //handleMessage();
                    
                    break;
                case "pcok":
                    sendNumberOfPackets();
                    handleMessage();
                    //SendMessage("data");
                    //handleMessage();
                    break;

                case "pcco":// Server got number of packets in its hand. Now send the real data
                    SendMessage("data");
                    handleMessage();
                    break;

                case "daok":
                    SendFile();
                    handleMessage();
                    break;

                case "chok": // server ACK
                    sendFileNamesToRename();
                    break;

                case "deok":
                    sendFileNameToDelete();
                    break;
                case "dook":
                    SendMessage("doco");
                   
                    break;
                case "refn":
                    sendFileNameToDownload();
                    break;
                case "dcom":
                    SendMessage("dcok");
                    
                        break;
                case "pckn": // packet numarasını alıyorum diyoruz
                    SendMessage("nuok");
                    NoOfPacketsDownload = rececivePacketNumber();
                    SendMessage("nudo");
                    break;
                case "fida":
                    SendMessage("fdok"); // anladım diyorum
                    downloadFileData();
                    //SendMessage("fcom");
                    break;



            }
        }

        public int rececivePacketNumber()
        {
            //Socket s = u.getSocket();
            byte[] countBuffer = new byte[64];
            int numberOfPacketsByteNumber = client.Receive(countBuffer); // The number of packets
            String numberOfPacketsString = Encoding.Default.GetString(countBuffer, 0, numberOfPacketsByteNumber);

            int packetNumberToReceive = Int32.Parse(numberOfPacketsString);
            return packetNumberToReceive;
        }
        


        public void downloadFileData()
        {

            int packetNumber = NoOfPacketsDownload;
            String myDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + username;
            System.IO.Directory.CreateDirectory(myDir);
            Stream fileStream = File.OpenWrite(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\"+username + "\\" + downloadFile_tb.Text);
            monitor.AppendText(username + " is uploading a file named" + downloadFile_tb.Text + "\n");

            for (int i = 0; i < packetNumber; i++)
            {
                try
                {
                    Byte[] buffer = new byte[BufferSize];
                    int rec = client.Receive(buffer);

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
                    //client.Close(); 
                }

            }
            monitor.AppendText(username + " finished uploading " + downloadFile_tb.Text + "\n");
            fileStream.Close();
        }

        private void sendFileNameToDownload()
        {
            String fileName = downloadFile_tb.Text;
            

            monitor.AppendText("Requested the file named " + fileName + "\n");

            byte[] SendingBuffer = Encoding.Default.GetBytes(fileName);
            client.Send(SendingBuffer);
        }


        public void sendFileNameToDelete()
        {
            String deleteFile = deleteFile_tb.Text;
            byte[] SendingBuffer = Encoding.Default.GetBytes(deleteFile);
            client.Send(SendingBuffer);
        }

        private void sendFileNamesToRename()
        {
            String oldFile = oldFile_tb.Text;
            String newFile = newFile_tb.Text;
            StringBuilder sb = new StringBuilder();
            sb.Append(oldFile + "*"+ newFile);
            
            monitor.AppendText("Started renaming the file named " + oldFile +" to " + newFile +"\n");

            byte[] SendingBuffer = Encoding.Default.GetBytes(sb.ToString());
            client.Send(SendingBuffer);
        }

        public void sendFileData()
        {
            String filePath = tb_File.Text;
            byte[] SendingBuffer = null;
            FileStream Fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            try
            {
                NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(BufferSize)));
                int TotalLength = (int)Fs.Length;
                int CurrentPacketLength;

                String[] nameArray = fileName.Split('\\');
                String fileToSend = nameArray[nameArray.Length - 1];
                monitor.AppendText("Started sending the file named " + fileToSend + "\n");

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

        private void sendNumberOfPackets()
        {
            String filePath = tb_File.Text;
            byte[] SendingBuffer = null;
            FileStream Fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(BufferSize)));

            String num_packets = NoOfPackets.ToString();
            SendingBuffer = Encoding.Default.GetBytes(num_packets);
            client.Send(SendingBuffer);

        }

        private  void sendFileName()
        {
            String[] nameArray = fileName.Split('\\');
            fileToSend = nameArray[nameArray.Length - 1];

            monitor.AppendText("Started sending the file named " + fileToSend + "\n");
            
            byte [] SendingBuffer = Encoding.Default.GetBytes(fileToSend);
            client.Send(SendingBuffer);
        }

        static public void receiveFileList()
        {
            byte[] buffer = new byte[20000];
            int receivedSize = client.Receive(buffer);// the buffer has now the byte array of string array

            
            String s = Encoding.Default.GetString(buffer, 0, receivedSize);
            String[] files = s.Split('*');
            monitor.AppendText("Your files are below\n");
            foreach(string str in files)
            {
                monitor.AppendText(str + "\n");
            }

            
        }

        private void btn_ChangeFileName_Click(object sender, EventArgs e)
        {
            
            SendMessage("chan");
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            SendMessage("dlte");
        }

        private void btn_Download_Click(object sender, EventArgs e)
        {
            SendMessage("down");
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