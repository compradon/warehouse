using System;
using System.Collections.Generic;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Represents the result with value of a warehouse operation.
    /// </summary>
    /// <typeparam name="TValue">The type encapsulating a result.</typeparam>
    public class WarehouseResult<TValue> : WarehouseResult
    {
        #region Properties

        /// <summary>
        /// Gets value of result that occurred during the warehouse operation.
        /// </summary>
        /// <value>The value of result.</value>
        public TValue Value { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="WarehouseResult"/> indicating a successful warehouse operation.
        /// </summary>
        /// <param name="value">A value of result.</param>
        public WarehouseResult(TValue value)
            : base(true)
        {
            Value = value;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="WarehouseResult"/> indicating a failed warehouse operation.
        /// </summary>
        /// <param name="exception">An <see cref="Exception"/> which caused the operation to fail for this result.</param>
        public WarehouseResult(Exception exception)
            : base(exception)
        {

        }

        #endregion
    }

    /// <summary>
    /// Represents the result of a warehouse operation.
    /// </summary>
    public class WarehouseResult
    {
        #region Static

        /// <summary>
        /// Returns an <see cref="WarehouseResult"/> indicating a successful warehouse operation.
        /// </summary>
        /// <returns>An <see cref="WarehouseResult"/> indicating a successful operation.</returns>
        public static WarehouseResult Success => _success;
        
        private static readonly WarehouseResult _success = new WarehouseResult(true);

        #endregion

        #region Properties

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="WarehouseError"/>s containing an errors that occurred during the warehouse operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="WarehouseError"/>s.</value>
        public IEnumerable<WarehouseError> Errors { get; }

        /// <summary>
        /// An <see cref="Exception"/> that occurred during the warehouse operation.
        /// </summary>
        /// <value>An <see cref="Exception"/>.</value>
        public Exception Exception { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="WarehouseResult"/> indicating the result of the warehouse operation.
        /// </summary>
        /// <param name="successed">Flag indicating whether if the operation succeeded or not.</param>
        internal WarehouseResult(bool successed)
        {
            Succeeded = successed;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="WarehouseResult"/> indicating a failed warehouse operation.
        /// </summary>
        /// <param name="exception">An <see cref="Exception"/> which caused the operation to fail for this result.</param>
        public WarehouseResult(Exception exception)
            : this(false)
        {
            Errors = new WarehouseError[1] { new WarehouseError(string.Empty, exception.Message) };
            Exception = exception;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="WarehouseResult"/> indicating a failed warehouse operation.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="WarehouseError"/>s which caused the operation to fail for this result.</param>
        public WarehouseResult(IEnumerable<WarehouseError> errors)
            : this(false)
        {
            Errors = errors;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates an <see cref="WarehouseResult"/> indicating a failed warehouse operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="WarehouseError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="WarehouseResult"/> indicating a failed warehouse operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static WarehouseResult Failed(params WarehouseError[] errors)
        {
            return new WarehouseResult(errors);
        }

        /// <summary>
        /// Creates an <see cref="WarehouseResult"/> indicating a failed warehouse operation, with a list of <paramref name="exception"/> if applicable.
        /// </summary>
        /// <param name="exception">An <see cref="Exception"/> which caused the operation to fail.</param>
        /// <returns>An <see cref="WarehouseResult"/> indicating a failed warehouse operation, with a <paramref name="exception"/> if applicable.</returns>
        public static WarehouseResult Failed(Exception exception)
        {
            return new WarehouseResult(exception);
        }

        #endregion
    }
}
