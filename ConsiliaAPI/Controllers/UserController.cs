using System;
using System.Threading.Tasks;
using ConsiliaAPI.Objects;
using Microsoft.AspNetCore.Mvc;
using User = ConsiliaAPI.Objects.User;

namespace ConsiliaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        [HttpPost]
        public async Task<User> CreateUser([FromBody] User u)
        {
            return await Objects.User.CreateUser(u.FirstName, u.LastName, u.SSOKey);
        }
    }
}