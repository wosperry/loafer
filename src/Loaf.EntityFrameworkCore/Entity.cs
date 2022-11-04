namespace Loaf.EntityFrameworkCore;

public abstract class Entity<TKey> : IEntity where TKey : struct
{
    public long Id { get; set; }
}