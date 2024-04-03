using Flex.Application.Contracts.Exceptions;
using Flex.Core.Helper.ImgFiles;
using Flex.Core.Helper.UploadHelper;
using Flex.Domain.Dtos.Picture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

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

        public async Task<ProblemDetails<List<CatchRemoteImagesDto>>> UploadRemoteImage(string[] imgs)
        {
            try
            {
                var imgremoteresult = new List<CatchRemoteImagesDto>();
                foreach (var item in imgs)
                {
                    var result = UploadImgService(await DownloadAndSaveImageAsync(item));
                    if (result.IsSuccess)
                        imgremoteresult.Add(new CatchRemoteImagesDto { source = item, url = result.Detail, state = "SUCCESS" });
                    else
                        imgremoteresult.Add(new CatchRemoteImagesDto { source = item, url = result.Detail, state = "ERROR" });
                }
                return new ProblemDetails<List<CatchRemoteImagesDto>>(HttpStatusCode.OK, imgremoteresult);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IFormFileCollection> DownloadAndSaveImageAsync(string imageUrl)
        {
            // 使用 HttpClient 下载远程图片
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(imageUrl);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{imageUrl} 下载失败");
                }
                // 通过 Split 方法拆分 URL，并获取最后一个部分（即文件名部分）
                string[] parts = imageUrl.Split('/');
                string fileName = parts[parts.Length - 1];
                IFormFileCollection fileCollection = new FormFileCollection();
                if (!FileCheckHelper.IsImage(fileName))
                    return fileCollection;
                // 读取响应内容
                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                // 创建一个虚拟的文件对象
                IFormFile file = new FormFile(new MemoryStream(imageBytes), 0, imageBytes.Length, "image", fileName);

                // 创建 IFormFileCollection 对象，并添加虚拟文件对象
                fileCollection = new FormFileCollection { file };
                return fileCollection;
            }
        }
        public ProblemDetails<string> UploadImgService(IFormFileCollection input)
        {
            if (input.Count == 0)
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.UploadFail.GetEnumDescription());
            var file = input[0];
            if (!FileCheckHelper.IsAllowedExtension(file))
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
                using (FileStream fileStream = new FileStream(savepath, FileMode.Create))
                { file.CopyTo(fileStream); }
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
                if (IsSuccess && FileCheckHelper.IsThumbImage(extension))
                    PlatformsPictureOperation.MakeThumb(savepath, 200, 200, ImageThumbEnum.Cut);
            }
        }
    }
}
