﻿using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;

namespace TumblThree.Domain.Models
{
    [Export(typeof(IUrlValidator))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
    public class UrlValidator : IUrlValidator
    {
        private static readonly Regex tumbexRegex = new Regex("(http[A-Za-z0-9_/:.]*www.tumbex.com[A-Za-z0-9_/:.-]*tumblr/)");
        private static readonly Regex urlRegex = new Regex(@"^(?:http(s)?:\/\/){1}?[\w.-]+(?:\.[\w\.-]+)+[/]??$");
        private static readonly Regex twitterRegex = new Regex("(^https?://twitter.com/[A-Za-z0-9_]+$)");
        private static readonly Regex newTumblRegex = new Regex(@"(^https?://[\w.-]+newtumbl.com[/]??$)");
        private static readonly Regex tumblrUrl = new Regex(@"^(?:http(s)?:\/\/)[\w.-]+.tumblr.com[/]??$");
        private static readonly Regex tumblrUrlNew = new Regex(@"^(?:http(s)?:\/\/)www.tumblr.com\/[^\/]+$");

        private static bool CheckNullLengthProtocolAndWhiteSpace(string url, int minLength)
        {
            return url != null && (minLength <= 0 || url.Length > minLength) && !url.Any(char.IsWhiteSpace) &&
                (url.StartsWith("http://", true, null) || url.StartsWith("https://", true, null));
        }

        public static bool IsValidTumblrUrlInNewFormat(string url)
        {
            return CheckNullLengthProtocolAndWhiteSpace(url, 0) && tumblrUrlNew.IsMatch(url);
        }

        public bool IsValidTumblrUrl(string url)
        {
            var b = CheckNullLengthProtocolAndWhiteSpace(url, 18) &&
                //url.Contains(".tumblr.com") &&
                //(!url.Contains("//www.tumblr.com") || url.EndsWith("www.tumblr.com/likes", true, null)) &&
                !url.Contains(".media.tumblr.com") &&
                (tumblrUrl.IsMatch(url) || tumblrUrlNew.IsMatch(url));
            return b;
        }

        public bool IsTumbexUrl(string url)
        {
            return tumbexRegex.IsMatch(url);
        }

        public bool IsValidTumblrHiddenUrl(string url)
        {
            return CheckNullLengthProtocolAndWhiteSpace(url, 38) && url.Contains("www.tumblr.com/dashboard/blog/");
        }

        public bool IsValidTumblrLikesUrl(string url)
        {
            return CheckNullLengthProtocolAndWhiteSpace(url, 0) && url.Contains("www.tumblr.com/likes");
        }

        public bool IsValidTumblrLikedByUrl(string url)
        {
            return CheckNullLengthProtocolAndWhiteSpace(url, 31) && url.Contains("www.tumblr.com/liked/by/");
        }

        public bool IsValidTumblrSearchUrl(string url)
        {
            return CheckNullLengthProtocolAndWhiteSpace(url, 29) && url.Contains("www.tumblr.com/search/");
        }

        public bool IsValidTumblrTagSearchUrl(string url)
        {
            return CheckNullLengthProtocolAndWhiteSpace(url, 29) && url.Contains("www.tumblr.com/tagged/");
        }

        public bool IsValidUrl(string url)
        {
            return CheckNullLengthProtocolAndWhiteSpace(url, 0) && urlRegex.IsMatch(url);
        }

        public bool IsValidTwitterUrl(string url)
        {
            return url != null && twitterRegex.IsMatch(url) && !url.EndsWith("/home");
        }

        public bool IsValidNewTumblUrl(string url)
        {
            return url != null && newTumblRegex.IsMatch(url);
        }

        public string AddHttpsProtocol(string url)
        {
            if (url == null)
            {
                return string.Empty;
            }

            if (!url.StartsWith("http"))
            {
                return "https://" + url;
            }

            return url;
        }
    }
}
