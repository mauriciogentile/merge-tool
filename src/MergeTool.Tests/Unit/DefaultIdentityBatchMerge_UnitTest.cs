using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;

namespace MergeTool.Tests.Unit
{
    [TestClass]
    public class DefaultIdentityBatchMerge_UnitTest
    {
        [TestMethod]
        public void when_input_is_null_should_throw_error()
        {
            var target = new DefaultIdentityBatchMerge(It.IsAny<IIdentityComparer<Contact>>(), It.IsAny<IIdentityMerge<Contact>>());

            try
            {
                target.Merge(null, null);
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail("bad exception");
            }
        }

        [TestMethod]
        public void when_ProcessedItem_is_null_should_throw_error()
        {
            var target = new DefaultIdentityBatchMerge(It.IsAny<IIdentityComparer<Contact>>(), It.IsAny<IIdentityMerge<Contact>>());

            try
            {
                target.Merge(It.IsAny<List<Contact>>(), It.IsAny<List<Contact>>());
                Assert.Fail("exception not thrown");
            }
            catch (ArgumentNullException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail("bad exception");
            }
        }

        [TestMethod]
        public void when_error_call_error_handler()
        {
            var fixture = new Fixture();

            var leftList = fixture.CreateMany<Contact>().ToList();
            var rightList = fixture.CreateMany<Contact>().ToList();

            var signal = new ManualResetEvent(false);

            Exception actual = null;

            var faultyMerger = new Mock<IIdentityMerge<Contact>>();
            faultyMerger.Setup(x => x.Merge(It.IsAny<Contact>(), It.IsAny<Contact>())).Throws<ApplicationException>();

            var target = new DefaultIdentityBatchMerge(new AlwaysSameIdentity(), faultyMerger.Object)
            {
                ProcessedItem = x => { },
                Error = exc => { actual = exc; signal.Set(); }
            };

            target.MergeAsync(leftList, rightList);

            signal.WaitOne();

            Assert.AreEqual(typeof(AggregateException), actual.GetType());
            Assert.IsTrue(((AggregateException)actual).InnerExceptions.All(x => x.GetType() == typeof(ApplicationException)));
        }

        [TestMethod]
        public void when_contacts_are_same_person_they_should_be_merged()
        {
            var fixture = new Fixture();

            var contact1 = fixture.Create<Contact>();
            var contact2 = fixture.Create<Contact>();

            var leftList = new[] { contact2 }.ToList();
            var rightList = new[] { contact1 }.ToList();

            var signal = new ManualResetEvent(false);

            MergeOutput<Contact> actual = null;
            int outpuItems = 0;

            var target = new DefaultIdentityBatchMerge(new AlwaysSameIdentity(), new AlwaysMerge())
            {
                ProcessedItem = x => { outpuItems++; actual = x; },
                Finish = () => signal.Set()
            };

            target.MergeAsync(leftList, rightList);

            signal.WaitOne();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.MergeOperation == MergeOperation.Merged);
            Assert.IsTrue(outpuItems == 1);
        }

        [TestMethod]
        public void when_contacts_are_diff_person_they_should_not_be_merged()
        {
            var fixture = new Fixture();

            var contact1 = fixture.Create<Contact>();
            var contact2 = fixture.Create<Contact>();

            var leftList = new[] { contact1 }.ToList();
            var rightList = new[] { contact2 }.ToList();

            var signal = new ManualResetEvent(false);

            var actual = new List<MergeOutput<Contact>>();

            var target = new DefaultIdentityBatchMerge(new NeverSameIdentity(), It.IsAny<IIdentityMerge<Contact>>())
            {
                Finish = () => signal.Set(),
                ProcessedItem = x => actual.Add(x)
            };

            target.MergeAsync(leftList, rightList);

            signal.WaitOne();

            Assert.IsTrue(actual.Any(x => x.MergeOperation == MergeOperation.Added));
            Assert.IsTrue(actual.Any(x => x.MergeOperation == MergeOperation.NotChanged));
            Assert.IsTrue(actual.Any(x => x.Result.Print() == contact1.Print()));
            Assert.IsTrue(actual.Any(x => x.Result.Print() == contact2.Print()));
            Assert.IsTrue(actual.Count == 2);
        }

        [TestMethod]
        public void when_right_list_is_empty_all_left_list_should_be_NotChanged()
        {
            var fixture = new Fixture();

            var contact1 = fixture.Create<Contact>();
            var contact2 = fixture.Create<Contact>();

            var leftList = new[] { contact1, contact2 }.ToList();
            var rightList = new List<Contact>();

            var signal = new ManualResetEvent(false);

            var actual = new List<MergeOutput<Contact>>();

            var target = new DefaultIdentityBatchMerge(new NeverSameIdentity(), It.IsAny<IIdentityMerge<Contact>>())
            {
                Finish = () => signal.Set(),
                ProcessedItem = x => actual.Add(x)
            };

            target.MergeAsync(leftList, rightList);

            signal.WaitOne();

            Assert.IsTrue(actual.All(x => x.MergeOperation == MergeOperation.NotChanged));
            Assert.IsTrue(actual.Any(x => x.Result.Print() == contact1.Print()));
            Assert.IsTrue(actual.Any(x => x.Result.Print() == contact2.Print()));
            Assert.IsTrue(actual.Count == 2);
        }

        [TestMethod]
        public void when_left_list_is_empty_all_right_list_should_be_Added()
        {
            var fixture = new Fixture();

            var contact1 = fixture.Create<Contact>();
            var contact2 = fixture.Create<Contact>();

            var emptyList = new List<Contact>();
            var rightList = new[] { contact1, contact2 }.ToList();

            var signal = new ManualResetEvent(false);

            var actual = new List<MergeOutput<Contact>>();

            var target = new DefaultIdentityBatchMerge(new NeverSameIdentity(), It.IsAny<IIdentityMerge<Contact>>())
            {
                Finish = () => signal.Set(),
                ProcessedItem = x => actual.Add(x)
            };

            target.MergeAsync(emptyList, rightList);

            signal.WaitOne();

            Assert.IsTrue(actual.All(x => x.MergeOperation == MergeOperation.Added));
            Assert.IsTrue(actual.Any(x => x.Result.Print() == contact1.Print()));
            Assert.IsTrue(actual.Any(x => x.Result.Print() == contact2.Print()));
            Assert.IsTrue(actual.Count == 2);
        }
    }
}
