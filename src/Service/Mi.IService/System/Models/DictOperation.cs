﻿using System.Diagnostics.CodeAnalysis;

namespace Mi.IService.System.Models
{
    public class DictOperation
    {
        public long Id { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        [NotNull]
        public string? Name { get; set; }

        /// <summary>
        /// 字典Key，唯一
        /// </summary>
        [NotNull]
        public string? Key { get; set; }

        /// <summary>
        /// 字典值
        /// </summary>
        public string? Value { get; set; }

        public long ParentId { get; set; }
        public string? Remark { get; set; }
        public int Sort { get; set; }
    }
}