namespace Compradon.Warehouse
{
    /// <summary>
    /// Service to enable localization for application facing warehouse errors.
    /// </summary>
    /// <remarks>
    /// These errors are returned to controllers and are generally used as display messages to end users.
    /// </remarks>
    public class WarehouseErrorDescriber
    {
        /// <summary>
        /// Returns the default <see cref="WarehouseError"/>.
        /// </summary>
        /// <returns>The default <see cref="WarehouseError"/>.</returns>
        public virtual WarehouseError DefaultError()
        {
            return new WarehouseError(nameof(DefaultError), nameof(DefaultError));
        }
    }
}
