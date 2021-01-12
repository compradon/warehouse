using System;
using System.Data;
using Xunit;

namespace Compradon.Warehouse.Database.Tests
{
    public class DatabaseQueryBuilderTest
    {
        public IDbConnection Connection { get; }

        public DatabaseQueryBuilderTest()
        {
            var connector = new FakeConnector("connection string");

            Connection = connector.CreateConnectionAsync(false).GetAwaiter().GetResult();
        }

        [Fact]
        public void Constructor_WithNullConnection_ThrowsArgumentNullException()
        {
            Action actual = () => new DatabaseQueryBuilder(null, "SQL");

            Assert.Throws<ArgumentNullException>(actual);
        }

        [Fact]
        public void Constructor_WithNullCommandText_ThrowsArgumentNullException()
        {
            Action actual = () => new DatabaseQueryBuilder(Connection, null);

            Assert.Throws<ArgumentNullException>(actual);
        }

        [Fact]
        public void Constrcuctor_CommandText_SetsCorrectValue()
        {
            var expected = "SQL";
            var queryBuilder = new DatabaseQueryBuilder(Connection, expected);
            var actual = queryBuilder.Command.CommandText;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(CommandType.StoredProcedure)]
        [InlineData(CommandType.TableDirect)]
        [InlineData(CommandType.Text)]
        public void Constrcuctor_CommandType_SetsCorrectValue(CommandType commandType)
        {
            var queryBuilder = new DatabaseQueryBuilder(Connection, "SQL", commandType);

            var expected = commandType;
            var actual = queryBuilder.Command.CommandType;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddParameter_ReturnsSameInstance()
        {
            var expected = new DatabaseQueryBuilder(Connection, "SQL");
            var actual = expected.AddParameter("p1");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void AddParameter_WithNullName_ThrowsArgumentNullException()
        {
            var queryBuilder = new DatabaseQueryBuilder(Connection, "SQL");
            Action actual = () => queryBuilder.AddParameter(null);

            Assert.Throws<ArgumentNullException>(actual);
        }

        [Fact]
        public void AddParameter_AddsParameterToCommand()
        {
            var expected_parameterName = "p1";
            var expected_parameterValue = "value";
            var expected_parameterType = DbType.String;
            var expected_parameterSize = 1;
            var expected_parameterDirection = ParameterDirection.Input;

            var queryBuilder = new DatabaseQueryBuilder(Connection, "SQL").AddParameter(
                expected_parameterName,
                expected_parameterValue,
                expected_parameterType,
                expected_parameterSize,
                expected_parameterDirection
            );

            var actual = queryBuilder.Command.Parameters[expected_parameterName];

            Assert.NotNull(actual);

            Assert.Equal(expected_parameterName, actual.ParameterName);
            Assert.Equal(expected_parameterValue, actual.Value);
            Assert.Equal(expected_parameterType, actual.DbType);
            Assert.Equal(expected_parameterSize, actual.Size);
            Assert.Equal(expected_parameterDirection, actual.Direction);
        }

        [Fact]
        public void Parameters_AddsParametersToCommand()
        {
            var expected_parameterName = "p1";
            var expected_parameterValue = "value";

            var queryBuilder = new DatabaseQueryBuilder(Connection, "SQL").Parameters(new {
                p1 = expected_parameterValue
            });

            var actual = queryBuilder.Command.Parameters[expected_parameterName];

            Assert.NotNull(actual);

            Assert.Equal(expected_parameterName, actual.ParameterName);
            Assert.Equal(expected_parameterValue, actual.Value);
        }

        [Fact]
        public void Timeout_ReturnsSameInstance()
        {
            var expected = new DatabaseQueryBuilder(Connection, "SQL");
            var actual = expected.Timeout(30);

            Assert.Same(expected, actual);
        }
    }
}
