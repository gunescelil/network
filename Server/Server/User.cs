using System;
using System.Net.Sockets;

namespace UserNamespace { 

    public class User
    {
        String username;
        Socket socket;
        int  NumberOfPacketsToReceive;
        String fileNameToReceive;
        String fileNameForDownload;

        public User(String u, Socket s)
        {
            username = u;
            socket = s;
        }

        public void setUserName(String s)
        {
            username = s;
        }

        public string getUserName()
        {
            return username;
        }

        public Socket getSocket()
        {
            return socket;
        }

        public void setPacketNumber(int i)
        {
            NumberOfPacketsToReceive = i;
        }

        public int getPacketNumber()
        {
            return NumberOfPacketsToReceive;
        }

        public void setFileNameToReceive(String s)
        {
            fileNameToReceive = s;
        }
        
        public String getFileNameToReceive()
        {
            return fileNameToReceive;
        }

        public String getFileNameForDownload()
        {
            return fileNameForDownload;
        }

        public void setFileNameForDownload(String s)
        {
            fileNameForDownload = s;
        }



    }


    


}
