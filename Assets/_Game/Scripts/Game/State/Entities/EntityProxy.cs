
public class EntityProxy
{
    public int Id { get; }
    public string TypeId { get; }

    public EntityProxy(Entity entity)
    {
        Id = entity.Id;
        TypeId = entity.TypeId;
    }
}