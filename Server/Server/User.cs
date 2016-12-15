using System;
using System.Net.Sockets;

namespace UserNamespace { 

    public class User
    {
        String username;
        Socket socket;

        public User(String u, Socket s)
        {
            username = u;
            socket = s;
        }

        public string getUserName()
        {
            return username;
        }

        public Socket getSocket()
        {
            return socket;
        }
    }


    


}
