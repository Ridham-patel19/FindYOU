namespace FindYOU;

public interface IUserAlgoInterface
{
Task<List<ChatEntry>> GetRecommendedChatsAsync(int userId);
}
