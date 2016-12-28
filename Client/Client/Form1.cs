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
using System.Text.RegularExpressions;

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
        private FolderBrowserDialog folderBrowserDialog1;
        private string chosenDownloadFolder;
        private OpenFileDialog openFileDialog1;

        String fileToReceive;
        String fileToSend;

        int NoOfPackets;
        int NoOfPacketsDownload;

        Thread thrReceive;
        Thread thrMessage;

        bool connected;

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
                    SendFile();
                }
                else
                {
                    monitor.AppendText(Environment.NewLine + "Please choose a file\n");
                }
            }
            catch
            {
            }
}

        private void SendFile()
        {
            SendMessage("file");            
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {

            username = nameUserTb.Text;
            Regex regex = new Regex("^([a-z][a-z0-9]+|[a-z]){1,25}$");

            if(regex.IsMatch(username))
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
                
                SendMessage("unme");
                handleMessage();


                byte[] bufferName = Encoding.Default.GetBytes(username);
                client.Send(bufferName);


                thrMessage = new Thread(new ThreadStart(receiveMessage));
                thrMessage.Start();
            }
            else
            {
                connectButton.Enabled = true;
                showNotificationBalloon("User name contains invalid characters");                
            }
        }

        public void showNotificationBalloon(String message)
        {
            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                Icon = System.Drawing.SystemIcons.Information,
                BalloonTipText = message,
            };

            // Display for 3 seconds.
            notification.ShowBalloonTip(3);
            notification.Dispose();
        }


        private void receiveMessage()
        {
            connected = true;
            while (connected)
            {
                handleMessage();
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
                    SendMessage("unam"); // now sending the real user name
                    break;
                case "usno":
                    //button
                    connected = false;
                    connectButton.Enabled = true;
                    nameUserTb.Enabled = true;
                    PortNumber.Enabled = true;
                    IP.Enabled = true;
                    client.Close();
                    monitor.AppendText("Username is already connected to the server.\n");
                    

                    break;
                case "down": // send request to download the file
                    break;
                
                case "dlco": // deletion of the of the file is okey
                    monitor.AppendText(tb_FileToDelete.Text + " is deleted\n");
                    break;
                case "fiok": // Server send ack to receive data 
                    SendMessage("fnme"); // Send info about sending file name                    
                    break ;
                case "fnok":
                    sendFileName();

                    break;
                case "fnco":
                    //SendMessage("data"); // send the real data of the file
                    SendMessage("pckn");
                   
                    
                    break;
                case "pcok":
                    sendNumberOfPackets();

                    break;

                case "pcco":// Server got number of packets in its hand. Now send the real data
                    SendMessage("data");
                    break;

                case "daok":
                    sendFileData();
                    break;
                case "dafi":
                    monitor.AppendText("Sent the file named " + fileToSend + "\n");
                    break;

                case "chok": // server ACK
                    sendFileNamesToRename();
                    break;

                case "fdne" : // file does not exist error message
                    showNotificationBalloon("File does not exist in the server. Check the file name");
                    break;
                case "":
                    showNotificationBalloon("New file name is invalid\n");
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
                case "dsok":
                    connected = false;
                    SendMessage("dico");
                   
                    break;
                case "dido":
                    client.Close();
                    monitor.AppendText("Disconnected from the server\n");
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
            String myDir = chosenDownloadFolder + "\\" + username;
            System.IO.Directory.CreateDirectory(myDir);
            Stream fileStream = File.OpenWrite(chosenDownloadFolder + "\\" + fileToReceive);
            monitor.AppendText(username + " is uploading a file named" + fileToReceive + "\n");

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
                    
                    fileStream.Write(buffer, 0, rec);
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
            if(tb_NewFileName.Text.Equals("") || tb_OldFileName.Text.Equals(""))
            {
                showNotificationBalloon("The file names must be entered");
            }
            else
            {
                SendMessage("chan");
            }
            
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (tb_FileToDelete.Text.Equals("") )
            {
                showNotificationBalloon("The file name must be entered");
            }else
            {
                SendMessage("dlte");
            }
            
        }

        private void btn_Download_Click(object sender, EventArgs e)
        {
            if(!tb_FileToDownload.Text.Equals(""))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult result = fbd.ShowDialog();
                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    chosenDownloadFolder = fbd.SelectedPath;
                }

                fileToReceive = downloadFile_tb.Text;
                SendMessage("down");
            }
            else
            {
                showNotificationBalloon("The file name must be entered");
            }       
        }

        private void btn_Disconnect_Click(object sender, EventArgs e)
        {
            //Ask user 
            connectButton.Enabled = true;
            nameUserTb.Enabled = true;
            PortNumber.Enabled = true;
            IP.Enabled = true;

            SendMessage("disc");
            
        }
    }
}