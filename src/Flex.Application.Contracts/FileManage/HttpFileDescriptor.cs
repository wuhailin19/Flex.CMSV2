using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.FileManage
{
    public class HttpFileDescriptor
    {
        public bool Exists { get; set; }
        public bool IsDirectory { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
        public string PhysicalPath { get; set; }
        public string RelatePath { get; set; }

        public HttpFileDescriptor()
        { }

        public HttpFileDescriptor(IFileInfo fileInfo, Func<string, string> physicalPathResolver,string webroot)
        {
            this.Exists = fileInfo.Exists;
            this.IsDirectory = fileInfo.IsDirectory;
            this.LastModified = fileInfo.LastModified;
            this.Length = fileInfo.Length;
            this.Name = fileInfo.Name;
            this.PhysicalPath = physicalPathResolver(fileInfo.Name);
            this.RelatePath = fileInfo.PhysicalPath.Replace(webroot,"").Replace("\\","/");
        }

        public IFileInfo ToFileInfo(HttpClient httpClient)
        {
            return this.Exists
                ? new HttpFileInfo(this, httpClient)
                : (IFileInfo)new NotFoundFileInfo(this.Name);
        }
    }

}
