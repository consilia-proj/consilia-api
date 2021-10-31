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
            return await Objects.User.CreateUser(u);
        }
        
        [HttpPut]
        [HttpPatch]
        [Route("{uuid}")]
        public async Task<User> UpdateUser(string uuid, [FromBody] User u)
        {
            User cuser = await Objects.User.GetUser(uuid);
            await cuser.UpdateUser(u);
            return cuser;
        }
        
        [HttpPut]
        [HttpPatch]
        [Route("{uuid}/profile-picture")]
        public async Task<User> UpdateProfilePicture(string uuid, [FromBody] ProfilePictureUpdateRequest pfp)
        {
            User cuser = await Objects.User.GetUser(uuid);
            await cuser.UpdateProfilePicture(await pfp.OptimizedImage());
            return cuser;
        }
        
        [HttpGet]
        [Route("{userUUID}")]
        public async Task<User> GetUser(string userUUID)
        {
            return await Objects.User.GetUser(userUUID);
        }
    }
}