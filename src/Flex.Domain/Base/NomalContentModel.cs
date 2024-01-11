namespace Flex.Domain.Base
{
    public class NormalContentModel : BaseIntEntity
    {
        public int ParentId { set; get; }
        public int SiteId { set; get; }
        public string Title { set; get; }
        public string? KeyWord { set; get; }
        public string? Description { set; get; }
        public string? SimpleTitle { set; get; }
        public int OrderId { set; get; }
        public int Hits { set; get; }
        /// <summary>
        /// 置顶
        /// </summary>
        public bool IsTop { set; get; } = false;
        public bool IsRecommend { set; get; } = false;
        public bool IsHot { set; get; } = false;
        public bool IsSlide { set; get; } = false;
        public bool IsShow { set; get; } = false;
        /// <summary>
        /// 动态字段
        /// </summary>
        public string DynamicField { set; get; }
    }
}
