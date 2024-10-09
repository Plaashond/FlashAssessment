using FlashGroupTechAssessment.Models;
using FlashGroupTechAssessment.Models.Dto;
using FlashGroupTechAssessment.Services.Message;
using FlashGroupTechAssessment.Services.SensitiveWord;
using Microsoft.AspNetCore.Mvc;

namespace FlashGroupTechAssessment.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SensitiveWordController : ControllerBase
	{
		private readonly ILogger<SensitiveWordController> _logger;
		private readonly ISensitiveWordService _sensitiveWordService;

		public SensitiveWordController(ILogger<SensitiveWordController> logger, ISensitiveWordService sensitiveWordService)
		{
			_logger = logger;
			_sensitiveWordService = sensitiveWordService;
		}

		[HttpPost("add")]
		public Task<bool> Add([FromBody] SensitiveWordDto message)
		{
			return _sensitiveWordService.Add(message.Word);
		}
		[HttpGet("get-all")]
		public Task<ICollection<SensitiveWordDto>> GetAll()
		{
			return _sensitiveWordService.GetAllMessages();
		}
		[HttpGet("get-by-id")]
		public Task<SensitiveWordDto> GetByID(int id)
		{
			return _sensitiveWordService.GetById(id);
		}
		[HttpPut("update")]
		public Task<bool> Update([FromBody] SensitiveWordDto message)
		{
			return _sensitiveWordService.Update(message);
		}
		[HttpDelete("delete")]
		public Task<bool> Delete(int id)
		{
			return _sensitiveWordService.Delete(id);
		}
	}
}
