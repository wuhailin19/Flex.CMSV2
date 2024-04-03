using System.Reflection;
using SqlSugar;

namespace Flex.SqlSugarFactory.UnitOfWorks
{
    public interface IUnitOfWorkManage
    {
        SqlSugarClient GetDbClient();
        int TranCount { get; }

        UnitOfWork CreateUnitOfWork();

        void BeginTran();
        void BeginTran(MethodInfo method);
        void CommitTran();
        void CommitTran(MethodInfo method);
        void RollbackTran();
        void RollbackTran(MethodInfo method);
    }
}