using System;

namespace ConsiliaAPI.Objects
{
    public class Event
    {
        public string EventID { get; private set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public double LocationLat { get; set; }
        public double LocationLong { get; set; }
        public double Range { get; set; }
        public string Type { get; set; }
    }
}