using FlashGroupTechAssessment.Models;

namespace FlashGroupTechAssessment.Repositories.Message
{
	public interface IMessageRepository
	{
		/// <summary>
		/// This function returns a task that will asynchronously retrieve a list of customer messages.
		/// </summary>
		public Task<List<CustomerMessage>> GetAll();
		/// <summary>
		/// This function retrieves a customer message by its ID asynchronously.
		/// </summary>
		/// <param name="id">The `id` parameter is a unique identifier used to retrieve a specific
		/// `CustomerMessage` object from a data source. The `GetById` method is expected to return a `Task`
		/// that may contain a `CustomerMessage` object corresponding to the provided `id`.</param>
		public Task<CustomerMessage?> GetById(string id);
		/// <summary>
		/// This function creates a customer message and returns a task indicating whether the operation was
		/// successful.
		/// </summary>
		/// <param name="CustomerMessage">The `Create` method takes a `CustomerMessage` object as a parameter.
		/// This object likely contains information about a customer message that needs to be processed or
		/// stored in some way. The method returns a `Task<bool>` indicating whether the creation of the
		/// customer message was successful or not.</param>
		public Task<bool> Create(CustomerMessage message);
		/// <summary>
		/// This function updates a customer message and returns a task with a boolean indicating success.
		/// </summary>
		/// <param name="CustomerMessage">The `CustomerMessage` parameter in the `Update` method represents a
		/// message object related to a customer. This object likely contains information such as the
		/// customer's details, message content, timestamp, and any other relevant data associated with the
		/// customer interaction. The `Update` method is expected to update this customer</param>
		public Task<bool> Update(CustomerMessage message);
		/// <summary>
		/// The Delete function in C# is a Task that returns a boolean value indicating whether the deletion
		/// was successful or not.
		/// </summary>
		/// <param name="id">The `id` parameter in the `Delete` method is typically a unique identifier that
		/// specifies the item to be deleted. This identifier is used to locate and remove the specific item
		/// from the system or database.</param>
		public Task<bool> Delete(string id);
	}
}
