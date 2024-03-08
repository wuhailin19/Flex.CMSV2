using Microsoft.Extensions.FileProviders;

namespace Flex.Application.Contracts.FileManage
{
    public class HttpDirectoryContentsDescriptor
    {
        public bool Exists { get; set; }
        public IEnumerable<HttpFileDescriptor> FileDescriptors { get; set; }

        public HttpDirectoryContentsDescriptor()
        {
            this.FileDescriptors = new HttpFileDescriptor[0];
        }

        public HttpDirectoryContentsDescriptor(IDirectoryContents directoryContents, Func<string, string> physicalPathResolver,string webroot)
        {
            this.Exists = directoryContents.Exists;
            this.FileDescriptors = directoryContents.Select(_ => new HttpFileDescriptor(_, physicalPathResolver, webroot));
        }
    }

}
