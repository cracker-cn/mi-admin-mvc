using Mi.Entity.System.Enum;

namespace Mi.Core.Models.UI
{
    public class LayuiTreeModel
    {
        public string? Title { get; set; }
        public string? Id { get; set; }
        public string? Field { get; set; }
        public string? Href { get; set; }
        public IList<LayuiTreeModel>? Children { get; set; }
        public bool Spread { get; set; }
        public bool Checked { get; set; }
        public bool Disabled { get; set; } = false;

        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public long ParentId { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public EnumTreeNode Node { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int Sort { get; set; }
    }
}