﻿namespace Mi.Entity.System
{
    public class SysLog : EntityBase
    {
        public long UserId { get; set; }

        public string? UserName { get; set; }

        public string? ActionFullName { get; set; }

        public string? RequestParams { get; set; }

        public string? RequestUrl { get; set; }

        public string? ContentType { get; set; }

        public int Succeed { get; set; } = 0;

        public string? Exception { get; set; }

        public string? UniqueId { get; set; }
    }
}
