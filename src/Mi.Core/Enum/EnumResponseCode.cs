using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi.Core.Enum
{
    public enum EnumResponseCode
    {
        /// <summary>
        ///     成功
        /// </summary>
        Success = 90001,

        /// <summary>
        ///     错误请求，参数验证失败
        /// </summary>
        BadRequest = 90002,

        /// <summary>
        ///     未登录
        /// </summary>
        NonAuth = 90003,

        /// <summary>
        ///     禁止访问
        /// </summary>
        Forbidden = 90004,

        /// <summary>
        ///     找不到，不存在
        /// </summary>
        NonExist = 90005,

        /// <summary>
        ///     程序错误
        /// </summary>
        Error = 90006,

        /// <summary>
        ///     失败
        /// </summary>
        Fail = 90007,

        /// <summary>
        /// 参数错误
        /// </summary>
        ParameterError = 90008
	}
}
