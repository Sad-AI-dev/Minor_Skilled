using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Game.Core;

namespace Game {
    public class ResultScreen : MonoBehaviour
    {
        [System.Serializable]
        public class StatSettings
        {
            public string name;
            public float scoreMult = 1f;
            public UnityEvent<StatSettings, ResultScreen> onGenerateField;
            //set during runtime
            [HideInInspector] public StatLabel statLabel;
            [HideInInspector] public TMP_Text scoreLabel;
        }

        [Header("PlayerStat Refs")]
        [SerializeField] private GameObject evenField;
        [SerializeField] private GameObject oddField;
        [Space(10f)]
        [SerializeField] private GameObject evenStat;
        [SerializeField] private GameObject oddStat;
        [Space(10f)]
        [SerializeField] private RectTransform statHolder;
        [SerializeField] private RectTransform scoreHolder;
        [Space(10f)]
        [SerializeField] private List<StatSettings> statSettings;

        [Header("Total Score Refs")]
        [SerializeField] private TMP_Text totalScoreLabel;

        [Header("Inventory Refs")]
        [SerializeField] private InventoryUI inventory;

        //vars
        [HideInInspector] public StatTracker statTracker;
        [HideInInspector] public int totalScore;

        private void Awake()
        {
            inventory.inventory = GameStateManager.instance.player.inventory as SlotInventory;
        }

        private void OnEnable()
        {
            InitializeVars();
            //generate fields
            GeneratePlayerStats();
            //show total score
            totalScoreLabel.text = totalScore.ToString();
            //pause game
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }

        //============ Initialize =============
        private void InitializeVars()
        {
            statTracker = GameStateManager.instance.statTracker;
            //block inputs
            EventBus<GamePauseEvent>.Invoke(new GamePauseEvent(true));
        }

        //=========== Generate Player Stats =============
        private void GeneratePlayerStats()
        {
            for (int i = 0; i < statSettings.Count; i++)
            {
                //generate fields
                GenerateStatFields(statSettings[i], i % 2 == 0);
                //setup fields
                statSettings[i].onGenerateField?.Invoke(statSettings[i], this);
            }
        }

        private void GenerateStatFields(StatSettings settings, bool even)
        {
            //stat label
            settings.statLabel = Instantiate(even ? evenStat : oddStat, statHolder).GetComponent<StatLabel>();
            //score label
            settings.scoreLabel = Instantiate(even ? evenField : oddField, scoreHolder).GetComponentInChildren<TMP_Text>();
        }

        //======== Handle Unpause ==========
        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}
