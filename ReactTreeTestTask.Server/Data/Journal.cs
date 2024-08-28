using Microsoft.EntityFrameworkCore;

namespace ReactTreeTestTask.Server.Data
{
    [PrimaryKey("Id")]
    public class Journal
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string QueryParams { get; set; }
        public string Body { get; set; }
        public string StackTrace { get; set; }

    }
}
