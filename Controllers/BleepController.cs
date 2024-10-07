using FlashGroupTechAssessment.Models;
using FlashGroupTechAssessment.Models.Dto;
using FlashGroupTechAssessment.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlashGroupTechAssessment.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BleepController : ControllerBase
	{
		private readonly ILogger<BleepController> _logger;
		private readonly IMessageService _messageService;

		public BleepController(ILogger<BleepController> logger,IMessageService messageService)
		{
			_logger = logger;
			_messageService = messageService;
		}

		[HttpPost("single-bleep")]
		public Task<CustomerMessageDTO> Bleep([FromBody] CustomerMessageDTO message)
		{
			return _messageService.SanatizeMessageAsync(message.Message.Trim());
		}
	}
}
