using Flex.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Core.Config
{
    public class ServerConfig
    {
        public static string ImageServerUrl => "ImageServerUrl".Config(string.Empty);
        public static string FileServerUrl => "FileServerUrl".Config(string.Empty);
        public static string ServerUrl => "ServerUrl".Config(string.Empty);
    }
}
