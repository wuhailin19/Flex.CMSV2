using Flex.Application.Contracts.Exceptions;
using Flex.Core.Helper.ImgFiles;
using Flex.Core.Helper.UploadHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public class PictureServices : BaseService, IPictureServices
    {
        IWebHostEnvironment _env;
        string basePath = string.Empty;
        /// <summary>
        /// 图片储存位置
        /// </summary>
        private string imgpath = $"/upload/images/{DateTime.Now.ToDefaultDateTimeStr()}";
        public PictureServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims, IWebHostEnvironment env) 
            : base(unitOfWork, mapper, idWorker, claims)
        {
            _env = env;
            basePath = _env.WebRootPath;
        }
        public ProblemDetails<string> UploadImgService(IFormFileCollection input)
        {
            if (input.Count == 0)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.UploadFail.GetEnumDescription());
            var file = input[0];
            if(!FileCheckHelper.IsAllowedExtension(file))
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.UploadTypeDenied.GetEnumDescription());
            string uniqueFileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + new Random().Next(1000, 9999).ToString();
            var extension = Path.GetExtension(file.FileName);

            if (!Directory.Exists(Path.Combine(basePath + imgpath)))
                Directory.CreateDirectory(Path.Combine(basePath + imgpath));
            string imgrelationpath = imgpath + "/" + uniqueFileName + extension;
            string savepath = Path.Combine(basePath + imgrelationpath);
            bool IsSuccess = false;
            try
            {
                using FileStream fileStream = new FileStream(savepath, FileMode.Create);
                file.CopyTo(fileStream);
                //ImgFile imgFile = new ImgFile();
                //imgFile.DeafultImgPath = savepath;
                //await _repository.InsertAsync(imgFile);
                IsSuccess = true;
                return new ProblemDetails<string>(HttpStatusCode.OK, imgrelationpath);
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                return new ProblemDetails<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                if (IsSuccess)
                    PlatformsPictureOperation.MakeThumb(savepath, 200, 200, ImageThumbEnum.Cut);
            }
        }
    }
}
