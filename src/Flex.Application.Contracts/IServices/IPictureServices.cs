using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.Picture;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices
{
    public interface IPictureServices
    {
        ProblemDetails<string> UploadImgService(IFormFileCollection input);
        Task<ProblemDetails<List<CatchRemoteImagesDto>>> UploadRemoteImage(string[] imgs);
    }
}
