using System;
using System.Collections.Generic;
using UnityEngine;
namespace TestFarm
{
    [Serializable]
    public class Inventory<T> : MonoBehaviour where T : IInventoriable
    {
        public GameObject itemPrefab;
        protected Transform content;
        private Action<string> _action;

        protected Dictionary<string, T> _itemsDict = new Dictionary<string, T>();
        private T InstantiateItem()
        {
            GameObject go = (GameObject)Instantiate(itemPrefab, content);
            return go.GetComponent<T>();
        }
        private void DestroyItem(T item)
        {
            Destroy(item.GameObject);
        }
        protected void SetAction(Action<string> action)
        {
            _action = action;
        }
        protected void Add(string name)
        {
            T item;
            if (TryGetItem(name, out item))
            {
                item.SetCount(item.GetCount() + 1);
            }
            else
            {
                item = InstantiateItem();
                AddItem(name, item);
                item.SetCount(1);
                SetContent(item, name);
            }
        }
        protected bool Remove(string name)
        {
            T item;
            if (TryGetItem(name, out item))
            {
                var count = item.GetCount();
                count -= 1;
                if (count == 0)
                {
                    RemoveItem(name);
                    DestroyItem(item);
                    return true;
                }
                else if (count < 0)
                {
                    RemoveItem(name);
                    DestroyItem(item);
                    return false;
                }
                else
                {
                    item.SetCount(count);
                    return true;
                }
            }
            return false;
        }
        protected virtual void SetContent(T item, string name)
        {
            item.SetAction(_action, name);
            item.SetContent(name);
        }

        private void AddItem(string name, T item)
        {
            if (!_itemsDict.ContainsKey(name))
            {
                _itemsDict.Add(name, item);
            }
        }
        private void RemoveItem(string name)
        {
            if (_itemsDict.ContainsKey(name))
            {
                _itemsDict.Remove(name);
            }
        }
        protected bool ContainsKey(string key)
        {
            if (_itemsDict.ContainsKey(key))
            {
                return true;
            }
            else { return false; }
        }
        protected bool TryGetItem(string key, out T t)
        {
            if (_itemsDict.TryGetValue(key, out t))
            {
                return true;
            }
            return false;
        }
    }
}