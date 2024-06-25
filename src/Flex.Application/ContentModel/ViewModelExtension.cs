using Flex.Domain.Dtos.System.ContentModel;
using ShardingCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Flex.Application.ContentModel
{
    public static class ViewModelExtension
    {
        public static string baseLink => string.Empty;

        public static string ToImgUrl(this object obj)
        {
            return baseLink + obj;
        }
        public static string ToHtmlDeconde(this object obj)
        {
            string str = obj.ToString();
            str = HttpUtility.HtmlDecode(str);
            var matchs = Regex.Matches(str, "<img[^>]*>");
            if (matchs.Count > 0)
            {
                foreach (var item in matchs)
                {
                    var itemstr = Regex.Replace(item.ToString(), "style=\\\"(.*?)\\\"", "style=\"max-width:100% !important;height:auto !important;\"");
                    str = str.Replace(item.ToString(), itemstr);
                }
            }
            return str.Replace("<img src=\"/", "<img style=\"max-width:100% !important;height:auto !important;\" src=\"" + baseLink).Replace("<a href=\"/", "<a href=\"" + baseLink);
        }
        public static string ToHtmlEnconde(this object obj)
        {
            return HttpUtility.HtmlEncode(ToHtmlDeconde(obj));
        }
        public static List<JsonDocxImages> ToImgListUrl(this string imgurl, int mode = 1)
        {
            if (imgurl.IsEmpty())
                return new List<JsonDocxImages>();
            List<ImgListDto> ImgListDtos = JsonHelper.Json<List<ImgListDto>>(imgurl);
            var newImgListDtos = new List<JsonDocxImages>();
            foreach (var item in ImgListDtos)
            {
                JsonDocxImages ImgListDto;
                if (mode == 1)
                {
                    ImgListDto = new JsonDocxImages
                    {
                        img_src = item.imgsrc,
                        img_title = item.title,
                        img_content = item.content.Replace("\n", "<br>")
                    };
                }
                else
                {
                    ImgListDto = new JsonDocxImages()
                    {
                        img_src = "<span class='json-url'><a href='" + item.imgsrc + "' target='_blank'>" + item.imgsrc + "</a></span>",
                        img_content = "<span class='json-string'>" + item.content.Replace("\n","<br>") + "</span>",
                        img_title = "<span class='json-string'>" + item.title + "</span>"
                    };
                }
                newImgListDtos.Add(ImgListDto);
            }
            return newImgListDtos;
        }
        public static string ReplaceDefaultStr(this string str, string str1 = "\r\n", string str2 = "<br/>")
        {
            return str.Replace(str1, str2);
        }
        public static string ReplaceDefaultStrEncode(this string str, string str1 = "\r\n", string str2 = "<br/>")
        {
            return str.ReplaceDefaultStr(str1, HttpUtility.HtmlEncode(str2));
        }
    }
}
