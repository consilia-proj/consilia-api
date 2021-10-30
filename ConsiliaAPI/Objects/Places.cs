using System;

namespace ConsiliaAPI.Objects
{
    public class Places
    {
        public Guid PlaceID { get; private set; }

        public Guid EventID { get; private set; }

        public static async Task<Places> PlacesRequest()
        {
            string request = "https://maps.googleapis.com/maps/api/place/findplacefromtext/output?parameters";
"; 
        }
    }


}
