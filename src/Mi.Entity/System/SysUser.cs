﻿namespace Mi.Entity.System
{
    /// <summary>
    /// 用户
    /// </summary>
    [Table("SysUser")]
    public class SysUser : EntityBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [NotNull]
        public string? UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [NotNull]
        public string? Password { get; set; }

        /// <summary>
        /// 密码盐
        /// </summary>
        [NotNull]
        public string? PasswordSalt { get; set; }
    }
}