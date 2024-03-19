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
        static DataBaseConfig() {
            var usedb = "DataConfig:UseDb".Config(string.Empty) ?? "Sqlserver";
            switch (usedb)
            {
                case "Sqlserver":
                    dataBase = DataBaseType.SqlServer;
                    break;
                case "Mysql":
                    dataBase = DataBaseType.Mysql;
                    break;
                case "DM8":
                    dataBase = DataBaseType.DM;
                    break;
                case "PostgreSQL":
                    dataBase = DataBaseType.PgSql;
                    break;
            }
        }
    }
}
