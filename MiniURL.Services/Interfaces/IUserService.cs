using MiniURL.Services.Models.Request;
using MiniURL.Services.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User_Lite> Get_Lite(Guid Id);
        public IAsyncEnumerable<User_Lite> GetAll_Lite();
        public Task<Guid> AddUser(User_Create request);
        public Task UpdateUserLevel(User_UpdateLevel request);
        public Task<string> AddMiniURLRecord(MiniURL_Create request, Guid? userId = null);
    }
}
