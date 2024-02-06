using Flex.Domain.Dtos.Field;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.SqlServerSQLString
{
    public class SqlServerServices : ISqlTableServices
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
                                      "[IsShow] [bit] NOT NULL default 0," +
                                      "[IsSilde] [bit] NOT NULL default 0," +
                                      "[OrderId] [int] NOT NULL," +
                                      "[StatusCode] [int] NOT NULL default 1," +
                                      "[ReviewStepId] [nvarchar](255) NULL," +
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
        public StringBuilder CreateInsertSqlString(Hashtable table, string TableName, out SqlParameter[] commandParameters)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("insert into " + TableName + " (");
            string key = "";
            string keyvar = "";
            foreach (DictionaryEntry myDE in table)
            {
                key += "[" + myDE.Key.ToString() + "],";
                keyvar += "@" + myDE.Key.ToString() + ",";

            }
            builder.Append(key.Substring(0, key.Length - 1) + " ) values(" + keyvar.Substring(0, keyvar.Length - 1) + " )");
            commandParameters = new SqlParameter[table.Count];
            int num = 0;
            foreach (DictionaryEntry myDE in table)
            {
                commandParameters[num] = new SqlParameter("@" + myDE.Key.ToString(), myDE.Value);
                num++;
            }
            return builder;
        }
        public StringBuilder CreateUpdateSqlString(Hashtable table, string TableName, out SqlParameter[] commandParameters)
        {
            StringBuilder builder = new StringBuilder();
            int Id = table["Id"].ToInt();
            table.Remove("Id");
            builder.Append("Update " + TableName + " set ");
            string keyvar = "";
            foreach (DictionaryEntry myDE in table)
            {
                keyvar += "[" + myDE.Key.ToString() + "]" + "=" + "@" + myDE.Key.ToString() + ",";

            }
            builder.Append(keyvar.Substring(0, keyvar.Length - 1));
            commandParameters = new SqlParameter[table.Count];
            int num = 0;
            foreach (DictionaryEntry myDE in table)
            {
                commandParameters[num] = new SqlParameter("@" + myDE.Key.ToString(), myDE.Value);
                num++;
            }
            builder.Append(" where Id=" + Id);
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
    }
}
