using FlashGroupTechAssessment.Models;

namespace FlashGroupTechAssessment.Services
{
	public interface IMessageService
	{
		public CustomerMessage SanatizeMessage(string message);
	}
}
