using System;
using System.Linq;
using System.Threading;
using MergeTool.Data;
using System.Diagnostics;

namespace MergeTool.Console
{
    class Program
    {
        static readonly ManualResetEvent EndSignal = new ManualResetEvent(false);
        const double DefaultConfidence = 0.7;

        static void Main(string[] args)
        {
            string file = @".\Test\Contacts1.json";
            double confidence = DefaultConfidence;

            if (args.Length > 0)
            {
                file = @args[0];
            }
            if (args.Length > 1)
            {
                if (!double.TryParse(args[1], out confidence))
                {
                    confidence = DefaultConfidence;
                }
            }

            ConfigureListener();

            var leftList = new InMemoryContactsRepository().FindAll().ToList();
            var rightList = Helper.ReadFromFile(file);

            var batchMerge = new DefaultIdentityBatchMerge(new SimpleIdentityComparer(confidence), new SimpleIdentityMerge())
            {
                ProcessedItem = OnProcessedItem,
                Error = OnError,
                Finish = OnFinish
            };

            Helper.LogInfo("Working...");
            Helper.LogInfo("-------------------------------");

            batchMerge.MergeAsync(leftList, rightList);

            EndSignal.WaitOne();

            Helper.LogInfo("Press any key to exit...");

            System.Console.Read();
        }

        static void ConfigureListener()
        {
            string fileOutput = string.Format("{0}.json", Guid.NewGuid());
            Helper.LogInfo("Writing to file '{0}'", fileOutput);
            Trace.Listeners.Add(new TextWriterTraceListener(fileOutput));
            Trace.AutoFlush = true;
            Trace.WriteLine("[");
        }

        static void OnProcessedItem(MergeOutput<Contact> mergeItem)
        {
            Helper.LogInfo("{0} => {1}", mergeItem.MergeOperation.ToString().PadRight(10), mergeItem.Result.Print());
            Trace.WriteLine(mergeItem.Result.ToJson() + ",");
        }

        static void OnError(Exception exc)
        {
            Helper.LogError("Error processing contacts: {0}", exc);
            EndSignal.Set();
        }

        static void OnFinish()
        {
            Helper.LogInfo("-------------------------------");
            Trace.WriteLine("]");
            EndSignal.Set();
        }
    }
}
