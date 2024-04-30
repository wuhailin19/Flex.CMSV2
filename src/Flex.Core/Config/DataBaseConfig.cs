using Autofac.Core;
using Flex.Core.Extensions;
using Flex.Core.Framework.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Core.Config
{
    public static class DataBaseConfig
    {
        public static DataBaseType dataBase { get; set; }
        public static string ConnectionString { get; set; } = string.Empty;
        static DataBaseConfig() {
            var usedb = "DataConfig:UseDb".Config(string.Empty) ?? "Sqlserver";
            switch (usedb)
            {
                case "Sqlserver":
                    dataBase = DataBaseType.SqlServer;
                    ConnectionString = "DataConfig:Sqlserver:ConnectionString".Config(string.Empty);
                    break;
                case "Mysql":
                    dataBase = DataBaseType.Mysql;
                    ConnectionString = "DataConfig:Mysql:ConnectionString".Config(string.Empty);
                    break;
                case "DM8":
                    dataBase = DataBaseType.DM;
                    ConnectionString = "DataConfig:DM8:ConnectionString".Config(string.Empty);
                    break;
                case "PostgreSQL":
                    dataBase = DataBaseType.PgSql;
                    ConnectionString = "DataConfig:PostgreSQL:ConnectionString".Config(string.Empty);
                    break;
            }
        }
    }
}
