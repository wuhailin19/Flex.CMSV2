using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.System.ContentModel
{
    /// <summary>
    /// 模型字段和表名
    /// </summary>
    public class ContentModelDto
    {
        public string TableName { set; get; }
        public string ModelName { set; get; }
        public string TableColumnStr { set; get; }
        public List<FiledModel> TableColumnList { set; get; }
        /// <summary>
        /// 子列表
        /// </summary>
        public List<ContentModelDto> childModelDtos { set; get; }
    }
    public class FiledModel
    {
        public string FiledName { set; get; }
        public string RealFiledName { set; get; }
        public string FiledDesc { set; get; }
        public string FiledMode { set; get; }
        public bool IsSearch { set; get; } = false;
        public FiledModel() { }
        public FiledModel(string filedname, string fileddesc, string filedMode, bool issearch = false, string realFiledName = null)
        {
            FiledName = filedname;
            FiledDesc = fileddesc;
            FiledMode = filedMode;
            IsSearch = issearch;
            RealFiledName = realFiledName.IsNullOrEmpty() ? filedname : realFiledName;
        }
    }
    /// <summary>
    /// 接口返回数据模型
    /// </summary>
    public class ContentModelPage
    {
        public int page { set; get; }
        public int pagesize { set; get; }
        public int dataCount { set; get; }
        public int pageCount
        {
            get
            {
                if (dataCount != 0)
                    return (dataCount - 1) / pagesize + 1;
                else
                    return 0;
            }
        }
        public Dictionary<object, object> data { set; get; }
    }

    /// <summary>
    /// 接口返回数据模型不分页
    /// </summary>
    public class ContentModelPageNoPager
    {
        public Dictionary<object, object> columnList { set; get; }
        public Dictionary<object, object> data { set; get; }
    }
}
