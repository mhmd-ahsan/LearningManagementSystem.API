using LMS.API.Models;

namespace LMS.API.Repositories
{
    public interface IBatchRepository
    {
        // Fetch full batch details (optional use)
        Task<Batch?> GetBatchByCodeAsync(string batchCode);

        // ✅ New method to fetch only the BatchId (used in Register)
        Task<int?> GetBatchIdByCodeAsync(string batchCode);
    }
}
