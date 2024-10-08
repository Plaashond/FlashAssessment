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
}
