using System.Data;

namespace Compradon.Warehouse.Database
{
    /// <summary>
    /// Extensions for <see cref="IDataReader"/>.
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Creates and binds an instance from the <see cref="IDataReader"/>.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        public static T Build<T>(this IDataReader dataReader)
        {
            return default(T);
        }
    }
}
