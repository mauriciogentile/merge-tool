using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MergeTool
{
    public class DefaultIdentityBatchMerge : IIdentityBatchMerge<Contact>
    {
        private readonly IIdentityComparer<Contact> _proximityComparer;
        private readonly IIdentityMerge<Contact> _identityMerge;

        public Action<MergeOutput<Contact>> ProcessedItem { get; set; }
        public Action<Exception> Error { get; set; }
        public Action Finish { get; set; }

        public DefaultIdentityBatchMerge()
            : this(new SimpleIdentityComparer(), new SimpleIdentityMerge())
        {
        }

        public DefaultIdentityBatchMerge(IIdentityComparer<Contact> proximityComparer, IIdentityMerge<Contact> identityMerge)
        {
            _proximityComparer = proximityComparer;
            _identityMerge = identityMerge;
        }

        public Task MergeAsync(IList<Contact> leftList, IList<Contact> rightList)
        {
            ValidateParams(leftList, rightList);

            return Task.Factory.StartNew(() => Merge(leftList, rightList));
        }

        public void Merge(IList<Contact> leftList, IList<Contact> rightList)
        {
            ValidateParams(leftList, rightList);

            try
            {
                MergeInternal(leftList, rightList);
            }
            catch (Exception exc)
            {
                if (Error != null)
                {
                    Error(exc);
                }
            }
            finally
            {
                if (Finish != null)
                {
                    Finish();
                }
            }
        }

        private void ValidateParams(IList<Contact> leftList, IList<Contact> rightList)
        {
            if (leftList == null)
            {
                throw new ArgumentNullException("leftList");
            }
            if (rightList == null)
            {
                throw new ArgumentNullException("rightList");
            }
            if (ProcessedItem == null)
            {
                throw new InvalidOperationException("processedItem");
            }
        }

        void MergeInternal(IList<Contact> leftList, IList<Contact> rightList)
        {
            var mergedContacts = new ConcurrentDictionary<int, int>();

            //list from the left can be safely scanned in parallel
            Parallel.ForEach(leftList, contact1 =>
            {
                bool merged = false;
                for (int i = 0; i < rightList.Count; i++)
                {
                    //already merged contacts are discarded for avoiding overlapping
                    if (mergedContacts.ContainsKey(i))
                    {
                        break;
                    }

                    Contact contact2 = rightList[i];
                    if (_proximityComparer.Compare(contact1, contact2).Match)
                    {
                        var clone = (Contact)contact1.Clone();
                        bool modified = _identityMerge.Merge(clone, contact2);

                        //notify subscriber
                        ProcessedItem(new MergeOutput<Contact>
                        {
                            Result = clone,
                            MergeOperation = modified ? MergeOperation.Merged : MergeOperation.NotChanged
                        });

                        //mark the contact for later skipping
                        mergedContacts.AddOrUpdate(i, 1, (k, v) => v + 1);
                        merged = true;
                    }
                }

                // the contact doesn't have any match
                if (!merged)
                {
                    ProcessedItem(new MergeOutput<Contact>
                    {
                        Result = contact1,
                        MergeOperation = MergeOperation.NotChanged
                    });
                }
            });

            //scan not merged contacts from right list
            Parallel.For(0, rightList.Count, i =>
            {
                if (!mergedContacts.ContainsKey(i))
                {
                    ProcessedItem(new MergeOutput<Contact>
                    {
                        Result = rightList[i],
                        MergeOperation = MergeOperation.Added
                    });
                }
            });
        }
    }
}
