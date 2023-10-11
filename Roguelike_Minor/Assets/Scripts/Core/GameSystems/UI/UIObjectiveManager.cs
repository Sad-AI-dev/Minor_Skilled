using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Game.Core.GameSystems {
    public class UIObjectiveManager : MonoBehaviour
    {
        [SerializeField] UIManager uiManager;
        [SerializeField] GameObject checkMark;
        [SerializeField] TextMeshProUGUI ObjectiveTextUI;
        [SerializeField] string ObjectiveText;

        private void Start()
        {
            ObjectiveTextUI.text = ObjectiveText;
        }

        private void Update()
        {
            checkMark.SetActive(uiManager.ObjectiveComplete);
        }
    }
}
