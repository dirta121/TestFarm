using TestFarm.SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
namespace TestFarm
{
    public class Player : MonoBehaviour
    {
        [Tooltip("Tilemap controller")]
        public TilemapController tilemapController;
        [Tooltip("Warehouse controller")]
        public Warehouse warehouse;
        [Tooltip("Store controller")]
        public Store store;
        [Tooltip("Picker controller")]
        public Picker picker;
        [Tooltip("Top panel controller")]
        public TopPanel topPanel;
        [Tooltip("Size of a tilemap")]
        public Vector3Int size;
        [Tooltip("Default amout of the gold")]
        public int startGold;
        public static int Gold { get; private set; }
        private int gold { get { return _gold; } set { _gold = value; Gold = value; onGoldChanged?.Invoke(value); } }
        private int _gold;
        private bool _drag;
        private Vector3Int? _currentSelected;
        private SaveController _saveController;
        // key - Goods.name value - goods count with the same name
        public static Dictionary<Vector3Int, (string, Transform)> fieldsDict;
        public GoldEvent onGoldChanged = new GoldEvent();
        private void Start()
        {
            fieldsDict = new Dictionary<Vector3Int, (string, Transform)>();
            _saveController = new SaveController();
            warehouse.SetContainer(UIController.instance.warehouseContent);
            store.SetContainer(UIController.instance.storeContent);
            topPanel.SetContainer(UIController.instance.topPanelContent);
            picker.SetContainer(UIController.instance.pickerContent);
            warehouse.SetNewAction(Sell);
            store.SetNewAction(Buy);
            picker.SetNewAction(Place);
            LoadGame();
        }
        /// <summary>
        /// Buy goods from store
        /// </summary>
        /// <param name="name"></param>
        public void Buy(string name)
        {
            Goods goods;
            if (SoFabricMethod.instance.TryGetGoodsByName(name, out goods))
            {
                var price = goods.buyPrice;
                if (_gold >= price)
                {
                    gold -= price;
                    warehouse.Add(name);
                    store.Remove(name);
                }
                else //Её [индексации] нигде нет, мы вообще не принимали, просто денег нет сейчас. Найдём деньги, сделаем индексацию. Вы держитесь здесь, вам всего доброго, хорошего настроения и здоровья!
                {

                }
            }
        }
        /// <summary>
        /// Sell goods from warehouse
        /// </summary>
        /// <param name="name"></param>
        public void Sell(string name)
        {
            Goods goods;
            if (SoFabricMethod.instance.TryGetGoodsByName(name, out goods))
            {
                if (warehouse.TryRemove(name))
                {
                    store.Add(name);
                    gold += goods.sellPrice;
                }
            }
        }
        /// <summary>
        /// Load game from the save file or default start default initialization
        /// </summary>
        private void LoadGame()
        {
            var data = SaveController.instance.LoadData();
            if (data.HasValue)
            {
                Init(data.Value);
            }
            else
            {
                Init();
            }
        }
        private void Init()
        {
            gold = startGold;
            tilemapController.onTileSelected.AddListener(OnTileSelected);
            tilemapController.onTileDragBegin.AddListener(OnTileDragBegin);
            tilemapController.onTileDragEnd.AddListener(OnTileDragEnd);
            tilemapController.SetSize(size);
            FarmSubject.onProductsReady.AddListener(OnProductsReady);
            FarmSubject.onFoodNeeded.AddListener(OnFoodRequest);
            FarmSubject.onSubjectDied.AddListener(OnSubjectDied);
        }
        private void Init(SavePlayerData data)
        {
            tilemapController.onTileSelected.AddListener(OnTileSelected);
            tilemapController.onTileDragBegin.AddListener(OnTileDragBegin);
            tilemapController.onTileDragEnd.AddListener(OnTileDragEnd);
            tilemapController.SetSize(size);
            FarmSubject.onProductsReady.AddListener(OnProductsReady);
            FarmSubject.onFoodNeeded.AddListener(OnFoodRequest);
            ParseData(data);
        }
        /// <summary>
        /// Parse data from save file
        /// </summary>
        /// <param name="data"></param>
        private void ParseData(SavePlayerData data)
        {
            gold = data.gold;
            SaveWarehouseData saveWarehouse = data.warehouse;
            foreach (var item in saveWarehouse.items)
            {
                var count = 0;
                while (count < item.count)
                {
                    count++;
                    warehouse.Add(item.name);
                }
            }
            SaveFieldsData savefields = data.fields;
            foreach (var item in savefields.items)
            {
                Vector3Int cell = new Vector3Int(item.cell[0], item.cell[1], item.cell[2]);
                var obj = SpawnOnTile(item.name, cell);
                obj.position = new Vector3(
                    item.position[0],
                    item.position[1],
                    item.position[2]);
                obj.rotation = new Quaternion(
                   item.rotation[0],
                   item.rotation[1],
                   item.rotation[2],
                   item.rotation[3]);
                fieldsDict.Add(
                    cell,
                    (item.name, obj));
            }
        }
        /// <summary>
        /// Place goods from warehouse to the selected field
        /// </summary>
        /// <param name="name"></param>
        private void Place(string name)
        {
            if (fieldsDict.ContainsKey(_currentSelected.Value))
            {
                _currentSelected = null;
                tilemapController.ClearSelection();
                return;
            }
            var obj = SpawnOnTile(name, _currentSelected.Value);
            fieldsDict.Add(_currentSelected.Value, (name, obj));
            warehouse.Remove(name);
            _currentSelected = null;
            SoundController.instance.PlayCurrentClip(SoFabricMethod.instance.GetGoodsByName(name).audioClips);
            tilemapController.ClearSelection();
        }
        /// <summary>
        /// Reverse operation to the Place
        /// </summary>
        /// <param name="cell"></param>
        public void Unplace(Vector3Int cell)
        {
            (string, Transform) tuple;
            if (fieldsDict.TryGetValue(cell, out tuple))
            {
                warehouse.Add(tuple.Item1);
                tuple.Item2.gameObject.SetActive(false);
                fieldsDict.Remove(cell);
            }
        }
        /// <summary>
        /// Take the instance of the prefab from the pool 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Transform SpawnOnTile(string name, Vector3Int cell)
        {
            var position = tilemapController.WorldToCellCenter(cell);
            var farmSubject = Pooler.instance.SpawnFromPool(name, position, Quaternion.identity);
            farmSubject.GetComponent<FarmSubject>().cell = cell;
            return farmSubject.transform;
        }
        /// <summary>
        /// Listener for the TileSelected Event
        /// </summary>
        /// <param name="cell"></param>
        private void OnTileSelected(Vector3Int cell)
        {
            if (fieldsDict.ContainsKey(cell))
            {
                _currentSelected = null;
                return;
            }
            _currentSelected = cell;
            UIController.instance.ShowPicker(true);
        }
        private void OnTileDragBegin(Vector3Int cell)
        {
            tilemapController.SetSelection(cell, Color.green);
            _drag = true;
            (string, Transform) tuple;
            if (fieldsDict.TryGetValue(cell, out tuple))
            {
                SoundController.instance.PlayCurrentClip(SoFabricMethod.instance.GetGoodsByName(fieldsDict[cell].Item1).audioClips);
                StartCoroutine(DragCoroutine(cell, tuple.Item2));
            }
        }
        private void OnTileDragEnd(Vector3Int cell)
        {
            tilemapController.ClearSelection();
            _drag = false;
        }
        /// <summary>
        /// Drag in pdate
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="dragable"></param>
        /// <returns></returns>
        IEnumerator DragCoroutine(Vector3Int cell, Transform dragable)
        {
            var startPosition = dragable.position;
            while (_drag)
            {
                yield return null;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                dragable.position = mousePos;
            }
            if (Vector3.Distance(startPosition, dragable.position) > 6)
            {
                Unplace(cell);
            }
            else
            {
                dragable.DOMove(startPosition, 1);
            }
        }
        private void OnProductsReady(Goods[] goods, FarmSubject farmSubject)
        {
            foreach (var item in goods)
            {
                warehouse.Add(item.name);
                Debug.Log($"Product is ready: {item.name} by {farmSubject.name}");

                Vector2 toCoords = Camera.main.ScreenToWorldPoint(topPanel.GetItemPosition(item.name));

                var obj = Pooler.instance.SpawnFromPool("Product", farmSubject.transform.position, Quaternion.identity);
                obj.GetComponent<SpriteRenderer>().sprite = item.sprite;

                obj.transform.DOMove(toCoords, 1).OnComplete(delegate { obj.SetActive(false); });
            }
        }
        private void OnFoodRequest(Goods[] goods, FarmSubject farmSubject)
        {
            Debug.Log("Request food from " + farmSubject.name);
            foreach (var item in goods)
            {
                if (warehouse.TryRemove(item.name))
                {
                    farmSubject.Feed();
                    break;
                }
            }
        }
        private void OnSubjectDied(Goods[] goods, FarmSubject farmSubject)
        {
            fieldsDict.Remove(farmSubject.cell);
            farmSubject.Die();
        }
        /// <summary>
        /// Autosave
        /// </summary>
        void OnApplicationQuit()
        {
            Debug.Log("SAVED");
            SaveController.instance.Save();
        }
        [Serializable]
        public class GoldEvent : UnityEvent<int>
        {
        }
    }
}

