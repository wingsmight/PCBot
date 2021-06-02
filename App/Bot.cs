using System;

namespace NetCore.Docker
{
    public class Bot
    {
        public string id;
        public string token;
        public Configuration configuration;
        public State state;


        public Bot()
        {
            this.id = new Random().Next(0, int.MaxValue).ToString();
            this.token = "NULL_TOKEN";
            this.configuration = new Configuration();
            this.state = new State();
        }
        public Bot(string id) : this()
        {
            this.id = id;
        }


        public override string ToString()
        {
            return String.Format($"Bot(id: {id}, token: {token}, configuration: {configuration}, state: {state}");
        }
    }
}