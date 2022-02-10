// Licensed to the Compradon Inc. under one or more agreements.
// The Compradon Inc. licenses this file to you under the MIT license.

using System.Data;
using System.Text;
using System.Text.Json;

namespace Compradon.Warehouse.Database;

/// <summary>
/// Extensions for <see cref="IDataReader"/>.
/// </summary>
public static class DataReaderExtensions
{
    /// <summary>
    /// Reads the string value from the <see cref="IDataReader"/>.
    /// </summary>
    public static string ReadString(this IDataReader dataReader)
    {
        var builder = new StringBuilder();
        while (dataReader.Read()) builder.Append(dataReader.GetString(0));

        return builder.ToString();
    }

    /// <summary>
    /// Creates and binds an instance from the <see cref="IDataReader"/>.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    public static T Build<T>(this IDataReader dataReader) where T : class
    {
        var value = dataReader.ReadString();
        if (value.Length == 0) return null;

        return JsonSerializer.Deserialize<T>(value);
    }
}
