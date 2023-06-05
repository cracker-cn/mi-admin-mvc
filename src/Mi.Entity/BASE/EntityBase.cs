using System.Text;

namespace Mi.Entity.BASE
{
    public abstract class EntityBase : IEntityBase
    {
        [Key]
        public long Id { get; set; }

        [DefaultValue(-1)]
        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now.ToLocalTime();
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [DefaultValue(0)]
        public int IsDeleted { get; set; } = 0;

        public void EchoBaseFields()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Id:{Id}");
            sb.AppendLine($"CreatedBy:{CreatedBy}");
            sb.AppendLine($"CreatedOn:{CreatedOn}");
            sb.AppendLine($"ModifiedBy:{ModifiedBy}");
            sb.AppendLine($"ModifiedOn:{ModifiedOn}");
            sb.AppendLine($"IsDeleted:{IsDeleted == 1}");

            Console.WriteLine("========== EchoBaseFields Start ==========");
            Console.WriteLine(sb.ToString());
            Console.WriteLine("========== EchoBaseFields End ==========");
        }
    }
}