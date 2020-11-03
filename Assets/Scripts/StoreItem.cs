using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
namespace TestFarm
{
    public class StoreItem : MonoBehaviour, IInventoriable
    {
        public Image image;
        public TextMeshProUGUI buyPriceText;
        public TextMeshProUGUI countText;
        public Button buyButton;
        private int _count;
        private string _name;
        public GameObject GameObject { get { return gameObject; } }

        public void SetAction(Action<string> action, string name)
        {
            buyButton.onClick.AddListener(delegate { action.Invoke(name); });
            buyButton.onClick.AddListener(delegate { SoundController.instance.PlayCoinsClip(); });
        }
        public string GetName()
        {
            return _name;
        }
        public void SetContent(string name)
        {
            Goods goods = SoFabricMethod.instance.GetGoodsByName(name);
            image.sprite = goods.sprite;
            _name = name;
            buyPriceText.text = goods.buyPrice.ToString();
        }
        public void SetCount(int count)
        {
            _count = count;
            countText.text = count.ToString();
        }
        public int GetCount()
        {
            return _count;
        }
    }
}