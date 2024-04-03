using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.Admin;
using Flex.Domain.Dtos.AuthCode;

namespace Flex.Application.Contracts.IServices
{
    public interface IAccountServices
    {
        bool CheckAuthCode(AuthCodeInputDto authCodeInput);
        Task<ProblemDetails<UserData>> LoginAuthorAsync(AdminLoginDto adminLoginDto);
    }
}
