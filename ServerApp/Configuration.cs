using System;

namespace NetCore.Docker
{
    public class Configuration
    {
        public string decription;
        public long lastChangeTime;


        public Configuration()
        {
            lastChangeTime = DateTime.UtcNow.Ticks;
        }
        public Configuration(string description) : this()
        {
            this.decription = decription;
        }
        public Configuration(string description, long lastChangeTime) : this(description)
        {
            this.lastChangeTime = lastChangeTime;
        }


        public override string ToString()
        {
            return decription + $" ({new DateTime(lastChangeTime)})";
        }
    }
}