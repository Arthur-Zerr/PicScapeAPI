using Microsoft.AspNetCore.Mvc;

namespace PicScapeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        [HttpGet]
        [Route("IsOnline")]
        public IActionResult IsOnline()
        {
            return Ok("IsOnline");
        }
    }
}