using Newtonsoft.Json;
using System;
using Flex.Core.Extensions;
using System.ComponentModel.DataAnnotations;
using Flex.Core.JsonConvertExtension;

namespace Flex.Domain.Base
{
    public abstract class BaseLongEntity : IEntity<long>
    {
        [JsonConverter(typeof(IdToStringConverter))]
        [Display(Name ="编号")]
        public override long Id { set; get; }
    }
}
