using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.WhiteFileds
{
    public class ColumnContentUpdateFiledConfig
    {
        //默认加载字段
        public const string defaultFields = "IsTop,IsRecommend,IsHot,IsHide,IsSilde,SeoTitle,KeyWord,Description" +
            ",Title,Id,AddTime,LastEditDate,StatusCode,ReviewAddUser,AddUserName,LastEditUserName,OrderId,ParentId,ReviewStepId,ContentGroupId,MsgGroupId,";

        //修改历史版本字段
        public const string updatehistoryFields = "IsTop,IsRecommend,IsHot,IsHide,IsSilde,SeoTitle,KeyWord,Description" +
            ",Title,Id,AddTime,AddUser,AddUserName,ParentId,ContentGroupId,";

        //修改审核相关字段
        public static List<string> reviewContentFields = new List<string> { "ParentId", "Id", "StatusCode", "ReviewStepId", "ReviewAddUser", "MsgGroupId" };

        //简单修改的字段
        public static List<string> simpleUpdateFields = new List<string> { "ParentId", "OrderId", "Id", "IsTop", "Ids", "IsRecommend", "IsHot", "IsHide", "IsSilde" };
    }
}
