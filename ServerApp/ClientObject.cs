using System;
using System.Net.Sockets;
using System.Text;
 
namespace NetCore.Docker
{
    public class ClientObject
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream {get; private set;}
        string botName;
        public Bot bot;
        TcpClient client;
        ServerObject server; // объект сервера
 
        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }
 
        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                // получаем иинформацию о боте
                string message = GetMessage();
                botName = message;

                bot = new Bot()
                {
                    id = botName.ExtractFromBetweenChars("id: ", ","),
                    token = botName.ExtractFromBetweenChars("token: ", ","),
                };
                
                string configurationString = botName.ExtractFromBetweenChars("configuration: ", ",");
                string configurationDecription = configurationString.Substring(0, configurationString.IndexOf(" (") + 1);
                long configurationLastChangeTime = DateTime.Parse(configurationString.ExtractFromBetweenChars("(", ")")).Ticks;
                bot.configuration = new Configuration(configurationDecription, configurationLastChangeTime);

                string stateStatus = botName.ExtractFromBetweenChars("State(", ",");
                string stateMood = botName.ExtractFromBetweenChars("State(" + stateStatus + ",", ";");
                string stateMode = botName.ExtractFromBetweenChars(";", ")");
                var state = new State()
                {
                    status = (Status)Enum.Parse(typeof(Status), stateStatus),
                    mood = (Mood)Enum.Parse(typeof(Mood), stateMood),
                    mode = (Mode)Enum.Parse(typeof(Mode), stateMode),
                };

 
                message = botName + " has connected";
                Console.WriteLine(message);
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        if (string.IsNullOrEmpty(message))
                        {
                            server.RemoveConnection(this.Id);
                            Close();
                        }
                        Console.WriteLine("{0}: {1}", botName, message);
                    }
                    catch
                    {
                        message = String.Format("{0} has disconnected", botName);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Close();
            }
        }
 
        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);
 
            return builder.ToString();
        }
 
        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}