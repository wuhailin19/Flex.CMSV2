using System.ComponentModel.DataAnnotations;

namespace Flex.Domain.Dtos.System.Menu
{
    public class MenuQuickEditDto
    {
        [Required]
        public int Id { set; get; }
        public string Status { set; get; }
        public string isMenu { set; get; }
    }
}
