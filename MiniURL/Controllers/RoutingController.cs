using Microsoft.AspNetCore.Mvc;
using MiniURL.Services.Exceptions;
using MiniURL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniURL.Controllers
{
    [ApiController]
    [Route("r")]
    public class RoutingController : ControllerBase
    {
        [HttpGet("{urlRef}")]
        public async Task<IActionResult> Navigate([FromRoute] string urlRef, [FromServices] IMiniUrlService serv)
        {
            try
            {
                var url = await serv.GetUrlByRef(urlRef);
                return RedirectPermanent(url);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
