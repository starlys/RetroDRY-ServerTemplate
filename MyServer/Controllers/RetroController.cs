using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RetroDRY;

namespace MyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetroController : ControllerBase
    {
        [HttpPost("main")]
        public Task<MainResponse> Main([FromBody]MainRequest req)
        {
            return Globals.Retroverse.HandleHttpMain(req);
        }

        [HttpPost("long")]
        public Task<LongResponse> Long(LongRequest req)
        {
            return Globals.Retroverse.HandleHttpLong(req);
        }

        [HttpGet("export")]
        public async Task Export(string key)
        {
            Response.Headers.Add(HeaderNames.ContentType, "text/plain");
            Response.Headers.Add(HeaderNames.ContentDisposition, "attachment; filename=data.csv");

            await Globals.Retroverse.HandleHttpExport(Response.Body, key);
        }
    }
}
