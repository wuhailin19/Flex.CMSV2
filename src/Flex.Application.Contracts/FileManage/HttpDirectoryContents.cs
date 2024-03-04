﻿using Microsoft.Extensions.FileProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.FileManage
{
    public class HttpDirectoryContents : IDirectoryContents
    {
        private IEnumerable<IFileInfo> _fileInfos;
        public bool Exists { get; private set; }

        public HttpDirectoryContents(HttpDirectoryContentsDescriptor descriptor, HttpClient httpClient)
        {
            this.Exists = descriptor.Exists;
            _fileInfos = descriptor.FileDescriptors.Select(file => file.ToFileInfo(httpClient));
        }

        public IEnumerator<IFileInfo> GetEnumerator() => _fileInfos.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _fileInfos.GetEnumerator();
    }
}
