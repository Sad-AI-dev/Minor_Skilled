using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    public class StageLabel : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private TMP_Text label;

        //vars
        private string baseText;

        private void Awake()
        {
            baseText = label.text;
            //subscribe to event
            EventBus<SceneLoadedEvent>.AddListener(HandleStageLoadEvent);
        }

        private void HandleStageLoadEvent(SceneLoadedEvent eventData)
        {
            StartCoroutine(UpdateLabelCo());
        }
        private IEnumerator UpdateLabelCo()
        {
            yield return null; //wait a frame
            label.text = baseText + " " + GameStateManager.instance.currentStage;
        }

        //==== Handle Destroy ===
        private void OnDestroy()
        {
            EventBus<SceneLoadedEvent>.RemoveListener(HandleStageLoadEvent);
        }
    }
}
