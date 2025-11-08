using Microsoft.AspNetCore.Http;
using Nexora.Core.Common.Extensions;
using System.Net;

namespace Nexora.Core.Common.Helpers
{
    public static class IpAddressHelper
    {
        public static string Normalize(string ipAddress)
        {
            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                ipAddress = ipAddress.Trim().Trim('"', '\'');

                if (ipAddress.StartsWith("[") && ipAddress.Contains("]"))
                {
                    var end = ipAddress.IndexOf(']');
                    ipAddress = ipAddress.Substring(1, end - 1);
                }
                else
                {
                    if (ipAddress.Contains('.') && ipAddress.Count(c => c == ':') == 1)
                    {
                        ipAddress = ipAddress[..ipAddress.LastIndexOf(':')];
                    }
                }

                if (IPAddress.TryParse(ipAddress, out var ip))
                {
                    if (ip.IsIPv4MappedToIPv6)
                    {
                        return ip.MapToIPv4().ToString();
                    }

                    return ip.ToString();
                }
            }

            return string.Empty;
        }

        public static string GetClientIp(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var xff) && !string.IsNullOrEmpty(xff))
            {
                var ipAddress = xff.ToString().Split(',')[0].Trim().Trim('"', '\'');

                return IpAddressHelper.Normalize(ipAddress!);
            }
            else if (context.Request.Headers.TryGetValue("X-ARR-ClientIP", out var arr) && !string.IsNullOrEmpty(arr))
            {
                return IpAddressHelper.Normalize(arr!);
            }
            else
            {
                var remote = context.Connection.RemoteIpAddress;

                if (remote.HasValue() && remote!.IsIPv4MappedToIPv6)
                {
                    var ip = remote.MapToIPv4().ToString();

                    return ip;
                }

                return string.Empty;
            }
        }
    }
}