namespace FlashGroupTechAssessment.Models.Dto
{
	public class SensitiveWordDto
	{
		public SensitiveWordDto()
		{
			
		}
		public SensitiveWordDto(string Word)
		{
			this.Word = Word;
		}
		public SensitiveWordDto(string Word, int Id)
		{
			this.Word = Word;
			this.Id = Id;
		}
		public int Id { get; set; }
		public string Word { get; set; }
	}
}
