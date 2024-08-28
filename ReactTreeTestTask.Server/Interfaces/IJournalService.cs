using ReactTreeTestTask.Server.Data;

namespace ReactTreeTestTask.Server.Interfaces
{
    public interface IJournalService
    {
        public Task Create(Exception exception, HttpContext context);
        public Task<JournalDto> GetSingle(int id);
        public Task<JournalRangeDto> GetRange(int skip, int take, JournalFilter filter);
    }
}
