using System.Collections.Generic;
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
        public async Task<List<Places>> GetPlaces(string eventId)
        {
            Event e = await Event.GetEvent(eventId);
            return await e.GetPlaces();
        }
        
    }
}