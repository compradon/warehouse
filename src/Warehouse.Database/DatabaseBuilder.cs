using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Compradon.Warehouse.Database
{
    /// <summary>
    /// Provides an abstraction class the APIs for building a warehouse database schema.
    /// </summary>
    public abstract class DatabaseBuilder : IDatabaseBuilder
    {
        #region Properties

        /// <summary>
        /// The <see cref="IDatabaseConnector"/>.
        /// </summary>
        protected IDatabaseConnector Connector { get; }

        /// <summary>
        /// The <see cref="ILogger"/> used to log messages from the manager.
        /// </summary>
        /// <value>
        /// The <see cref="ILogger"/> used to log messages from the manager.
        /// </value>
        public virtual ILogger<DatabaseBuilder> Logger { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="DatabaseBuilder"/>.
        /// </summary>
        /// <param name="connector">The <see cref="IDatabaseConnector"/>.</param>
        /// <param name="logger">The <see cref="ILogger"/>.</param>
        public DatabaseBuilder(
            IDatabaseConnector connector,
            ILogger<DatabaseBuilder> logger = null)
        {
            if (connector == null) throw new ArgumentNullException(nameof(connector));

            Connector = connector;
            Logger = logger;
        }

        #endregion

        #region IDatabaseBuilder

        /// <summary>
        /// Creates the warehouse database schema.
        /// </summary>
        public async Task<WarehouseResult> BuildAsync()
        {
            var script = await GetScriptAsync("build");

            return await ExecuteAsync(script);
        }

        /// <summary>
        /// Drops the warehouse database schema.
        /// </summary>
        public async Task<WarehouseResult> ClearAsync()
        {
            var script = await GetScriptAsync("clear");

            return await ExecuteAsync(script);
        }

        #endregion

        #region Helpers

        private async Task<WarehouseResult> ExecuteAsync(string commandText)
        {
            using var connector = await Connector.CreateConnectionAsync();
            var command = connector.CreateCommand();

            command.CommandText = commandText;
            command.CommandType = CommandType.Text;

            try { command.ExecuteNonQuery(); }
            catch (InvalidOperationException exception)
            {
                return WarehouseResult.Failed(exception);
            }

            return WarehouseResult.Success;
        }

        private static async Task<string> GetScriptAsync(string fileName)
        {
            var manifestEmbeddedProvider = new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly());
            var scriptFile = manifestEmbeddedProvider.GetFileInfo($"Scripts/{fileName}.sql");

            using var stream = new StreamReader(scriptFile.CreateReadStream());

            return await stream.ReadToEndAsync();
        }

        #endregion
    }
}
