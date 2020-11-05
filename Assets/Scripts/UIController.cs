using UnityEngine;
namespace TestFarm
{
    public class UIController : Singleton<UIController>
    {
        public GameObject menu;
        public GameObject menuButton;
        public GameObject menuCloseButton;
        public GameObject menuStartButton;
        public GameObject menuContinueButton;
        public GameObject menuExitButton;
        public GameObject store;
        public Transform storeContent;
        public GameObject storeButton;
        public GameObject warehouse;
        public Transform warehouseContent;
        public GameObject warehouseButton;
        public GameObject picker;
        public Transform pickerContent;
        public GameObject gold;
        public GameObject topPanel;
        public Transform topPanelContent;
        public GameObject loadingScreen;
        public GameObject background;
        /// <summary>
        /// Show or hide menu
        /// </summary>
        /// <param name="state"></param>
        public void ShowMenu(bool state)
        {
            InputController.canInput = !state;
            menu.SetActive(state);
        }
        /// <summary>
        /// Show or hide menu button
        /// </summary>
        /// <param name="state"></param>
        public void ShowMenuButton(bool state)
        {
            menuButton.SetActive(state);
        }
        /// <summary>
        /// Show or hide menu close button
        /// </summary>
        /// <param name="state"></param>
        public void ShowMenuCloseButton(bool state)
        {
            menuCloseButton.SetActive(state);
        }
        /// <summary>
        /// Show or hide menu start button
        /// </summary>
        /// <param name="state"></param>
        public void ShowMenuStartButton(bool state)
        {
            menuStartButton.SetActive(state);
        }
        /// <summary>
        /// Show or hide menu continue button
        /// </summary>
        /// <param name="state"></param>
        public void ShowMenuContinueButton(bool state)
        {
            menuContinueButton.SetActive(state);
        }
        /// <summary>
        /// Show or hide menu exit button
        /// </summary>
        /// <param name="state"></param>
        public void ShowMenuExitButton(bool state)
        {
            menuExitButton.SetActive(state);
        }
        /// <summary>
        /// Show or hide store
        /// </summary>
        /// <param name="state"></param>
        public void ShowStore(bool state)
        {
            InputController.canInput = !state;
            store.SetActive(state);
        }
        /// <summary>
        /// Show or hide store button
        /// </summary>
        /// <param name="state"></param>
        public void ShowStoreButton(bool state)
        {
            storeButton.SetActive(state);
        }
        /// <summary>
        /// Show or hide warehouse
        /// </summary>
        /// <param name="state"></param>
        public void ShowWarehouse(bool state)
        {
            InputController.canInput = !state;
            warehouse.SetActive(state);
        }
        /// <summary>
        /// Show or hide picker
        /// </summary>
        /// <param name="state"></param>
        public void ShowPicker(bool state)
        {
            InputController.canInput = !state;
            picker.SetActive(state);
        }
        /// <summary>
        /// Show or hide warehouse button
        /// </summary>
        /// <param name="state"></param>
        public void ShowWarehouseButton(bool state)
        {
            warehouseButton.SetActive(state);
        }
        /// <summary>
        /// Show or hide gold
        /// </summary>
        /// <param name="state"></param>
        public void ShowGold(bool state)
        {
            gold.SetActive(state);
        }
        /// <summary>
        /// Show or hide top panel
        /// </summary>
        /// <param name="state"></param>
        public void ShowTopPanel(bool state)
        {
            topPanel.SetActive(state);
        }
        /// <summary>
        /// Disable all UI components
        /// </summary>
        public void DisableAll()
        {
            menu.SetActive(false);
            menuButton.SetActive(false);
            menuStartButton.SetActive(false);
            menuContinueButton.SetActive(false);
            menuExitButton.SetActive(false);
            store.SetActive(false);
            storeButton.SetActive(false);
            warehouse.SetActive(false);
            warehouseButton.SetActive(false);
            gold.SetActive(false);
            topPanel.SetActive(false);
            loadingScreen.SetActive(false);
            background.SetActive(false);
            picker.SetActive(false);
        }
        /// <summary>
        /// Show scene loading progress
        /// </summary>
        /// <param name="state"></param>
        public void ShowLoadingScreen(bool state)
        {
            InputController.canInput = !state;
            loadingScreen.SetActive(state);
        }
        /// <summary>
        /// Show background picture
        /// </summary>
        /// <param name="state"></param>
        public void ShowBackground(bool state)
        {
            background.SetActive(state);
        }
        /// <summary>
        /// Play click clip by button click
        /// </summary>
        public void PlayUIClick()
        {
            SoundController.instance.PlayUIClick();
        }
    }
}