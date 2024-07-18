using Flex.Application.Contracts.Exceptions;
using Flex.Core.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public class FileServices : BaseService, IFileServices
    {
        IWebHostEnvironment _env;
        string basePath = string.Empty;
        /// <summary>
        /// 文件储存位置
        /// </summary>
        private string uploadsFolder = CurrentSiteInfo.SiteUploadPath + $"/upload/files/{DateTime.Now.ToDefaultDateTimeStr()}";
        public FileServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims, IWebHostEnvironment env)
            : base(unitOfWork, mapper, idWorker, claims)
        {
            _env = env;
            basePath = _env.WebRootPath;
        }
        /// <summary>
        /// 文件管理处上传到指定位置
        /// </summary>
        /// <param name="input"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public ProblemDetails<string> UploadFilesToPathService(IFormFileCollection input, string path)
        {
            if (input == null) return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.UploadFail.GetEnumDescription()); ;
            var file = input[0];
           
            if (!FileCheckHelper.IsAllowedFileManageExtension(file))
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.UploadTypeDenied.GetEnumDescription());
            if (!Directory.Exists(Path.Combine(basePath + path)))
                Directory.CreateDirectory(Path.Combine(basePath + path));
            string filePath = path + "/" + file.FileName;
            string savePath = Path.Combine(basePath + filePath);
            try
            {
                using FileStream fileStream = new FileStream(savePath, FileMode.Create);
                file.CopyTo(fileStream);
                return Problem<string>(HttpStatusCode.OK, filePath);
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.UploadFail.GetEnumDescription(), ex);
            }
        }
        /// <summary>
        /// 普通上传
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ProblemDetails<string> UploadFilesService(IFormFileCollection input)
        {
            if (input == null) return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.UploadFail.GetEnumDescription()); ;
            var file = input[0];
            if (!FileCheckHelper.IsAllowedExtension(file))
                return Problem<string>(HttpStatusCode.BadRequest, ErrorCodes.UploadTypeDenied.GetEnumDescription());
            if (!Directory.Exists(Path.Combine(basePath + uploadsFolder)))
                Directory.CreateDirectory(Path.Combine(basePath + uploadsFolder));
            string uniqueFileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + new Random().Next(1000, 9999).ToString() + Path.GetExtension(file.FileName);
            string filePath = uploadsFolder + "/" + uniqueFileName;
            string savePath = Path.Combine(basePath + filePath);
            try
            {
                using FileStream fileStream = new FileStream(savePath, FileMode.Create);
                file.CopyTo(fileStream);
                return Problem<string>(HttpStatusCode.OK, filePath);
            }
            catch (Exception ex)
            {
                return Problem<string>(HttpStatusCode.InternalServerError, ErrorCodes.UploadFail.GetEnumDescription(), ex);
            }
        }
    }
}
