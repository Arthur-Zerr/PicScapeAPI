using System.Security.Claims;
using System;
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
using System.Text;
using PicScapeAPI.DAL;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PicScapeAPI.Helper;

namespace PicScapeAPI.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class PictureController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly PicScapeRepository picScapeRepository;
        public GenericResponse GenericResponse { get; }

        public PictureController(UserManager<User> userManager, PicScapeRepository picScapeRepository, GenericResponse genericResponse)
        {
            this.GenericResponse = genericResponse;
            this.picScapeRepository = picScapeRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("ProfilePicture")]
        public async Task<IActionResult> GetProfilePicture([FromQuery] string Username)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await userManager.FindByNameAsync(Username);
            if (user == null)
                return BadRequest();

            var returnPicture = await picScapeRepository.GetCurrentProfilePictureAsync(user.Id);
            if(returnPicture == null)
                return BadRequest();
            var pictureStream = new MemoryStream(returnPicture.Img);
            
            return File(pictureStream, "image/jpeg");
        }

        [HttpPost]
        [Route("ProfilePicture")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest(GenericResponse.GetResponse("PICTURE_UPLOAD_ERROR", true, false));

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest(GenericResponse.GetResponse("PICTURE_UPLOAD_ERROR", true, false));

            var temp = new ProfilePicture();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    temp.Img = ToByte(stream);
                    temp.UserID = user.Id;
                    temp.ImgType = file.ContentType;
                    temp.UploadDate = DateTime.Now;
                    temp.isCurrentPicture = true;
                }
            }
            await picScapeRepository.SaveProfilePicture(temp);

            return Ok(GenericResponse.GetResponse("PICTURE_UPLOAD_SUCCESS",true, true));
        }

        private byte[] ToByte(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                input.Position = 0;
                return ms.ToArray();
            }
        }
    }
}