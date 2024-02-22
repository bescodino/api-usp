using Dapper;
using Dapper.Contrib.Extensions;
using LabSid.Infra.Interfaces;
using LabSid.Models.Interfaces;
using System.Data;
using TableAttribute = Dapper.Contrib.Extensions.TableAttribute;

namespace LabSid.Infra
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnection connection;

        public OrderRepository(IPostgresqlContext context)
        {
            this.connection = context.GetConnection();
        }

        public async Task<IEnumerable<IOrder>> Get()
        {
            try
            {
                string sql = $@"select 
                                        * 
                                from {OrderDao.TABLE_NAME} u 
                                order by Id 
                                limit 100";

                return await connection.QueryAsync<IOrder>(sql).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IOrder?> GetByIdAsync(long id)
        {
            try
            {
                string sql = $@"SELECT 
                                        * 
                                FROM 
                                        {OrderDao.TABLE_NAME} u
                                WHERE
                                        Id = @id;
                ";

                return await connection.QueryFirstOrDefaultAsync<IOrder>(sql, new { id }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IOrder> SaveAsync(IOrder user)
        {
            try
            {
                var dao = new OrderDao(user);

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


        [Table(TABLE_NAME)]
        public class OrderDao : IOrder
        {
            public const string TABLE_NAME = "order";
            public const string ALL_COLUMNS = @"
                Id,
                Name,
                UserId
            ";

            [Key]
            public int Id { get; set; }
            public string Name { get; set; }
            public long UserId { get; set; }
            public List<IProduct> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public OrderDao() { }

            public OrderDao(IOrder order)
            {
                Id = order.Id;
                Name = order.Name;
                UserId = order.UserId;
            }

            public IOrder Export() => new OrderDao(this);

        }

    }
}
