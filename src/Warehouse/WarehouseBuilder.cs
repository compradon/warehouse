// Licensed to the Compradon Inc. under one or more agreements.
// The Compradon Inc. licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Compradon.Warehouse;

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

    #region Methods

    /// <summary>
    /// Adds an <see cref="WarehouseErrorDescriber"/>.
    /// </summary>
    /// <typeparam name="TDescriber">The type of the error describer.</typeparam>
    /// <returns>The current <see cref="WarehouseBuilder"/> instance.</returns>
    public virtual WarehouseBuilder AddErrorDescriber<TDescriber>() where TDescriber : WarehouseErrorDescriber
        => AddScoped(typeof(WarehouseErrorDescriber), typeof(TDescriber));

    #endregion

    #region Helpers

    private WarehouseBuilder AddScoped(Type serviceType, Type concreteType)
    {
        Services.AddScoped(serviceType, concreteType);

        return this;
    }

    #endregion
}
