namespace DataLayer.Interfaces
{
    public interface IEntitySerializer<T>
    {
        string Serialize(T objInstance);
    }
}