using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;


namespace NetCore.Docker
{
    public static class ClientConnecton
    {
        private const string HOST = "server";
        private const int PORT = 8000;
        

        static Bot bot;
        static TcpClient client;
        static NetworkStream stream;


        public static void Connect()
        {
            bot = new Bot();
            client = new TcpClient();

            Thread.Sleep(new TimeSpan(0, 0, 4));

            try
            {
                client.Connect(HOST, PORT); //подключение клиента
                stream = client.GetStream(); // получаем поток
 
                string message = bot.ToString();
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
 
                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока
                Console.WriteLine("Connected to server.");
                MonitorCpu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        static void MonitorCpu()
        {
            while (true)
            {
                string cpuInfo = CpuUsage.GetInfo();
                
                SendMessage("CPU_USAGE: " + cpuInfo);

                Thread.Sleep(new TimeSpan(0, 0, 5));
            }
        }
        private static void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length); 
        }
        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
 
                    string message = builder.ToString();
                    if (string.IsNullOrEmpty(message))
                    {
                        throw new Exception();
                    }
                    if (message.Contains("configuration"))
                    {
                        string configurationString = message.ExtractFromBetweenChars("configuration: ", ",");
                        string configurationDecription = configurationString.Substring(0, configurationString.IndexOf(" (") + 1);
                        long configurationLastChangeTime = DateTime.Parse(configurationString.ExtractFromBetweenChars("(", ")")).Ticks;
                        bot.configuration = new Configuration(configurationDecription, configurationLastChangeTime);
                    }
                    Console.WriteLine(message);//вывод сообщения
                }
                catch
                {
                    Console.WriteLine("Connection has been interrupted!"); //соединение было прервано
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }
        static void Disconnect()
        {
            if(stream!=null)
                stream.Close();//отключение потока
            if(client!=null)
                client.Close();//отключение клиента
            Environment.Exit(0); //завершение процесса
        }
    }
}