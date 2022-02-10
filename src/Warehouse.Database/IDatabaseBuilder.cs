// Licensed to the Compradon Inc. under one or more agreements.
// The Compradon Inc. licenses this file to you under the MIT license.

using System.Threading.Tasks;

namespace Compradon.Warehouse.Database;

/// <summary>
/// Provides an abstraction for building a warehouse database schema.
/// </summary>
public interface IDatabaseBuilder
{
    /// <summary>
    /// Creates the warehouse database schema.
    /// </summary>
    Task<WarehouseResult> BuildAsync();

    /// <summary>
    /// Drops the warehouse database schema.
    /// </summary>
    Task<WarehouseResult> ClearAsync();
}
