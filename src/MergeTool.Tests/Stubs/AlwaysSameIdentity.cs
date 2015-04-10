namespace MergeTool.Tests
{
    class AlwaysSameIdentity : IIdentityComparer<Contact>
    {
        public IdentityComparison Compare(Contact obj1, Contact obj2)
        {
            return new IdentityComparison { Match = true, Confidence = 1 };
        }
    }
}