using UnityEngine;
namespace TestFarm
{
    public class GameController : Singleton<GameController>
    {
        [Tooltip("Scene <GameScene> index")]
        public int gameSceneIndex;
        [Tooltip("Scene <StartScene> index")]
        public int startSceneIndex;
        void Start()
        {
            SceneLoader.instance.onSceneStartLoading.AddListener(SceneStartLoading);
            SceneLoader.instance.onSceneLoaded.AddListener(SceneLoaded);
            SceneLoader.instance.LoadScene(startSceneIndex);
        }
        public void StartGame()
        {
            SaveController.instance.DeleteSaveGame();
            SceneLoader.instance.LoadScene(gameSceneIndex);
        }
        public void ContinueGame()
        {
            SceneLoader.instance.LoadScene(gameSceneIndex);
        }
        public void ExitGame()
        {
            Application.Quit();
        }
        /// <summary>
        /// Event listener for what scene is loading now
        /// </summary>
        /// <param name="sceneIndex"></param>
        private void SceneStartLoading(int sceneIndex)
        {
            if (sceneIndex == gameSceneIndex)
            {
                DisableStartSceneUI();
            }
            else if (sceneIndex == startSceneIndex)
            {
                DisableLoadingSceneUI();
            }
        }
        /// <summary>
        /// Event listener for what scene just loaded
        /// </summary>
        /// <param name="sceneIndex"></param>
        private void SceneLoaded(int sceneIndex)
        {
            if (sceneIndex == gameSceneIndex)
            {
                EnableGameSceneUI();
            }
            else if (sceneIndex == startSceneIndex)
            {
                EnableStartSceneUI();
            }
        }
        /// <summary>
        /// Enable UI for GameScene scene
        /// </summary>
        private void EnableGameSceneUI()
        {
            UIController.instance.ShowGold(true);
            UIController.instance.ShowTopPanel(true);
            UIController.instance.ShowMenuButton(true);
            UIController.instance.ShowMenuExitButton(true);
            UIController.instance.ShowStoreButton(true);
            UIController.instance.ShowWarehouseButton(true);
            UIController.instance.ShowBackground(false);
            UIController.instance.ShowLoadingScreen(false);
            UIController.instance.ShowMenuCloseButton(true);
        }
        /// <summary>
        /// Enable UI for StartScene scene
        /// </summary>
        private void EnableStartSceneUI()
        {
            UIController.instance.ShowBackground(true);
            UIController.instance.ShowMenu(true);
            UIController.instance.ShowMenuStartButton(true);
            UIController.instance.ShowMenuContinueButton(true);
            UIController.instance.ShowLoadingScreen(false);
        }
        /// <summary>
        /// Disable UI for GameScene scene
        /// </summary>
        private void DisableGameSceneUI()
        {
            //
        }
        /// <summary>
        /// Disable UI for StartScene scene
        /// </summary>
        private void DisableStartSceneUI()
        {
            UIController.instance.DisableAll();
            UIController.instance.ShowBackground(true);
            UIController.instance.ShowLoadingScreen(true);
        }
        /// <summary>
        /// Disable UI for LoadingScene scene
        /// </summary>
        private void DisableLoadingSceneUI()
        {
            UIController.instance.DisableAll();
            UIController.instance.ShowMenuCloseButton(false);
            UIController.instance.ShowBackground(true);
            UIController.instance.ShowLoadingScreen(true);
        }
    }
}
