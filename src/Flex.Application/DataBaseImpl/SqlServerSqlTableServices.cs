using Dapper;
using Flex.Core.Extensions.CommonExtensions;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.Field;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.SqlServerSQLString
{
    public class SqlServerSqlTableServices : ISqlTableServices
    {
        public string CreateContentTableSql(string TableName) => "CREATE TABLE " + TableName + "" +
                                     "(" +
                                      "[Id] [int] IDENTITY (1, 1) PRIMARY Key NOT NULL," +
                                      "[ParentId] [int] NOT NULL," +
                                      "[SiteId] [int] NOT NULL default 1," +
                                      "[Title] [nvarchar](255) NOT NULL," +
                                      "[SimpleTitle] [nvarchar](255)  NULL," +
                                      "[Hits] [int] NOT NULL default 1," +
                                      "[SeoTitle] [nvarchar](255)  NULL," +
                                      "[KeyWord] [nvarchar](500)  NULL," +
                                      "[Description] [nvarchar](1000)  NULL," +
                                      "[AddTime] [datetime] NOT NULL default getdate()," +
                                      "[IsTop] [bit] NOT NULL default 0," +
                                      "[IsRecommend] [bit] NOT NULL default 0," +
                                      "[IsHot] [bit] NOT NULL default 0," +
                                      "[IsHide] [bit] NOT NULL default 0," +
                                      "[IsSilde] [bit] NOT NULL default 0," +
                                      "[OrderId] [int] NOT NULL default 0," +
                                      "[StatusCode] [int] NOT NULL default 1," +
                                      "[ReviewStepId] [nvarchar](255) NULL," +
                                      "[ContentGroupId] [bigint] NULL," +
                                      "[MsgGroupId] [bigint] NULL," +
                                      "[ReviewAddUser] [bigint] NULL," +
                                      "[AddUser] [bigint] NULL," +
                                      "[AddUserName] [nvarchar](100) NULL," +
                                      "[LastEditUser] [bigint] NULL," +
                                      "[LastEditUserName] [nvarchar](100) NOT NULL," +
                                      "[LastEditDate] [datetime] NOT NULL default getdate()," +
                                      "[Version] [int] NOT NULL default 0 " +
                                      " )";
        public string InsertTableField(string TableName, sysField model) => "ALTER TABLE [" + TableName + "]  ADD [" + model.FieldName + "]  " + ConvertDataType(model.FieldType) + ";";
        public string InsertTableField(string TableName, string filedName, string filedtype) => "ALTER TABLE [" + TableName + "]  ADD [" + filedName + "]  " + ConvertDataType(filedtype) + ";";
        public string AlertTableField(string TableName, string oldfiledName, string filedName, string filedtype) => "EXEC sp_rename '" + TableName + "." + oldfiledName + "', '" + filedName + "', 'COLUMN';ALTER TABLE " + TableName + " ALTER COLUMN " + filedName + " " + ConvertDataType(filedtype) + ";";
        public string ReNameTableField(string TableName, string oldfiledName, string filedName) => "EXEC sp_rename '" + TableName + "." + oldfiledName + "', '" + filedName + "', 'COLUMN';";
        public string ReNameTableName(string TableName, string NewTableName) => "EXEC sp_rename '" + TableName + "', '" + NewTableName + "';";
        public string UpdateContentStatus(string TableName, int ContentId, StatusCode statusCode) => $"update {TableName} set StatusCode={statusCode.ToInt()} where Id={ContentId}";
        public string UpdateContentReviewStatus(string TableName, int ContentId, StatusCode statusCode, string ReviewStepId) => $"update {TableName} set StatusCode={statusCode.ToInt()},ReviewStepId='{ReviewStepId}' where Id={ContentId}";
        public string DeleteTableField(string TableName, List<sysField> Fields)
        {
            string sql = string.Empty;
            foreach (var item in Fields)
            {
                sql += "ALTER TABLE [" + TableName + "]  DROP COLUMN [" + item.FieldName + "];";
            }
            return sql;
        }
        public string DeleteContentTableData(string TableName, string Ids) => "update " + TableName + " set StatusCode=0 where Id in(" + Ids + ")";
        public StringBuilder CreateInsertCopyContentSqlString(Hashtable data, List<string> table, string TableName, int contentId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("insert into " + TableName + " (");
            string key = "";
            string keyvar = "";
            foreach (var item in table)
            {
                if (item.ToString().ToLower() == "id")
                    continue;
                key += "[" + item + "],";
                keyvar += "" + item + ",";
            }
            builder.Append(key.Substring(0, key.Length - 1) + ",StatusCode,OrderId,ReviewStepId,MsgGroupId,LastEditUser,LastEditUserName,LastEditDate) " +
                "select " + keyvar.Substring(0, keyvar.Length - 1) + ",6,OrderId,'',0," + data["LastEditUser"] + ",'" + data["LastEditUserName"] + "','" + data["LastEditDate"] + "' from " + TableName + " where Id=" + contentId);
            return builder;
        }

        public string GetNextOrderIdDapperSqlString(string tableName)
        {
            return $"SELECT ISNULL(MAX(OrderId) + 1, 0) FROM {tableName}";
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
                    swhere += " and (Title like '%'+@k+'%' or Id=cast(@k as int))";
                else
                    swhere += " and Title like '%'+@k+'%'";
            }
            if (contentPageListParam.timefrom.IsNotNullOrEmpty())
            {
                parameters.Add("@timefrom", contentPageListParam.timefrom);
                swhere += " and AddTime>=@timefrom";
            }
            if (contentPageListParam.timeto.IsNotNullOrEmpty())
            {
                parameters.Add("@timeto", contentPageListParam.timeto);
                swhere += " and AddTime<DATEADD(day, 1, @timeto)";
            }
            if (contentPageListParam.ContentGroupId.IsNotNullOrEmpty())
            {
                parameters.Add("@ContentGroupId", contentPageListParam.ContentGroupId);
                swhere += " and ContentGroupId=@ContentGroupId";
            }
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
                key += $"{myDE.Key},";
                keyvar += $"@{myDE.Key},";
                commandParameters.Add(myDE.Key.ToString(), myDE.Value);
            }
            builder.Append($"{key}OrderId) VALUES ({keyvar} {nextOrderId})");
            builder.Append(";SELECT SCOPE_IDENTITY();");
            return builder;
        }
        public StringBuilder CreateDapperUpdateSqlString(Hashtable table, string TableName, out DynamicParameters commandParameters)
        {
            StringBuilder builder = new StringBuilder();
            int Id = table["Id"].ToInt();
            var Ids = table.GetStringValue("Ids");

            table.Remove("Id");
            table.Remove("Ids");
            builder.Append("Update " + TableName + " set ");
            string keyvar = "";
            foreach (DictionaryEntry myDE in table)
            {
                keyvar += "[" + myDE.Key.ToString() + "]" + "=" + "@" + myDE.Key.ToString() + ",";

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
            if (Ids.IsNullOrEmpty())
            {
                builder.Append(" where Id=" + Id);
            }
            else
            {
                builder.Append(" where Id in(" + Ids + ")");
            }
            return builder;
        }

        /// <summary>
        /// 转换数据类型(数据库数据类型)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string ConvertDataType(string FieldType)
        {
            string returntype = string.Empty;
            switch (FieldType)
            {
                case "input": returntype = "nvarchar(255)"; break;
                case "password": returntype = "nvarchar(255)"; break;
                case "select": returntype = "nvarchar(255)"; break;
                case "radio": returntype = "nvarchar(255)"; break;
                case "checkbox": returntype = "nvarchar(255)"; break;
                case "switch": returntype = "nvarchar(255)"; break;
                case "slider": returntype = "nvarchar(255)"; break;
                case "numberInput": returntype = "nvarchar(255)"; break;
                case "labelGeneration": returntype = "nvarchar(500)"; break;
                case "bottom": returntype = "nvarchar(255)"; break;
                case "sign": returntype = "nvarchar(255)"; break;
                case "iconPicker": returntype = "nvarchar(255)"; break;
                case "cron": returntype = "nvarchar(max)"; break;
                case "date": returntype = "datetime"; break;
                case "dateRange": returntype = "nvarchar(255)"; break;
                case "rate": returntype = "nvarchar(255)"; break;
                case "carousel": returntype = "nvarchar(500)"; break;
                case "colorpicker": returntype = "nvarchar(255)"; break;
                case "image": returntype = "nvarchar(255)"; break;
                case "file": returntype = "nvarchar(255)"; break;
                case "textarea": returntype = "nvarchar(max)"; break;
                case "editor": returntype = "nvarchar(max)"; break;
                case "blockquote": returntype = "nvarchar(255)"; break;
                case "line": returntype = "nvarchar(255)"; break;
                case "spacing": returntype = "nvarchar(255)"; break;
                case "textField": returntype = "nvarchar(255)"; break;
                case "grid": returntype = "nvarchar(255)"; break;
            }
            return returntype;
        }
        public string AlertTableField(string TableName, string oldfieldName, string fieldName) =>
    $"EXEC sp_rename '{TableName}.{oldfieldName}', '{fieldName}', 'COLUMN';";

        public string AlertTableFieldType(string TableName, string fieldName, string fieldType) =>
            $"ALTER TABLE {TableName} ALTER COLUMN {fieldName} {ConvertDataType(fieldType)};";

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
            commandParameters = new SqlSugar.SugarParameter[table.Count + 1]; // +1 for nextOrderId parameter
            int num = 0;
            foreach (DictionaryEntry myDE in table)
            {
                commandParameters[num] = new SqlSugar.SugarParameter($"@{myDE.Key}", myDE.Value);
                num++;
            }
            // Add nextOrderId parameter
            commandParameters[num] = new SqlSugar.SugarParameter("@nextOrderId", nextOrderId);

            builder.Append($"{key}OrderId) VALUES ({keyvar}@nextOrderId); SELECT SCOPE_IDENTITY();");
            return builder;
        }


        public void CreateSqlSugarColumnContentSelectSql(ContentPageListParamDto contentPageListParam, out string swhere, out SqlSugar.SugarParameter[] parameters)
        {
            throw new NotImplementedException();
        }

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
    }
}
