using System;
using UnityEngine;
using UnityEngine.Events;
namespace TestFarm
{

    [CreateAssetMenu(menuName = "Farm/Subject", fileName = "New subject")]
    [Serializable]
    public class Goods : ScriptableObject
    {
        [Tooltip("Subject Name")]
        public string subjectName;
        [Tooltip("Buy price")]
        public int buyPrice;
        [Tooltip("Sell price")]
        public int sellPrice;
        [Tooltip("Production time in seconds")]
        public int productionTime;
        public Goods[] products;
        [Tooltip("Animal?")]
        public bool isAnimal;
        public Goods[] food;
        [Tooltip("How much can be feeded before asking for the food")]
        public int produceTimes;
        public AudioClip[] audioClips;
        public Sprite sprite;
        public bool canBePlaced;
    }
}