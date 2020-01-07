using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PicScapeAPI.DAL.Models;
using PicScapeAPI.Helper;

namespace PicScapeAPI.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly GenericResponse genericResponse;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, GenericResponse genericResponse)
        {
            this.genericResponse = genericResponse;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [Route("IsOnline")]
        public IActionResult IsOnline()
        {
            return Ok(genericResponse.GetResponse("ISONLINE", true, true));
        }

        [HttpGet("{name}", Name="UserData")]
        public async Task<IActionResult> UserData([FromQuery]string name)
        {
            var user = await userManager.FindByNameAsync(name);

            if(user == null)
                return BadRequest(genericResponse.GetResponse("USER_NOT_FOUND_ERROR", true, false));

            return Ok(genericResponse.GetResponseWithData("USER_SUCCESS", false, true, user));
        }
    }
}