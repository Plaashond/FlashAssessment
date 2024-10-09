using FlashGroupTechAssessment.Models.Dto;
using FlashGroupTechAssessment.Repositories.SensitiveWord;

namespace FlashGroupTechAssessment.Services.SensitiveWord
{
	public class SensitiveWordService : ISensitiveWordService
	{
		public readonly ISensitiveWordRepository _sensitiveWordRepository;
		public SensitiveWordService(ISensitiveWordRepository sensitiveWordRepository)
		{
			_sensitiveWordRepository = sensitiveWordRepository;
		}
		public async Task<bool> Delete(int id)
		{
			return await _sensitiveWordRepository.Delete(id);
		}

		public async Task<ICollection<SensitiveWordDto>> GetAllMessages()
		{
			List<Models.SensitiveWord> words = await _sensitiveWordRepository.GetAll();
			return words.Select(x=>new SensitiveWordDto(x.Word,x.Id)).ToList();
		}

		public async Task<SensitiveWordDto> GetById(int id)
		{
			Models.SensitiveWord word = await _sensitiveWordRepository.GetById(id);
			if (word == null) {
				return new SensitiveWordDto();
			}
			return new SensitiveWordDto(word.Word, word.Id);
		}

		public async Task<bool> Add(string word)
		{
			return await _sensitiveWordRepository.Create(new Models.SensitiveWord() { Word = word, StarredWord = new string('*', word.Length) });
		}

		public async Task<bool> Update(SensitiveWordDto word)
		{
			return await _sensitiveWordRepository.Update(new Models.SensitiveWord() { Id = word.Id, Word = word.Word, StarredWord = new string('*', word.Word.Length) });
			throw new NotImplementedException();
		}
	}
}
