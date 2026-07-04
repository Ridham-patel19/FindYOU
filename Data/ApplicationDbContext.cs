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

           public DbSet<Bookmark> Bookmarks { get; set; }

           public DbSet<Like> Likes { get; set; }


           protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Like>()
    .HasOne(l => l.User)
    .WithMany(u => u.Likes)
    .HasForeignKey(l => l.UserId)
    .OnDelete(DeleteBehavior.Cascade);

modelBuilder.Entity<Like>()
    .HasOne(l => l.ChatEntry)
    .WithMany(c => c.Likes)
    .HasForeignKey(l => l.ChatEntryId)
    .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Like>()
    .HasIndex(l => new { l.UserId, l.ChatEntryId })
    .IsUnique();

    modelBuilder.Entity<Bookmark>()
        .HasOne(b => b.User)
        .WithMany(u => u.Bookmarks)
        .HasForeignKey(b => b.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Bookmark>()
        .HasOne(b => b.ChatEntry)
        .WithMany(c => c.Bookmarks)
        .HasForeignKey(b => b.ChatEntryId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Bookmark>()
        .HasIndex(x => new
        {
            x.UserId,
            x.ChatEntryId
        })
        .IsUnique();
}
}
