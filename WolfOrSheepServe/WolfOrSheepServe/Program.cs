using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WolfOrSheepServe {
    class Program {
        static void Main(string[] args) {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 66);
            server.Bind(iep);
            server.Listen(50);
            Console.WriteLine("等待玩家连接。。。。。。");
            while (true) {
                try {
                    Socket client = server.Accept();
                    ServeControl newPlayer = new ServeControl(client);
                    Thread newThread = new Thread(new ThreadStart(newPlayer.MessageHandle));
                    newThread.Start();
                }
                catch(Exception e) {
                    Console.WriteLine(e.ToString());                  
                }
            }     
        }
    }
}
