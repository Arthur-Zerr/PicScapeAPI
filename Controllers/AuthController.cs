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
        private readonly UserActivityLogger userActivityLogger;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, GenericResponse genericResponse, PicScapeRepository picScapeRepository,
                                IConfiguration config, UserActivityLogger userActivityLogger)
        {
            this.config = config;
            this.userActivityLogger = userActivityLogger;
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
                //Debug 
                Console.WriteLine($"{user.UserName} logged in on {DateTime.Now.ToLongTimeString()}");

                await userActivityLogger.LogLogin(user.Id);
                var token = generateToken(user);

                return Ok(genericResponse.GetResponseWithDataString("LOGIN_SUCCESS", true, true, token));
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
                await userActivityLogger.LogRegistration(user.Id);
                var token = generateToken(user);

                return Ok(genericResponse.GetResponseWithDataString("REGISTER_SUCCESS", true, true, token));
            }

            return BadRequest(genericResponse.GetResponseWithData("REGISTER_ERROR", true, false, result.Errors.ToString()));
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout([FromBody] UserForLogoutDto userForLogoutDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(genericResponse.GetResponse("", false, false));

            var user = await userManager.FindByIdAsync(userForLogoutDto.UserId);

            if(user.UserName == userForLogoutDto.Username)
            {
                await userActivityLogger.LogLogout(user.Id);
                return Ok(genericResponse.GetResponse("LOGOUT_SUCCESS",true, true)); 
            }
            return BadRequest(genericResponse.GetResponse("LOGOUT_ERROR",true, true));
        }

        private string generateToken(User user)
        {
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
            return tokenHandler.WriteToken(token);
        }
    }
}