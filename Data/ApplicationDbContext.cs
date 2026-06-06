using Microsoft.EntityFrameworkCore;

namespace FindYOU;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
    {
        
    }



//so i have use EF to create or migrate whole data base by it self so if i  i have to check thsi tabke i have to use lil SELECT * FROM "Users";
        public DbSet<Category> Categories { get; set; }

        public DbSet<ChatEntry> ChatEntries { get; set; }

           public DbSet<User> Users { get; set; }  
}
