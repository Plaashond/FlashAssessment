using FlashGroupTechAssessment.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlashGroupTechAssessment.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BleepController : ControllerBase
	{
		private readonly ILogger<BleepController> _logger;

		public BleepController(ILogger<BleepController> logger)
		{
			_logger = logger;
		}

		[HttpPost(Name = "Message")]
		public CustomerMessage Message([FromBody] CustomerMessage message)
		{
			return message;
		}
		[HttpPost(Name = "Messages")]
		public IEnumerable<CustomerMessage> Messages([FromBody] ICollection<CustomerMessage> messages)
		{
			return messages;
		}
	}
}
