using System;

namespace ConsiliaAPI.Objects
{
    public class Vote
    {
        public Guid PlaceUUID { get; set; }

        public Guid EventUUID { get; set; }

        public Guid UserUUID { get; set; }

        public Guid VoteUUID { get; set; }

        public int VoteType { get;  set; }
    }
}
