using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MergeTool
{
    public interface IIdentityBatchMerge<T>
    {
        Action<MergeOutput<Contact>> ProcessedItem { get; set; }
        Action<Exception> Error { get; set; }
        Action Finish { get; set; }
        Task MergeAsync(IList<Contact> leftList, IList<Contact> rightList);
        void Merge(IList<Contact> leftList, IList<Contact> rightList);
    }
}