namespace Mi.Entity.Field
{
    public interface IPrimaryKey<T>
    {
        T Id { get; set; }
    }
}