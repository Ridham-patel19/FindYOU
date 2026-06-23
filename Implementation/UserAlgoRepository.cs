

using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FindYOU;

public class UserAlgoRepository : IUserAlgoInterface
{

     private readonly ApplicationDbContext _context;

     private readonly NpgsqlConnection _conn;

    public UserAlgoRepository(ApplicationDbContext context , NpgsqlConnection conn)
    {
        _context = context;
        _conn = conn;
    }

    public async Task<List<FeedChatDto>> GetRecommendedChatsAsync(int userId)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Id == userId);

    if (user == null ||
        string.IsNullOrWhiteSpace(user.InterestTags))
    {
        return new List<FeedChatDto>();
    }

    var interestList = user.InterestTags
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(tag => tag.ToLower())
        .ToList();

    var bookmarkedIds = await _context.Bookmarks
        .Where(b => b.UserId == userId)
        .Select(b => b.ChatEntryId)
        .ToListAsync();

    var chats = await _context.ChatEntries
        .Include(c => c.Category)
        .Where(c =>
            c.IsPublic &&
            c.ChatTags != null &&
            interestList.Any(tag =>
                c.ChatTags.ToLower().Contains(tag)))
        .ToListAsync();

    return chats.Select(c => new FeedChatDto
    {
        Id = c.Id,
        Title = c.Title,
        ChatLink = c.ChatLink,
        Summary = c.Summary,
        Notes = c.Notes,
        IsPublic = c.IsPublic,
        CreatedAt = c.CreatedAt,
        CategoryId = c.CategoryId,
        Category = c.Category,
        ChatTags = c.ChatTags,
        UserId = c.UserId,

        IsBookmarked =
            bookmarkedIds.Contains(c.Id)
    }).ToList();
}

   public async Task<List<FeedChatDto>> GetVectorFeedAsync(
    int userId,
    string query)
{
    var qry = @"
SELECT
    c.*,
    CASE
        WHEN b.""Id"" IS NOT NULL THEN true
        ELSE false
    END AS ""IsBookmarked""
FROM ""ChatEntries"" c
LEFT JOIN ""Bookmarks"" b
    ON c.""Id"" = b.""ChatEntryId""
    AND b.""UserId"" = @userId
WHERE c.search_vector @@ plainto_tsquery('english', @query)
ORDER BY ts_rank(
    c.search_vector,
    plainto_tsquery('english', @query)
) DESC
LIMIT 10;";

    var chats = new List<FeedChatDto>();

    try
    {
        using var cmd = new NpgsqlCommand(qry, _conn);

        cmd.Parameters.AddWithValue("query", query);
        cmd.Parameters.AddWithValue("userId", userId);

        await _conn.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            chats.Add(new FeedChatDto
            {
                Id = reader.GetInt32(
                    reader.GetOrdinal("Id")),

                Title = reader.GetString(
                    reader.GetOrdinal("Title")),

                Summary = reader.IsDBNull(
                    reader.GetOrdinal("Summary"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Summary")),

                ChatLink = reader.GetString(
                    reader.GetOrdinal("ChatLink")),

                ChatTags = reader.IsDBNull(
                    reader.GetOrdinal("ChatTags"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("ChatTags")),

                CreatedAt = reader.GetDateTime(
                    reader.GetOrdinal("CreatedAt")),

                CategoryId = reader.GetInt32(
                    reader.GetOrdinal("CategoryId")),

                UserId = reader.GetInt32(
                    reader.GetOrdinal("UserId")),

                IsPublic = reader.GetBoolean(
                    reader.GetOrdinal("IsPublic")),

                IsBookmarked = reader.GetBoolean(
                    reader.GetOrdinal("IsBookmarked"))
            });
        }

        return chats;
    }
    catch (Exception e)
    {
        Console.WriteLine(
            "error in feed generation with vector " + e.Message);

        return new List<FeedChatDto>();
    }
    finally
    {
        if (_conn.State == System.Data.ConnectionState.Open)
        {
            await _conn.CloseAsync();
        }
    }
}
}

