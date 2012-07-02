namespace Teamworks.Core.Services.Storage
{
    public interface IStorage
    {
        object this[object key] { get; set; }
        int Count { get; }
        void Clear();
    }
}