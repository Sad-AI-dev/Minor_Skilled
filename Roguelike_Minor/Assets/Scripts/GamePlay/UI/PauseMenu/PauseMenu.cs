using Game.Core.GameSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        public void ActivateMenu()
        {
            darkBackground.SetActive(true);
            menu.SetActive(true);
            paused = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            //notify
            onOpenPause?.Invoke();
        }

        public void DeactivateMenu()
        {
            darkBackground.SetActive(false);
            menu.SetActive(false);
            paused = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            //notify
            onClosePause?.Invoke();
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

        public void Settings()
        {

        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
