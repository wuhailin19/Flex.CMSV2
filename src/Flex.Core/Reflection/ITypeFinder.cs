﻿namespace Flex.Core.Reflection
{
    /// <summary> 类型查找器 </summary>
    public interface ITypeFinder : ILifetimeDependency
    {
        Type[] Find(Func<Type, bool> expression);

        Type[] FindAll();
    }
}
