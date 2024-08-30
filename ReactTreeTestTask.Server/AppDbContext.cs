using Microsoft.EntityFrameworkCore;
using ReactTreeTestTask.Server.Data;

namespace ReactTreeTestTask.Server
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Node> Nodes { get; set; }
        public DbSet<Journal> Journals { get; set; }

        public Task<List<Node>> SelectFullTree(int id) =>
            Nodes.FromSqlRaw(
                "WITH RECURSIVE tree AS (" +
                "SELECT \"Id\", \"Name\", \"ParentId\" FROM public.\"Nodes\" WHERE \"ParentId\" IS NULL AND \"Id\" = {0} " +
                "UNION ALL        " +
                "SELECT n.\"Id\", n.\"Name\", n.\"ParentId\" FROM \"Nodes\" n INNER JOIN tree t ON t.\"Id\" = n.\"ParentId\")" +
                "SELECT * FROM tree", id)
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();

    }
}
