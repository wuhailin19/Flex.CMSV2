using Flex.Core.Extensions;
using Flex.Core.Helper;
using System.Net;

namespace Flex.Application.Contracts.Basics.ResultModels
{
    public class ProblemDetails<T>
    {
        public ProblemDetails(HttpStatusCode? statusCode, string detail = null)
        {
            var status = statusCode.HasValue ? (int)statusCode.Value : (int)HttpStatusCode.BadRequest;
            Status = status;
            Detail = detail;
        }
        public ProblemDetails(HttpStatusCode? statusCode, T content, string detail = null)
        {
            var status = statusCode.HasValue ? (int)statusCode.Value : (int)HttpStatusCode.BadRequest;
            Status = status;
            Detail = detail;
            Content = content;
        }
        public override string ToString() => JsonHelper.ToJson(this);
        public int Status { get; set; }
        public T Content { get; set; }
        public bool IsSuccess => Detail.IsNullOrEmpty() || Status == HttpStatusCode.OK.ToInt();
        public string Detail { get; set; }
    }
}
