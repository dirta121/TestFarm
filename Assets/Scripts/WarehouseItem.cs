using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace TestFarm
{
    public class WarehouseItem : MonoBehaviour, IInventoriable
    {
        public Image image;
        public TextMeshProUGUI sellPriceText;
        public TextMeshProUGUI countText;
        public Button sellButton;
        private string _name;
        private int _count;
        public GameObject GameObject { get { return gameObject; } }

        public void SetAction(Action<string> action, string name)
        {
            sellButton.onClick.AddListener(delegate { action.Invoke(name); });
            sellButton.onClick.AddListener(delegate { SoundController.instance.PlayCoinsClip(); });
        }
        public void SetContent(string name)
        {
            Goods goods = SoFabricMethod.instance.GetGoodsByName(name);
            _name = name;
            image.sprite = goods.sprite;
            sellPriceText.text = goods.sellPrice.ToString();
        }
        public string GetName()
        {
            return _name;
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