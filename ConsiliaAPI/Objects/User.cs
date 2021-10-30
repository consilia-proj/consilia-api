using System;
using System.Threading.Tasks;

namespace ConsiliaAPI.Objects
{
    public class User
    {
        public string SSOKey { get; private set; }
        public string firstName { get; private set; }
        public string lastName { get; private set; }
        public string profilePic { get; private set; }
        public Guid UserUID { get; private set; }

        
    }
}