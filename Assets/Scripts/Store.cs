using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.XR.WSA.Input;
namespace TestFarm
{
    public class Store : Inventory<IInventoriable>
    {
        public int refreshTime;
        private void Start()
        {
            StartCoroutine(RefreshStoreInventoryCoroutine());
        }
        IEnumerator RefreshStoreInventoryCoroutine()
        {
            while (true)
            {
                GenerateGoods();
                yield return new WaitForSeconds(refreshTime);
            }
        }
        public Goods[] hardcodeGoods;
        public void SetContainer(UnityEngine.Transform transform)
        {
            content = transform;
        }
        public void SetNewAction(Action<string> action)
        {
            base.SetAction(action);
        }
        private void GenerateGoods()
        {
            foreach (Goods goods in hardcodeGoods)
            {
                var count = UnityEngine.Random.Range(1, 20);
                for (int i = 0; i < count; i++)
                {
                    Add(goods.name);
                }
            }
        }
        public new bool Remove(string name)
        {
            return base.Remove(name);
        }
        public new void Add(string name)
        {
            base.Add(name);
        }
    }
}