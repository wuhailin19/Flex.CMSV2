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
    public class PostgreSqlTableServices:ISqlTableServices
    {
        public string CreateContentTableSql(string TableName) => "CREATE TABLE " + TableName + "" +
                                     "(" +
                                      "Id SERIAL PRIMARY KEY," +
                                      "ParentId INT NOT NULL," +
                                      "SiteId INT NOT NULL DEFAULT 1," +
                                      "Title VARCHAR(255) NOT NULL," +
                                      "SimpleTitle VARCHAR(255) NULL," +
                                      "Hits INT NOT NULL DEFAULT 1," +
                                      "SeoTitle VARCHAR(255) NULL," +
                                      "KeyWord VARCHAR(500) NULL," +
                                      "Description VARCHAR(1000) NULL," +
                                      "AddTime TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP," +
                                      "IsTop BOOLEAN NOT NULL DEFAULT FALSE," +
                                      "IsRecommend BOOLEAN NOT NULL DEFAULT FALSE," +
                                      "IsHot BOOLEAN NOT NULL DEFAULT FALSE," +
                                      "IsHide BOOLEAN NOT NULL DEFAULT FALSE," +
                                      "IsSilde BOOLEAN NOT NULL DEFAULT FALSE," +
                                      "OrderId INT NOT NULL DEFAULT 0," +
                                      "StatusCode INT NOT NULL DEFAULT 1," +
                                      "ReviewStepId VARCHAR(255) NULL," +
                                      "ContentGroupId BIGINT NULL," +
                                      "MsgGroupId BIGINT NULL," +
                                      "ReviewAddUser BIGINT NULL," +
                                      "AddUser BIGINT NULL," +
                                      "AddUserName VARCHAR(100) NULL," +
                                      "LastEditUser BIGINT NULL," +
                                      "LastEditUserName VARCHAR(100) NOT NULL," +
                                      "LastEditDate TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP," +
                                      "Version INT NOT NULL DEFAULT 0 " +
                                      " )";

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

        public StringBuilder CreateInsertCopyContentSqlString(Hashtable data, List<string> table, string TableName, int contentId)
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
                $"SELECT {keyvar.Substring(0, keyvar.Length - 1)},6,OrderId,'',0,{data["LastEditUser"]},'{data["LastEditUserName"]}','{data["LastEditDate"]}' FROM {TableName} WHERE Id={contentId}");
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
            builder.Append(" RETURNING OrderId;");
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
        public void CreateDapperColumnContentSelectSql(ContentPageListParamDto contentPageListParam, out string swhere, out DynamicParameters parameters)
        {
            parameters = new DynamicParameters();
            parameters.Add("@parentId", contentPageListParam.ParentId);
            swhere = " and ParentId=@parentId";

            if (contentPageListParam.k.IsNotNullOrEmpty())
            {
                parameters.Add("@k", contentPageListParam.k);
                if (contentPageListParam.k.ToInt() != 0)
                    swhere += " and (Title like '%' || @k || '%' or Id=@k)";
                else
                    swhere += " and Title like '%' || @k || '%'";
            }

            if (contentPageListParam.timefrom.IsNotNullOrEmpty())
            {
                parameters.Add("@timefrom", contentPageListParam.timefrom);
                swhere += " and AddTime >= @timefrom";
            }

            if (contentPageListParam.timeto.IsNotNullOrEmpty())
            {
                parameters.Add("@timeto", contentPageListParam.timeto);
                swhere += " and AddTime < (@timeto + INTERVAL '1 DAY')";
            }
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
                case "date": returntype = "TIMESTAMP"; break;
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
            }
            return returntype;
        }
    }

}
