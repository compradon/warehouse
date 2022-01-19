using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Compradon.Warehouse.Database;
using Npgsql;

namespace Compradon.Warehouse.PostgreSQL
{
    /// <summary>
    /// Represents a new instance of a persistence store for warehouse entity types, using the default implementation
    /// of <see cref="ITypeStore{TWarehouseType, TKey}"/> with a <see cref="short" /> as a primary key.
    /// </summary>
    public class PostgresTypeStore : TypeStore
    {
        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="TypeStore"/>.
        /// </summary>
        /// <param name="connector">The <see cref="IDatabaseConnector"/>.</param>
        /// <param name="errorDescriber">The <see cref="WarehouseErrorDescriber"/>.</param>
        public PostgresTypeStore(
            IDatabaseConnector connector,
            WarehouseErrorDescriber errorDescriber) : base(connector, errorDescriber)
        {
            
        }

        #endregion

        /// <summary>
        /// Creates the specified <paramref name="warehouseType"/> in the database.
        /// </summary>
        /// <param name="warehouseType">The warehouse type to save.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        public override async Task<WarehouseResult> CreateAsync(WarehouseType warehouseType, CancellationToken cancellationToken = default)
        {
            if (warehouseType == null) throw new ArgumentNullException(nameof(warehouseType));
            if (warehouseType.Key != 0) throw new ArgumentException(nameof(warehouseType.Key));

            using var connection = await Connector.CreateConnectionAsync();

            try
            {
                var json = JsonSerializer.Serialize(warehouseType);
                
                var query = connection
                    .StoredProcedure("warehouse.create_or_update_entity_type");

                var parameter = query.Command.CreateParameter() as NpgsqlParameter;

                parameter.ParameterName = "item";
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Jsonb;
                parameter.Value = json;

                var reader = await query
                    .AddParameter(parameter)
                    .ExecuteAsync(cancellationToken);

                var _warehouseType = reader.Build<WarehouseType>();

                warehouseType.Key = _warehouseType.Key;

                foreach (var attribute in warehouseType.Attributes)
                    attribute.Key = _warehouseType.Attributes.First(a => a.Alias == attribute.Alias).Key;

                return WarehouseResult.Success;
            }
            catch (Exception exception)
            {
                return WarehouseResult.Failed(exception);
            }
        }

        /// <summary>
        /// Deletes the specified warehouse type from the store, if any, who has the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of warehouse type to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public override async Task<WarehouseResult> DeleteAsync(short key, CancellationToken cancellationToken = default)
        {
            var connection = await Connector.CreateConnectionAsync();

            try
            {
                var rowsAffected = await connection
                    .Query("warehouse.delete_entity_type")
                    .AddParameter("key", key)
                    .RunAsync();

                return WarehouseResult.Success;
            }
            catch (Exception exception)
            {
                return WarehouseResult.Failed(exception);
            }
        }

        /// <summary>
        /// Finds and returns an warehouse type, if any, who has the specified <paramref name="alias"/>.
        /// </summary>
        /// <param name="alias">The unique alias of the warehouse type.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the warehouse type matching the specified <paramref name="alias"/> if it exists.
        /// </returns>
        public override async Task<WarehouseResult<WarehouseType>> FindByAliasAsync(string alias, CancellationToken cancellationToken = default)
        {
            var connection = await Connector.CreateConnectionAsync();

            try
            {
                var result = await connection
                    .StoredProcedure("warehouse.find_entity_type_by_alias")
                    .AddParameter("alias", alias)
                    .ExecuteAsync();

                var value = result.Build<WarehouseType>();

                return new WarehouseResult<WarehouseType>(value);
            }
            catch (Exception exception)
            {
                return new WarehouseResult<WarehouseType>(exception);
            }
        }

        /// <summary>
        /// Finds and returns an warehouse type, if any, who has the specified <paramref name="className"/>.
        /// </summary>
        /// <param name="className">The class name of the warehouse type.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the warehouse type matching the specified <paramref name="className"/> if it exists.
        /// </returns>
        public override async Task<WarehouseResult<WarehouseType>> FindByClassAsync(string className, CancellationToken cancellationToken = default)
        {
            var connection = await Connector.CreateConnectionAsync();

            try
            {
                var result = await connection
                    .StoredProcedure("warehouse.find_entity_type_by_class")
                    .AddParameter("class", className)
                    .ExecuteAsync();

                return new WarehouseResult<WarehouseType>(result.Build<WarehouseType>());
            }
            catch (Exception exception)
            {
                return new WarehouseResult<WarehouseType>(exception);
            }
        }

        /// <summary>
        /// Finds and returns an warehouse type, if any, who has the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The primary key of the warehouse type.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the warehouse type matching the specified <paramref name="key"/> if it exists.
        /// </returns>
        public override async Task<WarehouseResult<WarehouseType>> FindByIdAsync(short key, CancellationToken cancellationToken = default)
        {
            var connection = await Connector.CreateConnectionAsync();

            try
            {
                var result = await connection
                    .StoredProcedure("warehouse.find_entity_type_by_id")
                    .AddParameter("entity_type_id", key)
                    .ExecuteAsync();

                return new WarehouseResult<WarehouseType>(result.Build<WarehouseType>());
            }
            catch (Exception exception)
            {
                return new WarehouseResult<WarehouseType>(exception);
            }
        }

        /// <summary>
        /// Gets all an warehouse types from the database.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        public override async Task<WarehouseResult<IEnumerable<WarehouseType>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var connection = await Connector.CreateConnectionAsync();

            try
            {
                var reader = await connection
                    .StoredProcedure("warehouse.get_all_entity_types")
                    .ExecuteAsync();

                return new WarehouseResult<IEnumerable<WarehouseType>>(reader.Build<IEnumerable<WarehouseType>>());
            }
            catch (Exception exception)
            {
                return new WarehouseResult<IEnumerable<WarehouseType>>(exception);
            }
        }

        /// <summary>
        /// Updates the specified <paramref name="warehouseType"/> in the database.
        /// </summary>
        /// <param name="warehouseType">The warehouse type to save.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="WarehouseResult"/> of the operation.</returns>
        public override async Task<WarehouseResult> UpdateAsync(WarehouseType warehouseType, CancellationToken cancellationToken = default)
        {
            if (warehouseType == null) throw new ArgumentNullException(nameof(warehouseType));
            if (warehouseType.Key == 0) throw new ArgumentException(nameof(warehouseType.Key));

            var connection = await Connector.CreateConnectionAsync();

            try
            {
                var json = JsonSerializer.Serialize(warehouseType);
                
                var query = connection
                    .StoredProcedure("warehouse.create_or_update_entity_type");

                var parameter = query.Command.CreateParameter() as NpgsqlParameter;

                parameter.ParameterName = "item";
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Jsonb;
                parameter.Value = json;

                var reader = await query
                    .AddParameter(parameter)
                    .ExecuteAsync(cancellationToken);

                warehouseType = reader.Build<WarehouseType>();

                return WarehouseResult.Success;
            }
            catch (Exception exception)
            {
                return WarehouseResult.Failed(exception);
            }
        }
    }
}
