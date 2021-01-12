using System;
using System.Data;
using System.Data.Common;

namespace Compradon.Warehouse.Database.Tests
{
    public class FakeConnection : DbConnection
    {
        private ConnectionState _state;
        public override string ConnectionString { get; set; }
        public override string Database => throw new NotImplementedException();
        public override string DataSource => throw new NotImplementedException();
        public override string ServerVersion => throw new NotImplementedException();
        public override ConnectionState State => _state;

        public FakeConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            _state = ConnectionState.Closed;
        }

        public override void Open()
        {
            _state = ConnectionState.Open;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand CreateDbCommand()
        {
            return new FakeCommand();
        }
    }
}
