using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Core.Helper
{
	public static class SaltStringHelper
	{
		/// <summary>
		/// 获取随机盐值
		/// </summary>
		/// <returns></returns>
		public static string getSaltStr()
		{
			var guidstr = Guid.NewGuid().ToString("N");
			return guidstr.subString(16);
		}
		public static string subString(this string str, int length)
		{
			if (str.Length < length || length < 0)
				return str;
			return str.Substring(0, length);
		}
	}
}
