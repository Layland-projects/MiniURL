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
    [Route("[controller]")]
    public class DemoController : ControllerBase 
    {
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Setup([FromServices] MiniURLContext db)
        {
            if (db.Users.ToList().Count > 0)
                return Ok();
            var users = new User[] {
                new User("Mr", "Dan", "Layland", "dan.test@me.com", new UserLevel("Guest", "default account level", 1)),
                new User("Mr", "Ben", "Jackson", "ben.test@me.com", new UserLevel("Regular", "A signed up user", 5)),
                new User("Mrs", "Marge", "Johnson", "marge.test@me.com", new UserLevel("Premium", "A paying user", null))
            };
            db.Users.AddRange(users);
            await db.SaveChangesAsync();
            return CreatedAtAction("GetUser", "User", new { userId = users.First().Id }, null);
        }
    }
}
