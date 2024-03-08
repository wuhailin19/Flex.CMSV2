using Dapper;
using Flex.Core;
using Flex.Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices
{
    public interface ISqlTableServices
    {
        string AlertTableField(string TableName, string oldfiledName, string filedName, string filedtype);
        string CreateContentTableSql(string TableName);
        StringBuilder CreateDapperInsertSqlString(Hashtable table, string TableName, out DynamicParameters commandParameters);
        StringBuilder CreateInsertCopyContentSqlString(Hashtable data, List<string> table, string TableName, int contentId);
        StringBuilder CreateInsertSqlString(Hashtable table, string TableName, out SqlParameter[] commandParameters);
        StringBuilder CreateUpdateSqlString(Hashtable table, string TableName, out SqlParameter[] commandParameters);
        string DeleteContentTableData(string TableName, string Ids);
        string DeleteTableField(string TableName, List<sysField> model);
        string InsertTableField(string TableName, sysField model);
        string InsertTableField(string TableName, string filedName, string filedtype);
        string ReNameTableField(string TableName, string oldfiledName, string filedName);
        string ReNameTableName(string TableName, string NewTableName);
        string UpdateContentReviewStatus(string TableName, int ContentId, StatusCode statusCode, string ReviewStepId);
        string UpdateContentStatus(string TableName, int ContentId, StatusCode statusCode);
    }
}
