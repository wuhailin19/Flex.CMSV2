namespace Flex.Domain.Entities
{
    public class SysMenu : BaseIntEntity, EntityContext
    {
        public string Name { get; set; }
        public string Icode { get; set; }
        public int ParentID { get; set; }
        public bool ShowStatus { get; set; }
        public bool isMenu { get; set; }
        public bool IsControllerUrl { get; set; }
        public string LinkUrl { get; set; }
        public string FontSort { get; set; }
        public int Level { get; set; }
        public int OrderId { get; set; }
    }
}
