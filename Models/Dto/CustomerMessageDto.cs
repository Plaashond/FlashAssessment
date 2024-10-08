namespace FlashGroupTechAssessment.Models.Dto
{
	public class CustomerMessageDTO
	{
		public CustomerMessageDTO(string message)
		{
			Message = message;
		}
		public Guid? Id { get; set; }
		public string Message { get; set; }
	}
}
