﻿using Flex.Application.Contracts.Basics.ResultModels;
using Flex.Domain.Dtos.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices
{
    public interface IMessageServices
    {
        Task<ProblemDetails<MessageOutputDto>> GetMessageById(int id);
        Task<PagedList<MessageTitleListDto>> GetMessageTitleListDtoAsync(int page, int pagesize);
        int GetNotReadMessageCount();
        Task<ProblemDetails<string>> SendReviewMessage(SendReviewMessageDto model);
    }
}