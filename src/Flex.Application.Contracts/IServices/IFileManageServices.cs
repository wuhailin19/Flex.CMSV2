using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Application.Contracts.FileManage;

namespace Flex.Application.Contracts.IServices
{
    public interface IFileManageServices
    {
        ProblemDetails<string> CreateDirectory(string path, string dirname);
        List<FileModel> GetDirectoryByPath(string path);
    }
}
