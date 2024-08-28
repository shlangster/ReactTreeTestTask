namespace ReactTreeTestTask.Server.Data
{
    public class JournalDto
    {
        public int Id { get; set; }
        public string EventId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; }

        public JournalDto(Journal entity)
        {
            Id = entity.Id;
            EventId = entity.UniqueId;
            CreatedAt = entity.Timestamp;
            Text = $"Request ID: {entity.UniqueId}\r\nQuery Parameters: {entity.QueryParams}\r\nBody: {entity.Body}\r\nType: {entity.Type}\r\nMessage: {entity.Message}\r\nStack trace: {entity.StackTrace}";
        }
    }
}
