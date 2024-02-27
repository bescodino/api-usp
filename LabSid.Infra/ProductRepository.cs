using Dapper;
using Dapper.Contrib.Extensions;
using LabSid.Infra.Interfaces;
using LabSid.Models;
using LabSid.Models.Interfaces;
using System.Data;
using static LabSid.Infra.UserRepository;
using TableAttribute = Dapper.Contrib.Extensions.TableAttribute;

namespace LabSid.Infra
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection connection;

        public ProductRepository(ISqliteContext context)
        {
            this.connection = context.GetConnection();
        }

        public async Task<IEnumerable<IProduct>> Get()
        {
            try
            {
                string sql = $@"select 
                                        * 
                                from {ProductDao.TABLE_NAME} u 
                                limit 100";

                return await connection.QueryAsync<ProductDao>(sql).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IProduct?> GetByIdAsync(long id)
        {
            try
            {              
                return await connection.GetAsync<ProductDao>(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IProduct> SaveAsync(IProduct product)
        {
            try
            {
                var dao = new ProductDao(product);

                if (dao.Id == 0)
                    dao.Id = await connection.InsertAsync(dao).ConfigureAwait(false);
                else
                    await connection.UpdateAsync(dao).ConfigureAwait(false);

                return dao.Export();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Table(TABLE_NAME)]
        public class ProductDao : IProduct
        {
            public const string TABLE_NAME = "product";
            public const string ALL_COLUMNS = @"
                Id,
                Name,
                Value,
                OrderId
            ";

            [Key]
            public int Id { get; set; }
            public string Name { get; set; }
            public double Value { get; set; }
            public long OrderId { get; set; }

            public ProductDao() { }

            public ProductDao(IProduct product)
            {
                Id = product.Id;
                Name = product.Name;
                Value = product.Value;
                OrderId = product.OrderId;
            }

            public IProduct Export() => new ProductDao(this);

        }

    }
}
