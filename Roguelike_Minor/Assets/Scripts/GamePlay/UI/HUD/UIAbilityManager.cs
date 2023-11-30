using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Core;
using TMPro;

namespace Game {
    public class UIAbilityManager : MonoBehaviour
    {
        [SerializeField] UIManager uiManager;

        [System.Serializable]
        private class UIAbilityVars
        {
            public Image icon;
            public TMP_Text cooldownLabel;
            public TMP_Text inputLabel;
            public TMP_Text usesLabel;
        }

        [SerializeField] private UIAbilityVars primaryVars, secondaryVars, specialVars, utilityVars;

        [Header("Cooldown Settings")]
        [SerializeField] string colorCode = "#9C9C9C";
        Color cooldownColor;
        [SerializeField] float oppacity = 0.8f;

        Color baseColor = Color.white;

        private void Start()
        {
            ColorUtility.TryParseHtmlString(colorCode, out cooldownColor);
            cooldownColor.a = oppacity;
            baseColor.a = 1;
        }
        private void Update()
        {
            HandleUI(primaryVars, uiManager.agent.abilities.primary);
            HandleUI(secondaryVars, uiManager.agent.abilities.secondary);
            HandleUI(specialVars, uiManager.agent.abilities.special);
            HandleUI(utilityVars, uiManager.agent.abilities.utility);
        }

        void HandleUI(UIAbilityVars vars, Ability ability)
        {
            //uses
            if (ability.uses > 1 || (ability.uses > 0 && ability.maxUses > 1))
            {
                vars.usesLabel.text = ability.uses.ToString();
            }
            else { vars.usesLabel.text = ""; } //hide label
            //cooldown
            if (ability.isCoolingDown && ability.coolDownMode == Ability.CoolDownMode.coolDown)
            {
                vars.icon.color = cooldownColor;
                vars.cooldownLabel.text = Mathf.CeilToInt(ability.coolDownTimer).ToString();
                vars.cooldownLabel.gameObject.SetActive(true);
            }
            else
            {
                vars.icon.color = baseColor;
                vars.cooldownLabel.gameObject.SetActive(false);
            }
        }
    }
}
