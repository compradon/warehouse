using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Compradon.Warehouse.Database.Postgres
{
    /// <summary>
    /// 
    /// </summary>
    public class PostgresDatabaseBuilder : IDatabaseBuilder
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
        public virtual ILogger Logger { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public PostgresDatabaseBuilder(
            IDatabaseConnector connector,
            ILogger<PostgresDatabaseBuilder> logger)
        {
            Connector = connector;
            Logger = logger;
        }

        #endregion

        #region IDatabaseBuilder

        public Task CreateAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task DropAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ExistsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> VersionAsync()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// 
        /// </summary>
        public static async Task<string> GetCommandAsync(string command)
        {
            var manifestEmbeddedProvider = new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly());
            var scriptFile = manifestEmbeddedProvider.GetFileInfo($"Scripts/{command}.sql");

            using var stream = new StreamReader(scriptFile.CreateReadStream());

            return await stream.ReadToEndAsync();
        }

        #endregion
    }
}
