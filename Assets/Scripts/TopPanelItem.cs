using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace TestFarm
{
    public class TopPanelItem : MonoBehaviour, IInventoriable
    {
        public TextMeshProUGUI countText;
        public Image image;
        public GameObject GameObject { get { return gameObject; } }
        private int _count;
        private string _name;
        public int GetCount()
        {
            return _count;
        }
        public string GetName()
        {
            return _name;
        }
        public void SetAction(Action<string> action, string name)
        {
            //
        }
        public void SetContent(string name)
        {
            Goods goods = SoFabricMethod.instance.GetGoodsByName(name);
            image.sprite = goods.sprite;
            _name = name;
            countText.text = _count.ToString();
        }
        public void SetCount(int count)
        {
            _count = count;
            countText.text = count.ToString();
        }
    }
}