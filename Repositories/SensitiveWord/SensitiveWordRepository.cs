using Dapper;
using FlashGroupTechAssessment.Models;
using FlashGroupTechAssessment.Models.Dto;
using FlashGroupTechAssessment.Repositories.Message;
using System.Data;
using System.Data.Common;

namespace FlashGroupTechAssessment.Repositories.SensitiveWord
{
	public class SensitiveWordRepository : ISensitiveWordRepository
	{
		private readonly IDbConnection _connection;
		private readonly IMessageRepository _messageRepository;

		public SensitiveWordRepository(IDbConnection dbConnection, IMessageRepository messageRepository)
		{
			_connection = dbConnection;
			_messageRepository = messageRepository;
		}
		/// <inheritdoc/>
		public async Task<CustomerMessageDTO> BleepWordsAsync(string words,bool audit)
		{
			// Split the original string into batches
			IEnumerable<string> batches = SplitIntoBatches(words, 2000);//After about 1500 there is diminishing returns
			IEnumerable<Task<string>> tasks = batches.Select(SanitizeBatchAsync);
			string[] results = await Task.WhenAll(tasks);
			string sanitized = string.Join(" ", results);
			if (audit)
			{
				await _messageRepository.Create(new CustomerMessage(words!, sanitized));
			}
			return new CustomerMessageDTO(sanitized);
		}

		/// <inheritdoc/>
		public bool ContainsSensitiveWord(string words)
		{
			object parameters = new { Words = words };
			string query = @"
			DECLARE @sentence NVARCHAR(MAX) = @Words;
			SELECT
				CASE
					WHEN EXISTS (
						SELECT 1
						FROM SensitiveWord
						 WHERE CHARINDEX(' ' + word + ' ', @sentence) > 0
					)
					THEN 1
					ELSE 0
				END AS ContainsWord";
			return _connection.Query<string>(query, parameters).Any();
		}
		/// <summary>
		/// The function `SanitizeBatchAsync` sanitizes a batch of words by replacing sensitive words with
		/// starred versions.
		/// </summary>
		/// <param name="batch">The `batch` parameter in the `SanitizeBatchAsync` method is a string
		/// containing a batch of words that needs to be sanitized. The method processes this batch of words
		/// to replace any sensitive words with their corresponding starred words based on a predefined list
		/// of sensitive words.</param>
		/// <returns>
		/// The `SanitizeBatchAsync` method returns a sanitized version of the input `batch` string after
		/// replacing any sensitive words with their corresponding starred words. The sanitized string is
		/// returned as a `Task<string>` asynchronously.
		/// </returns>
		private async Task<string> SanitizeBatchAsync(string batch)
		{
			object parameters = new { Words = batch };
			string query = @"
            DECLARE @originalString NVARCHAR(MAX) = @Words;
            DECLARE @modifiedString NVARCHAR(MAX) = @originalString;
            DECLARE @currentWord NVARCHAR(255);
            DECLARE @replaceWord NVARCHAR(255);
            DECLARE @SensitiveWords TABLE (word NVARCHAR(255), starred_word NVARCHAR(255));
            INSERT INTO @SensitiveWords (word, starred_word)
            SELECT word, starred_word
            FROM SensitiveWord
            WHERE CHARINDEX(' ' + word + ' ', @originalString) > 0;
            WHILE EXISTS (SELECT 1 FROM @SensitiveWords)
            BEGIN
                SELECT TOP 1 @currentWord = word, @replaceWord = starred_word
                FROM @SensitiveWords;
                SET @modifiedString = REPLACE(@modifiedString, ' ' + @currentWord + ' ', ' ' + @replaceWord + ' ');
                DELETE FROM @SensitiveWords WHERE word = @currentWord;
            END
            SELECT @modifiedString AS ModifiedString;";
			if (_connection.State == ConnectionState.Closed)
			{
				_connection.Open();
			}
			return await _connection.QuerySingleAsync<string>(query, parameters);
		}

		/// <summary>
		/// The function `SplitIntoBatches` takes a string of words and splits it into batches of a specified
		/// size.
		/// </summary>
		/// <param name="words">The `words` parameter in the `SplitIntoBatches` method is a string containing
		/// a sequence of words separated by spaces.</param>
		/// <param name="batchSize">Batch size is the number of words you want to include in each batch when
		/// splitting the input words into batches.</param>
		private static IEnumerable<string> SplitIntoBatches(string words, int batchSize)
		{
			var wordList = words.Split(' ');
			for (int i = 0; i < wordList.Length; i += batchSize)
			{
				yield return string.Join(" ", wordList.Skip(i).Take(batchSize));
			}
		}
	}
}
