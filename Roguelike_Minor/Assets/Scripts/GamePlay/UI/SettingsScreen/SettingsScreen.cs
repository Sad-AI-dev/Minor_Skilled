using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Core.GameSystems;

namespace Game
{
    public class SettingsScreen : MonoBehaviour
    {
        public SettingsSO settings;

        [Header("Tabs Refs")]
        [SerializeField] private GameObject[] tabContentHolders;

        [Header("Setting Details Refs")]
        [SerializeField] private TMP_Text descriptionTitleLabel;
        [SerializeField] private TMP_Text descriptionLabel;

        //vars
        [HideInInspector] public SettingsSaveLoad serializer;

        //tabs
        private int openTab;
        //settings
        private SettingData currentHover;

        private void Awake()
        {
            serializer = new SettingsSaveLoad("settings.json");
            //initialize tabs
            InitializeTabs();
            //initialize descriptions
            descriptionTitleLabel.text = "";
            descriptionLabel.text = "";
        }

        //===== Initailize ======
        private void InitializeTabs()
        {
            //setup tab contents
            for (int i = 0; i < tabContentHolders.Length; i++)
            {
                tabContentHolders[i].SetActive(false);
                InitializeHoverListeners(tabContentHolders[i]);
            }
            //open default tab
            OpenTab(0);
        }
        private void InitializeHoverListeners(GameObject holder)
        {
            SettingData[] settings = holder.GetComponentsInChildren<SettingData>();
            for (int i = 0; i < settings.Length; i++)
            {
                settings[i].onHover += HandleHoverSetting;
                settings[i].onEndHover += HandleEndHoverSetting;
            }
        }

        //======== Manage Tabs ========
        public void OpenTab(int tabIndex)
        {
            //close old tab
            CloseTab(openTab);
            //open new tab
            tabContentHolders[tabIndex].SetActive(true);
            openTab = tabIndex;
        }

        private void CloseTab(int tabIndex)
        {
            tabContentHolders[tabIndex].SetActive(false);
        }

        //======== Manage Hover Setting ========
        private void HandleHoverSetting(SettingData setting)
        {
            descriptionTitleLabel.text = setting.title;
            descriptionLabel.text = setting.description;
            //register
            currentHover = setting;
        }

        private void HandleEndHoverSetting(SettingData setting)
        {
            if (currentHover == setting)
            {
                descriptionTitleLabel.text = "";
                descriptionLabel.text = "";
                currentHover = null;
            }
        }
    }
}
