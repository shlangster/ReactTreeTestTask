using ReactTreeTestTask.Server.Data;
using ReactTreeTestTask.Server.Interfaces;
using System.Text;

namespace ReactTreeTestTask.Server.Services
{
    public class JournalService : IJournalService
    {
        private AppDbContext _dbContext { get; set; }
        public JournalService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Exception exception, HttpContext httpContext)
        {
            string requestBody = "";
            var request = httpContext.Request;
            if (request.Method == HttpMethods.Post && request.ContentLength > 0)
            {
                request.Body.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(request.Body, Encoding.UTF8))
                {
                    requestBody = await reader.ReadToEndAsync();
                    request.Body.Seek(0, SeekOrigin.Begin);
                }
            }
            var entity = new Journal()
            {
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Timestamp = DateTime.UtcNow,
                UniqueId = httpContext.TraceIdentifier,
                Body = requestBody,
                QueryParams = httpContext.Request.QueryString.ToString(),
                Type = exception.GetType().ToString()
            };

            _dbContext.Journals.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<JournalDto> GetSingle(int id)
        {
            var entity = _dbContext.Journals.FirstOrDefault(_ => _.Id == id);
            if(entity == null)
                throw new SecureException($"Journal with ID = {id} was not found");
            
            return new JournalDto(entity);
        }

        public async Task<JournalRangeDto> GetRange(int skip, int take, JournalFilter filter)
        {
            var query = _dbContext.Journals.AsQueryable();
            if (filter.Search != null)
                query = query.Where(_ => _.Message.ToLower().Contains(filter.Search.ToLower().Trim()) 
                || _.Type.ToLower().Contains(filter.Search.ToLower().Trim()) 
                || _.StackTrace.ToLower().Contains(filter.Search.ToLower().Trim()));

            if (filter.From != DateTime.MinValue)
                query = query.Where(_ => _.Timestamp >= filter.From);

            if (filter.To != DateTime.MinValue)
                query = query.Where(_ => _.Timestamp <= filter.To);

            var count = query.Count();

            var data = query.Skip(skip).Take(take).ToList();

            return new JournalRangeDto(data, skip, count);
        }
    }
}
