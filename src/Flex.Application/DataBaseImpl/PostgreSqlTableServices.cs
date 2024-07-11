using Dapper;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.Field;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.SqlServerSQLString
{
    public class PostgreSqlTableServices : ISqlTableServices
    {
        public string CreateContentTableSql(string TableName) => "CREATE TABLE " + TableName +
                "(" +
                    "Id SERIAL PRIMARY KEY," +
                    "ParentId INT NOT NULL," +
                    "SiteId INT NOT NULL DEFAULT 1," +
                    "Title VARCHAR(255) NOT NULL," +
                    "SimpleTitle VARCHAR(255) NULL," +
                    "Hits INT NOT NULL DEFAULT 1," +
                    "PId INT NULL DEFAULT 0," +
                    "SeoTitle VARCHAR(255) NULL," +
                    "KeyWord VARCHAR(500) NULL," +
                    "Description VARCHAR(1000) NULL," +
                    "AddTime timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP," +
                    "IsTop BOOLEAN NOT NULL DEFAULT FALSE," +
                    "IsRecommend BOOLEAN NOT NULL DEFAULT FALSE," +
                    "IsHot BOOLEAN NOT NULL DEFAULT FALSE," +
                    "IsHide BOOLEAN NOT NULL DEFAULT FALSE," +
                    "IsSilde BOOLEAN NOT NULL DEFAULT FALSE," +
                    "OrderId INT NOT NULL DEFAULT 0," +
                    "StatusCode INT NOT NULL DEFAULT 1," +
                    "ReviewStepId VARCHAR(255) NULL," +
                    "RefLinkClassId VARCHAR(500) NULL," +
                    "ContentGroupId BIGINT NULL," +
                    "MsgGroupId BIGINT NULL," +
                    "ReviewAddUser BIGINT NULL," +
                    "AddUser BIGINT NULL," +
                    "AddUserName VARCHAR(100) NULL," +
                    "LastEditUser BIGINT NULL," +
                    "LastEditUserName VARCHAR(100) NOT NULL," +
                    "LastEditDate timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP," +
                    "Version INT NOT NULL DEFAULT 0" +
                ")";


        public string InsertTableField(string TableName, sysField model) => $"ALTER TABLE {TableName} ADD COLUMN {model.FieldName} {ConvertDataType(model.FieldType)};";

        public string InsertTableField(string TableName, string filedName, string filedtype) => $"ALTER TABLE {TableName} ADD COLUMN {filedName} {ConvertDataType(filedtype)};";

        public string AlertTableField(string TableName, string oldfiledName, string filedName, string filedtype) => $"ALTER TABLE {TableName} RENAME COLUMN {oldfiledName} TO {filedName}; ALTER TABLE {TableName} ALTER COLUMN {filedName} TYPE {ConvertDataType(filedtype)};";

        public string ReNameTableField(string TableName, string oldfiledName, string filedName) => $"ALTER TABLE {TableName} RENAME COLUMN {oldfiledName} TO {filedName};";

        public string ReNameTableName(string TableName, string NewTableName) => $"ALTER TABLE {TableName} RENAME TO {NewTableName};";

        public string UpdateContentStatus(string TableName, int ContentId, StatusCode statusCode) => $"UPDATE {TableName} SET StatusCode={statusCode.ToInt()} WHERE Id={ContentId};";

        public string UpdateContentReviewStatus(string TableName, int ContentId, StatusCode statusCode, string ReviewStepId) => $"UPDATE {TableName} SET StatusCode={statusCode.ToInt()}, ReviewStepId='{ReviewStepId}' WHERE Id={ContentId};";

        public string DeleteTableField(string TableName, List<sysField> Fields)
        {
            StringBuilder sql = new StringBuilder();
            foreach (var item in Fields)
            {
                sql.Append($"ALTER TABLE {TableName} DROP COLUMN {item.FieldName};");
            }
            return sql.ToString();
        }

        public string DeleteContentTableData(string TableName, string Ids) => $"UPDATE {TableName} SET StatusCode=0 WHERE Id IN ({Ids})";

        public StringBuilder CreateInsertCopyContentSqlString(List<string> table, string TableName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"INSERT INTO {TableName} (");
            string key = "";
            string keyvar = "";
            foreach (var item in table)
            {
                if (item.ToLower() == "id")
                    continue;
                key += $"{item},";
                keyvar += $"{item},";
            }
            builder.Append($"{key.Substring(0, key.Length - 1)},StatusCode,OrderId,ReviewStepId,MsgGroupId,LastEditUser,LastEditUserName,LastEditDate) " +
                $"SELECT {keyvar.Substring(0, keyvar.Length - 1)},6,OrderId,'',0,@LastEditUser,@LastEditUserName,@LastEditDate FROM {TableName} WHERE Id=@Id");
            return builder;
        }

        public StringBuilder CreateCopyContentSqlString(List<string> table, string TableName, List<int> IdList, List<SysColumn> sysColumns)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var column in sysColumns)
            {
                int StatusCode = 1;
                if (column.ReviewMode != 0)
                    StatusCode = 5;
                string key = "";
                string keyvar = "";
                foreach (var item in table)
                {
                    if (item.ToString().ToLower() == "id")
                        continue;
                    key += $"{item},";
                    keyvar += $"{item},";
                }
                foreach (var item in IdList)
                {
                    builder.Append("insert into " + TableName + " (");
                    builder.Append(key.Substring(0, key.Length - 1) + @$",SiteId,StatusCode,ParentId,OrderId,ReviewStepId,MsgGroupId) 
                select {keyvar.Substring(0, keyvar.Length - 1)},{column.SiteId},{StatusCode},{column.Id},OrderId,'',0 from {TableName} where Id={item};");
                }
            }

            return builder;
        }

        public string GetNextOrderIdDapperSqlString(string tableName)
        {
            return $"SELECT COALESCE(MAX(OrderId) + 1, 0) FROM {tableName}";
        }

        public StringBuilder CreateDapperInsertSqlString(Hashtable table, string tableName, int nextOrderId, out DynamicParameters commandParameters)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"INSERT INTO {tableName} (");
            string key = "";
            string keyvar = "";
            table.Remove("OrderId");
            commandParameters = new DynamicParameters();
            foreach (DictionaryEntry myDE in table)
            {
                key += $"{myDE.Key.ToString()},";
                keyvar += $"@{myDE.Key.ToString()},";
                commandParameters.Add(myDE.Key.ToString(), myDE.Value);
            }
            builder.Append($"{key}OrderId) VALUES ({keyvar} {nextOrderId})");
            builder.Append(" RETURNING Id;");
            return builder;
        }
        public string GenerateAddColumnStatement(string tableName, List<FiledHtmlStringDto> insertfiledlist)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"ALTER TABLE {tableName} ");

            bool isFirst = true;
            foreach (var column in insertfiledlist)
            {
                if (!isFirst)
                {
                    sb.Append(", ");
                }
                else
                {
                    isFirst = false;
                }

                sb.Append($"ADD COLUMN {column.id} {ConvertDataType(column.tag)}");
            }

            return sb.ToString();
        }
        public void CreateDapperColumnContentSelectSql(ContentPageListParamDto contentPageListParam, int modetype, out string swhere, out DynamicParameters parameters)
        {
            parameters = new DynamicParameters();
            swhere = string.Empty;
            if (modetype == 3)
            {
                parameters.Add("@parentId", contentPageListParam.ParentId);
                swhere = " and ParentId=@parentId";
            }
            if (modetype == 1)
            {
                parameters.Add("@parentId", contentPageListParam.ParentId);
                swhere = $" and (ParentId=@parentId or RefLinkClassId like '%,{contentPageListParam.ParentId},%')";
            }
            if (contentPageListParam.k.IsNotNullOrEmpty())
            {
                parameters.Add("@k", contentPageListParam.k);
                if (contentPageListParam.k.ToInt() != 0)
                    swhere += " and (Title like '%' || @k || '%' or Id=@k::int)";
                else
                    swhere += " and Title like '%' || @k || '%'";
            }

            if (contentPageListParam.timefrom != null)
            {
                parameters.Add("@timefrom", contentPageListParam.timefrom);
                swhere += " and AddTime >= @timefrom";
            }

            if (contentPageListParam.timeto != null)
            {
                parameters.Add("@timeto", contentPageListParam.timeto);
                swhere += " and AddTime < (@timeto + INTERVAL '1 DAY')";
            }
            if (contentPageListParam.ContentGroupId.IsNotNullOrEmpty())
            {
                parameters.Add("@ContentGroupId", contentPageListParam.ContentGroupId);
                swhere += " and ContentGroupId=@ContentGroupId::bigint";
            }
        }

        public StringBuilder CreateSqlsugarUpdateSqlString(Hashtable table, string TableName, out SqlSugar.SugarParameter[] commandParameters)
        {
            StringBuilder builder = new StringBuilder();
            int Id = (int)table["Id"];
            var Ids = (string)table["Ids"];

            table.Remove("Id");
            table.Remove("Ids");
            builder.Append($"UPDATE {TableName} SET ");
            string keyvar = "";

            foreach (DictionaryEntry myDE in table)
            {
                keyvar += $"{myDE.Key.ToString()}=@{myDE.Key.ToString()},";
            }
            builder.Append(keyvar.Substring(0, keyvar.Length - 1));
            commandParameters = new SqlSugar.SugarParameter[table.Count];

            int num = 0;
            foreach (DictionaryEntry myDE in table)
            {
                commandParameters[num] = new SqlSugar.SugarParameter(myDE.Key.ToString(), myDE.Value);
                num++;
            }
            table.Add("Id", Id);
            if (string.IsNullOrEmpty(Ids))
            {
                builder.Append($" WHERE Id={Id}");
            }
            else
            {
                builder.Append($" WHERE Id IN ({Ids})");
            }
            return builder;
        }

        public StringBuilder CreateDapperUpdateSqlString(Hashtable table, string TableName, out DynamicParameters commandParameters)
        {
            StringBuilder builder = new StringBuilder();
            int Id = (int)table["Id"];
            var Ids = (string)table["Ids"];

            table.Remove("Id");
            table.Remove("Ids");
            builder.Append($"UPDATE {TableName} SET ");
            string keyvar = "";
            foreach (DictionaryEntry myDE in table)
            {
                keyvar += $"{myDE.Key.ToString()}=@{myDE.Key.ToString()},";
            }
            builder.Append(keyvar.Substring(0, keyvar.Length - 1));
            commandParameters = new DynamicParameters();
            int num = 0;
            foreach (DictionaryEntry myDE in table)
            {
                commandParameters.Add(myDE.Key.ToString(), myDE.Value);
                num++;
            }
            table.Add("Id", Id);
            if (string.IsNullOrEmpty(Ids))
            {
                builder.Append($" WHERE Id={Id}");
            }
            else
            {
                builder.Append($" WHERE Id IN ({Ids})");
            }
            return builder;
        }

        private string ConvertDataType(string FieldType)
        {
            string returntype = string.Empty;
            switch (FieldType)
            {
                case "input": returntype = "VARCHAR(255)"; break;
                case "password": returntype = "VARCHAR(255)"; break;
                case "select": returntype = "VARCHAR(255)"; break;
                case "radio": returntype = "VARCHAR(255)"; break;
                case "checkbox": returntype = "VARCHAR(255)"; break;
                case "switch": returntype = "VARCHAR(255)"; break;
                case "slider": returntype = "VARCHAR(255)"; break;
                case "numberInput": returntype = "VARCHAR(255)"; break;
                case "labelGeneration": returntype = "VARCHAR(500)"; break;
                case "bottom": returntype = "VARCHAR(255)"; break;
                case "sign": returntype = "VARCHAR(255)"; break;
                case "iconPicker": returntype = "VARCHAR(255)"; break;
                case "cron": returntype = "TEXT"; break;
                case "date": returntype = "timestamp without time zone"; break;
                case "dateRange": returntype = "VARCHAR(255)"; break;
                case "rate": returntype = "VARCHAR(255)"; break;
                case "carousel": returntype = "VARCHAR(500)"; break;
                case "colorpicker": returntype = "VARCHAR(255)"; break;
                case "image": returntype = "VARCHAR(255)"; break;
                case "file": returntype = "VARCHAR(255)"; break;
                case "textarea": returntype = "TEXT"; break;
                case "editor": returntype = "TEXT"; break;
                case "blockquote": returntype = "VARCHAR(255)"; break;
                case "line": returntype = "VARCHAR(255)"; break;
                case "spacing": returntype = "VARCHAR(255)"; break;
                case "textField": returntype = "VARCHAR(255)"; break;
                case "grid": returntype = "VARCHAR(255)"; break;
                case "multiimage": returntype = "TEXT"; break;
            }
            return returntype;
        }

        public string AlertTableField(string TableName, string oldfiledName, string filedName) =>
        $"ALTER TABLE {TableName} RENAME COLUMN {oldfiledName} TO {filedName};";

        public string AlertTableFieldType(string TableName, string fieldName, string fieldType) =>
            $"ALTER TABLE {TableName} ALTER COLUMN {fieldName} TYPE {ConvertDataType(fieldType)};";

        public StringBuilder CreateSqlsugarInsertSqlString(Hashtable table, string tableName, int nextOrderId, out SqlSugar.SugarParameter[] commandParameters)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"INSERT INTO {tableName} (");
            string key = "";
            string keyvar = "";
            table.Remove("OrderId");
            int count = 0;
            foreach (DictionaryEntry myDE in table)
            {
                key += $"{myDE.Key},";
                keyvar += $"@{myDE.Key},";
                count++;
            }
            commandParameters = new SqlSugar.SugarParameter[table.Count];
            int num = 0;
            foreach (DictionaryEntry myDE in table)
            {
                commandParameters[num] = new SqlSugar.SugarParameter($"@{myDE.Key}", myDE.Value);
                num++;
            }
            builder.Append($"{key}OrderId) VALUES ({keyvar}{nextOrderId})");
            builder.Append(" RETURNING Id;");
            return builder;
        }


        public void CreateSqlSugarColumnContentSelectSql(ContentPageListParamDto contentPageListParam, out string swhere, out SqlSugar.SugarParameter[] parameters)
        {
            throw new NotImplementedException();
        }
        public string CompletelyDeleteContentTableData(string TableName, string Ids) => "Delete from " + TableName + " where Id in(" + Ids + ")";
        public string RestContentTableData(string TableName, string Ids) => "update " + TableName + " set StatusCode=1 where Id in(" + Ids + ")";
        public void InitDapperColumnContentSwheresql(ref string swhere, ref DynamicParameters parameters, Dictionary<string, object> dataparams)
        {
            foreach (var item in dataparams.Keys)
            {
                parameters.Add("@" + item, dataparams[item]);
                if (swhere.IsNotNullOrEmpty())
                    swhere += " and";
                swhere += " " + item + "=@" + item;
            }
        }

        public string CreateMoveContentSqlString(string TableName, string Ids, SysColumn targetcolumn)
        {
            int StatusCode = 1;
            if (targetcolumn.ReviewMode != 0)
                StatusCode = 5;
            return $"update {TableName} set " +
                $"ParentId={targetcolumn.Id}," +
                $"ReviewStepId=''," +
                $"StatusCode={StatusCode}," +
                $"SiteId={targetcolumn.SiteId}," +
                $"MsgGroupId=0 " +
                $"where Id in ({Ids})";
        }

        public string CreateLinkContentSqlString(string TableName, string Ids, string targetcolumn)
        {
            return $"update {TableName} set " +
                $"RefLinkClassId='{targetcolumn}'" +
                $" where Id in ({Ids})";
        }
    }
}
