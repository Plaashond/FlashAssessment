using FlashGroupTechAssessment.Models;
using FlashGroupTechAssessment.Models.Dto;
using FlashGroupTechAssessment.Repositories.SensitiveWord;

namespace FlashGroupTechAssessment.Services
{
	public class MessageService : IMessageService
	{
		private readonly ISensitiveWordRepository _sensitiveWordRepository;
		public MessageService(ISensitiveWordRepository sensitiveWordRepository)
		{
			_sensitiveWordRepository = sensitiveWordRepository;
		}
		public async Task<CustomerMessageDTO> SanatizeMessageAsync(string message)
		{
			if (message == null || !_sensitiveWordRepository.ContainsSensitiveWord(message))
			{
				return new CustomerMessageDTO(message);
			}
			//TODO: add message for auditing purposes
			return await _sensitiveWordRepository.BleepWordAsync(message);
		}
	}
}
