namespace FindYOU;

public interface ILikeInterface
{
    Task AddLikeAsync(Like like);

    Task DeleteLikeAsync(int chatId, int userId);

    Task<int> GetLikeCountByChatAsync(int chatId);

    Task<bool> HasUserLikedChatAsync(int chatId, int userId);

    Task Save();
}