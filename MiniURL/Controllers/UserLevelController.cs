using Microsoft.AspNetCore.Mvc;
using MiniURL.Core;
using MiniURL.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniURL.Controllers
{
    [ApiController]
    [Route("ul")]
    public class UserLevelController
    {
        [HttpGet]
        [Route("")]
        public async IAsyncEnumerable<UserLevel> GetUserLevels([FromServices] MiniURLContext db)
        {
            await foreach (var level in db.UserLevels.AsAsyncEnumerable())
            {
                yield return level;
            }
        }
    }
}
