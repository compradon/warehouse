// Licensed to the Compradon Inc. under one or more agreements.
// The Compradon Inc. licenses this file to you under the MIT license.

using System;
using Compradon.Warehouse;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Contains extension methods to <see cref="IServiceCollection"/> for configuring warehouse services.
/// </summary>
public static class ServiceCollection
{
    /// <summary>
    /// Adds and configures the warehouse system for the specified entity types which uses a GUID as a primary key.
    /// </summary>
    /// <param name="services">The services available in the application.</param>
    public static WarehouseBuilder AddWarehouse(this IServiceCollection services)
        => services.AddWarehouse(null);

    /// <summary>
    /// Adds and configures the warehouse system for the specified entity types which uses a GUID as a primary key.
    /// </summary>
    /// <param name="services">The services available in the application.</param>
    /// <param name="setupAction">An action to configure the <see cref="WarehouseOptions"/>.</param>
    public static WarehouseBuilder AddWarehouse(this IServiceCollection services, Action<WarehouseOptions> setupAction)
    {
        services.AddOptions().AddLogging();

        // Services used by warehouse

        // Warehouse services
        services.TryAddScoped<WarehouseErrorDescriber>();
        services.TryAddScoped<DictionaryManager>();
        services.TryAddScoped<WarehouseManager>();

        if (setupAction != null)
        {
            services.Configure(setupAction);
        }

        return new WarehouseBuilder(typeof(Guid), services);
    }


    /// <summary>
    /// Adds and configures the warehouse system.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the entity.</typeparam>
    public static WarehouseBuilder AddWarehouse<TKey>(this IServiceCollection services)
        where TKey : IEquatable<TKey>
    {
        services.AddOptions().AddLogging();

        // Services used by warehouse system
        services.TryAddScoped<WarehouseManager<WarehouseEntity<TKey>, TKey>>();

        return new WarehouseBuilder(typeof(TKey), services);
    }
}
