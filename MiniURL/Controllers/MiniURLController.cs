using Microsoft.AspNetCore.Mvc;
using MiniURL.Services.Exceptions;
using MiniURL.Services.Interfaces;
using MiniURL.Services.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniURL.Controllers
{
    [ApiController]
    [Route("")]
    public class MiniURLController : ControllerBase
    {
        [HttpPost]
        [Route("quickCreate")]
        public async Task<IActionResult> QuickCreateMiniUrl([FromBody] MiniURL_Create request, [FromServices] IUserService serv)
        {
            try
            {
                var uRef = await serv.AddMiniURLRecord(request);
                return CreatedAtAction("Navigate", "Routing", new { urlRef = uRef }, uRef);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
