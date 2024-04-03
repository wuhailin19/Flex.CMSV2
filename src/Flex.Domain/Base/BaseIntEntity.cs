using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SqlSugar;

namespace Flex.Domain.Base
{
    public abstract class BaseIntEntity : IEntity<int>
    {
        [Key] //主键 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //设置自增
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public override int Id { get; set; }
    }
}
