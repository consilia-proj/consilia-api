using System;

namespace ConsiliaAPI.Objects
{
    public class Vote
    {
        public Guid placeUUID { get; private set; }

        public Guid EventUUID { get; private set; }

        public Guid userUUID { get; private set; }

        public Guid voteUUID { get; private set; }

        public int voteType { get;  set; }
    }
}
