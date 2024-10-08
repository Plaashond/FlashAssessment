using FlashGroupTechAssessment.Models;
using System.Data.Common;
using System.Data;
using Dapper;

namespace FlashGroupTechAssessment.Repositories.Message
{
	public class MessageRepository : IMessageRepository
	{
		private readonly IDbConnectionWrapper _dbConnection;

		public MessageRepository(IDbConnectionWrapper dbConnection)
		{
			_dbConnection = dbConnection;
		}
		/// <inheritdoc/>
		public async Task<List<CustomerMessage>> GetAll()
		{
			try
			{
				_dbConnection.Open();
				string query = @"SELECT id as ID, [timestamp] as TimeStamp, message as Message, sanitized_message as SanatizedMessage FROM MyDatabase.dbo.CustomerMessage;";

				var projects = await _dbConnection.QueryAsync<CustomerMessage>(query);
				return projects.ToList();
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				_dbConnection.Close();
			}
		}
		/// <inheritdoc/>
		public async Task<CustomerMessage?> GetById(string id)
		{
			try
			{
				_dbConnection.Open();

				string query = $@"SELECT id, [timestamp], message, sanitized_message FROM MyDatabase.dbo.CustomerMessage where id = '{id}'";

				var customer = await _dbConnection.QueryAsync<CustomerMessage>(query, id);
				return customer.FirstOrDefault();
			}
			catch (Exception)
			{
				return null;
			}
			finally
			{
				_dbConnection.Close();
			}
		}
		/// <inheritdoc/>
		public async Task<bool> Create(CustomerMessage message)
		{
			try
			{
				_dbConnection.Open();

				string query = @"INSERT INTO MyDatabase.dbo.CustomerMessage
								( [timestamp], message, sanitized_message)
								VALUES( @TimeStamp, @Message, @SanatizedMessage); ";

				await _dbConnection.ExecuteAsync(query, message);
				return true;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				_dbConnection.Close();
			}
		}
		/// <inheritdoc/>
		public async Task<bool> Update(CustomerMessage message)
		{
			try
			{
				_dbConnection.Open();

				string selectQuery = $@"SELECT id as ID, [timestamp] as TimeStamp, message as Message, sanitized_message as SanatizedMessage FROM MyDatabase.dbo.CustomerMessage where id = '{message.Id}'";

				var entity = await _dbConnection.QueryAsync<CustomerMessage>(selectQuery, message);

				if (entity is null)
					return false;

				string updateQuery = @"UPDATE MyDatabase.dbo.CustomerMessage
										SET [timestamp]=@TimeStamp, message=@Message, sanitized_message=@SanatizedMessage
										WHERE id=@Id;";

				await _dbConnection.ExecuteAsync(updateQuery, message);
				return true;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				_dbConnection.Close();
			}
		}
		/// <inheritdoc/>
		public async Task<bool> Delete(string id)
		{
			try
			{
				_dbConnection.Open();

				string selectQuery = $@"select id as ID, [timestamp] as TimeStamp, message as Message, sanitized_message as SanatizedMessage from CustomerMessage where id = '{id}'";

				var entity = await _dbConnection.QueryAsync<CustomerMessage>(selectQuery, id);

				if (entity is null)
					return false;

				string query = $@"delete from CustomerMessage where id = '{id}'";

				await _dbConnection.ExecuteAsync(query);
				return true;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				_dbConnection.Close();
			}
		}
	}
}
