using FlashGroupTechAssessment.Models;
using FlashGroupTechAssessment.Models.Dto;

namespace FlashGroupTechAssessment.Services.Message
{
	public interface IMessageService
	{
		/// <summary>
		/// The SanitizeMessageAsync function in C# returns a Task that sanitizes a CustomerMessageDTO object
		/// asynchronously.
		/// </summary>
		/// <param name="message">The `message` parameter in the `SanitizeMessageAsync` method is likely a
		/// string that contains customer input or data that needs to be sanitized or cleaned up in some way.
		/// This could involve removing sensitive information, filtering out inappropriate content, or
		/// formatting the message for display. The method returns a `</param>
		public Task<CustomerMessageDTO> SanitizeMessageAsync(string message, bool audit = false);
		/// <summary>
		/// This function retrieves a customer message by its unique identifier.
		/// </summary>
		/// <param name="Guid">A Guid (Globally Unique Identifier) is a 128-bit integer value used to uniquely
		/// identify an object or entity. In the context of the method signature you provided, the Guid
		/// parameter represents the unique identifier of a customer message that you want to
		/// retrieve.</param>
		public Task<CustomerMessageDTO> GetById(Guid id);
		/// <summary>
		/// The function Delete takes a Guid id as input and returns a Task<bool> indicating whether the
		/// deletion was successful.
		/// </summary>
		/// <param name="Guid">A Guid (Globally Unique Identifier) is a 128-bit integer value used to uniquely
		/// identify objects or entities in software applications. It is often used as a unique identifier for
		/// records in databases or as a way to reference specific resources. In the context of the provided
		/// method signature, the Guid parameter</param>
		public Task<bool> Delete(Guid id);
		/// <summary>
		/// This C# function Update takes a CustomerMessageDTO object as input and returns a Task<bool>.
		/// </summary>
		/// <param name="CustomerMessageDTO">The CustomerMessageDTO is a data transfer object (DTO) that
		/// likely contains information related to a customer message. It could include properties such as the
		/// message content, sender information, recipient information, timestamp, and any other relevant
		/// details related to the message.</param>
		public Task<bool> Update(CustomerMessageDTO message);
		/// <summary>
		/// This function returns a collection of CustomerMessageDTO objects asynchronously.
		/// </summary>
		public Task<ICollection<CustomerMessageDTO>> GetAllMessages();
	}
}
