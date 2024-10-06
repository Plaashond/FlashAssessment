namespace FlashGroupTechAssessment.Models
{
	public class CustomerMessage
	{
		public string Message { get; set; }
		public string SanatizedMessage { get; set; }
		public DateTime TimeStamp { get; set; }
		public Guid CustomerId { get; set; }

	}
}
