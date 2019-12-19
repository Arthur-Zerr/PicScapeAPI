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
                return BadRequest(genericResponse.GetResponse("LOGIN_MODEL_ERROR", false ,false));

            var result = await signInManager.PasswordSignInAsync(userForLoginDto.Username, userForLoginDto.Password, false, true);
            if(result.Succeeded)
                return Ok(genericResponse.GetResponseWithData("LOGIN_SUCCESS", true, true, "TestData"));

            if(result.IsLockedOut)
                return BadRequest(genericResponse.GetResponse("LOGIN_LOCKEDOUT", true ,false));

            return BadRequest(genericResponse.GetResponse("LOGIN_ERROR",true ,false));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(genericResponse.GetResponse("", false ,false));

            if (userForRegisterDto.Password != userForRegisterDto.ConfirmPassword)
                return BadRequest(genericResponse.GetResponse("REGISTER_PASSWORD_NOT_SAME_ERROR", true, false));

            var user = new User
            {
                UserName = userForRegisterDto.Username,
                Email = userForRegisterDto.Email
            };

            var result = await userManager.CreateAsync(user, userForRegisterDto.Password);
            if(result.Succeeded)
            {
                return Ok(genericResponse.GetResponseWithData("REGISTER_SUCCESS", true, true, "TestData"));
            }
            

            return BadRequest(genericResponse.GetResponseWithData("REGISTER_ERROR", true, false, result.Errors));
        }
    }
}