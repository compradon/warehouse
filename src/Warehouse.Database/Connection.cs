// Licensed to the Compradon Inc. under one or more agreements.
// The Compradon Inc. licenses this file to you under the MIT license.

using System.Data;

namespace Compradon.Warehouse.Database;

/// <summary>
/// Extensions for <see cref="IDbConnection"/>.
/// </summary>
public static class ConnectionExtensions
{
    /// <summary>
    /// Creates an instance of the <see cref="DatabaseQueryBuilder"/> class for building and executing database queries.
    /// </summary>
    public static DatabaseQueryBuilder Query(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text)
        => new DatabaseQueryBuilder(connection, commandText, commandType);

    /// <summary>
    /// Creates an instance of the <see cref="DatabaseQueryBuilder"/> class for building and executing database stored procedures.
    /// </summary>
    public static DatabaseQueryBuilder StoredProcedure(this IDbConnection connection, string commandText)
        => new DatabaseQueryBuilder(connection, commandText, CommandType.StoredProcedure);
}
