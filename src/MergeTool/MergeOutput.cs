namespace MergeTool
{
    public enum MergeOperation
    {
        Added,
        Merged,
        NotChanged
    }

    public class MergeOutput<T>
    {
        public MergeOperation MergeOperation { get; set; }
        public T Result { get; set; }
    }
}