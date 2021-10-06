using Microsoft.EntityFrameworkCore;
using MiniURL.Core;
using MiniURL.Services.Exceptions;
using MiniURL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Services.Implementations
{
    public class MiniUrlService : IMiniUrlService, IDisposable, IAsyncDisposable
    {
        private readonly MiniURLContext db;
        public MiniUrlService(MiniURLContext db)
        {
            this.db = db;
        }
        public async Task<string> GetUrlByRef(string urlRef)
        {
            var mUrl = await db.MiniUrls
                .Where(FilterByReference(urlRef))
                .FirstOrDefaultAsync();
            if (mUrl == null)
                throw new NotFoundException(typeof(Core.Models.MiniURL), new { Reference = urlRef });
            return mUrl.Url;
        }
        public async Task<Core.Models.MiniURL> GetByRef(string urlRef)
        {
            var mUrl = await db.MiniUrls
                .Where(FilterByReference(urlRef))
                .FirstOrDefaultAsync();
            if (mUrl == null)
                throw new NotFoundException(typeof(Core.Models.MiniURL), new { Reference = urlRef });
            return mUrl;
        }

        private static Expression<Func<Core.Models.MiniURL, bool>> FilterByReference(string urlRef) => x => x.Reference == urlRef &&
                    (!x.ExpiresOn.HasValue || x.ExpiresOn > DateTimeOffset.UtcNow);

        public void Dispose()
        {
            db.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await db.DisposeAsync();
        }
    }
}
