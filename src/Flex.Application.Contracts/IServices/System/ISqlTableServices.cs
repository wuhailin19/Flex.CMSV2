using Dapper;
using Flex.Core;
using Flex.Domain.Dtos.ColumnContent;
using Flex.Domain.Dtos.Field;
using Flex.Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices
{
    public interface ISqlTableServices
    {
        string CreateContentTableSql(string TableName);
        StringBuilder CreateDapperInsertSqlString(Hashtable table, string TableName, int nextOrderId, out DynamicParameters commandParameters);
        StringBuilder CreateInsertCopyContentSqlString(List<string> table, string TableName);
        StringBuilder CreateDapperUpdateSqlString(Hashtable table, string TableName, out DynamicParameters commandParameters);
        string DeleteContentTableData(string TableName, string Ids);
        string DeleteTableField(string TableName, List<sysField> model);
        string GetNextOrderIdDapperSqlString(string tableName);
        string InsertTableField(string TableName, sysField model);
        string InsertTableField(string TableName, string filedName, string filedtype);
        string ReNameTableField(string TableName, string oldfiledName, string filedName);
        string ReNameTableName(string TableName, string NewTableName);
        string UpdateContentReviewStatus(string TableName, int ContentId, StatusCode statusCode, string ReviewStepId);
        string UpdateContentStatus(string TableName, int ContentId, StatusCode statusCode);
        void CreateDapperColumnContentSelectSql(ContentPageListParamDto contentPageListParam, int modetype, out string swhere, out DynamicParameters parameters);
        string GenerateAddColumnStatement(string tableName, List<FiledHtmlStringDto> insertfiledlist);
        string AlertTableFieldType(string TableName, string filedName, string filedtype);
        string AlertTableField(string TableName, string oldfiledName, string filedName);
        StringBuilder CreateSqlsugarInsertSqlString(Hashtable table, string tableName, int nextOrderId, out SqlSugar.SugarParameter[] commandParameters);
        void CreateSqlSugarColumnContentSelectSql(ContentPageListParamDto contentPageListParam, out string swhere, out SqlSugar.SugarParameter[] parameters);
        void InitDapperColumnContentSwheresql(ref string swhere, ref DynamicParameters parameters, Dictionary<string, object> dataparams);
        StringBuilder CreateSqlsugarUpdateSqlString(Hashtable table, string TableName, out SqlSugar.SugarParameter[] commandParameters);
        string CompletelyDeleteContentTableData(string TableName, string Ids);
        string RestContentTableData(string TableName, string Ids);
        StringBuilder CreateCopyContentSqlString(List<string> table, string TableName, List<int> IdList, List<SysColumn> sysColumns);
        string CreateMoveContentSqlString(string TableName, string Ids, SysColumn targetcolumn);
        string CreateLinkContentSqlString(string TableName, string Ids, string targetcolumn);
    }
}
