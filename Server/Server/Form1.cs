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
using UserNamespace;

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
        static List<User> userList = new List<User>();
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
                    /*if (TakeUserNameExists(possibleClient))
                    {
                        byte[] buffer = Encoding.ASCII.GetBytes("usno");
                        possibleClient.Send(buffer);
                        possibleClient.Close();
                    }
                    else
                    {
                        byte[] buffer = Encoding.ASCII.GetBytes("welc");
                        possibleClient.Send(buffer);

                       // Thread thrReceive;
                        //thrReceive = new Thread(new ThreadStart(Receive));
                        //thrReceive.Start();

                        int managedThreadId = Thread.CurrentThread.ManagedThreadId;
                        Console.WriteLine("ManagedThreadId = " + managedThreadId);
                        monitor.AppendText("ManagedThreadId = " + managedThreadId);*/
                        Thread thrMessage;
                        //thrMessage = new Thread(new ThreadStart( ReceiveMessage ));                
                        thrMessage = new Thread(() => ReceiveMessage(possibleClient));
                        thrMessage.Start();
                    //}
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

        public User getUser(Socket s)
        {
            for(int i = 0; i< userList.Count; i++)
            {
                if (s == userList[i].getSocket())
                {
                    return userList[i];
                }
            }
            return null;
        }

        public bool checkUserNameExists(User u)
        {
            bool result = false;
            for(int i =0; i< userList.Count;i++)
            {
                if (userList[i].getUserName().Equals(u.getUserName()))
                    result = true;
            }
            return result;
        }


        /* This function is to receive text messages from client.
         * But it is not used and implemented
         * */
        private void ReceiveMessage(Socket s)
        {
            bool connected = true;
            Socket n = s;
            
            byte[] namebuffer = new byte[64];
            int receivedNameCharCount = n.Receive(namebuffer);
            String firstMessageFromClient = Encoding.Default.GetString(namebuffer);
            firstMessageFromClient = firstMessageFromClient.Substring(0, firstMessageFromClient.IndexOf("\0"));

            User possibleUser = new User("", n);
            handleMessage(firstMessageFromClient, possibleUser);
            
            User currentUser = possibleUser;
            
            while (connected)
            {
                try
                {
                    Byte[] buffer = new byte[4];
                    int rec = currentUser.getSocket().Receive(buffer);

                    if (rec <= 0)
                    {
                        throw new SocketException();
                    }

                    string newmessage = Encoding.Default.GetString(buffer);
                    //newmessage = newmessage.Substring(0, newmessage.IndexOf("\0"));
                    handleMessage(newmessage, currentUser);

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

        public string receiveFileName(User u)
        {
            Socket s = u.getSocket();
            byte[] fileNameArray = new byte[64];
            int receivedCount = s.Receive(fileNameArray);
            String fileName = System.Text.Encoding.UTF8.GetString(fileNameArray, 0, receivedCount);
            return fileName;
        }

        public void receiveUserName(User u)
        {
            byte[] namebuffer= new byte[64];
            int receivedNameCharCount = u.getSocket().Receive(namebuffer);
            String possibleUserName = Encoding.Default.GetString(namebuffer,0, receivedNameCharCount);
            u.setUserName(possibleUserName);
           
            if (checkUserNameExists(u)) // User name exists
            {
                byte[] buffer = Encoding.ASCII.GetBytes("usno");
                monitor.AppendText("A client named " + u.getUserName() + " is rejected to connect \n");
                u.getSocket().Send(buffer);
                u.getSocket().Close();
                
            }
            else if (!checkUserNameExists(u)) // User name does not exist
            {
                byte[] buffer = Encoding.ASCII.GetBytes("welc");
                monitor.AppendText("A client named " + u.getUserName() + " is connected \n");
                u.getSocket().Send(buffer);
                userList.Add(u);
                socketList.Add(u.getSocket());
                nameList.Add(u.getUserName());
            }
        }


        public void handleMessage(String message, User u )
        {
            message = message.ToLower();
            switch (message)
            {
                case "unme": // Some client is sending the username
                    sendMessage("unok",u);
                    break;               
                case "unam":
                    receiveUserName(u);
                    break;
               
                case "file": // client will send the file
                    sendMessage("fiok",u);
                    break;
                case "fnme": //  client will send the file name
                    sendMessage("fnok",u);
                    String fileName = receiveFileName(u);
                    monitor.AppendText("User " + u.getUserName() + " is sending a file named " + fileName + "\n");
                    u.setFileNameToReceive(fileName);                  
                    sendMessage("fnco",u);
                    break;
                case "pckn":
                    sendMessage("pcok",u);// ACK to understand packet numbers
                    int packetNumber = receivePacketNumber(u);
                    u.setPacketNumber(packetNumber);
                    sendMessage("pcco",u);
                    break;

                case "data":
                    sendMessage("daok",u);
                    receiveFileData(u);
                    sendMessage("dafi",u);
                    break;


                case "list": // requested the list of files
                    sendMessage("liok", u);                    
                    
                    break;
                case "lico":
                    String username = u.getUserName();
                    monitor.AppendText(username);
                    String[] fileArray = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Server\\" + username);
                    StringBuilder sb = new StringBuilder();
                    for(int i = 0; i < fileArray.Length ;i++)
                    {
                        String[] temp = fileArray[i].Split('\\');
                        sb.Append( temp[temp.Length -1 ] + "*");
                    }
                    byte[] bytes = Encoding.Default.GetBytes(sb.ToString());

                    //byte[] bytes = fileArray.Select(str => Convert.ToByte(str)).ToArray();                  
                    u.getSocket().Send(bytes);
                    break;
                case "chan":
                    sendMessage("chok", u);
                    receiveFileNamesForRename(u);


                    break;
                case "dlte": // request to delete
                    sendMessage("deok", u);
                    receiveFileNameToDelete(u);
                    break;

                case "down": // request to download
                    sendMessage("dook",u);
                    break;
                case "doco": // Request the file name from the server
                    sendMessage("refn",u);
                    String name = receiveFileName(u);
                    u.setFileNameForDownload(name);
                    sendMessage("dcom",u);
                    break;
                case "dcok":
                    sendMessage("pckn", u); // packet numarası göndercem
                    //sendFileData(u);
                    break;

                case "nuok": // Packet numberı gönder
                    sendNumberOfPackets(u);
                    break;
                case "nudo": // client packet sayısı aldı.
                    sendMessage("fida",u); // datayı göndercem diyorum
                    break;

                case "fdok": // dosyayı gönder dedi client bende gönderiyorum
                    sendFileData(u);
                    break;
                case "fcom":
                    monitor.AppendText("File is sent to the server");
                    break;
                
                    


            }


        }

        private void sendNumberOfPackets(User u)
        {

            ////////////////////////


            String filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Server\\" + u.getUserName() + "\\" + u.getFileNameForDownload();
            byte[] SendingBuffer = null;
            FileStream Fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            int x =  Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(BufferSize)));
            monitor.AppendText("packet number is: " + x);

            String num_packets = x.ToString();
            SendingBuffer = Encoding.Default.GetBytes(num_packets);
            u.getSocket().Send(SendingBuffer);
            Fs.Close();
        }

        public void sendFileData(User u)
        {

            String filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Server\\" + u.getUserName() + "\\" + u.getFileNameForDownload();
            
            byte[] SendingBuffer = null;
            FileStream Fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            try
            {
                int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(BufferSize)));
                int TotalLength = (int)Fs.Length;
                int CurrentPacketLength;

                String[] nameArray = filePath.Split('\\');
                String fileToSend = nameArray[nameArray.Length - 1];
                monitor.AppendText("Started sending the file named " + fileToSend + "\n");

                for (int i = 0; i < u.getPacketNumber(); i++)
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
                    u.getSocket().Send(SendingBuffer);
                }
                monitor.AppendText("Sent the file named " + fileToSend + "\n");
                Fs.Close();
            }
            catch
            {

            }

        }

        public void sendFileToClient(User u)
        {

        }



        public void receiveFileNameToDelete(User u)
        {
            Socket s = u.getSocket();
            byte[] buffer = new byte[200];
            int receivedNum = s.Receive(buffer);
            String file = System.Text.Encoding.UTF8.GetString(buffer, 0, receivedNum);
            String filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Server\\" + u.getUserName() + "\\" + file;

            System.IO.File.Delete(filePath);
            monitor.AppendText("Deleted the file named " + file + " \n");

        }


        public void receiveFileNamesForRename(User u)
        {

            Socket s = u.getSocket();
            byte[] buffer = new byte[200];
            int receivedNum = s.Receive(buffer);
            String twofiles = System.Text.Encoding.UTF8.GetString(buffer, 0, receivedNum);

            String usersDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Server\\" + u.getUserName();
            String[] files = twofiles.Split('*');
            String oldFile = files[0];
            String newFile = files[1];




            System.IO.File.Move(usersDirectory+"\\"+ oldFile, usersDirectory + "\\" + newFile);
            monitor.AppendText("Renamed the file named " + oldFile + " to " + newFile + "\n");
        }


        public int receivePacketNumber(User u)
        {
            Socket s = u.getSocket();
            byte[] countBuffer = new byte[64];
            int numberOfPacketsByteNumber = s.Receive(countBuffer); // The number of packets
            String numberOfPacketsString = System.Text.Encoding.UTF8.GetString(countBuffer, 0, numberOfPacketsByteNumber);
        
            int packetNumberToReceive = Int32.Parse(numberOfPacketsString);
            return packetNumberToReceive;
        }


        public void receiveFileData(User u)
        {
            String username = u.getUserName();
            String filename = u.getFileNameToReceive();
            int packetNumber = u.getPacketNumber();
            Socket s = u.getSocket();
            String myDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Server\\" + username;
            System.IO.Directory.CreateDirectory(myDir);
            Stream fileStream = File.OpenWrite(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Server\\" + username + "\\" + filename);
            monitor.AppendText(username + " is uploading a file named" + filename + "\n");

            for (int i = 0; i < packetNumber; i++)
            {
                try
                {
                    Byte[] buffer = new byte[BufferSize];
                    int rec = s.Receive(buffer);

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
                    s.Close(); socketList.Remove(s); userList.Remove(u); nameList.Remove(username);
                }

            }
            monitor.AppendText(username + " finished uploading " + filename + "\n");
            fileStream.Close();
        }      


            

        static public void sendMessage(String message, User u)
        {
            byte[] buffer = Encoding.Default.GetBytes(message);
            u.getSocket().Send(buffer);          
        }

        static public String getUserName(Socket socket)
        {
            String result = "";
            for(int i=0; i< userList.Count; i++)
            {
                if(socket == userList[i].getSocket() )
                {
                    result = userList[i].getUserName();
                }
            }
            return result;
        }

        




        //this function is used in ThrReceive
        //very similar to the client version
        //displays the received message in the console.
        static private void Receive()
        {
            Socket n = socketList[socketList.Count - 1];
            bool connected = true;
            while (connected)
            {
                //Socket n = socketList[socketList.Count - 1];
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
                    String username = "", filename = "";
                    int packetNumberToReceive = 0;
                    // String filename = Convert.ToString(namebuffer);
                    String[] narray = System.Text.Encoding.UTF8.GetString(namebuffer, 0, receivedNameCharCount).Split('\\');
                    if (narray[0].Length >= 3)
                        if (narray[0].Substring(0, 3) == "PNP")
                        {
                            username = narray[0].Substring(3, narray[0].Length - 3);

                            // String filename = System.Text.Encoding.UTF8.GetString(namebuffer, 0, receivedNameCharCount);
                            filename = narray[1];
                            //filename.Substring(0, filename.IndexOf('0'));

                            //byte[] countBuffer = new byte[64];
                            //int numberOfPacketsByteNumber = n.Receive(countBuffer); // The number of packets
                            // String numberOfPacketsString = System.Text.Encoding.UTF8.GetString(countBuffer, 0, numberOfPacketsByteNumber);
                            String numberOfPacketsString = narray[2];
                            packetNumberToReceive = Int32.Parse(numberOfPacketsString);
                        }

                    //String myDir = "C:\\Users\\asus\\Desktop\\Server\\" + nameList[nameList.Count-1];
                    if (username != "" && filename != "")
                    {
                        String myDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Server\\" + username;
                        System.IO.Directory.CreateDirectory(myDir);
                        Stream fileStream = File.OpenWrite(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Server\\" + username + "\\" + filename);

                        monitor.AppendText(username + " is uploading a file named" + filename + "\n");

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
                        monitor.AppendText(username + " finished uploading " + filename + "\n");
                        fileStream.Close();


                    }
                }
            }
        }


    }

}




