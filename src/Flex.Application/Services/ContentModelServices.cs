using Flex.Application.Contracts.Exceptions;
using Flex.Domain.Dtos.ContentModel;
using Flex.EFSqlServer.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public class ContentModelServices : BaseService, IContentModelServices
    {
        public ContentModelServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {
        }
        public async Task<IEnumerable<ContentModelColumnDto>> ListAsync()
        {
            var responsity = _unitOfWork.GetRepository<SysContentModel>();
            var list = await responsity.GetAllAsync();
            return _mapper.Map<List<ContentModelColumnDto>>(list);
        }

        public async Task<ProblemDetails<string>> Add(AddContentModelDto model) {
            var responsity = _unitOfWork.GetRepository<SysContentModel>();
            var contentmodel = _mapper.Map<SysContentModel>(model);
            AddIntEntityBasicInfo(contentmodel);
            try
            {
                responsity.Insert(contentmodel);
                await _unitOfWork.SaveChangesAsync();
                return new ProblemDetails<string>(HttpStatusCode.OK, ErrorCodes.DataInsertSuccess.Message<ErrorCodes>());
            }
            catch (Exception ex)
            {
                return new ProblemDetails<string>(HttpStatusCode.BadRequest, ErrorCodes.DataInsertError.Message<ErrorCodes>());
            }
        }
    }
}
