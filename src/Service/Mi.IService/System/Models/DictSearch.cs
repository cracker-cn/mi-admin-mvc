using Mi.Entity.Field;

namespace Mi.IService.System.Models
{
    public class DictSearch : PagingSearchModel, IRemark
    {
        /// <summary>
        /// Name/Key
        /// </summary>
        public string? Vague { get; set; }

        public string? Remark { get; set; }
    }
}