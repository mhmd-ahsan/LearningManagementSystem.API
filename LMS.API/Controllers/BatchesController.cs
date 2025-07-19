using LMS.API.Helpers;
using LMS.API.Models;
using LMS.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchesController : ControllerBase
    {
        private readonly IBatchRepository _batchRepository;

        public BatchesController(IBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        [HttpGet("code/{batchCode}")]
        public async Task<IActionResult> GetBatchByCode(string batchCode)
        {
            var batch = await _batchRepository.GetBatchByCodeAsync(batchCode);

            if (batch == null)
            {
                return NotFound(ResponseHelper<string>.Fail("Batch not found"));
            }

            return Ok(ResponseHelper<Batch>.Success(batch, "Batch found"));
        }
    }
}
