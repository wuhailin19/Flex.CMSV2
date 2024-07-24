using Flex.Application.Contracts.ISignalRBus.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.ISignalRBus.Model
{
    public class TaskModel<T>
    {
        public long TaskId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public GlobalTaskStatus Status { get; set; }
        /// <summary>
        /// 进度
        /// </summary>
        public decimal Percent { get; set; }
        public string StatusString { get; set; }
        /// <summary>
        /// 区分任务类型
        /// </summary>
        public T TaskCate { get; set; }
        public DateTime AddTime { set; get; }
    }
}
