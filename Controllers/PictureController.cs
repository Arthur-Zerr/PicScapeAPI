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

namespace PicScapeAPI.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class PictureController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly PicScapeContext picScapeContext;

        public PictureController(UserManager<User> userManager, PicScapeContext picScapeContext)
        {
            this.picScapeContext = picScapeContext;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("ProfilePicture")]
        public async Task<IActionResult> GetProfilePicture([FromQuery] string Username)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var user = await userManager.FindByNameAsync(Username);
            if(user == null)
                return BadRequest();
            
            var returnPicture = await picScapeContext.ProfilePictures.Where(x => x.UserID == user.Id && x.isCurrentPicture == true).FirstOrDefaultAsync();
            
            var pictureStream = new MemoryStream(returnPicture.Img);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(pictureStream);

            response.Content.Headers.ContentType = new MediaTypeHeaderValue(returnPicture.ImgType);
            response.Content.Headers.ContentLength = pictureStream.Length;

            return File(pictureStream, "image/jpeg");
        }

        [HttpPost]
        [Route("ProfilePicture")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile file)
        {
            if(!ModelState.IsValid)
                return BadRequest();
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);
            if(user == null)
                return BadRequest();

            var profilePictureList = await picScapeContext.ProfilePictures.Where(x => x.UserID == user.Id).ToListAsync();
            var profilePicture = profilePictureList.LastOrDefault();
            
            if(profilePicture != null)
                profilePicture.isCurrentPicture = false;

            var temp = new ProfilePicture();

            if(file.Length > 0 )
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
            await picScapeContext.ProfilePictures.AddAsync(temp);
            await picScapeContext.SaveChangesAsync();

            return Ok();
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