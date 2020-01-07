using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PicScapeAPI.DAL.Dtos;
using PicScapeAPI.DAL;
using PicScapeAPI.DAL.Models;
using PicScapeAPI.Helper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace PicScapeAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly GenericResponse genericResponse;
        private readonly PicScapeRepository picScapeRepository;
        private readonly IConfiguration config;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, GenericResponse genericResponse, PicScapeRepository picScapeRepository,
                                IConfiguration config)
        {
            this.config = config;
            this.picScapeRepository = picScapeRepository;
            this.genericResponse = genericResponse;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]UserForLoginDto userForLoginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(genericResponse.GetResponse("LOGIN_MODEL_ERROR", false, false));

            var result = await signInManager.PasswordSignInAsync(userForLoginDto.Username, userForLoginDto.Password, false, true);
            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(userForLoginDto.Username);

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config.GetSection("AppSettings:Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tempResult = new UserForReturnLoginDto { Token = tokenHandler.WriteToken(token)};

                return Ok(genericResponse.GetResponseWithDataString("LOGIN_SUCCESS", true, true, tokenHandler.WriteToken(token)));
            }
            if (result.IsLockedOut)
                return BadRequest(genericResponse.GetResponse("LOGIN_LOCKEDOUT", true, false));

            return BadRequest(genericResponse.GetResponse("LOGIN_ERROR", true, false));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(genericResponse.GetResponse("", false, false));

            if (userForRegisterDto.Password != userForRegisterDto.ConfirmPassword)
                return BadRequest(genericResponse.GetResponse("REGISTER_PASSWORD_NOT_SAME_ERROR", true, false));

            var user = new User
            {
                UserName = userForRegisterDto.Username,
                Email = userForRegisterDto.Email
            };

            var result = await userManager.CreateAsync(user, userForRegisterDto.Password);
            if (result.Succeeded)
            {
                return Ok(genericResponse.GetResponseWithData("REGISTER_SUCCESS", true, true, "TestData"));
            }


            return BadRequest(genericResponse.GetResponseWithData("REGISTER_ERROR", true, false, result.Errors.ToString()));
        }
        //TODO Add Logout method to Track Activity
    }
}