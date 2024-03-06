using CommonLibrary.Dapper.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WebAPITemp.DBContexts.Dapper
{
    public class ProjectDBContext : IDbContext
    {
        private readonly string _connectionString;

        public ProjectDBContext(string ConnectionString)
        {
            _connectionString = ConnectionString;
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
