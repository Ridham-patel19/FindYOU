namespace FindYOU;

public interface IUserAlgoInterface
{
Task<List<FeedChatDto>> GetRecommendedChatsAsync(int userId);

Task<List<FeedChatDto>> GetVectorFeedAsync(
    int userId,
    string query
);
}
