using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace MergeTool.Tests.Unit
{
    [TestClass]
    public class SimpleIdentityComparer_UnitTest
    {
        [TestMethod]
        public void when_confidence_is_greater_than_1_should_throw_error()
        {
            try
            {
                new SimpleIdentityComparer(2);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail("bad exception");
            }
        }

        [TestMethod]
        public void when_confidence_is_lower_than_0_should_throw_error()
        {
            try
            {
                new SimpleIdentityComparer(-1);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail("bad exception");
            }
        }

        [TestMethod]
        public void when_identical_confidence_should_be_1()
        {
            var fixture = new Fixture();

            var contact1 = fixture.Create<Contact>();
            var clone = (Contact)contact1.Clone();

            var target = new SimpleIdentityComparer();

            double expected = 1;
            double actual = target.Compare(contact1, clone).Confidence;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void when_totally_difference_confidence_should_be_0()
        {
            var fixture = new Fixture();

            var contact1 = fixture.Create<Contact>();
            var contact2 = fixture.Create<Contact>();

            var target = new SimpleIdentityComparer();

            const double expected = 0;
            double actual = target.Compare(contact1, contact2).Confidence;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void when_emails_are_identical_then_confidence_should_be_90_percent()
        {
            var fixture = new Fixture();

            var contact1 = fixture.Create<Contact>();
            var contact2 = fixture.Create<Contact>();
            contact1.EmailAddress = contact2.EmailAddress = "a@b.com";

            var target = new SimpleIdentityComparer();

            const double expected = 0.9;
            double actual = target.Compare(contact1, contact2).Confidence;

            Assert.IsTrue(expected <= actual);
        }

        [TestMethod]
        public void when_phone_are_identical_then_confidence_should_be_90_percent()
        {
            var fixture = new Fixture();

            var contact1 = fixture.Create<Contact>();
            var contact2 = fixture.Create<Contact>();
            contact1.PhoneNumber = contact2.PhoneNumber = "654 654654 65465";

            var target = new SimpleIdentityComparer();

            const double expected = 0.9;
            double actual = target.Compare(contact1, contact2).Confidence;

            Assert.IsTrue(expected <= actual);
        }
    }
}
