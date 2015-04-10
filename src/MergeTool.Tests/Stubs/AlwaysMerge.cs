namespace MergeTool.Tests
{
    class AlwaysMerge : IIdentityMerge<Contact>
    {
        public bool Merge(Contact obj1, Contact obj2)
        {
            return true;
        }
    }
}
