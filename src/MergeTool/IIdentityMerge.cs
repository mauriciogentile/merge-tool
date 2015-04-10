namespace MergeTool
{
    public interface IIdentityMerge<in T>
    {
        bool Merge(T obj1, T obj2);
    }
}