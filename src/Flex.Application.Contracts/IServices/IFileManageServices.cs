using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Application.Contracts.FileManage;
using Flex.Domain.Dtos.FileManage;

namespace Flex.Application.Contracts.IServices
{
    public interface IFileManageServices
    {
        ProblemDetails<string> ChangeDirectory(ChangeDirectoryQueryDto directoryQueryDto);
        ProblemDetails<string> ChangeFileContent(ChangeFileContentQueryDto changeFileContentQueryDto);
        ProblemDetails<string> CreateDirectory(DirectoryQueryDto directoryQueryDto);
        List<FileModel> GetDirectoryByPath(string path);
        ProblemDetails<string> RenameDirectoryorFile(RenameDirectoryQueryDto renameDirectoryQueryDto);
    }
}
