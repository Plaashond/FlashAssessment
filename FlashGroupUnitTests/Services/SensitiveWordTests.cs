using FlashGroupTechAssessment.Models;
using FlashGroupTechAssessment.Models.Dto;
using FlashGroupTechAssessment.Repositories.SensitiveWord;
using FlashGroupTechAssessment.Services.SensitiveWord;
using Moq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace FlashGroupUnitTests.Services
{
	public class SensitiveWordServiceTests
	{
		private readonly Mock<ISensitiveWordRepository> _mockSensitiveWordRepository;
		private readonly SensitiveWordService _sensitiveWordService;

		public SensitiveWordServiceTests()
		{
			_mockSensitiveWordRepository = new Mock<ISensitiveWordRepository>();
			_sensitiveWordService = new SensitiveWordService(_mockSensitiveWordRepository.Object);
		}

		[Fact]
		public async Task Delete_ReturnsTrue_WhenWordIsDeleted()
		{
			int id = 1;
			_mockSensitiveWordRepository.Setup(r => r.Delete(id))
				.ReturnsAsync(true);

			var result = await _sensitiveWordService.Delete(id);

			Assert.True(result);
		}

		[Fact]
		public async Task Delete_ReturnsFalse_WhenWordIsNotDeleted()
		{
			int id = 1;
			_mockSensitiveWordRepository.Setup(r => r.Delete(id))
				.ReturnsAsync(false);

			var result = await _sensitiveWordService.Delete(id);

			Assert.False(result);
		}

		[Fact]
		public async Task GetAllMessages_ReturnsListOfSensitiveWordDto()
		{
			var expectedWords = new List<SensitiveWord>
		{
			new SensitiveWord { Id = 1, Word = "Test Word 1", StarredWord = "*********" },
			new SensitiveWord { Id = 2, Word = "Test Word 2", StarredWord = "**********" }
		};

			_mockSensitiveWordRepository.Setup(r => r.GetAll())
				.ReturnsAsync(expectedWords);

			var result = await _sensitiveWordService.GetAllMessages();

			Assert.Equal(expectedWords.Count, result.Count);
			Assert.Equal(expectedWords.Select(x => new SensitiveWordDto(x.Word, x.Id)), result, new SensitiveWordDtoComparer());
		}

		[Fact]
		public async Task GetById_ReturnsSensitiveWordDto_WhenWordExists()
		{
			int id = 1;
			var expectedWord = new SensitiveWord { Id = id, Word = "Test Word", StarredWord = "*********" };

			_mockSensitiveWordRepository.Setup(r => r.GetById(id))
				.ReturnsAsync(expectedWord);

			var result = await _sensitiveWordService.GetById(id);

			Assert.Equal(expectedWord.Id, result.Id);
			Assert.Equal(expectedWord.Word, result.Word);
		}

		[Fact]
		public async Task Add_ReturnsTrue_WhenWordIsAdded()
		{
			string word = "New Test Word";
			_mockSensitiveWordRepository.Setup(r => r.Create(It.IsAny<SensitiveWord>()))
				.ReturnsAsync(true);

			var result = await _sensitiveWordService.Add(word);

			Assert.True(result);
		}

		[Fact]
		public async Task Add_ReturnsFalse_WhenWordIsNotAdded()
		{
			string word = "New Test Word";
			_mockSensitiveWordRepository.Setup(r => r.Create(It.IsAny<SensitiveWord>()))
				.ReturnsAsync(false);

			var result = await _sensitiveWordService.Add(word);

			Assert.False(result);
		}

		[Fact]
		public async Task Update_ReturnsTrue_WhenWordIsUpdated()
		{
			var wordDto = new SensitiveWordDto { Id = 1, Word = "Updated Test Word" };
			_mockSensitiveWordRepository.Setup(r => r.Update(It.IsAny<SensitiveWord>()))
				.ReturnsAsync(true);

			var result = await _sensitiveWordService.Update(wordDto);

			Assert.True(result);
		}

		[Fact]
		public async Task Update_ReturnsFalse_WhenWordIsNotUpdated()
		{
			var wordDto = new SensitiveWordDto { Id = 1, Word = "Updated Test Word" };
			_mockSensitiveWordRepository.Setup(r => r.Update(It.IsAny<SensitiveWord>()))
				.ReturnsAsync(false);

			var result = await _sensitiveWordService.Update(wordDto);

			Assert.False(result);
		}

		private class SensitiveWordDtoComparer : IEqualityComparer<SensitiveWordDto>
		{
			public bool Equals(SensitiveWordDto x, SensitiveWordDto y)
			{
				return x.Id == y.Id &&
					   x.Word == y.Word;
			}

			public int GetHashCode(SensitiveWordDto obj)
			{
				return obj.Id.GetHashCode();
			}
		}
	}
}