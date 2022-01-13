using System.Data;
using System.Data.Common;

namespace Compradon.Warehouse.Database.Tests
{
    public class FakeCommand : DbCommand
    {
        public override string CommandText { get; set; }

        public override int CommandTimeout { get; set; }

        public override CommandType CommandType { get; set; }

        public override bool DesignTimeVisible { get; set; }

        public override UpdateRowSource UpdatedRowSource { get; set; }

        protected override DbConnection DbConnection { get; set; }

        protected override DbParameterCollection DbParameterCollection { get; } = new FakeParameterCollection();

        protected override DbTransaction DbTransaction { get; set; }

        public override void Cancel()
        {
            throw new System.NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            return 0;
        }

        public override object ExecuteScalar()
        {
            return "value";
        }

        public override void Prepare()
        {
            throw new System.NotImplementedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            return new FakeParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            throw new System.NotImplementedException();
        }
    }
}
