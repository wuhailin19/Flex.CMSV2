﻿using Flex.Domain.Dtos.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.ColumnContent
{
    public class OutputContentAndWorkFlowDto
    {
        public IEnumerable<StepActionButtonDto> stepActionButtonDto { set; get; }
        public dynamic Content{ set; get; }
        public bool NeedReview{ set; get; }
        /// <summary>
        /// 判断归属
        /// </summary>
        public bool OwnerShip { set; get; }
    }
}
