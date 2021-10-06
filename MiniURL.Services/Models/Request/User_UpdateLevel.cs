using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Services.Models.Request
{
    public class User_UpdateLevel
    {
        public Guid UserId { get; set; }
        public Guid LevelId { get; set; }
    }
}
