using MiniURL.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Core.ValueObjects
{
    public record Url
    {
        public string Scheme { get; private set; }
        public string Host { get; private set; }
        public string Path { get; private set; }
        public string Query { get; private set; }
        internal Url() { }
        public Url(string url)
        {
            if (!Uri.TryCreate(url.ToLower(), UriKind.Absolute, out var res))
                throw new InvalidUrlException($"Unable to parse Url: {url}");
            Scheme = res.Scheme;
            Host = res.Host;
            Path = res.AbsolutePath;
            Query = res.Query;
        }

        public override string ToString()
        {
            return $"{Scheme}://{Host}{Path}{(string.IsNullOrEmpty(Query) ? "" : $"{Query}")}";
        }

        public static implicit operator string(Url u) => u.ToString();
        public static implicit operator Url(string s) => new(s);
    }
}
