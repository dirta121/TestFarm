using System;
using System.Collections.Generic;
using UnityEngine;
namespace TestFarm
{
    public class Pooler : Singleton<Pooler>
    {
        [Serializable]
        public class Pool
        {
            public string tag;
            public List<GameObject> prefabs;
            public int size;
        }

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDict;
        void Awake()
        {
            poolDict = new Dictionary<string, Queue<GameObject>>();
            var random = new System.Random();
            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    var obj = Instantiate(pool.prefabs[random.Next(0, pool.prefabs.Count)], transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDict.Add(pool.tag.ToLower(), objectPool);
            }
        }
        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            var key = tag.ToLower();

            Queue<GameObject> queue;
            if (poolDict.TryGetValue(key, out queue))
            {
                GameObject objectToSpawn = poolDict[key].Dequeue();
                objectToSpawn.SetActive(true);
                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = rotation;
                poolDict[key].Enqueue(objectToSpawn);

                IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();
                if (pooledObject != null)
                    pooledObject.OnObjectSpawn();

                return objectToSpawn;
            }
            return null;
        }
        public GameObject SpawnFromPool(string tag, Transform parent, Vector3 position, Quaternion rotation)
        {
            var key = tag.ToLower();
            Queue<GameObject> queue;
            if (poolDict.TryGetValue(key, out queue))
            {
                GameObject objectToSpawn = poolDict[key].Dequeue();
                objectToSpawn.SetActive(true);

                objectToSpawn.transform.localPosition = position;
                objectToSpawn.transform.localRotation = rotation;

                objectToSpawn.transform.SetParent(parent, false);
                objectToSpawn.transform.SetAsLastSibling();

                poolDict[key].Enqueue(objectToSpawn);

                IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();
                if (pooledObject != null)
                    pooledObject.OnObjectSpawn();

                return objectToSpawn;
            }
            return null;
        }
    }
}