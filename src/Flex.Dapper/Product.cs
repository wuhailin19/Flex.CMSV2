using Dapper.Contrib.Extensions;

namespace Flex.Dapper
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
