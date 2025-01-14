
public class EntityProxy
{
    public int Id { get; }
    public EntityProxy(Entity entity)
    {
        Id = entity.Id;
    }
}