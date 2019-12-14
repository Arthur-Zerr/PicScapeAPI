using System.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PicScapeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : Controller
    {
        
 //       [HttpPost]
 //       public async Task<IActionResult> Picture()
 //       {
  //          return Ok();
  //      }

        [HttpGet]
        public async Task<IActionResult> Picture()
        {
            var stream = System.IO.File.OpenRead(@"./Resources/icybay.jpg");
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            response.Content.Headers.ContentLength = stream.Length;
            return Ok(response);
        }

    }
}