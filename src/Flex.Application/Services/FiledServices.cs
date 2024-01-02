using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Services
{
    public class FiledServices : BaseService, IFiledServices
    {
        public FiledServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims) 
            : base(unitOfWork, mapper, idWorker, claims)
        {
        }
    }
}
