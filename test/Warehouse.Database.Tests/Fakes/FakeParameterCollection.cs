using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Compradon.Warehouse.Database.Tests
{
    public class FakeParameterCollection : DbParameterCollection
    {
        private List<DbParameter> _parameters = new List<DbParameter>();
        public override int Count => _parameters.Count;
        public override object SyncRoot => throw new NotImplementedException();

        public override int Add(object value)
        {
            _parameters.Add(value as DbParameter);

            return _parameters.IndexOf(value as DbParameter);
        }

        public override void AddRange(Array values)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(string value)
        {
            throw new NotImplementedException();
        }

        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public override int IndexOf(string parameterName)
        {
            var parameter = _parameters.FirstOrDefault(r => r.ParameterName == parameterName);

            return _parameters.IndexOf(parameter);
        }

        public override void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public override void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(string parameterName)
        {
            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(int index)
        {
            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return _parameters.FirstOrDefault(r => r.ParameterName == parameterName);
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            throw new NotImplementedException();
        }
    }
}
