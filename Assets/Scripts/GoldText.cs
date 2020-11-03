using TMPro;
using UnityEngine;
using DG.Tweening;
namespace TestFarm
{
    public class GoldText : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public Vector3 offset;
        private void Start()
        {
            transform.localPosition = offset;
        }
        /// <summary>
        /// Show additional amount of gold after sell.buy operation
        /// </summary> 
        public void ShowRedText(int amount)
        {
            transform.DOMoveY(transform.position.y + 25, 1f).OnComplete(delegate { Destroy(this.gameObject); });
            text.text = amount.ToString();
            text.color = Color.red;
        }
        /// <summary>
        /// Show additional amount of gold after sell.buy operation
        /// </summary>
        public void ShowGreenText(int amount)
        {
            transform.DOMoveY(transform.position.y + 25, 1f).OnComplete(delegate { Destroy(this.gameObject); });
            text.text = "+" + amount.ToString();
            text.color = Color.green;
        }
    }
}