using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
namespace TestFarm
{
    public class Warehouse : Inventory<IInventoriable>
    {
        public WarehouseEvent onWarehouseItemAdded = new WarehouseEvent();
        public WarehouseEvent onWarehouseItemRemoved = new WarehouseEvent();
        private static Dictionary<string, IInventoriable> _items;
        private void Start()
        {
            _items = _itemsDict;
        }
        public static List<IInventoriable> GetItems()
        {
            return _items.Values.ToList();
        }
        public void SetContainer(UnityEngine.Transform transform)
        {
            content = transform;
        }
        public void SetNewAction(Action<string> action)
        {
            base.SetAction(action);
        }
        public new void Remove(string name)
        {
            base.Remove(name);
            onWarehouseItemRemoved?.Invoke(name);
        }
        public bool TryRemove(string name)
        {
            onWarehouseItemRemoved?.Invoke(name);
            return base.Remove(name);
        }
        public new void Add(string name)
        {
            base.Add(name);
            onWarehouseItemAdded?.Invoke(name);
        }
        [Serializable]
        public class WarehouseEvent : UnityEvent<string>
        {
        }
    }
}


