using Dapper;
using System.Data;

namespace FlashGroupTechAssessment.Wrappers
{
	//Had to write a wrapper that abstracts the dapper package so I can test the queries written
	public class DbConnectionWrapper : IDbConnectionWrapper
	{
		private readonly IDbConnection _dbConnection;

		public DbConnectionWrapper(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

		public string ConnectionString
		{
			get => _dbConnection.ConnectionString;
			set => _dbConnection.ConnectionString = value;
		}

		public int ConnectionTimeout => _dbConnection.ConnectionTimeout;

		public string Database => _dbConnection.Database;

		public ConnectionState State => _dbConnection.State;

		public IDbTransaction BeginTransaction()
		{
			return _dbConnection.BeginTransaction();
		}

		public IDbTransaction BeginTransaction(IsolationLevel il)
		{
			return _dbConnection.BeginTransaction(il);
		}

		public void ChangeDatabase(string databaseName)
		{
			_dbConnection.ChangeDatabase(databaseName);
		}

		public void Close()
		{
			_dbConnection.Close();
		}

		public IDbCommand CreateCommand()
		{
			return _dbConnection.CreateCommand();
		}

		public void Dispose()
		{
			_dbConnection.Dispose();
		}

		public void Open()
		{
			_dbConnection.Open();
		}

		public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return _dbConnection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
		}

		public Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return _dbConnection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
		}

		public Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			return _dbConnection.QuerySingleAsync<T>(sql, param, transaction, commandTimeout, commandType);
		}
	}
}