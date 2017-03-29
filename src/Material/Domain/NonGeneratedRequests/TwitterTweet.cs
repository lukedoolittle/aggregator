namespace Material.Domain.Requests
{
    public partial class TwitterTweet
    {
        public override void AddUserIdParameter(string userId)
        {
            UserId = userId;
        }
    }
}
