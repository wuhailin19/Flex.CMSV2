﻿using Flex.Core.Extensions;

namespace Flex.Core
{
    /// <summary> 常量 </summary>
    public static class Consts
    {
        /// <summary> 版本号 </summary>
        public const string Version = "1.3.0";

        /// <summary> 产品模式 </summary>
        public static ProductMode Mode => "mode".Config(ProductMode.Dev);

        public static string RSAKeyMode => "RSAmode".Config("new");
    }
}