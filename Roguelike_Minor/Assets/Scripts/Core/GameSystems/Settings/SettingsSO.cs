using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems {
    [CreateAssetMenu(fileName = "settings", menuName = "ScriptableObjects/Test/SettingsTest")]
    public class SettingsSO : ScriptableObject
    {
        #region KeyBinds
        [Header("KeyBinds")]
        [Header("Movement KeyBinds")]
        public KeyCode forward = KeyCode.W;
        public KeyCode left = KeyCode.A;
        public KeyCode backward = KeyCode.S;
        public KeyCode right = KeyCode.D;
        [Space(10f)]
        public KeyCode jump = KeyCode.Space;

        [Header("Interaction KeyBinds")]
        public KeyCode interact = KeyCode.E;

        [Header("Ability KeyBinds")]
        public KeyCode primary = KeyCode.Mouse0;
        public KeyCode secondary = KeyCode.Mouse1;
        public KeyCode utility = KeyCode.LeftShift;
        public KeyCode special = KeyCode.Q;

        [Header("Inventory KeyBinds")]
        public KeyCode openInventory = KeyCode.Tab;
        public KeyCode dropItem = KeyCode.E;

        public KeyCode[] KeyBindsCollection { 
            get {
                return new KeyCode[] { 
                    forward, left, backward, right, jump,
                    interact,
                    primary, secondary, utility, special,
                    openInventory, dropItem
                };
            } 
        }
        #endregion
    }
}
