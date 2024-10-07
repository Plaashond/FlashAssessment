using FlashGroupTechAssessment.Models.Dto;

namespace FlashGroupTechAssessment.Repositories.SensitiveWord
{
	public interface ISensitiveWordRepository
	{
		public bool ContainsSensitiveWord(string words);
		public Task<CustomerMessageDTO> BleepWordAsync(string words);
	}
}
