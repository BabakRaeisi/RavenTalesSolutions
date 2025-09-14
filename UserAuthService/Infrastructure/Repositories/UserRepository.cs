

using Dapper;
using RavenTales.Core.DTO;
using RavenTales.Core.Entities;
using RavenTales.Core.RepositoryContracts;
using RavenTales.Infrastructure.DbContext;

namespace RavenTales.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        public readonly DapperDbContext _dbContext;
        public UserRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApplicationUser?> AddUser(ApplicationUser user)
        {
            user.UserID = Guid.NewGuid();
            string query =
       "INSERT INTO public.\"Users\"(\"UserID\", \"Email\",\"PersonName\",\"Gender\",\"Password\")VALUES(@UserID, @Email, @PersonName, @Gender, @Password)";
           
           int rowCountAffected = await _dbContext.DbConnection.ExecuteAsync(query,user);
            if (rowCountAffected>0)
            {
                return user;
            }
            else
            {
                return null;
            }
            
      

        }

        public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
        {
            string query = "SELECT * FROM public.\"Users\"WHERE \"Email\" = @Email AND \"Password\" = @Password";
            var parameters = new { Email = email, Password = password };
            ApplicationUser? user = await _dbContext.DbConnection.
                QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);

            return user;
        }
    }
}
