﻿namespace Mi.IRepository.System.QueryModels
{
    public class UserItem
    {
        public long UserId { get; set; }

        [NotNull]
        public string? UserName { get; set; }

        public string? RoleNameString { get; set; }

        public DateTime CreatedOn { get; set; }

        public IList<string>? RoleNames { get; set; }

        public int IsEnabled { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string? NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        public string? Signature { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; set; }
    }
}