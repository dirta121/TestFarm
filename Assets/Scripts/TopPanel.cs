using UnityEngine;
using TMPro;
namespace TestFarm
{
    public class TopPanel : Inventory<IInventoriable>
    {
        [Tooltip("Prefab for +-gold amount after sell/buy operation")]
        public GameObject goldTextPrefab;
        private TextMeshProUGUI _goldText;
        private int _gold;
        private void Awake()
        {
            _goldText = UIController.instance.gold.GetComponentInChildren<TextMeshProUGUI>();
        }
        public void SetContainer(UnityEngine.Transform transform)
        {
            content = transform;
        }
        public void OnGoldChange(int gold)
        {
            _goldText.text = gold.ToString();
            var changedAmountText = Instantiate(goldTextPrefab, _goldText.transform);
            if (_gold > gold)
            {
                changedAmountText.GetComponent<GoldText>()?.ShowRedText(gold - _gold);
            }
            else
            {
                changedAmountText.GetComponent<GoldText>()?.ShowGreenText(gold - _gold);
            }
            _gold = gold;
        }
        public Vector3 GetItemPosition(string name)
        {
            IInventoriable value;
            if (_itemsDict.TryGetValue(name, out value))
            {
                return value.GameObject.transform.position;
            }
            else
            {
                return transform.position;
            }
        }
        public new void Add(string name)
        {
                base.Add(name);
        }
        public new void Remove(string name)
        {
            base.Remove(name);
        }
    }
}