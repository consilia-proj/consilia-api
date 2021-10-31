using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsiliaAPI.Objects;
using Microsoft.AspNetCore.Mvc;

namespace ConsiliaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : Controller
    {
        [HttpPost]
        public async Task<Event> CreateEvent([FromBody] Event e)
        {
            return await Objects.Event.CreateEvent(e);
        }
        
        [HttpGet]
        [Route("{eventId}")]
        public async Task<Event> GetEvent(string eventId)
        {
            Event e = await Event.GetEvent(eventId);
            return e;
        }
        
        [HttpGet]
        [Route("{eventId}/places")]
        public async Task<Event> GetPlaces(string eventId)
        {
            Event e = await Event.GetEvent(eventId);
            return e;
        }
        
        [HttpPost]
        [Route("{eventId}/{placeUUID}/vote/{userId}/{ssoKey}")]
        //NOTE: above uri structure is poor but too lazy to 
        // read out of auth header
        public async Task<Vote> CastVote(string eventId, string placeUUID, string userId, string ssoKey, [FromBody] Vote vote)
        {
            
            Objects.User u = await Objects.User.GetUser(userId);
            await u.ValidateSSOKey(ssoKey);
            Event e = await Event.GetEvent(eventId);
            await e.CastVote(u, vote, placeUUID);
            return vote;
        }
        
        [HttpGet]
        [Route("{eventId}/votes")]
        public async Task<List<Places>> GetRanked(string eventId)
        {
            Event e = await Event.GetEvent(eventId);
            List<Places> places = await e.GetPlaces();
            return places.OrderByDescending(x => x.Points).ToList();
        }
        
    }
}