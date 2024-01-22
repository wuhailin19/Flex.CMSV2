using Flex.Application.Contracts.Basics.ResultModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices
{
    public interface IFileServices
    {
        ProblemDetails<string> UploadFilesService(IFormFileCollection input);
    }
}
