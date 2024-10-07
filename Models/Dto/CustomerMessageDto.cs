namespace FlashGroupTechAssessment.Models.Dto
{
	public class CustomerMessageDTO
	{
		public CustomerMessageDTO(string message)
		{
			Message = message;
		}
		public string Message { get; set; }
	}
}
