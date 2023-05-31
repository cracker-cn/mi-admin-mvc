using Mi.Entity.System;

namespace Mi.IService.System.Models.Result
{
    public class SysDictItem : SysDict
    {
        public int ChildCount { get; set; }

        public string? ParentName { get; set; }
    }
}