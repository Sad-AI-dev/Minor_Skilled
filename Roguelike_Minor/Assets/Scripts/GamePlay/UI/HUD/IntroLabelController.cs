using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using Game.Core.GameSystems;
using TMPro;

namespace Game {
    public class IntroLabelController : MonoBehaviour
    {
        [Header("Stage Names")]
        [SerializeField] private string defaultName = "??? The realm beyond ???";
        [SerializeField] private UnityDictionary<int, string> planetNames;

        [Header("Effect Settings")]
        [SerializeField] private TMP_Text label;
        [SerializeField] private UIFader fader;

        private void Awake()
        {
            //listen to events
            EventBus<SceneLoadedEvent>.AddListener(HandleSceneLoad);
        }

        //=========== Play Animation ==========
        private void HandleSceneLoad(SceneLoadedEvent eventData)
        {
            //set label
            label.text = GetStageName(eventData.loadedIndex);
            //fade label
            fader.StartFade();
        }

        private string GetStageName(int index)
        {
            if (planetNames.ContainsKey(index))
            {
                return planetNames[index];
            }
            return defaultName;
        }

        //============ Handle Destroy ============
        private void OnDestroy()
        {
            EventBus<SceneLoadedEvent>.RemoveListener(HandleSceneLoad);
        }
    }
}
