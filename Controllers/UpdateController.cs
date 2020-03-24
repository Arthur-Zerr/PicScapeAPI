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
    public class UpdateController : Controller
    {

        private readonly PicScapeRepository picScapeRepository;
        private readonly GenericResponse genericResponse;

        public UpdateController(PicScapeRepository picScapeRepository, GenericResponse genericResponse)
        {
            this.genericResponse = genericResponse;
            this.picScapeRepository = picScapeRepository;
        }

        [HttpGet]
        [Route("UploadIdForPicture")]
        public async Task<IActionResult> GetUploadIdForPicture(int id)
        {
            var tempValue = await picScapeRepository.GetPictureUploadId(id);

            if (String.IsNullOrEmpty(tempValue))
                return BadRequest();

            return Ok(genericResponse.GetResponseWithDataString("", false, true, tempValue));
        }

        [HttpGet]
        [Route("UploadIdForProfilePicture")]
        public async Task<IActionResult> GetUploadIdForProfilePicture(int id)
        {
            var tempValue = await picScapeRepository.GetProfilePictureUploadId(id);

            if (String.IsNullOrEmpty(tempValue))
                return BadRequest();

            return Ok(genericResponse.GetResponseWithDataString("", false, true, tempValue));
        }


    }
}