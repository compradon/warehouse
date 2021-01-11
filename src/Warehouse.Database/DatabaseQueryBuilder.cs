using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Compradon.Warehouse.Database
{
    /// <summary>
    /// Helper for building and executing database queries. This class cannot be inherited.
    /// </summary>
    public sealed class DatabaseQueryBuilder
    {
        #region Properties

        /// <summary>
        /// The <see cref="DbCommand"/> to execute against a data source.
        /// </summary>
        public DbCommand Command { get; }

        /// <summary>
        /// The <see cref="DbConnection"/> to a database.
        /// </summary>
        public DbConnection Connection { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="DatabaseQueryBuilder"/>.
        /// </summary>
        /// <param name="connection">The connection to the data source.</param>
        /// <param name="commandText">The text command to run against the data source.</param>
        /// <param name="commandType">Specifies how a command string is interpreted.</param>
        public DatabaseQueryBuilder(IDbConnection connection, string commandText, CommandType commandType = CommandType.Text)
        {
            Command = CreateCommand(connection);
            Connection = CreateConnection(connection);

            Command.CommandText = commandText;
            Command.CommandType = commandType;
        }

        #endregion

        #region Helpers

        private static DbCommand CreateCommand(IDbConnection connection)
        {
            if (connection.CreateCommand() is DbCommand command) return command;

            throw new InvalidOperationException();
        }

        private static DbConnection CreateConnection(IDbConnection connection)
        {
            if (connection is DbConnection _connection) return _connection;

            throw new InvalidOperationException();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds parameter to execute a command.
        /// </summary>
        /// <param name="name">The connection to the data source.</param>
        /// <param name="value">The connection to the data source.</param>
        public DatabaseQueryBuilder AddParameter(string name, object value)
        {
            var parameter = Command.CreateParameter();

            parameter.ParameterName = name;
            parameter.Value = value;

            Command.Parameters.Add(parameter);

            return this;
        }

        /// <summary>
        /// Sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        public DatabaseQueryBuilder Timeout(int commandTimeout)
        {
            Command.CommandTimeout = commandTimeout;

            return this;
        }

        /// <summary>
        /// Sets the transaction with in which this query executes.
        /// </summary>
        public DatabaseQueryBuilder Transaction(DbTransaction transaction)
        {
            Command.Transaction = transaction;

            return this;
        }

        /// <summary>
        /// Executes an SQL statement and returns the number of rows affected.
        /// </summary>
        public async Task<int> RunAsync()
        {
            return await Command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result
        /// set returned by the query. All other columns and rows are ignored.
        /// </summary>
        public async Task<T> RunAsync<T>()
        {
            var value = await Command.ExecuteScalarAsync();

            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Executes the query against and returns an <see cref="DbDataReader"/>.
        /// </summary>
        public async Task<DbDataReader> ExecuteAsync()
        {
            return await Command.ExecuteReaderAsync();
        }

        /// <summary>
        /// Executes the query against and returns an <see cref="IAsyncEnumerable{T}"/>.
        /// </summary>
        public async IAsyncEnumerable<T> ExecuteAsync<T>()
        {
            var dataReader = await Command.ExecuteReaderAsync();

            if (dataReader.FieldCount == 0) yield break;

            while (await dataReader.ReadAsync())
            {
                yield return dataReader.Build<T>();
            }
        }

        #endregion
    }
}
