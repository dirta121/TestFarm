using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace TestFarm
{
    [RequireComponent(typeof(Animator))]
    public class FarmSubject : MonoBehaviour, IPooledObject
    {
        public string[] idleTriggers;
        public Vector3Int cell;
        public new string name;
        public Progressbar progressBar;
        public static ProductsEvent<Goods[], FarmSubject> onProductsReady = new ProductsEvent<Goods[], FarmSubject>();
        public static ProductsEvent<Goods[], FarmSubject> onFoodNeeded = new ProductsEvent<Goods[], FarmSubject>();
        public static ProductsEvent<Goods[], FarmSubject> onSubjectDied = new ProductsEvent<Goods[], FarmSubject>();
        private Animator _animator;
        private int _secondsToWaitIdle;
        private Coroutine _idleCoroutine;
        private Coroutine _produceCoroutine;
        private Coroutine _waitForFoodsCoroutine;
        private Goods _goods;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _goods = SoFabricMethod.instance.GetGoodsByName(name);
        }
        public void OnObjectSpawn()
        {
            Init();
        }
        private void Init()
        {
            Idle();
            if (_goods.isAnimal)
            {
                WaitForFood();
            }
            else { Eat(); }
        }
        private void Idle()
        {
            progressBar.gameObject.SetActive(false);
            _animator.SetTrigger("Idle");
            if (_idleCoroutine != null)
            {
                StopCoroutine(_idleCoroutine);
            }
            _idleCoroutine = StartCoroutine(IdleCoroutine());
        }
        private void Eat()
        {
            _animator.SetTrigger("Eat");
            if (_waitForFoodsCoroutine != null)
            {
                StopCoroutine(_waitForFoodsCoroutine);
            }
            if (_produceCoroutine != null)
            {
                StopCoroutine(_produceCoroutine);
            }
            _produceCoroutine = StartCoroutine(ProduceCoroutine());
        }
        private float _produceTime = 0;
        IEnumerator ProduceCoroutine()
        {
            progressBar.gameObject.SetActive(true);
            _produceTimes = _goods.produceTimes;

            do
            {
                while (_produceTime < _goods.productionTime)
                {
                    _produceTime += Time.deltaTime;
                    progressBar.ShowPorgress(_produceTime / _goods.productionTime);

                    yield return null;
                }
                _produceTime = 0;
                onProductsReady?.Invoke(_goods.products, this);
                _produceTimes--;
            } while (_produceTimes > 0);
            if (_goods.isAnimal)
            {
                WaitForFood();
            }
            else
            {
                onSubjectDied?.Invoke(_goods.products, this);
            }
        }
        private int _produceTimes;
        private void WaitForFood()
        {
            Idle();
            if (_waitForFoodsCoroutine != null)
            {
                StopCoroutine(_waitForFoodsCoroutine);
            }
            _waitForFoodsCoroutine = StartCoroutine(WaitForFoodCoroutine());
        }
        IEnumerator WaitForFoodCoroutine()
        {
            while (true)
            {
                onFoodNeeded?.Invoke(_goods.food, this);
                yield return new WaitForSeconds(5);
            }
        }
        public void Feed()
        {
            Eat();
        }
        public void Die()
        {
            gameObject.SetActive(false);
            _animator.SetTrigger("Idle");
            StopAllCoroutines();
        }
        IEnumerator IdleCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_secondsToWaitIdle);
                _secondsToWaitIdle = GetRandomTimeToWait(20);
                _animator.SetTrigger(GetRandomTrigger(idleTriggers));
            }
        }
        /// <summary>
        /// Get random time in [1; maxTime] between  idle animations
        /// </summary>
        /// <param name="maxTime"></param>
        /// <returns></returns>
        private int GetRandomTimeToWait(int maxTime)
        {
            return Random.Range(1, maxTime >= 1 ? maxTime : 1);
        }
        /// <summary>
        /// Get random item from array
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        private string GetRandomTrigger(string[] names)
        {
            return names[Random.Range(0, names.Length)];
        }
        public class ProductsEvent<T, V> : UnityEvent<T, V>
        {
        }
    }
}