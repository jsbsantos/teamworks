namespace Teamworks.Core.Extensions {
    public interface ILocalData {
        object this[object key] { get; set; }
        int Count { get; }
        void Clear();
    }
}