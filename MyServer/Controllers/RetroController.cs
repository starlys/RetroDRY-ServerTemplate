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
            if (Globals.Retroverse == null) throw new Exception("Uninitialized");
            return Globals.Retroverse.HandleHttpMain(req);
        }

        [HttpPost("long")]
        public Task<LongResponse> Long(LongRequest req)
        {
            if (Globals.Retroverse == null) throw new Exception("Uninitialized");
            return Globals.Retroverse.HandleHttpLong(req);
        }

        [HttpGet("export")]
        public async Task Export(string key)
        {
            if (Globals.Retroverse == null) throw new Exception("Uninitialized");
            Response.Headers.Add(HeaderNames.ContentType, "text/plain");
            Response.Headers.Add(HeaderNames.ContentDisposition, "attachment; filename=data.csv");

            await Globals.Retroverse.HandleHttpExport(Response.Body, key);
        }
    }
}
