using Mi.Entity.Field;

namespace Mi.Core.CommonOption
{
    public class TreeOption : Option, IChildren<IList<TreeOption>>
    {
        public IList<TreeOption>? Children { get; set; }
    }
}