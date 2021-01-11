using System;
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
            Logger?.LogInformation("Trying to create the warehouse database schema.");

            var script = await GetScriptAsync("build");

            using var connection = await Connector.CreateConnectionAsync();

            var result = await ExecuteAsync(script);

            if (result.Succeeded)
            {
                Logger?.LogInformation("Successfully created the warehouse database schema.");
            }
            else
            {
                Logger?.LogError(result.Exception, "Fail created the warehouse database schema.");
            }

            return result;
        }

        /// <summary>
        /// Drops the warehouse database schema.
        /// </summary>
        public async Task<WarehouseResult> ClearAsync()
        {
            Logger?.LogInformation("Trying to clear the warehouse database schema.");

            var script = await GetScriptAsync("clear");
            var result = await ExecuteAsync(script);

            if (result.Succeeded)
            {
                Logger?.LogInformation("Successfully cleared the warehouse database schema.");
            }
            else
            {
                Logger?.LogError(result.Exception, "Fail cleared the warehouse database schema.");
            }

            return result;
        }

        #endregion

        #region Helpers

        private async Task<WarehouseResult> ExecuteAsync(string commandText)
        {
            using var connection = await Connector.CreateConnectionAsync();

            try { await connection.Query(commandText).RunAsync(); }
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
