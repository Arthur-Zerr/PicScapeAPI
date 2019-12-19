using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PicScapeAPI.DAL.Dtos;
using PicScapeAPI.DAL.Models;
using PicScapeAPI.Helper;

namespace PicScapeAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly GenericResponse genericResponse;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, GenericResponse genericResponse)
        {
            this.genericResponse = genericResponse;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]UserForLoginDto userForLoginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(genericResponse.GetResponse("LOGIN_MODEL_ERROR", false));


            return BadRequest(genericResponse.GetResponse("LOGIN_MODEL_ERROR", false));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(genericResponse.GetResponse("LOGIN_MODEL_ERROR", false));

            if (userForRegisterDto.Password != userForRegisterDto.ConfirmPassword)
                return BadRequest();


            var user = new User
            {
                UserName = userForRegisterDto.Username,
                Email = userForRegisterDto.Email,
                Birthday = userForRegisterDto.Birthday,
                Firstname = userForRegisterDto.Forename
            };

            var result = await userManager.CreateAsync(user, userForRegisterDto.Password);

            return Ok();
        }
    }
}