using Flex.Dapper.Context;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using Flex.Core.Extensions;

namespace Flex.Dapper
{
    public class MyDBContext : DapperDBContext
    {
        public MyDBContext(IOptions<DapperDBContextOptions> optionsAccessor) : base(optionsAccessor)
        {
        }

        protected override IDbConnection CreateConnection()
        {
            IDbConnection conn = new SqlConnection("DataConfig:Sqlserver:ConnectionString".Config(string.Empty));
            return conn;
        }
    }
}
