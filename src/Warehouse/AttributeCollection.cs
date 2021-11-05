using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Compradon.Warehouse
{
    /// <summary>
    /// Represents a collection of attribute objects.
    /// </summary>
    public class AttributeCollection : ICollection<WarehouseAttribute>
    {
        private IList<WarehouseAttribute> list = new List<WarehouseAttribute>();

        /// <summary>
        /// 
        /// </summary>
        public int Count => list.Count;

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly => list.IsReadOnly;

        /// <summary>
        /// 
        /// </summary>
        public WarehouseAttribute this[string alias]
        {
            get
            {
                var item = list.First(i => i.Alias == alias);
                if (item == null) throw new ArgumentOutOfRangeException(nameof(alias));
                return item;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Add(WarehouseAttribute item)
        {
            if (list.Any(a => a.Alias == item.Alias)) throw new AggregateException(nameof(item));

            // TODO: Пометить данный экземпляр, как тот, который требуется создать в базе данных

            list.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Add(string name, string alias, AttributeTypes attributeType, string summary = null)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (alias == null) throw new ArgumentNullException(nameof(alias));

            Add(new WarehouseAttribute() {
                Alias = alias,
                AttributeType = attributeType,
                Name = name,
                Summary = summary
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear() => list.Clear();

        /// <summary>
        /// 
        /// </summary>
        public bool Contains(WarehouseAttribute item) => list.Contains(item);

        /// <summary>
        /// 
        /// </summary>
        public void CopyTo(WarehouseAttribute[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        /// <summary>
        /// 
        /// </summary>
        public IEnumerator<WarehouseAttribute> GetEnumerator() => list.GetEnumerator();

        /// <summary>
        /// 
        /// </summary>
        public bool Remove(WarehouseAttribute item)
        {
            return list.Remove(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Remove(string alias)
        {
            var item = list.First(a => a.Alias == alias);
            return Remove(item);
        }

        /// <summary>
        /// 
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
    }
}
