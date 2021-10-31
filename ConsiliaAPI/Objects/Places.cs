using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GooglePlacesApi.Models;
using Newtonsoft.Json.Linq;

namespace ConsiliaAPI.Objects
{
    public class Places
    {
        public Guid PlaceID { get; set; }

        public Guid EventID { get; set; }

        public decimal Rating { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string ImageURL { get; set; }
        public string GooglePlaceId { get; set; }

        public async Task Prepare()
        {
            var response = FileGetContents((
                $"https://maps.googleapis.com/maps/api/place/details/json?place_id={GooglePlaceId}&key={GlobalConstants.GOOGLE_MAPS_API_KEY}"
            ));
            
            JObject jobj = JObject.Parse(response);


            string imgurl = (jobj["result"]?["photos"]?[0]?["photo_reference"] ?? "").ToString();
            imgurl = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=1600&photo_reference=" + imgurl+$"&key={GlobalConstants.GOOGLE_MAPS_API_KEY}";
            
            Name = jobj["result"]["name"].ToString();
            Rating = decimal.Parse((jobj["result"]["rating"] ?? "-1").ToString());
            GooglePlaceId = jobj["result"]["place_id"].ToString();
            Description = "A Cool Place!";
            EventID = EventID;
            ImageURL = imgurl;
            Latitude = decimal.Parse(jobj["result"]["geometry"]["location"]["lat"].ToString());
            Longitude = decimal.Parse(jobj["result"]["geometry"]["location"]["lng"].ToString());
            PlaceID = Guid.NewGuid();
        }

        private static string FileGetContents(string fileName)
        {
            string sContents = string.Empty;
            string me = string.Empty;
            try
            {
                if (fileName.ToLower().IndexOf("https:") > -1)
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    byte[] response = wc.DownloadData(fileName);
                    sContents = System.Text.Encoding.ASCII.GetString(response);
                }
                else
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
                    sContents = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch
            {
                sContents = "unable to connect to server ";
            }

            return sContents;
        }
    }
}