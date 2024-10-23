using Flex.Core.Extensions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Core.Helper
{
    /// <inheritdoc />
    /// <summary> Http请求类 </summary>
    public class HttpHelper : IDisposable
    {
        private static readonly HttpClient Client = new HttpClient();
        //private static readonly ILogger Logger = LogManager.Logger<HttpHelper>();
        public static string Post(string postUrl, string postBody, Dictionary<string, string> headers = null)
        {
            try
            {
                var reqClient = new RestClient(postUrl);
                var restRequest = new RestRequest("", Method.Post);
                if (headers != null)
                {
                    restRequest.AddHeaders(headers);
                }
                restRequest.AddParameter("application/json", postBody, ParameterType.RequestBody);
                var restResponse = reqClient.Execute(restRequest);
                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    return restResponse.Content;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";
        }
        public static string Get(string postUrl, string postBody, Dictionary<string, string> headers = null)
        {
            try
            {
                var reqClient = new RestClient(postUrl);
                var restRequest = new RestRequest("", Method.Get);
                if (headers != null)
                {
                    restRequest.AddHeaders(headers);
                }
                restRequest.AddParameter("application/json", postBody, ParameterType.RequestBody);
                var restResponse = reqClient.Execute(restRequest);
                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    return restResponse.Content;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";
        }
        private static readonly IDictionary<string, string> DefaultHeaders = new Dictionary<string, string>
        {
            {"Accept-language", "zh-cn,zh;q=0.5"},
            {"Accept-Charset", "utf-8;q=0.7,*;q=0.7"},
            //{"Accept-Encoding", "gzip, deflate"},
            {"Keep-Alive", "350"},
            {"x-requested-with", "XMLHttpRequest"}
        };

        private HttpHelper()
        {
            //foreach (var header in DefaultHeaders)
            //{
            //    Client.DefaultRequestHeaders.Add(header.Key, header.Value);
            //}
        }

        /// <summary> 单例 </summary>
        public static HttpHelper Instance =>
            Singleton<HttpHelper>.Instance ?? (Singleton<HttpHelper>.Instance = new HttpHelper());

        /// <summary> 请求 </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> RequestAsync(HttpMethod method, string url, object param = null,
            object data = null, IDictionary<string, string> headers = null, HttpContent content = null)
        {
            if (param != null)
            {
                url += url.IndexOf('?') > 0 ? "&" : "?";
                url += param.ToDictionary().ToUrl();
            }

            var uri = new Uri(url);

            var req = new HttpRequestMessage(method, uri);
            if (headers != null)
            {
                foreach (var key in headers)
                {
                    req.Headers.Add(key.Key, key.Value);
                }
            }

            if (content == null && data != null)
            {
                content = new StringContent(JsonHelper.ToJson(data), Encoding.UTF8, "application/json");
            }

            if (content != null)
                req.Content = content;
            var formData = data == null ? string.Empty : "->" + data.ToDictionary().ToUrl(false);
            //Logger.Info($"HttpHelper：[{method}]{url}{formData}");
            return await Client.SendAsync(req);
        }

        /// <summary> 释放资源 </summary>
        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}
