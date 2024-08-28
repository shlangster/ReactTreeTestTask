namespace ReactTreeTestTask.Server.Data
{
    public class JournalRangeDto
    {
        public int Skip { get; set; }
        public int Count { get; set; }
        public IList<JournalInfoDto> Items { get; set; }

        public JournalRangeDto(IList<Journal> data, int skip, int count)
        {
            Skip = skip; 
            Count = count;
            Items = data.Select(_ => new JournalInfoDto(_)).ToList();
        }

    }
}
