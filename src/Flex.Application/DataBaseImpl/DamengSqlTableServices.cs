﻿using Dapper;
using Dm;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.Field;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.SqlServerSQLString
{
    public class DamengSqlTableServices : ISqlTableServices
    {
        public string CreateContentTableSql(string TableName) => @$"CREATE TABLE {TableName}
(
    Id INT IDENTITY(1, 1) NOT NULL,
    ParentId INT NOT NULL,
    SiteId INT NOT NULL DEFAULT 1,
    Title VARCHAR2(255) NOT NULL,
    SimpleTitle VARCHAR2(255),
    Hits INT NOT NULL DEFAULT 1,
    SeoTitle VARCHAR2(255),
    KeyWord VARCHAR2(500),
    Description VARCHAR2(1000),
    AddTime TIMESTAMP(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    IsTop BIT NOT NULL DEFAULT 0,
    IsRecommend BIT NOT NULL DEFAULT 0,
    IsHot BIT NOT NULL DEFAULT 0,
    IsHide BIT NOT NULL DEFAULT 0,
    IsSilde BIT NOT NULL DEFAULT 0,
    OrderId INT NOT NULL DEFAULT 0,
    StatusCode INT NOT NULL DEFAULT 1,
    ReviewStepId VARCHAR2(255),
    ContentGroupId BIGINT,
    MsgGroupId BIGINT,
    ReviewAddUser BIGINT,
    AddUser BIGINT,
    AddUserName VARCHAR2(100),
    LastEditUser BIGINT,
    LastEditUserName VARCHAR2(100) NOT NULL,
    LastEditDate TIMESTAMP(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Version INT NOT NULL DEFAULT 0
)";


        public string InsertTableField(string TableName, sysField model) => $"ALTER TABLE {TableName} ADD COLUMN {model.FieldName} {ConvertDataType(model.FieldType)};";

        public string InsertTableField(string TableName, string filedName, string filedtype) => $"ALTER TABLE {TableName} ADD COLUMN {filedName} {ConvertDataType(filedtype)};";

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


        public string AlertTableField(string TableName, string oldfiledName, string filedName) => $"ALTER TABLE {TableName} RENAME COLUMN {oldfiledName} TO {filedName};";
        public string AlertTableFieldType(string TableName, string filedName, string filedtype) => $"ALTER TABLE {TableName} MODIFY {filedName} {ConvertDataType(filedtype)};";



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
                $"SELECT {keyvar.Substring(0, keyvar.Length - 1)},6,OrderId,'',0,?,?,? FROM {TableName} WHERE Id=?");
            return builder;
        }

        public string GetNextOrderIdDapperSqlString(string tableName)
        {
            return $"SELECT COALESCE(MAX(OrderId) + 1, 0) FROM {tableName};";
        }

        public StringBuilder CreateDapperInsertSqlString(Hashtable table, string tableName, int nextOrderId, out DynamicParameters commandParameters)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"INSERT INTO {tableName} (");
            string key = "";
            string keyvar = "";
            table.Remove("OrderId");
            commandParameters = new DynamicParameters();
            int count = 0;
            foreach (DictionaryEntry myDE in table)
            {
                key += $"{myDE.Key},";
                keyvar += $"?,";
                commandParameters.Add("@" + myDE.Key, myDE.Value);
                count++;
            }
            builder.Append($"{key}OrderId) VALUES ({keyvar}{nextOrderId})");
            builder.Append(";SELECT SCOPE_IDENTITY();");
            return builder;
        }
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
                keyvar += $"?,";
                count++;
            }
            commandParameters = new SqlSugar.SugarParameter[table.Count];
            int num = 0;
            foreach (DictionaryEntry myDE in table)
            {
                commandParameters[num] = new SqlSugar.SugarParameter("@" + myDE.Key.ToString(), myDE.Value);
                num++;
            }
            builder.Append($"{key}OrderId) VALUES ({keyvar}{nextOrderId})");
            builder.Append(";SELECT SCOPE_IDENTITY();");
            return builder;
        }
        public void CreateSqlSugarColumnContentSelectSql(ContentPageListParamDto contentPageListParam, out string swhere, out SqlSugar.SugarParameter[] parameters)
        {
            Dictionary<string, object> paramdict = new Dictionary<string, object>();
            if (contentPageListParam.k.IsNotNullOrEmpty())
            { paramdict.Add("@k", contentPageListParam.k); }
            if (contentPageListParam.timefrom.IsNotNullOrEmpty())
            { paramdict.Add("@timefrom", contentPageListParam.timefrom); }
            if (contentPageListParam.timeto.IsNotNullOrEmpty())
            { paramdict.Add("@timeto", contentPageListParam.timeto); }
            if (contentPageListParam.ContentGroupId.IsNotNullOrEmpty())
            { paramdict.Add("@ContentGroupId", contentPageListParam.ContentGroupId); }
            int count = paramdict.Count() + 1;
            parameters = new SqlSugar.SugarParameter[count];
            parameters[0] = new SqlSugar.SugarParameter("@parentId", contentPageListParam.ParentId);
            swhere = " and ParentId=?";

            int index = 1;
            if (contentPageListParam.k.IsNotNullOrEmpty())
            {
                if (contentPageListParam.k.ToInt() != 0)
                    swhere += " and (Title like '%' || " + contentPageListParam.k + " || '%' or Id=" + contentPageListParam.k + ")";
                else
                {
                    parameters[index] = new SqlSugar.SugarParameter("@k", contentPageListParam.k);
                    swhere += " and Title like '%' || ? || '%'";
                    index++;
                }
            }
            if (contentPageListParam.timefrom.IsNotNullOrEmpty())
            {
                parameters[index] = new SqlSugar.SugarParameter("@timefrom", contentPageListParam.timefrom);
                index++;
                swhere += " and AddTime >= to_date(?, 'yyyy-mm-dd')";
            }
            if (contentPageListParam.timeto.IsNotNullOrEmpty())
            {
                parameters[index] = new SqlSugar.SugarParameter("@timeto", contentPageListParam.timeto);
                index++;
                swhere += " and AddTime < to_date(?, 'yyyy-mm-dd') + 1";
            }
            if (contentPageListParam.ContentGroupId.IsNotNullOrEmpty())
            {
                parameters[index] = new SqlSugar.SugarParameter("@ContentGroupId", contentPageListParam.ContentGroupId);
                index++;
                swhere += " and ContentGroupId=?";
            }
        }
        public void InitDapperColumnContentSwheresql(ref string swhere, ref DynamicParameters parameters, Dictionary<string, object> dataparams)
        {
            foreach (var item in dataparams.Keys)
            {
                parameters.Add("@" + item, dataparams[item]);
                if (swhere.IsNotNullOrEmpty())
                    swhere += " and";
                swhere += " " + item + "=?";
            }
        }
        public void CreateDapperColumnContentSelectSql(ContentPageListParamDto contentPageListParam, out string swhere, out DynamicParameters parameters)
        {
            parameters = new DynamicParameters();
            parameters.Add("@parentId", contentPageListParam.ParentId);
            swhere = " and ParentId=?";
            if (contentPageListParam.k.IsNotNullOrEmpty())
            {
                if (contentPageListParam.k.ToInt() != 0)
                    swhere += " and (Title like '%' || " + contentPageListParam.k + " || '%' or Id=" + contentPageListParam.k + ")";
                else
                {
                    parameters.Add("@k", contentPageListParam.k);
                    swhere += " and Title like '%' || ? || '%'";
                }
            }
            if (contentPageListParam.timefrom.IsNotNullOrEmpty())
            {
                swhere += " and AddTime >= to_date('" + contentPageListParam.timefrom + "', 'yyyy-mm-dd')";
            }
            if (contentPageListParam.timeto.IsNotNullOrEmpty())
            {
                swhere += " and AddTime < to_date('" + contentPageListParam.timeto + "', 'yyyy-mm-dd') + 1";
            }
            if (contentPageListParam.ContentGroupId.IsNotNullOrEmpty())
            {
                parameters.Add("@ContentGroupId", contentPageListParam.ContentGroupId);
                swhere += " and ContentGroupId=?";
            }
        }
        public StringBuilder CreateDapperUpdateSqlString(Hashtable table, string TableName, out DynamicParameters commandParameters)
        {
            StringBuilder builder = new StringBuilder();
            int Id = Convert.ToInt32(table["Id"]);
            var Ids = Convert.ToString(table["Ids"]);

            table.Remove("Id");
            table.Remove("Ids");
            builder.Append($"UPDATE {TableName} SET ");
            string keyvar = "";
            foreach (DictionaryEntry myDE in table)
            {
                keyvar += $"{myDE.Key}=?,";
            }
            builder.Append(keyvar.Substring(0, keyvar.Length - 1));
            commandParameters = new DynamicParameters();
            int num = 0;
            foreach (DictionaryEntry myDE in table)
            {
                commandParameters.Add("@" + myDE.Key.ToString(), myDE.Value);
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
                case "select": returntype = "VARCHAR2(1000)"; break;
                case "radio": returntype = "VARCHAR(255)"; break;
                case "checkbox": returntype = "VARCHAR(255)"; break;
                case "switch": returntype = "VARCHAR(255)"; break;
                case "slider": returntype = "VARCHAR(255)"; break;
                case "numberInput": returntype = "VARCHAR(255)"; break;
                case "labelGeneration": returntype = "VARCHAR(500)"; break;
                case "bottom": returntype = "VARCHAR(255)"; break;
                case "sign": returntype = "VARCHAR(255)"; break;
                case "iconPicker": returntype = "VARCHAR(255)"; break;
                case "cron": returntype = "CLOB"; break;
                case "date": returntype = "DATETIME"; break;
                case "dateRange": returntype = "VARCHAR(255)"; break;
                case "rate": returntype = "VARCHAR(255)"; break;
                case "carousel": returntype = "VARCHAR(500)"; break;
                case "colorpicker": returntype = "VARCHAR(255)"; break;
                case "image": returntype = "VARCHAR(255)"; break;
                case "file": returntype = "VARCHAR(255)"; break;
                case "textarea": returntype = "NVARCHAR2(8000)"; break;
                case "editor": returntype = "CLOB"; break;
                case "blockquote": returntype = "VARCHAR(255)"; break;
                case "line": returntype = "VARCHAR(255)"; break;
                case "spacing": returntype = "VARCHAR(255)"; break;
                case "textField": returntype = "VARCHAR(255)"; break;
                case "grid": returntype = "VARCHAR(255)"; break;
            }
            return returntype;
        }

        public StringBuilder CreateSqlsugarUpdateSqlString(Hashtable table, string TableName, out SqlSugar.SugarParameter[] commandParameters)
        {
            StringBuilder builder = new StringBuilder();
            int Id = Convert.ToInt32(table["Id"]);
            var Ids = Convert.ToString(table["Ids"]);

            table.Remove("Id");
            table.Remove("Ids");
            builder.Append($"UPDATE {TableName} SET ");
            string keyvar = "";
            foreach (DictionaryEntry myDE in table)
            {
                keyvar += $"{myDE.Key}=?,";
            }
            builder.Append(keyvar.Substring(0, keyvar.Length - 1));
            commandParameters = new SqlSugar.SugarParameter[table.Count];
            int num = 0;
            foreach (DictionaryEntry myDE in table)
            {
                commandParameters[num]=new SqlSugar.SugarParameter("@" + myDE.Key.ToString(), myDE.Value);
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
    }
}