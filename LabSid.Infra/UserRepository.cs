using Dapper;
using Dapper.Contrib.Extensions;
using LabSid.Infra.Interfaces;
using LabSid.Models;
using LabSid.Models.Interfaces;
using System.Data;
using TableAttribute = Dapper.Contrib.Extensions.TableAttribute;

namespace LabSid.Infra
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection connection;

        public UserRepository(IPostgresqlContext context)
        {
            this.connection = context.GetConnection();
        }

        public async Task<IEnumerable<IUser>> Get()
        {
            try
            {
                string sql = $@"select 
                                        * 
                                from {UserDao.TABLE_NAME} u 
                                limit 100";

                return await connection.QueryAsync<UserDao>(sql).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IUser> GetByIdAsync(long id)
        {
            try
            {
                string sql = $@"SELECT 
                                        * 
                                FROM 
                                        {UserDao.TABLE_NAME} u
                                WHERE
                                        Id = @id;
                ";

                var result = await connection.QueryFirstOrDefaultAsync<IUser>(sql, new { id }).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IUser> SaveAsync(IUser user)
        {
            try
            {
                var dao = new UserDao(user);

                if (dao.Id == 0)
                    dao.Id = await connection.InsertAsync(dao).ConfigureAwait(false);
                else
                    await connection.UpdateAsync(dao).ConfigureAwait(false);

                return dao.Export();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<string> Token(string user, string password)
        {
            throw new NotImplementedException();
        }

        [Table(TABLE_NAME)]
        public class UserDao : IUser
        {
            public const string TABLE_NAME = "user";
            public const string ALL_COLUMNS = @"
                Id,
                Name,
                Email,
                Password
            ";

            [Key]
            public long Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

            public UserDao() { }

            public UserDao(IUser user)
            {
                Id = user.Id;
                Name = user.Name;
                Email = user.Email;
                Password = user.Password;
            }

            public IUser Export() => new User(this);

        }

    }
}
