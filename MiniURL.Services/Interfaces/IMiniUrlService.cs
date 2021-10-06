using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Services.Interfaces
{
    public interface IMiniUrlService
    {
        public Task<string> GetUrlByRef(string urlRef);
        public Task<Core.Models.MiniURL> GetByRef(string urlRef);
    }
}
