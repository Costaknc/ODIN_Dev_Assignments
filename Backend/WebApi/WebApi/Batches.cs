using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using static Batches;

public class Batches
{
    public class updateRequest
    {
        public string IP { get; set; } = null!;
        public string field { get; set; } = null!;
        public string value { get; set; } = null!;
    }

    public class Batch
    {
        public Guid BatchId { get; set; }
        public List<updateRequest> Updates { get; set; } = new List<updateRequest>();
        public int ProcessedRequests { get; set; } = 0;
        public bool IsProcessing { get; set; } = false;
    }
}

