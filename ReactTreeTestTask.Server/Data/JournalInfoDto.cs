namespace ReactTreeTestTask.Server.Data
{
    public class JournalInfoDto
    {
        public int Id { get; set; }
        public string EventId { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public JournalInfoDto(Journal entity)
        {
            Id = entity.Id;
            EventId = entity.UniqueId;
            CreatedAt = entity.Timestamp;
        }
    }
}
