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
using System.Collections.Generic;

namespace PicScapeAPI.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class PictureController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly PicScapeRepository picScapeRepository;
        private readonly GenericResponse genericResponse;
        private readonly UploadHelper uploadHelper;

        public PictureController(UserManager<User> userManager, PicScapeRepository picScapeRepository, GenericResponse genericResponse, UploadHelper uploadHelper)
        {
            this.uploadHelper = uploadHelper;
            this.genericResponse = genericResponse;
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
            if (returnPicture == null)
                return BadRequest();
            var pictureStream = new MemoryStream(returnPicture.Img);

            return File(pictureStream, "image/jpg");
        }

        [HttpPost]
        [Route("ProfilePicture")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest(genericResponse.GetResponse("PICTURE_UPLOAD_ERROR", true, false));

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest(genericResponse.GetResponse("PICTURE_UPLOAD_ERROR", true, false));

            var temp = new ProfilePicture();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    temp.Img = ToByte(stream);
                    temp.UserID = user.Id;
                    temp.UploadId = uploadHelper.GetUploadId("Picture", await picScapeRepository.GetProfilePictureCount() + 1);
                    temp.ImgType = file.ContentType;
                    temp.UploadDate = DateTime.Now;
                    temp.isCurrentPicture = true;
                }
            }
            await picScapeRepository.SaveProfilePicture(temp);

            return Ok(genericResponse.GetResponse("PICTURE_UPLOAD_SUCCESS", true, true));
        }

        [HttpPost]
        [Route("Picture")]
        public async Task<IActionResult> UploadPicture([FromForm] IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest(genericResponse.GetResponse("PICTURE_UPLOAD_ERROR", true, false));

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest(genericResponse.GetResponse("PICTURE_UPLOAD_ERROR", true, false));

            var temp = new Picture();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    temp.Img = ToByte(stream);
                    temp.UploadId = uploadHelper.GetUploadId("Picture", await picScapeRepository.GetPictureCount() + 1);
                    temp.UserID = user.Id;
                    temp.Title = "test";
                    temp.ImgType = file.ContentType;
                    temp.UploadDate = DateTime.Now;
                }
            }

            var result = await picScapeRepository.AddPictureToUser(userId, temp);

            if (result > 0)
                return Ok(genericResponse.GetResponse("PICTURE_UPLOAD_SUCCESS", true, true));

            return BadRequest(genericResponse.GetResponse("PICTURE_UPLOAD_ERROR", true, false));
        }

        [HttpGet]
        [Route("FollowingPicture")]
        public async Task<IActionResult> GetFollowingPicture(int page, int pagesize)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return BadRequest();
        }

        [HttpGet]
        [Route("PictureData")]
        public async Task<IActionResult> GetPictureData(int id)
        {
            var pictureData = await picScapeRepository.GetPictureData(id);

            if (pictureData == null)
                return BadRequest();

            return Ok(pictureData);
        }


        [HttpGet]
        [Route("AllPictureForUser")]
        public async Task<IActionResult> GetAllPictureForUser(string Username, int page, int pageSize)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await userManager.FindByNameAsync(Username);
            if (user == null)
                return BadRequest();

            var pictureList = await picScapeRepository.GetUserPictures(user.Id, page, pageSize);

            return Ok(pictureList);
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