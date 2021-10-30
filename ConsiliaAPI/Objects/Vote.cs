using System;

namespace ConsiliaAPI.Objects
{
    public class Vote
    {
        public Guid PlaceUUID { get; private set; }

        public Guid EventUUID { get; private set; }

        public Guid UserUUID { get; private set; }

        public Guid VoteUUID { get; private set; }

        public int voteType { get;  set; }
    }
}
