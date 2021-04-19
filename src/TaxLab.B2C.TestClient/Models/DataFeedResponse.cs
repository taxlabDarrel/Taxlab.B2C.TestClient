using System;
using System.Collections.Generic;

namespace TaxLab.B2C.TestClient.Models
{
    public class DataFeedResponse
    {
        public IEnumerable<DataFeed> Documents { get; set; }
    }

    public class DataFeed
    {
        public string BatchKey { get; set; } = string.Empty;

        public DateTimeOffset Timestamp { get; set; }

        public string FileFormat { get; set; } = string.Empty;

        public Guid Id { get; set; }

        public string DocumentName { get; set; } = string.Empty;
    }
}
