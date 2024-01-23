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
        static string[] fileType = { "255216", "6677", "7173", "208207", "8297", "8075", "98109", "3780", "13780", "00", "60115" };
        static string[] deniedThumbImageExt = { ".bmp",".gif",".svg"};
        public static bool IsThumbImage(string ext) {
            if(!deniedThumbImageExt.Contains(ext))
                return true;
            return false;
        }
        public static bool IsAllowedExtension(IFormFile file)
        {
            long fileLen = file.Length;
            byte[] imgArray = new byte[fileLen];
            using var filesteam= file.OpenReadStream();
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
            */
            return fileType.Contains(fileclass);
        }
    }
}
