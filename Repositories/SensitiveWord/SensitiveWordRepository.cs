using Dapper;
using FlashGroupTechAssessment.Models.Dto;
using System.Data;
using System.Data.Common;

namespace FlashGroupTechAssessment.Repositories.SensitiveWord
{
	public class SensitiveWordRepository : ISensitiveWordRepository
	{
		private readonly IDbConnection _connection;

		public SensitiveWordRepository(IDbConnection dbConnection)
		{
			_connection = dbConnection;
		}

		public async Task<CustomerMessageDTO> BleepWordAsync(string words)
		{
			// Split the original string into batches
			var batches = SplitIntoBatches(words, 2000);//After about 1500 there is diminishing returns

			// Process each batch asynchronously
			var tasks = batches.Select(batch => SanitizeBatchAsync(batch));
			var results = await Task.WhenAll(tasks);

			// Combine the results
			var sanitized = string.Join(" ", results);

			return new CustomerMessageDTO(sanitized);
		}
		

		public bool ContainsSensitiveWord(string words)
		{
			var parameters = new { Words = words };
			string query = @"
			DECLARE @sentence NVARCHAR(MAX) = @Words;
			SELECT
				CASE
					WHEN EXISTS (
						SELECT 1
						FROM SensitiveWord
						WHERE @sentence COLLATE Latin1_General_CS_AS LIKE '%' + word + '%'
					)
					THEN 1
					ELSE 0
				END AS ContainsWord";
			return _connection.Query<string>(query, parameters).Any();
		}

		private async Task<string> SanitizeBatchAsync(string batch)
		{
			var parameters = new { Words = batch };
			string query = @"
            DECLARE @originalString NVARCHAR(MAX) = @Words;
            DECLARE @modifiedString NVARCHAR(MAX) = @originalString;
            DECLARE @currentWord NVARCHAR(255);
            DECLARE @replaceWord NVARCHAR(255);

            -- Store sensitive words and replacements in a temp table for faster access
            DECLARE @SensitiveWords TABLE (word NVARCHAR(255), starred_word NVARCHAR(255));

            INSERT INTO @SensitiveWords (word, starred_word)
            SELECT word, starred_word
            FROM SensitiveWord
            WHERE CHARINDEX(' ' + word + ' ', @originalString) > 0;

            -- Process replacements
            WHILE EXISTS (SELECT 1 FROM @SensitiveWords)
            BEGIN
                SELECT TOP 1 @currentWord = word, @replaceWord = starred_word
                FROM @SensitiveWords;

                -- Replace current sensitive word in the original string
                SET @modifiedString = REPLACE(@modifiedString, ' ' + @currentWord + ' ', ' ' + @replaceWord + ' ');

                -- Remove processed word from the temp table
                DELETE FROM @SensitiveWords WHERE word = @currentWord;
            END

            SELECT @modifiedString AS ModifiedString;";
			if (_connection.State == ConnectionState.Closed)
			{
				_connection.Open();
			}
			return await _connection.QuerySingleAsync<string>(query, parameters);
		}


		private IEnumerable<string> SplitIntoBatches(string words, int batchSize)
		{
			var wordList = words.Split(' ');
			for (int i = 0; i < wordList.Length; i += batchSize)
			{
				yield return string.Join(" ", wordList.Skip(i).Take(batchSize));
			}
		}
	}
}
