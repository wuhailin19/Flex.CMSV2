﻿using System.Reflection;

namespace Flex.Core.Reflection
{
    /// <summary> 程序集查找器 </summary>
    public interface IAssemblyFinder : ILifetimeDependency
    {
        /// <summary> 查找所有程序集 </summary>
        /// <returns></returns>
        IEnumerable<Assembly> FindAll();

        IEnumerable<Assembly> Find(Func<Assembly, bool> expression);
    }
}
