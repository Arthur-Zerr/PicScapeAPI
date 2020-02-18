using System.IO;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PicScapeAPI.DAL.Models;
using Microsoft.AspNetCore.Http;

namespace PicScapeAPI.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class PictureController : Controller
    {
        private readonly UserManager<User> userManager;

        public PictureController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetPicture()
        {
            var stream = System.IO.File.OpenRead(@"./Resources/rainbowlake.jpg");
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            response.Content.Headers.ContentLength = stream.Length;

            var returnFile = stream;

            return File(returnFile, "image/jpeg");
        }

        [HttpGet]
        [Route("UserPicture")]
        public async Task<IActionResult> GetUserPicture(string Username)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await userManager.FindByNameAsync(Username);
            if(user == null)
                return NotFound();

            FileStream returnResult = System.IO.File.OpenRead(@"./Resources/rainbowlake.jpg");
            try
            {
                returnResult = System.IO.File.OpenRead(@"./Resources/" + Username + ".jpg");
            }
            catch (System.Exception)
            {
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(returnResult);

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            response.Content.Headers.ContentLength = returnResult.Length;

            var returnFile = returnResult;

            return File(returnFile, "image/jpeg");
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> UploadPicture([FromForm]IFormFile file)
        {
            return BadRequest();
        }
    }
}