using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Mi.Core.Models
{
    public class SysConfigModel
    {
        /// <summary>
        /// 站点标题
        /// </summary>
        public string? site_title { get; set; }

        /// <summary>
        /// 站点ico
        /// </summary>
        public string? site_icon { get; set; }

        /// <summary>
        /// 进入后侧边栏Logo
        /// </summary>
        public string? logo { get; set; }

        /// <summary>
        /// 进入后侧边栏显示名称
        /// </summary>
        [NotNull]
        [Required]
        public string? header_name { get; set; }

        /// <summary>
        /// 登录页中间显示名称
        /// </summary>
        public string? login_middle_name { get; set; }

        /// <summary>
        /// 登录页底部显示文本
        /// </summary>
        public string? login_footer_word { get; set; }

        /// <summary>
        /// 默认页行数
        /// </summary>
        [NotNull]
        [Required]
        public int default_page_size { get; set; } = 10;

        /// <summary>
        /// 分页行数下拉选项 例：10,20,40,80
        /// </summary>
        [NotNull]
        [Required]
        public string? default_size_array { get; set; }

        /// <summary>
        /// 首页名称
        /// </summary>
        [NotNull]
        [Required]
        public string? home_page_name { get; set; }

        /// <summary>
        /// 首页地址
        /// </summary>
        [NotNull]
        public string? home_page_url { get; set; }
    }
}
