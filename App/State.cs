using System;

namespace NetCore.Docker
{
    public class State
    {
        public Status status;
        public Mood mood;
        public Mode mode;


        public override string ToString()
        {
            return String.Format($"State({status}, {mood}; {mode})");
        }
    }
    public enum Status
    {
        Ok,
        Offile,
        Error,
    }
    public enum Mood
    {
        Normal,
        Tired,
    }
    public enum Mode
    {
        Off,
        Manual,
        Auto,
    }
}