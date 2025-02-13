using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Batches;

namespace WebApi
{
    public class BatchHandler
    {
        static ConcurrentQueue<updateRequest> queue = new ConcurrentQueue<updateRequest>();
        static ConcurrentDictionary<Guid, Batch> batches = new ConcurrentDictionary<Guid, Batch>();

        const int thresh = 2;
        const int batchSize = 3;
        static Guid currentBatch = new Guid();

        public static Guid AddToBatch(updateRequest request)
        {
            Guid batchId;
            var lastBatch = batches.Values.FirstOrDefault();
            /*
             * For debugging purposes
             * Console.WriteLine($"== LastBatch check: {currentBatch}");
            */
            if (lastBatch == null || batches[currentBatch].Updates.Count >= batchSize)
            {
                batchId = Guid.NewGuid();
                currentBatch = batchId;
                batches[batchId] = new Batch { BatchId = batchId };
            }
            else
            {
                batchId = currentBatch;
            }

            batches[batchId].Updates.Add(request);
            /*
             * For debugging purposes
             * Console.WriteLine($"Batch ID: {batchId}");
             * Console.WriteLine($"Request Count: {batches[batchId].Updates.Count}, Batches Count: {batches.Count}");
             */
            if (batches[batchId].Updates.Count == batchSize && batches.Count >= thresh)
            {
                ProcessBatches();
            }

            return batchId;
        }

        public static void ProcessBatches()
        {
            /*
             * For debugging purposes
             * Console.WriteLine("Starting Execution of Batches");
             */
            while (batches.Count > 0)
            {
                var batch = batches.Values.LastOrDefault();
                if (batch != null)
                {
                    ProcessBatch(batch);
                    batches.TryRemove(batch.BatchId, out _);
                }
            }
        }

        public static async void ProcessBatch(Batch batch)
        {
            /*
             * For debugging purposes
             * Console.WriteLine($"Executing batch {batch.BatchId}");
             */
            while (batch.ProcessedRequests < batch.Updates.Count)
            {
                var chunk = batch.Updates.Skip(batch.ProcessedRequests).Take(batchSize).ToList();

                await Task.Run(() =>
                {
                    foreach (var request in chunk)
                    {
                        DbHandler.UpdateDatabase(request);
                    }
                });

                batch.ProcessedRequests += chunk.Count;
            }
            /*
             * For debugging purposes
             * Console.WriteLine("Done executing!");
             */
            Thread.Sleep(5000);
            batches.TryRemove(batch.BatchId, out _);
        }

        public static Batch? GetBatchProgress(Guid batchId)
        {
            if (batches.TryGetValue(batchId, out var progress))
            {
                return progress;
            }
            return null;
        }
    }
}
