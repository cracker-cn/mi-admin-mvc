﻿using Mi.Entity.System.Enum;

namespace Mi.Entity.System
{
    /// <summary>
    /// 功能
    /// </summary>
    [Table("SysFunction")]
    public class SysFunction : EntityBase, ISort, IParentId<long>, IChildren<string>
    {
        /// <summary>
        /// 功能名称
        /// </summary>
        [NotNull]
        public string? FunctionName { get; set; }

        /// <summary>
        /// 树节点
        /// </summary>
        public EnumTreeNode Node { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// 功能类型
        /// </summary>
        public EnumFunctionType FunctionType { get; set; }

        /// <summary>
        /// 子集，存SysFunction.Id，多个以','隔开
        /// </summary>
        public string? Children { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string? AuthorizationCode { get; set; }

        public long ParentId { get; set; }
        public int Sort { get; set; }
    }
}