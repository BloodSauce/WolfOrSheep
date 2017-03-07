using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WolfOrSheepServe {
    class ServeControl {
        public static Queue<ServeControl> players = new Queue<ServeControl>();
        private Socket server;
        private ServeControl oServer = null;
        public  static int playerNum;
        public ServeControl(Socket client) {
            this.server = client;
        }
        public void MessageHandle() {
            byte[] bytes = new byte[1024];
            int length;
            if (server != null) {            
                    playerNum++;
                Console.WriteLine("在线玩家数：{0}", playerNum);
            }
            try {
                while ((length= server.Receive(bytes)) != 0) {
                    string msg = Encoding.ASCII.GetString(bytes, 0, length);
                    Console.WriteLine(msg);
                    switch (msg[0]) {                    
                        case 's': while (true) {
                                if (players.Count == 0) {
                                    players.Enqueue(this); break;
                                }
                                if (players.Peek() != this) {
                                    oServer = players.Dequeue();
                                    oServer.oServer = this;

                                    if (oServer != null) {
                                        server.Send(Encoding.ASCII.GetBytes("yw"));
                                        oServer.server.Send(Encoding.ASCII.GetBytes("ys"));
                                        break;
                                    }
                                    break;
                                }       
                            }break;
                        case 'a':
                                oServer.server.Send(bytes, length,SocketFlags.None);break;
                        case 'm':
                                oServer.server.Send(bytes, length, SocketFlags.None);break;
                    }
                }
            }
            catch(Exception e) {       
                Console.WriteLine(e.ToString());
            }
            if (oServer != null) {
                oServer.server.Send(Encoding.ASCII.GetBytes("r"));
                oServer.oServer = null;
            }
            server.Close();
            if(playerNum>0)
            playerNum--;
            Console.WriteLine("在线玩家数：{0}", playerNum);
        }
    }
}
