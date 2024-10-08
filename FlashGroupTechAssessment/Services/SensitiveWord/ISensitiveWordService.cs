using FlashGroupTechAssessment.Models.Dto;

namespace FlashGroupTechAssessment.Services.SensitiveWord
{
	public interface ISensitiveWordService
	{
		public Task<SensitiveWordDto> GetById(int id);
		public Task<bool> Delete(int id);
		public Task<bool> Update(SensitiveWordDto word);
		public Task<ICollection<SensitiveWordDto>> GetAllMessages();
		public Task<bool> Add(string word);
	}
}
