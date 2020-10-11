using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using POC.Jobs.Manager;
using POC.Jobs.State.Communication.SignalR;

namespace POC.Jobs.Samples.ServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private IHubContext<JobStateHub, IJobStateClient> _hub;
        private JobManager _jobManager;

        public JobController(IHubContext<JobStateHub, IJobStateClient> hub, JobManager jobManager)
        {
            _hub = hub;
            _jobManager = jobManager;
        }

        public IActionResult Get()
        {
            return Ok(new { Message = "JobController" });
        }

        [HttpPost]
        [Route("queue")]
        public IActionResult Queue()
        {
            var job = new Job { Type = JobType.Test, Parameters = JsonSerializer.Serialize(new { Now = System.DateTime.UtcNow }), OwnerId = "User1", ProjectId = "Project1" };
            var jobStateId = _jobManager.QueueAsync(job).Result;
            return Ok(new { Message = "Request Completed", JobStateId = jobStateId });
        }

        [HttpPost]
        [Route("pause/{jobStateId}")]
        public IActionResult Pause(int jobStateId)
        {
            _jobManager.PauseAsync(jobStateId).Wait();
            return Ok(new { Message = "Request Completed", JobStateId = jobStateId });
        }

        [HttpPost]
        [Route("resume/{jobStateId}")]
        public IActionResult Resume(int jobStateId)
        {
            _jobManager.ResumeAsync(jobStateId).Wait();
            return Ok(new { Message = "Request Completed", JobStateId = jobStateId });
        }


        [HttpPost]
        [Route("cancel/{jobStateId}")]
        public IActionResult Cancel(int jobStateId)
        {
            _jobManager.CancelAsync(jobStateId).Wait();
            return Ok(new { Message = "Request Completed", JobStateId = jobStateId });
        }
    }
}
