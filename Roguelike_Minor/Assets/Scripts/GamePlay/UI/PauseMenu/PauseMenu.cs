using Game.Core.GameSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menu;

        public bool paused;

        public void ActivateMenu()
        {
            menu.SetActive(true);
            paused = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }

        public void DeactivateMenu()
        {
            menu.SetActive(false);
            paused = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void ReturnToMenu()
        {
            menu.SetActive(false);
            paused = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Settings()
        {

        }

        public void ExitGame()
        {
            if(Application.isEditor)
            {
                EditorApplication.ExitPlaymode();
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
