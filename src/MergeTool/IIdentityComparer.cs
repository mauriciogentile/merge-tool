namespace MergeTool
{
    public interface IIdentityComparer<in T>
    {
        IdentityComparison Compare(T obj1, T obj2);
    }
}