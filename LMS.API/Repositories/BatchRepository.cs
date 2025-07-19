using Dapper;
using LMS.API.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace LMS.API.Repositories
{
    public class BatchRepository : IBatchRepository
    {
        private readonly IConfiguration _config;

        public BatchRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Batch?> GetBatchByCodeAsync(string batchCode)
        {
            using var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")); // ✅ MySQL Connection
            string sql = "SELECT * FROM Batches WHERE BatchCode = @BatchCode AND IsActive = 1";
            return await connection.QueryFirstOrDefaultAsync<Batch>(sql, new { BatchCode = batchCode });
        }

        public async Task<int?> GetBatchIdByCodeAsync(string batchCode)
        {
            using var connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")); // ✅ MySQL Connection
            string sql = "SELECT BatchId FROM Batches WHERE BatchCode = @BatchCode AND IsActive = 1";
            return await connection.QueryFirstOrDefaultAsync<int?>(sql, new { BatchCode = batchCode });
        }
    }
}
