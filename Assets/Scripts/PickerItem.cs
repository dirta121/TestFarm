using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
namespace TestFarm
{
    public class PickerItem : MonoBehaviour, IInventoriable
    {
        public Image image;
        public Button pickButton;
        public TextMeshProUGUI countText;
        private string _name;
        private int _count;
        public GameObject GameObject { get { return gameObject; } }
        public void SetAction(Action<string> action, string name)
        {
            pickButton.onClick.AddListener(delegate { action.Invoke(name); });

            Goods goods;
            if (SoFabricMethod.instance.TryGetGoodsByName(name, out goods))
            {
                pickButton.onClick.AddListener(delegate { SoundController.instance.PlayCurrentClip(goods.audioClips); });
                pickButton.onClick.AddListener(delegate { UIController.instance.ShowPicker(false); });
            }
        }
        public void SetContent(string name)
        {
            Goods goods = SoFabricMethod.instance.GetGoodsByName(name);
            _name = name;
            image.sprite = goods.sprite;
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