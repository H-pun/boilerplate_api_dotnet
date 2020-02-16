using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;

namespace Common.Scheduler
{
    [DisallowConcurrentExecution]
    public class TestJob : IJob
    {
        private readonly ILogger<TestJob> _logger;
        public TestJob(ILogger<TestJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Quartz is running.");
            return Task.CompletedTask;
        }
    }
}
