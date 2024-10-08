using FlashGroupTechAssessment.Models.Dto;

namespace FlashGroupTechAssessment.Repositories.SensitiveWord
{
	public interface ISensitiveWordRepository
	{
		/// <summary>
		/// This C# function checks if a given string contains any sensitive words.
		/// </summary>
		/// <param name="words">The `ContainsSensitiveWord` method you provided seems to be a method that
		/// checks if a given string contains a sensitive word. The parameter `words` is the string that you
		/// want to check for sensitive words. The method will return a boolean value indicating whether the
		/// input string contains any sensitive words or not</param>
		public bool ContainsSensitiveWord(string words);
		/// <summary>
		/// The function `BleepWordAsync` asynchronously sanitizes a string by splitting it into batches,
		/// sanitizing each batch, and then joining the sanitized batches into a single string.
		/// </summary>
		/// <param name="message">The `words` parameter in the `BleepWordAsync` method is a string that contains
		/// the text to be processed. This method splits the original string into batches, sanitizes each
		/// batch asynchronously, and then joins the sanitized batches back together into a single string
		/// before returning a `CustomerMessageDTO`</param>
		/// <returns>
		/// The method `BleepWordAsync` returns a `Task<CustomerMessageDTO>`.
		/// </returns>
		public Task<CustomerMessageDTO> BleepWordsAsync(string message, bool audit);
	}
}
