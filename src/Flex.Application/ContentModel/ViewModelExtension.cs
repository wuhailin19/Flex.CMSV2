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
        public static List<ImgListDto> ToImgListUrl(this string imgurl, int mode = 1)
        {
            if (imgurl.IsEmpty())
                return new List<ImgListDto>();
            string[] list = imgurl.Split(new string[] { "_bigsplit_" }, StringSplitOptions.None);
            if (list.Length >= 1)
            {
                List<ImgListDto> ImgListDtos = new List<ImgListDto>();
                for (int i = 0; i < list.Length; i++)
                {
                    string linkurl = list[i].Split(new string[] { "_smallsplit_" }, StringSplitOptions.None)[2];
                    if (linkurl.IsEmpty() || linkurl == "http://")
                        linkurl = "javascript:;";
                    ImgListDto ImgListDto = new ImgListDto();
                    if (mode == 1)
                    {
                        ImgListDto = new ImgListDto()
                        {
                            imglist_img = baseLink + list[i].Split(new string[] { "_smallsplit_" }, StringSplitOptions.None)[0],
                            imglist_title = list[i].Split(new string[] { "_smallsplit_" }, StringSplitOptions.None)[1].ReplaceDefaultStr(),
                            imglist_link = linkurl
                        };
                    }
                    else
                    {
                        ImgListDto = new ImgListDto()
                        {
                            imglist_img = "<span class='json-url'><a href='" + baseLink + list[i].Split(new string[] { "_smallsplit_" }, StringSplitOptions.None)[0] + "' target='_blank'>" + baseLink + list[i].Split(new string[] { "_smallsplit_" }, StringSplitOptions.None)[0] + "</a></span>",
                            imglist_title = "<span class='json-string'>" + list[i].Split(new string[] { "_smallsplit_" }, StringSplitOptions.None)[1].ReplaceDefaultStr() + "</span>",
                            imglist_link = "<span class='json-string'><a href='" + linkurl + "' target='_blank'>" + linkurl + "</a></span>"
                        };
                    }
                    ImgListDtos.Add(ImgListDto);
                }
                return ImgListDtos;
            }
            return new List<ImgListDto>();
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
