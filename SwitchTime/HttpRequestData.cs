using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SwitchTime
{
    static class HttpRequestData
    {
        static HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://www.time100.ru/Moscow");
        static HttpClient httpClient = new HttpClient();
        public static async Task<string> GetDataByLink(string link)
        {
            request = new HttpRequestMessage(HttpMethod.Get, link);
            while (true)
            {
                try
                {
                    using HttpResponseMessage response = await httpClient.SendAsync(request);

                    foreach (var header in response.Headers)
                    {
                        foreach (var headerValue in header.Value)
                        {
                            string pattern = @"(\w{3},\s+\d{2}\s\w{3}\s\d{4}\s\d{2}:\d{2}:\d{2}\sGMT)\b";
                            Match match = Regex.Match(headerValue, pattern);

                            if (match.Success)
                            {
                                string dateStr = match.Groups[1].Value.Trim();
                                return dateStr;
                            }
                            break;
                        }
                        break;
                    }
                    break;
                }
                catch (Exception ex) { }
            }
            return "null";
        }

    }
}
