using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Game.Core;
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

        [Header("Scroll Rect Refs")]
        [SerializeField] private RectTransform scrollRect;

        [Header("Unload Vars")]
        [SerializeField] private int buildIndex; 

        //vars
        private SettingsSaveLoad serializer;
        [HideInInspector] public bool dirty;

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseMenu();
            }
        }

        //======= Close and Save ========
        public void CloseMenu()
        {
            SaveSettings();
            //hide object
            gameObject.SetActive(false);
            //unload scene
            SceneManager.UnloadSceneAsync(buildIndex);
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
            //refresh scroll rect
            RefreshScrollRect();
        }

        private void CloseTab(int tabIndex)
        {
            tabContentHolders[tabIndex].SetActive(false);
        }

        private void RefreshScrollRect()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect);
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

        //======= Save Settings ========
        private void SaveSettings()
        {
            if (dirty)
            {
                dirty = false;
                serializer.SaveSettings(settings);
                //notify settings changed
                EventBus<SettingsChanged>.Invoke(new SettingsChanged { settings = settings });
            }
        }
    }
}
