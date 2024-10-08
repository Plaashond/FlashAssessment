using FlashGroupTechAssessment.Models;
using FlashGroupTechAssessment.Models.Dto;
using FlashGroupTechAssessment.Repositories.Message;
using FlashGroupTechAssessment.Repositories.SensitiveWord;
using FlashGroupTechAssessment.Services.Message;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FlashGroupUnitTests.Services
{
    public class MessageServiceTests
	{
		private readonly Mock<ISensitiveWordRepository> _mockSensitiveWordRepository;
		private readonly Mock<IMessageRepository> _mockMessageRepository;
		private readonly MessageService _messageService;

		public MessageServiceTests()
		{
			_mockSensitiveWordRepository = new Mock<ISensitiveWordRepository>();
			_mockMessageRepository = new Mock<IMessageRepository>();
			_messageService = new MessageService(_mockSensitiveWordRepository.Object, _mockMessageRepository.Object);
		}

		[Fact]
		public async Task Delete_ReturnsTrue_WhenMessageIsDeleted()
		{
						Guid id = Guid.NewGuid();
			_mockMessageRepository.Setup(mr => mr.Delete(It.IsAny<string>()))
				.ReturnsAsync(true);

						var result = await _messageService.Delete(id);

						Assert.True(result);
		}

		[Fact]
		public async Task Delete_ReturnsFalse_WhenMessageIsNotDeleted()
		{
						Guid id = Guid.NewGuid();
			_mockMessageRepository.Setup(mr => mr.Delete(It.IsAny<string>()))
				.ReturnsAsync(false);

						var result = await _messageService.Delete(id);

						Assert.False(result);
		}

		[Fact]
		public async Task GetAllMessages_ReturnsListOfCustomerMessageDTO()
		{
						var expectedMessages = new List<CustomerMessage>
		{
			new CustomerMessage { Id = Guid.NewGuid(), Timestamp = DateTime.Today, Message = "Test Message 1", Sanatizedmessage = "Sanitized Test Message 1" },
			new CustomerMessage { Id = Guid.NewGuid(), Timestamp = DateTime.Today, Message = "Test Message 2", Sanatizedmessage = "Sanitized Test Message 2" }
		};

			_mockMessageRepository.Setup(mr => mr.GetAll())
				.ReturnsAsync(expectedMessages);

						var result = await _messageService.GetAllMessages();

						Assert.Equal(expectedMessages.Count, result.Count);
			Assert.Equal(expectedMessages.Select(x => new CustomerMessageDTO(x.Sanatizedmessage) { Id = x.Id }), result, new CustomerMessageDTOComparer());
		}

		[Fact]
		public async Task GetById_ReturnsCustomerMessageDTO_WhenMessageExists()
		{
						Guid id = Guid.NewGuid();
			var expectedMessage = new CustomerMessage { Id = id, Timestamp = DateTime.Today, Message = "Test Message", Sanatizedmessage = "Sanitized Test Message" };

			_mockMessageRepository.Setup(mr => mr.GetById(It.IsAny<string>()))
				.ReturnsAsync(expectedMessage);

						var result = await _messageService.GetById(id);

						Assert.Equal(expectedMessage.Id, result.Id);
			Assert.Equal(expectedMessage.Message, result.Message);
		}

		[Fact]
		public async Task GetById_ThrowsInvalidOperationException_WhenMessageDoesNotExist()
		{
						Guid id = Guid.NewGuid();
			_mockMessageRepository.Setup(mr => mr.GetById(It.IsAny<string>()))
				.ReturnsAsync((CustomerMessage?)null);

			// Act & Assert
			await Assert.ThrowsAsync<InvalidOperationException>(() => _messageService.GetById(id));
		}

		[Fact]
		public async Task SanitizeMessageAsync_ReturnsCustomerMessageDTO_WhenMessageContainsSensitiveWord()
		{
						string message = "This is a test message with sensitive words like password and username.";
			string sanitizedMessage = "This is a test message with sensitive words like ***** and ********.";
			_mockSensitiveWordRepository.Setup(swr => swr.ContainsSensitiveWord(It.IsAny<string>()))
				.ReturnsAsync(true);
			_mockSensitiveWordRepository.Setup(swr => swr.BleepWordsAsync(It.IsAny<string>(), It.IsAny<bool>()))
				.ReturnsAsync(new CustomerMessageDTO(sanitizedMessage));

						var result = await _messageService.SanitizeMessageAsync(message, true);

						Assert.Equal(sanitizedMessage, result.Message);
		}

		[Fact]
		public async Task SanitizeMessageAsync_ReturnsCustomerMessageDTO_WhenMessageDoesNotContainSensitiveWord()
		{
						string message = "This is a test message without sensitive words.";
			_mockSensitiveWordRepository.Setup(swr => swr.ContainsSensitiveWord(It.IsAny<string>()))
				.ReturnsAsync(false);

						var result = await _messageService.SanitizeMessageAsync(message, true);

						Assert.Equal(message, result.Message);
		}

		[Fact]
		public async Task SanitizeMessageAsync_DoesNotAudit_WhenAuditIsFalse()
		{
						string message = "This is a test message without sensitive words.";
			_mockSensitiveWordRepository.Setup(swr => swr.ContainsSensitiveWord(It.IsAny<string>()))
				.ReturnsAsync(false);

						var result = await _messageService.SanitizeMessageAsync(message, false);

						Assert.Equal(message, result.Message);
			_mockMessageRepository.Verify(mr => mr.Create(It.IsAny<CustomerMessage>()), Times.Never);
		}

		[Fact]
		public async Task Update_ReturnsTrue_WhenMessageIsUpdated()
		{
						var message = new CustomerMessageDTO { Id = Guid.NewGuid(), Message = "Updated Test Message" };
			var sanitizedMessage = new CustomerMessageDTO { Id = Guid.NewGuid(), Message = "*****" };
			_mockSensitiveWordRepository.Setup(swr => swr.BleepWordsAsync(It.IsAny<string>(), It.IsAny<bool>()))
				.ReturnsAsync(sanitizedMessage);
			_mockMessageRepository.Setup(mr => mr.Update(It.IsAny<CustomerMessage>()))
				.ReturnsAsync(true);

						var result = await _messageService.Update(message);

						Assert.True(result);
		}

		[Fact]
		public async Task Update_ReturnsFalse_WhenMessageIsNotUpdated()
		{
						var message = new CustomerMessageDTO { Id = Guid.NewGuid(), Message = "Updated Test Message" };
			_mockMessageRepository.Setup(mr => mr.Update(It.IsAny<CustomerMessage>()))
				.ReturnsAsync(false);

			_mockSensitiveWordRepository.Setup(swr => swr.BleepWordsAsync(It.IsAny<string>(), It.IsAny<bool>()))
				.ReturnsAsync(new CustomerMessageDTO { Message = "*****" }); 

						var result = await _messageService.Update(message);

						Assert.False(result);
		}

		private class CustomerMessageDTOComparer : IEqualityComparer<CustomerMessageDTO>
		{
			public bool Equals(CustomerMessageDTO x, CustomerMessageDTO y)
			{
				return x.Id == y.Id &&
					   x.Message == y.Message;
			}

			public int GetHashCode(CustomerMessageDTO obj)
			{
				return obj.Id.GetHashCode();
			}
		}
	}
}