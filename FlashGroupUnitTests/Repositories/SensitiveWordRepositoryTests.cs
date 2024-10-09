using Dapper;
using FlashGroupTechAssessment.Models;
using FlashGroupTechAssessment.Models.Dto;
using FlashGroupTechAssessment.Repositories.Message;
using FlashGroupTechAssessment.Repositories.SensitiveWord;
using Moq;
using System;
using System.Data;
using System.Threading.Tasks;
using Xunit;

public class SensitiveWordRepositoryTests
{
	private readonly Mock<IDbConnectionWrapper> _mockDbConnection;
	private readonly Mock<IMessageRepository> _mockMessageRepository;
	private readonly SensitiveWordRepository _sensitiveWordRepository;

	public SensitiveWordRepositoryTests()
	{
		_mockDbConnection = new Mock<IDbConnectionWrapper>();
		_mockMessageRepository = new Mock<IMessageRepository>();
		_sensitiveWordRepository = new SensitiveWordRepository(_mockDbConnection.Object, _mockMessageRepository.Object);
	}

	[Fact]
	public async Task BleepWordsAsync_ReturnsCustomerMessageDTO_WithSanitizedWords()
	{
				string words = "This is a test string with sensitive words like password and username.";
		string sanitizedWords = "This is a test string with sensitive words like ***** and ********.";
		_mockDbConnection.Setup(db => db.QuerySingleAsync<string>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
			.ReturnsAsync(sanitizedWords);

				var result = await _sensitiveWordRepository.BleepWordsAsync(words, true);

				Assert.Equal(sanitizedWords, result.Message);
		_mockMessageRepository.Verify(mr => mr.Create(It.IsAny<CustomerMessage>()), Times.Once);
	}

	[Fact]
	public async Task BleepWordsAsync_DoesNotAudit_WhenAuditIsFalse()
	{
				string words = "This is a test string with sensitive words like password and username.";
		_mockDbConnection.Setup(db => db.QuerySingleAsync<string>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
			.ReturnsAsync("Sanitized test string with sensitive words like ***** and ********.");

				var result = await _sensitiveWordRepository.BleepWordsAsync(words, false);

				Assert.Equal("Sanitized test string with sensitive words like ***** and ********.", result.Message);
		_mockMessageRepository.Verify(mr => mr.Create(It.IsAny<CustomerMessage>()), Times.Never);
	}

	[Fact]
	public async Task ContainsSensitiveWord_ReturnsTrue_WhenSensitiveWordIsPresent()
	{
				string words = "This is a test string with sensitive words like password and username.";
		_mockDbConnection.Setup(db => db.QuerySingleAsync<bool>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
			.ReturnsAsync(true);

				var result = await _sensitiveWordRepository.ContainsSensitiveWord(words);

				Assert.True(result);
	}

	[Fact]
	public async Task ContainsSensitiveWord_ReturnsFalse_WhenSensitiveWordIsNotPresent()
	{
				string words = "This is a test string without sensitive words.";
		_mockDbConnection.Setup(db => db.QuerySingleAsync<bool>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
			.ReturnsAsync(false);

				var result = await _sensitiveWordRepository.ContainsSensitiveWord(words);

				Assert.False(result);
	}
	[Fact]
	public async Task GetAll_ReturnsListOfSensitiveWord()
	{
				var expectedWords = new List<SensitiveWord>
		{
			new SensitiveWord { Id = 1, Word = "Test Word 1", StarredWord = "*********" },
			new SensitiveWord { Id = 2, Word = "Test Word 2", StarredWord = "**********" }
		};

		_mockDbConnection.Setup(db => db.QueryAsync<SensitiveWord>(It.IsAny<string>(), null, null, null, null))
			.ReturnsAsync(expectedWords);

				var result = await _sensitiveWordRepository.GetAll();

				Assert.Equal(expectedWords.Count, result.Count);
		Assert.Equal(expectedWords, result, new SensitiveWordComparer());
	}

	[Fact]
	public async Task GetById_ReturnsSensitiveWord_WhenWordExists()
	{
				int id = 1;
		var expectedWord = new SensitiveWord { Id = id, Word = "Test Word", StarredWord = "*********" };

		_mockDbConnection.Setup(db => db.QueryAsync<SensitiveWord>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
			.ReturnsAsync(new List<SensitiveWord> { expectedWord });

				var result = await _sensitiveWordRepository.GetById(id);

				Assert.Equal(expectedWord, result, new SensitiveWordComparer());
	}

	[Fact]
	public async Task GetById_ReturnsNull_WhenWordDoesNotExist()
	{
				int id = 1;

		_mockDbConnection.Setup(db => db.QueryAsync<SensitiveWord>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
			.ReturnsAsync(new List<SensitiveWord>());

				var result = await _sensitiveWordRepository.GetById(id);

				Assert.Null(result);
	}

	[Fact]
	public async Task Create_ReturnsTrue_WhenWordIsAdded()
	{
				var word = new SensitiveWord { Word = "New Test Word", StarredWord = "**************" };

		_mockDbConnection.Setup(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
			.ReturnsAsync(1);

				var result = await _sensitiveWordRepository.Create(word);

				Assert.True(result);
	}

	[Fact]
	public async Task Create_ReturnsFalse_WhenWordIsNotAdded()
	{
				var word = new SensitiveWord { Word = "New Test Word", StarredWord = "**************" };

		_mockDbConnection.Setup(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
			.ThrowsAsync(new Exception());

		await Assert.ThrowsAsync<Exception>(() => _sensitiveWordRepository.Create(word));
	}

	[Fact]
	public async Task Update_ReturnsTrue_WhenWordIsUpdated()
	{
				var word = new SensitiveWord { Id = 1, Word = "Updated Test Word", StarredWord = "**************" };

		_mockDbConnection.Setup(db => db.QueryAsync<SensitiveWord>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
			.ReturnsAsync(new List<SensitiveWord> { word });

		_mockDbConnection.Setup(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
			.ReturnsAsync(1);

				var result = await _sensitiveWordRepository.Update(word);

				Assert.True(result);
	}

	[Fact]
	public async Task Update_ReturnsFalse_WhenWordIsNotUpdated()
	{
				var word = new SensitiveWord { Id = 1, Word = "Updated Test Word", StarredWord = "**************" };

		_mockDbConnection.Setup(db => db.QueryAsync<SensitiveWord>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
			.ReturnsAsync((IEnumerable<SensitiveWord>)null);

		var result = await _sensitiveWordRepository.Update(word);

				Assert.False(result);
	}

	[Fact]
	public async Task Delete_ReturnsTrue_WhenWordIsDeleted()
	{
				int id = 1;

		_mockDbConnection.Setup(db => db.QueryAsync<SensitiveWord>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
			.ReturnsAsync(new List<SensitiveWord> { new SensitiveWord { Id = id } });

		_mockDbConnection.Setup(db => db.ExecuteAsync(It.IsAny<string>(), null, null, null, null))
			.ReturnsAsync(1);

				var result = await _sensitiveWordRepository.Delete(id);

				Assert.True(result);
	}

	[Fact]
	public async Task Delete_ReturnsFalse_WhenWordIsNotDeleted()
	{
				int id = 1;

		_mockDbConnection.Setup(db => db.QueryAsync<SensitiveWord>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
			 .ReturnsAsync((IEnumerable<SensitiveWord>)null);

		var result = await _sensitiveWordRepository.Delete(id);

				Assert.False(result);
	}

	private class SensitiveWordComparer : IEqualityComparer<SensitiveWord>
	{
		public bool Equals(SensitiveWord x, SensitiveWord y)
		{
			return x.Id == y.Id &&
				   x.Word == y.Word &&
				   x.StarredWord == y.StarredWord;
		}

		public int GetHashCode(SensitiveWord obj)
		{
			return obj.Id.GetHashCode();
		}
	}
}
