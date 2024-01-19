using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.Core;
using Game.Core.GameSystems;

namespace Game
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menu;

        [SerializeField] private GameObject darkBackground;
        public bool paused;

        [Header("Events")]
        [SerializeField] private UnityEvent onOpenPause;
        [SerializeField] private UnityEvent onClosePause;

        private bool settingsIsOpen;

        private void Start()
        {
            EventBus<SceneLoadedEvent>.AddListener(SceneLoaded);
            EventBus<SceneUnloadedEvent>.AddListener(SceneUnloaded);
        }

        //=========== Manage Menu ==============
        public void ActivateMenu()
        {
            darkBackground.SetActive(true);
            menu.SetActive(true);
            paused = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            //notify
            onOpenPause?.Invoke();
            EventBus<GamePauseEvent>.Invoke(new GamePauseEvent(true));
        }

        public void DeactivateMenu()
        {
            if (settingsIsOpen) { return; }

            darkBackground.SetActive(false);
            menu.SetActive(false);
            paused = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            //notify
            onClosePause?.Invoke();
            EventBus<GamePauseEvent>.Invoke(new GamePauseEvent(false));
        }

        public void ReturnToMenu()
        {
            darkBackground.SetActive(false);
            menu.SetActive(false);
            paused = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.None;
        }

        public void UnPause()
        {
            Time.timeScale = 1f;
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        //======== Handle Load Settings ========
        private void SceneLoaded(SceneLoadedEvent eventData)
        {
            if (eventData.loadedIndex == 5) { settingsIsOpen = true; }
        }

        private void SceneUnloaded(SceneUnloadedEvent eventData)
        {
            if (eventData.unloadedIndex == 5) { settingsIsOpen = false; }
        }

        //======== Handle Disable =========
        private void OnDisable()
        {
            EventBus<SceneLoadedEvent>.RemoveListener(SceneLoaded);
            EventBus<SceneUnloadedEvent>.RemoveListener(SceneUnloaded);
        }
    }
}
