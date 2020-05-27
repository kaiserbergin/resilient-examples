using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace ResilientAPI.Controllers
{
    [Route("do-work")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly ILogger<WorkerController> _logger;
        private readonly ICounter _counter;

        public WorkerController(IBackgroundTaskQueue backgroundTaskQueue, ILogger<WorkerController> logger, ICounter counter)
        {
            _backgroundTaskQueue = backgroundTaskQueue ?? throw new ArgumentNullException(nameof(backgroundTaskQueue));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _counter = counter ?? throw new ArgumentNullException(nameof(counter));
        }

        [HttpPost]
        public IActionResult DoWork()
        {
            _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
            {
                var guid = Guid.NewGuid().ToString();
                _logger.LogInformation($"Starting work on task {guid}");

                await Task.Delay(TimeSpan.FromSeconds(2), token);
                _counter.Inc();

                _logger.LogInformation($"Task {guid} is complete");
            });

            return Ok();
        }
    }
}
