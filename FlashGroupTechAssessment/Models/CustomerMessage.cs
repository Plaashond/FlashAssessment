using FlashGroupTechAssessment.Models.Dto;

namespace FlashGroupTechAssessment.Models
{
	public class CustomerMessage
	{
		public CustomerMessage()
		{
			
		}
		public CustomerMessage(string Message,string SanatizedMessage,DateTime? TimeStamp = null)
		{
			this.Message = Message;
			this.Sanatizedmessage = SanatizedMessage;
			TimeStamp ??= DateTime.UtcNow;
			this.Timestamp = (DateTime)TimeStamp;
		}
		public CustomerMessage(CustomerMessageDTO message)
		{
			this.Id = message.Id.Value;
			this.Message = message.Message;
			this.Sanatizedmessage = message.Message;
			this.Timestamp = DateTime.UtcNow;
		}
		public Guid Id { get; set; }
		public DateTime Timestamp { get; set; }
		public string Message { get; set; }
		public string Sanatizedmessage { get; set; }

	}
}
