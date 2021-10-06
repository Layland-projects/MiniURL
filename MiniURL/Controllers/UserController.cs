using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniURL.Core;
using MiniURL.Core.Models;
using MiniURL.Services.Exceptions;
using MiniURL.Services.Interfaces;
using MiniURL.Services.Models.Request;
using MiniURL.Services.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniURL.Controllers
{
    [ApiController]
    [Route("u")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public async IAsyncEnumerable<User_Lite> GetUsers([FromServices] IUserService serv)
        {
            await foreach (var res in serv.GetAll_Lite())
            {
                yield return res;
            }
        }

        [HttpGet]
        [Route("{userId:guid}")]
        public async Task<User_Lite> GetUser([FromRoute] Guid userId, [FromServices] IUserService serv)
        {
            return await serv.Get_Lite(userId);
        }



        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateUser([FromBody] User_Create request, [FromServices] IUserService serv)
        {
            try
            {
                var id = await serv.AddUser(request);
                var u = await serv.Get_Lite(id);
                return CreatedAtAction(nameof(GetUser), new { userId = id }, u);
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

        [HttpPut]
        [Route("{userId:guid}/level/{levelId:guid}")]
        public async Task<IActionResult> UpdateUserLevel([FromRoute] User_UpdateLevel request, [FromServices] IUserService serv)
        {
            try
            {
                await serv.UpdateUserLevel(request);
                return Accepted();
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

        [HttpPost]
        [Route("{userId:guid}/newUrl")]
        public async Task<IActionResult> CreateNewMiniURL([FromRoute] Guid userId, [FromBody] MiniURL_Create request, [FromServices] IUserService serv)
        {
            try
            {
                var uRef = await serv.AddMiniURLRecord(request, userId);
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
