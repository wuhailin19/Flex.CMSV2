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
            ",Title,Id,AddTime,LastEditDate,StatusCode,ReviewAddUser,AddUserName,LastEditUserName,OrderId,ParentId" +
            ",ReviewStepId,ContentGroupId,MsgGroupId,SiteId,PId,RefLinkClassId,PublishTime,";
        //默认导入字段白名单
        public const string defaultImportFields = "IsTop,IsRecommend,IsHot,IsHide,IsSilde,SeoTitle,KeyWord,Description" +
            ",Title,AddTime,LastEditDate,StatusCode,ReviewAddUser,AddUserName,LastEditUserName,OrderId" +
            ",ReviewStepId,ContentGroupId,MsgGroupId,SiteId,PublishTime,";
        //pgsql时候强制字段
        public const string pgsqlfields = @"istop AS ""IsTop"",isrecommend AS ""IsRecommend"",ishot AS ""IsHot"",ishide AS ""IsHide"",issilde AS ""IsSilde"",seotitle AS ""SeoTitle"",keyword AS ""KeyWord"",description AS ""Description"",title AS ""Title"",id AS ""Id"",addtime AS ""AddTime"",lasteditdate AS ""LastEditDate"",statuscode AS ""StatusCode"",reviewadduser AS ""ReviewAddUser"",addusername AS ""AddUserName"",lasteditusername AS ""LastEditUserName"",orderid AS ""OrderId"",parentid AS ""ParentId"",reviewstepid AS ""ReviewStepId"",contentgroupid AS ""ContentGroupId"",msggroupid AS ""MsgGroupId"",";

        //修改历史版本字段
        public const string updatehistoryFields = "IsTop,IsRecommend,IsHot,IsHide,IsSilde,SeoTitle,KeyWord,Description" +
            ",Title,Id,AddTime,AddUser,AddUserName,ParentId,ContentGroupId,PId,SiteId,PublishTime,";

        //复制数据字段
        public const string copyFields = "IsTop,IsRecommend,IsHot,IsHide,IsSilde,SeoTitle,KeyWord,Description" +
            ",Title,Id,AddTime,AddUser,AddUserName,ContentGroupId,PId,LastEditUser,LastEditUserName,LastEditDate,PublishTime,";

        //修改审核相关字段
        public static List<string> reviewContentFields = new List<string> { "ParentId", "Id", "StatusCode", "ReviewStepId", "ReviewAddUser", "MsgGroupId" };

        //简单修改的字段
        public static List<string> simpleUpdateFields = new List<string> { "ParentId", "OrderId", "Id", "IsTop", "Ids", "IsRecommend", "IsHot", "IsHide", "IsSilde" };
    }
}
