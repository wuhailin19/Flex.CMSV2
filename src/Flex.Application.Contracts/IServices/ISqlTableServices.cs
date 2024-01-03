using Flex.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Application.Contracts.IServices
{
    public interface ISqlTableServices
    {
        string CreateContentTableSql(string TableName);
        string DeleteTableField(string TableName, List<sysField> model);
        string InsertTableField(string TableName, sysField model);
    }
}
