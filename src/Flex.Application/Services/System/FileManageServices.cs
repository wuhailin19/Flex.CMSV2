using Flex.Application.Contracts.FileManage;
using Flex.Domain.Dtos.FileManage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using ShardingCore.Extensions;
using SkiaSharp;

namespace Flex.Application.Services
{
    public class FileManageServices : IFileManageServices
    {
        private IFileProvider _fileProvider;
        IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _Context;

        public FileManageServices(IHttpContextAccessor Context, IWebHostEnvironment env, IFileProvider fileProvider)
        {
            _Context = Context;
            _env = env;
            _fileProvider = fileProvider;
        }
        private Func<string, string> CreatePhysicalPathResolver(HttpContext context, bool isDirRequest)
        {
            string schema = context.Request.IsHttps ? "https" : "http";
            string host = context.Request.Host.Host;
            int port = context.Request.Host.Port ?? 8080;
            string pathBase = context.Request.PathBase.ToString().Trim('/');
            string path = context.Request.Path.ToString().Trim('/');

            pathBase = string.IsNullOrEmpty(pathBase) ? string.Empty : $"/{pathBase}";
            path = string.IsNullOrEmpty(path) ? string.Empty : $"/{path}";

            return isDirRequest
                ? (Func<string, string>)(name => $"{schema}://{host}:{port}{pathBase}{path}/{name}")
                : name => $"{schema}://{host}:{port}{pathBase}{path}";
        }

        public ProblemDetails<string> CreateDirectory(DirectoryQueryDto directoryQueryDto)
        {
            var dirpath = _env.WebRootPath + directoryQueryDto.path.TrimEnd('/') + "/" + directoryQueryDto.folder;
            if (Directory.Exists(dirpath))
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "文件夹已存在");
            }
            try
            {
                Directory.CreateDirectory(dirpath);
                return new ProblemDetails<string>(HttpStatusCode.OK, "创建成功");
            }
            catch (Exception)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "文件夹创建失败");
            }
        }

        public ProblemDetails<string> ChangeDirectory(ChangeDirectoryQueryDto directoryQueryDto)
        {
            var oldpath = _env.WebRootPath + directoryQueryDto.oldpath.TrimEnd('/');
            var newpath = _env.WebRootPath + directoryQueryDto.newpath.TrimEnd('/') + "/" + directoryQueryDto.name;
            if (!directoryQueryDto.Isoverride)
            {
                if (File.Exists(newpath))
                {
                    return new ProblemDetails<string>(HttpStatusCode.IMUsed, "文件已存在，是否覆盖");
                }
                if (Directory.Exists(newpath))
                {
                    return new ProblemDetails<string>(HttpStatusCode.BadRequest, "文件夹已存在，需删除目标文件夹或重命名");
                }
            }
            try
            {
                if (directoryQueryDto.type == "directory")
                    Directory.Move(oldpath, newpath);
                else
                    File.Move(oldpath, newpath, directoryQueryDto.Isoverride);
                return new ProblemDetails<string>(HttpStatusCode.OK, "移动成功");
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "移动失败");
            }
        }

        public ProblemDetails<string> RenameDirectoryorFile(RenameDirectoryQueryDto renameDirectoryQueryDto)
        {
            var olddirpath = _env.WebRootPath + renameDirectoryQueryDto.path;
            var newpath = _env.WebRootPath + renameDirectoryQueryDto.folder.TrimEnd('/') + "/" + renameDirectoryQueryDto.newName;
            if (!Directory.Exists(olddirpath) && !File.Exists(olddirpath))
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "文件夹或文件不存在");
            }
            try
            {
                if (renameDirectoryQueryDto.type == "directory")
                    Directory.Move(olddirpath, newpath);
                else
                    File.Move(olddirpath, newpath);

                return new ProblemDetails<string>(HttpStatusCode.OK, "修改成功");
            }
            catch (Exception)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "修改失败");
            }
        }

        public ProblemDetails<string> ChangeFileContent(ChangeFileContentQueryDto changeFileContentQueryDto)
        {
            var olddirpath = _env.WebRootPath + changeFileContentQueryDto.path;
            if (!File.Exists(olddirpath))
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "文件不存在");
            }
            try
            {
                File.WriteAllText(olddirpath, changeFileContentQueryDto.content);
                return new ProblemDetails<string>(HttpStatusCode.OK, "保存成功");
            }
            catch (Exception)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, "保存失败");
            }
        }

        public List<FileModel> GetDirectoryByPath(string path)
        {
            var dirContents = _fileProvider.GetDirectoryContents(path);
            bool isDirRequest = false;
            if (path.Contains("dir-meta"))
                isDirRequest = true;
            var dirDecriptor = new HttpDirectoryContentsDescriptor(dirContents, CreatePhysicalPathResolver(_Context.HttpContext, isDirRequest), _env.WebRootPath);
            List<FileModel> files = new List<FileModel>();
            if (dirDecriptor != null)
            {
                if (!dirDecriptor.Exists)
                    return files;
                foreach (var item in dirDecriptor.FileDescriptors)
                {
                    FileModel fileModel = new FileModel();
                    fileModel.filepath = item.PhysicalPath;
                    fileModel.name = item.Name;
                    fileModel.path = item.RelatePath;
                    fileModel.lastModify = item.LastModified.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    var extension = Path.GetExtension(item.Name);

                    fileModel.thumb = item.RelatePath;
                    fileModel.isdir = item.IsDirectory ? 1 : 0;
                    decimal sizef = item.Length / 1024;
                    fileModel.length = (sizef > 1024 ? decimal.Round(sizef / 1024, 2) + "M" : decimal.Round(sizef, 2) + "KB").ToString();
                    fileModel.type = item.IsDirectory ? "directory" : extension.Replace(".", "");
                    files.Add(fileModel);
                }
            }
            return files;
        }
        /// <summary>
        /// 判断是否是图片
        /// </summary>
        /// <param name="ext">文件后缀</param>
        /// <returns></returns>
        public static bool IsImage(string ext)
        {
            switch (ext)
            {
                case ".ico":
                    return true;
                case ".bmp":
                    return true;
                case ".png":
                    return true;
                case ".gif":
                    return true;
                case ".image":
                    return true;
                default:
                    return false;
            }
        }
    }
}
