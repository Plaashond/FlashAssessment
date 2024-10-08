using FlashGroupTechAssessment.Models;
using FlashGroupTechAssessment.Models.Dto;
using FlashGroupTechAssessment.Repositories.Message;
using FlashGroupTechAssessment.Repositories.SensitiveWord;
using System.Collections.Generic;
using System.Linq;

namespace FlashGroupTechAssessment.Services
{
	public class MessageService : IMessageService
	{
		private readonly ISensitiveWordRepository _sensitiveWordRepository;
		private readonly IMessageRepository _messageRepository;
		public MessageService(ISensitiveWordRepository sensitiveWordRepository, IMessageRepository messageRepository)
		{
			_sensitiveWordRepository = sensitiveWordRepository;
			_messageRepository = messageRepository;
		}
		/// <inheritdoc/>
		public async Task<bool> Delete(Guid id)
		{
			return await _messageRepository.Delete(id.ToString()); 
		}
		/// <inheritdoc/>
		public async Task<ICollection<CustomerMessageDTO>> GetAllMessages()
		{
			ICollection<CustomerMessage> mesages = await _messageRepository.GetAll();
			return mesages.Select(x=>new CustomerMessageDTO(x.Sanatizedmessage) { Id = x.Id}).ToList();
		}
		/// <inheritdoc/>
		public async Task<CustomerMessageDTO> GetById(Guid id)
		{
			CustomerMessage? message = await _messageRepository.GetById(id.ToString());
			return message == null
				? throw new InvalidOperationException("message does not exist")
				: new CustomerMessageDTO(message.Message) { Id = message.Id};
		}

		/// <inheritdoc/>
		public async Task<CustomerMessageDTO> SanitizeMessageAsync(string message,bool audit = false)
		{
			if (message == null || !_sensitiveWordRepository.ContainsSensitiveWord(message))
			{
				if (audit)
				{
					await _messageRepository.Create(new CustomerMessage(message!, message!));
				}
				return new CustomerMessageDTO(message!);
			}
			//TODO: add message for auditing purposes
			return await _sensitiveWordRepository.BleepWordsAsync(message, audit);
		}
		/// <inheritdoc/>
		public async Task<bool> Update(CustomerMessageDTO message)
		{
			CustomerMessageDTO sanatizedWord = await _sensitiveWordRepository.BleepWordsAsync(message.Message, false);
			Console.WriteLine(sanatizedWord.Message);
			Console.WriteLine(message.Id);
			sanatizedWord.Id = message.Id;
			return await _messageRepository.Update(new CustomerMessage(sanatizedWord));
		}
	}
}
