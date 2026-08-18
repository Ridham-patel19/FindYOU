namespace FindYOU;

public interface IUserAlgoInterface
{
Task<List<FeedChatDto>> GetRecommendedChatsAsync(int userId);

Task<List<FeedChatDto>> GetVectorFeedAsync(
    int userId,
    string query
);

Task<string> GetUpdatedUserInterestTag(int userid);

 public  Task<List<FeedChatDto>> GetViralChats();
}
