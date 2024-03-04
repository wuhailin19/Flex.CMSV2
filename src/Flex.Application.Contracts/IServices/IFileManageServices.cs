using Flex.Application.Contracts.FileManage;

namespace Flex.Application.Contracts.IServices
{
    public interface IFileManageServices
    {
        List<FileModel> GetDirectoryByPath(string path);
    }
}
