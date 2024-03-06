using System.Data;

namespace CommonLibrary.Dapper.Interfaces
{
    public interface IDbContext
    {
        public abstract IDbConnection CreateConnection();
    }
}