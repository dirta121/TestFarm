using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System;
using TestFarm.SaveData;
namespace TestFarm
{
    public class SaveController : Singleton<SaveController>
    {
        private JsonSerializer _serializer;
        private string _savePath;
        private void Start()
        {
            _serializer = new JsonSerializer();
            _savePath = Application.dataPath + "/Resources/Saves/";
        }
        public SavePlayerData? LoadData()
        {
            SavePlayerData? data = ReadFromFile();
            return data;
        }
        private SaveWarehouseData GetSaveDataFromWarehouse(List<IInventoriable> items)
        {
            SaveWarehouseItem[] warehouseItems = new SaveWarehouseItem[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                string name = items[i].GetName();
                int count = items[i].GetCount();
                SaveWarehouseItem item = new SaveWarehouseItem(name, count);
                warehouseItems[i] = item;
            }
            SaveWarehouseData warehouseData = new SaveWarehouseData(warehouseItems);
            return warehouseData;
        }
        private SaveFieldsData GetSaveDataFromFields()
        {
            var dict = Player.fieldsDict;
            var keys = dict.Keys.ToArray();
            SaveFieldItem[] items = new SaveFieldItem[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                var value = dict[key];
                var position = value.Item2.position;
                var rotation = value.Item2.rotation;
                items[i] = new SaveFieldItem(
                    value.Item1,
                    new int[] { key.x, key.y, key.z },//cell
                    new float[] { position.x, position.y, position.z },//transform position
                    new float[] { rotation.x, rotation.y, rotation.z, rotation.w });//transform position;
            }
            return new SaveFieldsData(items);
        }
        [ContextMenu("TESTSAVE")]
        public void Save()
        {
            SaveWarehouseData warehouseData = GetSaveDataFromWarehouse(Warehouse.GetItems());
            SaveFieldsData fields = GetSaveDataFromFields();
            int gold = Player.Gold;
            SavePlayerData saveData = new SavePlayerData(gold, warehouseData, fields);
            WriteToFile(saveData);
            Debug.Log("New save");
        }
        public void DeleteSaveGame()
        {
            string path;
            if (TryGetLatestSaveGamePath(out path))
            {
                File.Delete(path);
            }
            Debug.Log("Old save was deleted when u pressed the start button");
        }
        private void WriteToFile(SavePlayerData saveData)
        {
            string path;
            if (!TryGetLatestSaveGamePath(out path))
            {
                path = CreateNewSaveFilePath();
            }
            var file = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            using (StreamWriter stream = new StreamWriter(file))
            {
                _serializer.Serialize(stream, saveData);
            }
            file.Close();
        }
        private string CreateNewSaveFilePath()
        {
            string path = _savePath + $"AUTOSAVE{DateTime.Now:hh.mm.ss}.txt";
            return path;
        }
        private SavePlayerData? ReadFromFile()
        {
            SavePlayerData? data = null;
            string path;
            if (TryGetLatestSaveGamePath(out path))
            {
                using (StreamReader file = File.OpenText(path))
                {
                    data = (SavePlayerData)_serializer.Deserialize(file, typeof(SavePlayerData));
                }
            }
            return data;
        }
        private bool TryGetLatestSaveGamePath(out string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(_savePath);
            var files = directoryInfo.GetFiles().Where(w => w.Extension == ".txt").OrderByDescending(o => o.CreationTime);
            path = files.Count() > 0 ? files.First().FullName : "";
            if (path == "") return false;
            return true;
        }
    }
}


