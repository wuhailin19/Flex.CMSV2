using Flex.Application.Contracts.Basics.ResultModels;

namespace Flex.Application.Contracts.IServices
{
    public interface ITranscationServices
    {
        ProblemDetails<string> TestAddTranscation();

        Task Insert();
        bool InsertAsync();
    }
}
