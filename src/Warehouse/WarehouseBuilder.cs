using System;
using Microsoft.Extensions.DependencyInjection;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Helper functions for configuring warehouse services.
    /// </summary>
    public class WarehouseBuilder
    {
        #region Properties

        /// <summary>
        /// Gets the <see cref="Type"/> used for the primary key for the entity.
        /// </summary>
        public Type Key { get; private set; }

        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> services are attached to.
        /// </summary>
        /// <value>
        /// The <see cref="IServiceCollection"/> services are attached to.
        /// </value>
        public IServiceCollection Services { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="WarehouseBuilder"/>.
        /// </summary>
        /// <param name="key">The <see cref="Type"/> used for the primary key for the entity.</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to attach to.</param>
        public WarehouseBuilder(Type key, IServiceCollection services)
        {
            Key = key;
            Services = services;
        }

        #endregion
    }
}
