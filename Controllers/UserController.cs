using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PicScapeAPI.DAL;
using PicScapeAPI.DAL.Dtos;
using PicScapeAPI.DAL.Models;
using PicScapeAPI.Extensions;
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
        private readonly PicScapeRepository picScapeRepository;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, GenericResponse genericResponse, PicScapeRepository picScapeRepository)
        {
            this.genericResponse = genericResponse;
            this.picScapeRepository = picScapeRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [Route("IsOnline")]
        public IActionResult IsOnline()
        {
            return Ok(genericResponse.GetResponse("ISONLINE", true, true));
        }

        [HttpGet]
        [Route("name={name}")]
        public async Task<IActionResult> UserData(string name)
        {
            var user = await userManager.FindByNameAsync(name);

            if(user == null)
                return BadRequest(genericResponse.GetResponse("USER_NOT_FOUND_ERROR", true, false));

            return Ok(genericResponse.GetResponseWithData("USER_SUCCESS", false, true, user.ToUserForReturnDto()));
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route("UpdateUserData")]
        public async Task<IActionResult> UpdateUserData([FromBody]UserForUpdateDto userForUpdateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(genericResponse.GetResponse("", true, false));

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest(genericResponse.GetResponse("", true, false));

            var Birthdate = new DateTime();
            if(DateTime.TryParse(userForUpdateDto.Birthday, out Birthdate) == false)
                return BadRequest();


            var temp = new User{ 
                Id = userId, 
                Name = userForUpdateDto.Name, 
                Firstname = userForUpdateDto.Firstname, 
                City = userForUpdateDto.City, 
                Country = userForUpdateDto.Country,
                Birthday = Birthdate
            };

            if(await picScapeRepository.UpdateUserData(temp) == false)
                return BadRequest(genericResponse.GetResponse("", true, false));
            
            return Ok(genericResponse.GetResponse("", true, true));
        }
    }
}