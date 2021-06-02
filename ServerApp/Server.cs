using System;
using System.Threading;
 
namespace NetCore.Docker
{
    public static class Server
    {
        static ServerObject server; // сервер
        static Thread listenThread; // поток для прослушивания


        public static void Start()
        {
            try
            {
                server = new ServerObject();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start(); //старт потока
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}