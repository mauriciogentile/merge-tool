using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using MergeTool.Tests.Util;

namespace MergeTool.Tests.Unit
{
    [TestClass]
    public class SimpleIdentityMerge_UnitTest
    {
        [TestMethod]
        public void when_identical_should_not_merge()
        {
            var fixture = new Fixture();

            var contact1 = fixture.Create<Contact>();
            var clone = (Contact)contact1.Clone();

            var target = new SimpleIdentityMerge();

            bool merged = target.Merge(contact1, clone);

            Assert.IsFalse(merged);
        }

        [TestMethod]
        public void when_different_Emails_should_merge_both()
        {
            var fixture = new Fixture();

            var contact1 = fixture.Create<Contact>();
            var contact2 = (Contact)contact1.Clone();
            contact2.EmailAddress = "a@b.com";

            string expected = contact1.EmailAddress + ";" + contact2.EmailAddress;

            var target = new SimpleIdentityMerge();

            bool merged = target.Merge(contact1, contact2);

            Assert.IsTrue(merged);
            Assert.AreEqual(expected, contact1.EmailAddress);
        }

        [TestMethod]
        public void when_different_Phone_should_merge_both()
        {
            var fixture = new Fixture();

            var contact1 = fixture.Create<Contact>();
            var contact2 = (Contact)contact1.Clone();
            contact2.PhoneNumber = "+1 (202) 654-4987";

            string expected = contact1.PhoneNumber + ";" + contact2.PhoneNumber;

            var target = new SimpleIdentityMerge();

            bool merged = target.Merge(contact1, contact2);

            Assert.IsTrue(merged);
            Assert.AreEqual(expected, contact1.PhoneNumber);
        }

        [TestMethod]
        public void when_left_incomplete_and_right_complete_should_merge_to_left()
        {
            var fixture = new Fixture();

            var contact1 = new Contact();
            var contact2 = fixture.Create<Contact>();

            var target = new SimpleIdentityMerge();

            bool merged = target.Merge(contact1, contact2);

            Assert.IsTrue(merged);
            AssertEx.AreEqualByJson(contact1, contact2);
        }
    }
}
