using Flex.Core.Timing;
using Flex.Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using ShardingCore.Extensions;

namespace Flex.WebApi.NormalControllers
{
    [Route("api/[controller]")]
    public class ContentController : ApiBaseController
    {
        public MyDBContext _dapperDBContext;
        public ContentController(MyDBContext dapperDBContext)
        {
            _dapperDBContext = dapperDBContext;
        }
        [AllowAnonymous]
        [HttpGet("getSqlString")]
        public async Task<string> getSqlString() {
            var result = await _dapperDBContext.GetDynamicAsync(@"
                select * from urllist
            ");
            string insertsql = string.Empty;
            int count = 0;
            foreach (var item in result)
            {
                string datetime = DateTime.Now.AddMinutes(count).ToString("yyyy-MM-dd HH:mm:ss");
                insertsql += @$"INSERT INTO monitor (name, active, user_id, interval, url, type, weight, hostname, port, created_date, maxretries, ignore_tls, upside_down, maxredirects, accepted_statuscodes_json, retry_interval, method, packet_size, http_body_encoding, http_body_encoding,dns_resolve_type,dns_resolve_server,kafka_producer_brokers,kafka_producer_sasl_options,oauth_auth_method,timeout,expiry_notification)
                VALUES ('{item.name}', 1, 1, 1800, '{item.url}', 'http', 2000, NULL, NULL, '{datetime}', 
                5, 0, 0, 10, '[""200-299""]', 30, 'GET', 56, 'json', NULL,'A','1.1.1.1','[]','{{""mechanism"":""None""}}','client_secret_basic',48.0,{item.ishttps});";

                //insertsql += @"insert into heartbeat(important,monitor_id,status,msg,time,ping,duration,down_count) values (0,)";
                count++;
            }

            return insertsql;
        }

        [AllowAnonymous]
        [HttpGet("getInsertSqlString")]
        public async Task<string> getInsertSqlString()
        {
            var result = await _dapperDBContext.GetDynamicAsync(@"
                select * from news
            ");
            string insertsql = string.Empty;
            int count = 400;
            foreach (var item in result)
            {
                var id = count + Convert.ToInt32(item.id);
                 var datetime = item.ndate;
                // insertsql += @$"INSERT INTO `content_main_info` VALUES ({id}, NULL, 10969, -1, -1, '{item.subject_tc}', '', '', NULL, 'admin', '001', '', NULL, 1, '', 'color:;font-weight:;font-style:;text-decoration:',
                //'', '{datetime}', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 'id=0;t=i;sid=12;reUrl=;iw=246;ih=262;', 'id=0;t=i;sid=12;reUrl=;iw=246;ih=262;', 'id=0;t=i;sid=12;reUrl=;iw=246;ih=262;', 'id=0;t=i;sid=12;reUrl=;iw=246;ih=262;', '{datetime}', NULL, 'error:rule', NULL, 1, 1, 0, 1, {id}, '', NULL, 9836, '{datetime}', '9999-12-31 00:00:00', 1, 1, 0, 1, 1, '', '', '-1', 0, 0, 167, 12, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);";
                insertsql += @$"INSERT INTO jtopcms_model_zlwj values({id},'{item.subject_tc}','id=;t=f;sid=12;reUrl={item.upload_tc};fn={item.filenames};');";
            }

            return insertsql;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<string> GetAllContent()
        {
            var result = await _dapperDBContext.GetDynamicAsync(@"
select d.*,model.relateTableName,model.titleCol,model.dataModelId from (
SELECT c.*, sg.siteName,sg.siteId
FROM (
    SELECT cc.classId, cc.className, cc.siteFlag, cc.contentType
    FROM (
        SELECT classId, className, siteFlag, contentType
        FROM contentclass
        WHERE contentType > 0 AND contentType <> 152
        GROUP BY contentType
        HAVING COUNT(1) > 1
        ORDER BY COUNT(1) DESC
    ) AS b
    LEFT JOIN contentclass cc ON cc.contentType = b.contentType
) AS c
LEFT JOIN site_group sg ON sg.siteFlag = c.siteFlag
ORDER BY c.siteFlag ASC
) as d
left join model
on model.dataModelId=d.contentType
where classId=11607;");
            string sql = string.Empty;
            foreach (var item in result)
            {
                if (sql.IsEmpty())
                    sql += $"select {item.dataModelId} as modelId,contentId,{item.classId} as classId,{item.siteId} as siteId,jtopcms_def_{item.titleCol} as titles from {item.relateTableName} order by contentId";
                else
                    sql += $" union select {item.dataModelId} as modelId,contentId,{item.classId} as classId,{item.siteId} as siteId,jtopcms_def_{item.titleCol} as titles from {item.relateTableName}";
            }
            var contentlist = await _dapperDBContext.GetDynamicAsync(sql);

            string insertsql = string.Empty;
            var idlist = new List<int>();
            var count = 0;
            foreach (var item in contentlist)
            {
                var datetime = Convert.ToDateTime("2019-01-21 07:16:42").AddDays(count).ToString("yyyy-MM-dd HH:mm:ss");
                if (idlist.Contains(Convert.ToInt32(item.contentId)))
                    continue;
                idlist.Add(Convert.ToInt32(item.contentId));
                insertsql += @$"INSERT INTO `content_main_info` VALUES ({item.contentId}, NULL, {item.classId}, -1, -1, '{item.titles}', '', '', NULL, 'admin', '001', '', NULL, 1, '', 'color:;font-weight:;font-style:;text-decoration:',
'', '{datetime}', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 'id=0;t=i;sid={item.siteId};reUrl=;iw=246;ih=262;', 'id=0;t=i;sid={item.siteId};reUrl=;iw=246;ih=262;', 'id=0;t=i;sid={item.siteId};reUrl=;iw=246;ih=262;', 'id=0;t=i;sid={item.siteId};reUrl=;iw=246;ih=262;', '{datetime}', NULL, 'error:rule', NULL, 1, 1, 0, 1, {item.contentId}, '', NULL, 9836, '{datetime}', '9999-12-31 00:00:00', 1, 1, 0, 1, 1, '', '', '-1', 0, 0, {item.modelId}, {item.siteId}, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);";
                count++;
            }
            //var resultcount = _dapperDBContext.ExecuteScalar(insertsql);
            return insertsql;
        }
    }
}
