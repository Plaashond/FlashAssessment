using FlashGroupTechAssessment.Models;
using FlashGroupTechAssessment.Models.Dto;
using FlashGroupTechAssessment.Services.Message;
using Microsoft.AspNetCore.Mvc;

namespace FlashGroupTechAssessment.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CustomerMessageController : ControllerBase
	{
		private readonly ILogger<CustomerMessageController> _logger;
		private readonly IMessageService _messageService;

		public CustomerMessageController(ILogger<CustomerMessageController> logger,IMessageService messageService)
		{
			_logger = logger;
			_messageService = messageService;
		}

		[HttpPost("add")]
		public Task<CustomerMessageDTO> Add([FromBody] CustomerMessageDTO message)
		{
			return _messageService.SanitizeMessageAsync(message.Message.Trim(),true);
		}
		[HttpGet("get-all")]
		public Task<ICollection<CustomerMessageDTO>> GetAll()
		{
			return _messageService.GetAllMessages();
		}
		[HttpGet("get-by-id")]
		public Task<CustomerMessageDTO> GetByID(Guid guid)
		{
			return _messageService.GetById(guid);
		}
		[HttpPut("update")]
		public Task<bool> Update([FromBody] CustomerMessageDTO message)
		{
			return _messageService.Update(message);
		}
		[HttpDelete("delete")]
		public Task<bool> Delete(Guid guid)
		{
			return _messageService.Delete(guid);
		}
	}
}
