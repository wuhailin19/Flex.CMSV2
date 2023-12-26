using System.ComponentModel.DataAnnotations;

namespace Flex.Domain.Dtos.Admin
{
    public class AdminQuickEditDto
    {
        [Required]
        public long Id { set; get; }
        public string AllowMultiLogin { set; get; }
        public string Islock { get; set; }
    }
}
