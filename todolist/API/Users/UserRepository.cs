namespace API.Users
{
    using System.Threading;
    using System.Threading.Tasks;
    using View.Users;

    public sealed class UserRepository : IUserRepository
    {
        public Task<User> GetAsync(string login)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> CreateAsync(UserCreationInfo creationInfo, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}
