using System.Data;

namespace LabSid.Infra.Interfaces
{
    public interface IDbContext : IDisposable
    {
        IDbConnection GetConnection();
    }
}
