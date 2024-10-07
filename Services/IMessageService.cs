using FlashGroupTechAssessment.Models;
using FlashGroupTechAssessment.Models.Dto;

namespace FlashGroupTechAssessment.Services
{
	public interface IMessageService
	{
		public Task<CustomerMessageDTO> SanatizeMessageAsync(string message);
	}
}
