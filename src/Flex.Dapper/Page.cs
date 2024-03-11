﻿using System.Text.RegularExpressions;
using Flex.Core.Extensions;

namespace Flex.Dapper
{
    public class Page
    {
        /// <summary>
        /// The current page number contained in this page of result set 
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// The total number of pages in the full result set
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// The total number of records in the full result set
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// The number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The actual records on this page
        /// </summary>
        public IEnumerable<dynamic> Items { get; set; }
        //public List<T> Items { get; set; }
    }
    public class DapperPage
    {
        public static void BuildPageQueries(long skip, long take, string sql, out string sqlCount, out string sqlPage)
        {
            // Split the SQL
            if (!PagingHelper.SplitSQL(sql, out PagingHelper.SQLParts parts))
                throw new Exception("Unable to parse SQL statement for paged query");

            var usedb = "DataConfig:UseDb".Config(string.Empty) ?? "Sqlserver";
            var sqlconnection = "DataConfig:Sqlserver:ConnectionString".Config(string.Empty);
            switch (usedb)
            {
                case "Sqlserver":
                    sqlPage = BuildPageSql.BuildPageQuery2(skip, take, parts);
                    sqlCount = parts.sqlCount;
                    break;
                case "Mysql":
                    sqlPage = BuildMysqlPageSql.BuildPageQuery2(skip, take, parts);
                    sqlCount = parts.sqlCount;
                    break;
                case "DM8":
                    sqlPage = BuildDMPageSql.BuildPageQuery2(skip, take, parts);
                    sqlCount = parts.sqlCount;
                    break;
                case "PgSql":
                    sqlPage = BuildPgSqlPageSql.BuildPageQuery2(skip, take, parts);
                    sqlCount = parts.sqlCount;
                    break;
                default:
                    sqlPage = BuildPageSql.BuildPageQuery2(skip, take, parts);
                    sqlCount = parts.sqlCount;
                    break;
            }
        }
    }

    static class BuildPageSql
    {
        public static string BuildPageQuery(long skip, long take, PagingHelper.SQLParts parts)
        {
            parts.sqlSelectRemoved = PagingHelper.rxOrderBy.Replace(parts.sqlSelectRemoved, "", 1);
            if (PagingHelper.rxDistinct.IsMatch(parts.sqlSelectRemoved))
            {
                parts.sqlSelectRemoved = "peta_inner.* FROM (SELECT " + parts.sqlSelectRemoved + ") peta_inner";
            }
            var sqlPage = string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) peta_rn, {1}) peta_paged WHERE peta_rn>{2} AND peta_rn<={3}",
                                    parts.sqlOrderBy ?? "ORDER BY (SELECT NULL)", parts.sqlSelectRemoved, skip, skip + take);
            //args = args.Concat(new object[] { skip, skip + take }).ToArray();

            return sqlPage;
        }

        //SqlServer 2012及以上
        public static string BuildPageQuery2(long skip, long take, PagingHelper.SQLParts parts)
        {
            parts.sqlSelectRemoved = PagingHelper.rxOrderBy.Replace(parts.sqlSelectRemoved, "", 1);
            if (PagingHelper.rxDistinct.IsMatch(parts.sqlSelectRemoved))
            {
                parts.sqlSelectRemoved = "peta_inner.* FROM (SELECT " + parts.sqlSelectRemoved + ") peta_inner";
            }

            var sqlOrderBy = parts.sqlOrderBy ?? "ORDER BY (SELECT NULL)";
            var sqlPage = $"SELECT {parts.sqlSelectRemoved} {sqlOrderBy} OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            return sqlPage;
        }
    }
    static class BuildMysqlPageSql
    {
        public static string BuildPageQuery(long skip, long take, PagingHelper.SQLParts parts)
        {
            parts.sqlSelectRemoved = PagingHelper.rxOrderBy.Replace(parts.sqlSelectRemoved, "", 1);
            if (PagingHelper.rxDistinct.IsMatch(parts.sqlSelectRemoved))
            {
                parts.sqlSelectRemoved = "peta_inner.* FROM (SELECT " + parts.sqlSelectRemoved + ") peta_inner";
            }

            var sqlOrderBy = parts.sqlOrderBy ?? "ORDER BY (SELECT NULL)";
            var sqlPage = $"SELECT * FROM (SELECT ROW_NUMBER() OVER ({sqlOrderBy}) peta_rn, {parts.sqlSelectRemoved}) peta_paged WHERE peta_rn>{skip} AND peta_rn<={skip + take}";

            return sqlPage;
        }

        // 使用 LIMIT 子句的 MySQL 版本
        public static string BuildPageQuery2(long skip, long take, PagingHelper.SQLParts parts)
        {
            parts.sqlSelectRemoved = PagingHelper.rxOrderBy.Replace(parts.sqlSelectRemoved, "", 1);
            if (PagingHelper.rxDistinct.IsMatch(parts.sqlSelectRemoved))
            {
                parts.sqlSelectRemoved = "peta_inner.* FROM (SELECT " + parts.sqlSelectRemoved + ") peta_inner";
            }

            var sqlOrderBy = parts.sqlOrderBy ?? "ORDER BY (SELECT NULL)";
            var sqlPage = $"SELECT {parts.sqlSelectRemoved} {sqlOrderBy} LIMIT {skip}, {take}";

            return sqlPage;
        }
    }
    static class BuildDMPageSql
    {
        public static string BuildPageQuery(long skip, long take, PagingHelper.SQLParts parts)
        {
            parts.sqlSelectRemoved = PagingHelper.rxOrderBy.Replace(parts.sqlSelectRemoved, "", 1);
            if (PagingHelper.rxDistinct.IsMatch(parts.sqlSelectRemoved))
            {
                parts.sqlSelectRemoved = "peta_inner.* FROM (SELECT " + parts.sqlSelectRemoved + ") peta_inner";
            }

            var sqlOrderBy = parts.sqlOrderBy ?? "ORDER BY (SELECT NULL)";
            var sqlPage = $"SELECT * FROM (SELECT ROW_NUMBER() OVER ({sqlOrderBy}) peta_rn, {parts.sqlSelectRemoved}) peta_paged WHERE peta_rn>{skip} AND peta_rn<={skip + take}";

            return sqlPage;
        }

        // 使用 LIMIT 子句的达梦数据库版
        public static string BuildPageQuery2(long skip, long take, PagingHelper.SQLParts parts)
        {
            parts.sqlSelectRemoved = PagingHelper.rxOrderBy.Replace(parts.sqlSelectRemoved, "", 1);
            if (PagingHelper.rxDistinct.IsMatch(parts.sqlSelectRemoved))
            {
                parts.sqlSelectRemoved = "peta_inner.* FROM (SELECT " + parts.sqlSelectRemoved + ") peta_inner";
            }

            var sqlOrderBy = parts.sqlOrderBy ?? "ORDER BY (SELECT NULL)";
            var sqlPage = $"SELECT {parts.sqlSelectRemoved} {sqlOrderBy} OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";

            return sqlPage;
        }
    }
    static class BuildPgSqlPageSql
    {
        public static string BuildPageQuery(long skip, long take, PagingHelper.SQLParts parts)
        {
            parts.sqlSelectRemoved = PagingHelper.rxOrderBy.Replace(parts.sqlSelectRemoved, "", 1);
            if (PagingHelper.rxDistinct.IsMatch(parts.sqlSelectRemoved))
            {
                parts.sqlSelectRemoved = "peta_inner.* FROM (SELECT " + parts.sqlSelectRemoved + ") peta_inner";
            }

            var sqlOrderBy = parts.sqlOrderBy ?? "ORDER BY (SELECT NULL)";
            var sqlPage = $"SELECT * FROM (SELECT ROW_NUMBER() OVER ({sqlOrderBy}) peta_rn, {parts.sqlSelectRemoved}) peta_paged WHERE peta_rn>{skip} AND peta_rn<={skip + take}";

            return sqlPage;
        }

        // 使用 OFFSET 和 LIMIT 子句的 PostgreSQL 版本
        public static string BuildPageQuery2(long skip, long take, PagingHelper.SQLParts parts)
        {
            parts.sqlSelectRemoved = PagingHelper.rxOrderBy.Replace(parts.sqlSelectRemoved, "", 1);
            if (PagingHelper.rxDistinct.IsMatch(parts.sqlSelectRemoved))
            {
                parts.sqlSelectRemoved = "peta_inner.* FROM (SELECT " + parts.sqlSelectRemoved + ") peta_inner";
            }

            var sqlOrderBy = parts.sqlOrderBy ?? "ORDER BY (SELECT NULL)";
            var sqlPage = $"{parts.sqlSelectRemoved} {sqlOrderBy} OFFSET {skip} LIMIT {take}";

            return sqlPage;
        }
    }


    static class PagingHelper
    {
        public struct SQLParts
        {
            public string sql;
            public string sqlCount;
            public string sqlSelectRemoved;
            public string sqlOrderBy;
        }

        public static bool SplitSQL(string sql, out SQLParts parts)
        {
            parts.sql = sql;
            parts.sqlSelectRemoved = null;
            parts.sqlCount = null;
            parts.sqlOrderBy = null;

            // Extract the columns from "SELECT <whatever> FROM"
            var m = rxColumns.Match(sql);
            if (!m.Success)
                return false;

            // Save column list and replace with COUNT(*)
            Group g = m.Groups[1];
            parts.sqlSelectRemoved = sql.Substring(g.Index);

            if (rxDistinct.IsMatch(parts.sqlSelectRemoved))
                parts.sqlCount = sql.Substring(0, g.Index) + "COUNT(" + m.Groups[1].ToString().Trim() + ") " + sql.Substring(g.Index + g.Length);
            else
                parts.sqlCount = sql.Substring(0, g.Index) + "COUNT(*) " + sql.Substring(g.Index + g.Length);


            // Look for the last "ORDER BY <whatever>" clause not part of a ROW_NUMBER expression
            m = rxOrderBy.Match(parts.sqlCount);
            if (!m.Success)
            {
                parts.sqlOrderBy = null;
            }
            else
            {
                g = m.Groups[0];
                parts.sqlOrderBy = g.ToString();
                parts.sqlCount = parts.sqlCount.Substring(0, g.Index) + parts.sqlCount.Substring(g.Index + g.Length);
            }

            return true;
        }

        public static Regex rxColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        public static Regex rxOrderBy = new Regex(@"\bORDER\s+BY\s+(?!.*?(?:\)|\s+)AS\s)(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*", RegexOptions.RightToLeft | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        public static Regex rxDistinct = new Regex(@"\ADISTINCT\s", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
    }
}
