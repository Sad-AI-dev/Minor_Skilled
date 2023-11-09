using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menu;

        public void ActivateMenu()
        {
            menu.SetActive(true);
        }

        public void DeactivateMenu()
        {
            menu.SetActive(false);
        }
    }
}
