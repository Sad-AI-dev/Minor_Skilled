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

        [Header("Primary")]
        [SerializeField] Image primaryIcon;
        [SerializeField] TextMeshProUGUI primaryCooldownTimer;
        [SerializeField] TextMeshProUGUI primaryInput;

        [Header("Secondary")]
        [SerializeField] Image secondaryIcon;
        [SerializeField] TextMeshProUGUI secondaryCooldownTimer;
        [SerializeField] TextMeshProUGUI secondaryInput;

        [Header("Special")]
        [SerializeField] Image specialIcon;
        [SerializeField] TextMeshProUGUI specialCooldownTimer;
        [SerializeField] TextMeshProUGUI specialInput;

        [Header("Utility")]
        [SerializeField] Image utilityIcon;
        [SerializeField] TextMeshProUGUI utilityCooldownTimer;
        [SerializeField] TextMeshProUGUI utilityInput;

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
            HandleUI(primaryIcon, primaryCooldownTimer, uiManager.agent.abilities.primary);
            HandleUI(secondaryIcon, secondaryCooldownTimer, uiManager.agent.abilities.secondary);
            HandleUI(specialIcon, specialCooldownTimer, uiManager.agent.abilities.special);
            HandleUI(utilityIcon, utilityCooldownTimer, uiManager.agent.abilities.utility);
        }

        void HandleUI(Image icon, TextMeshProUGUI cooldownText, Ability ability)
        {
            if (ability.isCoolingDown)
            {
                icon.color = cooldownColor;
                cooldownText.text = Mathf.RoundToInt(ability.coolDownTimer).ToString();
                cooldownText.gameObject.SetActive(true);
            }
            else
            {
                icon.color = baseColor;
                cooldownText.gameObject.SetActive(false);
            }
        }
    }
}