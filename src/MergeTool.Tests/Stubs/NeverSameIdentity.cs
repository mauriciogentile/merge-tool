namespace MergeTool.Tests
{
    class NeverSameIdentity : IIdentityComparer<Contact>
    {
        public IdentityComparison Compare(Contact obj1, Contact obj2)
        {
            return new IdentityComparison { Confidence = 0, Match = false };
        }
    }
}