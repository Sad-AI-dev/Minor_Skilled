using Game.Core.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    [CreateAssetMenu(fileName = "KeyBindLabelDisplaySettings", menuName = "ScriptableObjects/Settings/KeyBindLabelDisplay")]
    public class KeyBindLabelDisplaySO : ScriptableObject
    {
        [System.Serializable]
        public class KeyBindDisplayData
        {
            public string replaceString = "key";
        }

        public UnityDictionary<KeyCode, KeyBindDisplayData> displayDataDict;
    }
}
