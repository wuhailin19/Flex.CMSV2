﻿using Flex.Domain.Dtos.Field;
using System;
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
                                      "[KeyWord] [nvarchar](255)  NULL," +
                                      "[Description] [nvarchar](255)  NULL," +
                                      "[AddTime] [datetime] NOT NULL default getdate()," +
                                      "[IsTop] [bit] NOT NULL default 0," +
                                      "[IsRecommend] [bit] NOT NULL default 0," +
                                      "[IsHot] [bit] NOT NULL default 0," +
                                      "[IsShow] [bit] NOT NULL default 0," +
                                      "[IsColor] [bit] NOT NULL default 0," +
                                      "[OrderId] [int] NOT NULL," +
                                      "[StatusCode] [int] NOT NULL default 1," +
                                      "[AddUser] [bigint] NULL," +
                                      "[AddUserName] [nvarchar](100) NULL," +
                                      "[LastEditUser] [bigint] NULL," +
                                      "[LastEditUserName] [nvarchar](100) NOT NULL," +
                                      "[LastEditDate] [datetime] NOT NULL default getdate()," +
                                      "[Version] [int] NOT NULL default 0 " +
                                      " )";
        public string InsertTableField(string TableName, sysField model) => "ALTER TABLE [" + TableName + "]  ADD [" + model.FieldName + "]  " + ConvertDataType(model);
        public string DeleteTableField(string TableName, List<sysField> Fields) 
         {
            string sql = string.Empty;
            foreach (var item in Fields)
            {
                sql+="ALTER TABLE [" + TableName + "]  DROP COLUMN [" + item.FieldName + "];";
            }
            return sql;
        }
            

        /// <summary>
        /// 转换数据类型(数据库数据类型)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string ConvertDataType(sysField model)
        {
            string returntype = string.Empty;
            switch (model.FieldType)
            {
                case "TextBox": returntype = "nvarchar(255)"; break;
                case "MultipleTextType": returntype = "nvarchar(2000)"; break;
                case "Editor": returntype = "nvarchar(max)"; break;
                case "eWebEditor": returntype = "nvarchar(max)"; break;
                case "ListBoxType": returntype = "nvarchar(255)"; break;
                case "PicType": returntype = "nvarchar(255)"; break;
                case "FileType": returntype = "nvarchar(255)"; break;
                case "FileUpload": returntype = "nvarchar(255)"; break;
                case "ColorPicker": returntype = "nvarchar(255)"; break;
                case "MutiImgSelect": returntype = "nvarchar(max)"; break;
                case "TimerPicker": returntype = "datetime"; break;
                case "ProvincialLinkage": returntype = "nvarchar(255)"; break;
                case "Dictionary-1": returntype = "nvarchar(50)"; break;
                case "Dictionary-2": returntype = "nvarchar(50)"; break;
                case "Relevance": returntype = "nvarchar(255)"; break;
            }
            return returntype;
        }
    }
}