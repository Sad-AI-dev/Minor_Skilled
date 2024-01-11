using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class KeyBindManager : MonoBehaviour
    {
        [Header("Refs")]
        public SettingsScreen screen;

        //========= Restore Keybinds ============
        public void RestoreDefaultKeyBinds()
        {
            //movement keybinds
            screen.settings.forward = KeyCode.W;
            screen.settings.left = KeyCode.A;
            screen.settings.backward = KeyCode.S;
            screen.settings.right = KeyCode.D;

            screen.settings.jump = KeyCode.Space;

            //interaction keybinds
            screen.settings.interact = KeyCode.E;

            //ability keybinds
            screen.settings.primary = KeyCode.Mouse0;
            screen.settings.secondary = KeyCode.Mouse1;
            screen.settings.utility = KeyCode.LeftShift;
            screen.settings.special = KeyCode.Q;

            //inventory keybinds
            screen.settings.openInventory = KeyCode.Tab;
            screen.settings.dropItem = KeyCode.E;
        }
    }
}
