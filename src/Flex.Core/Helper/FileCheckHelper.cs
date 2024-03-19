using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Core.Helper
{
    public class FileCheckHelper
    {
        /*文件扩展名说明
         jpg：255216
         bmp：6677
         gif：7173
         xls,doc,ppt：208207
         rar：8297
         zip：8075
         txt：98109
         pdf：3780
         png:13780
         svg:60115
         js:1310
         js:4949
         css:98111
         css:5050
         */
        static string[] fileType = { "255216", "6677", "7173", "208207", "8297", "8075", "98109", "3780", "13780", "00", "60115" };
        static string[] managefileType = { "255216", "6677", "7173", "208207", "8297", "8075", "98109", "3780", "13780", "00", "60115", "1310", "98111", "5050", "4949" };
        static string[] deniedThumbImageExt = { ".bmp", ".gif", ".svg" };
        public static bool IsThumbImage(string ext)
        {
            if (!deniedThumbImageExt.Contains(ext))
                return true;
            return false;
        }
        /// <summary>
        /// 用于文件管理处
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsAllowedFileManageExtension(IFormFile file)
        {
            long fileLen = file.Length;
            byte[] imgArray = new byte[fileLen];
            using var filesteam = file.OpenReadStream();
            BinaryReader r = new BinaryReader(filesteam);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();
            }
            catch
            {
                return false;
            }
            r.Close();

            return managefileType.Contains(fileclass);
        }
        /// <summary>
        /// 默认使用
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsAllowedExtension(IFormFile file)
        {
            long fileLen = file.Length;
            byte[] imgArray = new byte[fileLen];
            using var filesteam = file.OpenReadStream();
            BinaryReader r = new BinaryReader(filesteam);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();
            }
            catch
            {
                return false;
            }
            r.Close();

            return fileType.Contains(fileclass);
        }

        public static bool IsImage(string fileName)
        {
            if (!fileName.Contains("."))
                return false;
            var ext = Path.GetExtension(fileName).ToLower();
            switch (ext)
            {
                case "": return false;
                case ".bmp": return true;
                case ".jpg": return true;
                case ".png": return true;
                case ".gif": return true;
                case ".svg": return true;
                default: return false;
            }
        }
    }
}
