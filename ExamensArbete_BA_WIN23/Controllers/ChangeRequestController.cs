using ExamensArbete_BA_WIN23.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExamensArbete_BA_WIN23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChangeRequestController : ControllerBase
    {
        private readonly ILogger<ChangeRequestController> _logger;
        private readonly IChangeRequestService _changeRequestService;

        public ChangeRequestController(IChangeRequestService changeRequestService, ILogger<ChangeRequestController> logger)
        {
            _changeRequestService = changeRequestService;
            _logger = logger;
        }

        [HttpGet("/Get")]
        public async Task<IActionResult> GetAll()
        {
            var token = new CancellationToken();
            var result = await _changeRequestService.GetAllAsync(token);
            return result.Count() > 0 ?  Ok(result) : NotFound();
        }

        [HttpGet("/Get{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var token = new CancellationToken();
            var result = await _changeRequestService.GetOne(id, token);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpDelete("/Delete{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return BadRequest("no value provided");
            }

            var token = new CancellationToken();
            var exists = await _changeRequestService.Exists(id, token);
            if (!exists)
            {
                return NotFound();
            }
            try
            {
                var result = await _changeRequestService.Delete(id, token);
                return result ? Ok() : BadRequest("Object could not be deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting ChangeRequest with id: {ChangeRequestId}", id);
                return BadRequest(ex);
            }
        }
    }
}
