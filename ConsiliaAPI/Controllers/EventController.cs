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
        [Route("")]
        public async Task<Event> CreateEvent([FromBody] Event e)
        {
            return await Objects.Event.CreateEvent(e);
        }
        
    }
}