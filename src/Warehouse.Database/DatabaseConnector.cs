using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Compradon.Warehouse.Database
{
    /// <summary>
    /// Provides an abstraction class for connecting to the warehouse database.
    /// </summary>
    public abstract class DatabaseConnector : IDatabaseConnector
    {
        #region Variables

        private bool _disposed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the connection string used to connect to the database.
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// The connections that were created to the database.
        /// </summary>
        protected IList<IDbConnection> Connections { get; } = new List<IDbConnection>();

        /// <summary>
        /// Gets the number of database connections created.
        /// </summary>
        public int Count => Connections.Count;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="DatabaseConnector"/>.
        /// </summary>
        /// <param name="connectionString">The connection string used to connect to the database.</param>
        public DatabaseConnector(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException(nameof(connectionString));

            ConnectionString = connectionString;
        }

        #endregion

        #region IDatabaseConnector

        /// <summary>
        /// Creates a database connection as an asynchronous operation.
        /// </summary>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IDbConnection"/> of the creation operation.</returns>
        protected abstract Task<IDbConnection> InitializeСonnectionAsync();

        /// <summary>
        /// Creates a database connection as an asynchronous operation.
        /// </summary>
        /// <param name="open">Indicates the need to immediately open a connection to the database.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IDbConnection"/> of the creation operation.</returns>
        public virtual async Task<IDbConnection> CreateConnectionAsync(bool open = true)
        {
            var connection = await InitializeСonnectionAsync();

            Connections.Add(connection);

            if (open) await Task.Run(() => connection.Open());

            return connection;
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Releases all resources used by the class instance.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the class instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                foreach (var connection in Connections)
                    connection.Dispose();

                _disposed = true;
            }
        }

        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);
        }

        #endregion
    }
}
