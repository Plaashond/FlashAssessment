using Dapper;
using FlashGroupTechAssessment.Models;
using FlashGroupTechAssessment.Models.Dto;
using FlashGroupTechAssessment.Repositories.Message;
using Moq;
using System.Data;

namespace FlashGroupUnitTests.Repositories
{

	public class MessageRepositoryTests
	{
		private readonly Mock<IDbConnectionWrapper> _mockDbConnection;
		private readonly MessageRepository _messageRepository;

		public MessageRepositoryTests()
		{
			_mockDbConnection = new Mock<IDbConnectionWrapper>();
			_messageRepository = new MessageRepository(_mockDbConnection.Object);
		}

		[Fact]
		public async Task GetAll_ReturnsListOfCustomerMessages()
		{
						var expectedMessages = new List<CustomerMessage>
		{
			new() { Id = Guid.NewGuid(), Timestamp = DateTime.Now, Message = "Test Message 1", Sanatizedmessage = "Sanitized Test Message 1" },
			new() { Id = Guid.NewGuid(), Timestamp = DateTime.Now, Message = "Test Message 2", Sanatizedmessage = "Sanitized Test Message 2" }
		};

			_mockDbConnection.Setup(db => db.QueryAsync<CustomerMessage>(It.IsAny<string>(), null, null, null, null))
				.ReturnsAsync(expectedMessages);

						var result = await _messageRepository.GetAll();

						Assert.Equal(expectedMessages.Count, result.Count);
			Assert.Equal(expectedMessages, result, new CustomerMessageComparer());
		}
		[Fact]
		public async Task GetById_ReturnsCustomerMessage()
		{
						var expectedMessage = new CustomerMessage { Id = Guid.NewGuid(), Timestamp = DateTime.Now, Message = "Test Message", Sanatizedmessage = "Sanitized Test Message" };

			_mockDbConnection.Setup(db => db.QueryAsync<CustomerMessage>(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
				.ReturnsAsync(new List<CustomerMessage> { expectedMessage });

						var result = await _messageRepository.GetById(Guid.NewGuid().ToString());

						Assert.Equal(expectedMessage, result, new CustomerMessageComparer());
		}

		[Fact]
		public async Task Create_ReturnsTrue()
		{
						var message = new CustomerMessage { Id = Guid.NewGuid(), Timestamp = DateTime.Now, Message = "Test Message", Sanatizedmessage = "Sanitized Test Message" };

			_mockDbConnection.Setup(db => db.ExecuteAsync(It.IsAny<string>(), message, null, null, null))
				.ReturnsAsync(1);

						var result = await _messageRepository.Create(message);

						Assert.True(result);
		}

		[Fact]
		public async Task Update_ReturnsTrue()
		{
						var message = new CustomerMessage { Id = Guid.NewGuid(), Timestamp = DateTime.Now, Message = "Test Message", Sanatizedmessage = "Sanitized Test Message" };

			_mockDbConnection.Setup(db => db.QueryAsync<CustomerMessage>(It.IsAny<string>(), message, null, null, null))
				.ReturnsAsync(new List<CustomerMessage> { message });

			_mockDbConnection.Setup(db => db.ExecuteAsync(It.IsAny<string>(), message, null, null, null))
				.ReturnsAsync(1);

						var result = await _messageRepository.Update(message);

						Assert.True(result);
		}

		[Fact]
		public async Task Delete_ReturnsTrue()
		{
						var message = new CustomerMessage { Id = Guid.NewGuid(), Timestamp = DateTime.Now, Message = "Test Message", Sanatizedmessage = "Sanitized Test Message" };

			_mockDbConnection.Setup(db => db.QueryAsync<CustomerMessage>(It.IsAny<string>(), Guid.NewGuid(), null, null, null))
				.ReturnsAsync(new List<CustomerMessage> { message });

			_mockDbConnection.Setup(db => db.ExecuteAsync(It.IsAny<string>(), null, null, null, null))
				.ReturnsAsync(1);

						var result = await _messageRepository.Delete(Guid.NewGuid().ToString());

						Assert.True(result);
		}

		private class CustomerMessageComparer : IEqualityComparer<CustomerMessage>
		{
			public bool Equals(CustomerMessage x, CustomerMessage y)
			{
				return x.Id == y.Id &&
					   x.Timestamp == y.Timestamp &&
					   x.Message == y.Message &&
					   x.Sanatizedmessage == y.Sanatizedmessage;
			}

			public int GetHashCode(CustomerMessage obj)
			{
				return obj.Id.GetHashCode();
			}
		}
	}
}
