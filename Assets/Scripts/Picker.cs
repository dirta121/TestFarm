using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
namespace TestFarm
{
    public class Picker : Inventory<IInventoriable>
    {
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
        }
        public new void Add(string name)
        {
            base.Add(name);
        }
    }

}