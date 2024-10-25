using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Flex.Application.Word
{
    public class WordHelper
    {

        public static async Task<string> ReturnHtmlAsync(Stream inputStream, string filedate, string basepath)
        {
            using var document = new Document(inputStream);
            using var outputStream = new MemoryStream(); // 使用内存流来保存HTML内容
            string htmlstr;


            document.SaveToStream(outputStream, FileFormat.Html); // 保存到内存流
            outputStream.Position = 0; // 重置流的位置


            // 从内存流中读取HTML内容
            using var reader = new StreamReader(outputStream);
            htmlstr = await reader.ReadToEndAsync(); // 异步读取内容
            htmlstr = htmlstr.Replace("src=\"", $"src=\"{basepath}/"); // 替换路径
            htmlstr = htmlstr.Replace("&#xa0;", $""); // 替换路径
            htmlstr = htmlstr.Replace("_images/_img", $"{filedate}_images/{filedate}_img"); // 替换路径
            return htmlstr;
        }

        public static async Task<string> ReturnHtmlAsync(Stream stream, string path, string filedate, string basepath)
        {
            string newpath = Path.ChangeExtension(path, "html");
            string htmlstr = string.Empty;
            Document document = new Document(stream);

            document.SaveToFile(newpath, FileFormat.Html);
            htmlstr = await ReturnHtmlAsync(stream, filedate, basepath);

            return htmlstr;
        }
    }
}
