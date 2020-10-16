namespace Compradon.Warehouse
{
    /// <summary>
    /// Encapsulates an error from the warehouse subsystem.
    /// </summary>
    public class WarehouseError
    {
        #region Properties

        /// <summary>
        /// Gets the code for this error.
        /// </summary>
        /// <value>The code for this error.</value>
        public string Code { get; }

        /// <summary>
        /// Gets the description for this error.
        /// </summary>
        /// <value>The description for this error.</value>
        public string Description { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="WarehouseError"/>.
        /// </summary>
        /// <param name="description">The description for this error.</param>
        public WarehouseError(string description)
        {
            Description = description;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="WarehouseError"/>.
        /// </summary>
        /// <param name="code">The code for this error.</param>
        /// <param name="description">The description for this error.</param>
        public WarehouseError(string code, string description) : this (description)
        {
            Code = code;
        }

        #endregion
    }
}
