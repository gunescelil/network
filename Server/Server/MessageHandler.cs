using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class MessageHandler
    {
        public MessageHandler()
        {


        }

        public void Handle(String message)
        {
            switch(message.ToLower())
            {
                case "send": // The client is sending file


                    break;
                case "reqf": // The client request a file
                    break;
                case "dcon": // client is disconecting
                    break;

                default:
                    break;
                

            }



        }


    }
}
