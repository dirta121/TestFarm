using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
namespace TestFarm
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        public SceneEventInt onSceneStartLoading;
        public SceneEventFloat onSceneLoading;
        public SceneEventInt onSceneLoaded;
        public void LoadScene(int sceneIndex)
        {
            StartCoroutine(LoadAsyncCoroutine(sceneIndex));
        }
        IEnumerator LoadAsyncCoroutine(int sceneIndex)
        {
            onSceneStartLoading?.Invoke(sceneIndex);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            operation.allowSceneActivation = false;
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                onSceneLoading?.Invoke(progress);
                if (operation.progress >= 0.9f)
                {
                    yield return new WaitForSeconds(0.5f);
                    operation.allowSceneActivation = true;
                }
                yield return null;
            }
            onSceneLoaded?.Invoke(sceneIndex);
        }
        [Serializable]
        public class SceneEventFloat : UnityEvent<float>
        {
        }
        [Serializable]
        public class SceneEventInt : UnityEvent<int>
        {
        }
    }
}