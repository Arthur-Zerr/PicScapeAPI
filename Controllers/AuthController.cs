using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PicScapeAPI.DAL.Dtos;

namespace PicScapeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            if(userForLoginDto.Username == "Arthur" && userForLoginDto.Password == "Test")
                return Ok(true);
            
            return BadRequest(false);
        }
    }
}