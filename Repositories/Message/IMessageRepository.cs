using FlashGroupTechAssessment.Models;

namespace FlashGroupTechAssessment.Repositories.Message
{
	public interface IMessageRepository
	{
		public string Add(string message);
		public List<Models.CustomerMessage> GetAllForUser(string message);
	}
}
