using backend.Models;
using MongoDB.Driver;

namespace backend.Services
{
    public class UsersService
    {
        private readonly IMongoCollection<User> _usersCollection;
        public UsersService(IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:MongoDB"];
            var databaseName = configuration["DatabaseName"];

            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseName);

            _usersCollection = mongoDatabase.GetCollection<User>("Users"); 
        }

        public virtual async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _usersCollection
                .Find(x => x.Email == email)
                .FirstOrDefaultAsync();
        }


        public async Task CreateUserAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }
        public async Task<User> GetByIdAsync(string id) {
            return await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }
       public virtual async Task<User?> ValidateUserPasswordAsync(string email, string password)
       {
      var user = await GetUserByEmailAsync(email);
      if (user == null)
     {
        return null;
     }
       bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
      return isValid ? user : null;
       }
    }
}
