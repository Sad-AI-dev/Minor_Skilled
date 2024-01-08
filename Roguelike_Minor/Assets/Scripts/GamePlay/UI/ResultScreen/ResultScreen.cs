using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

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

        //vars
        [HideInInspector] public StatTracker statTracker;
        [HideInInspector] public int totalScore;

        private void Start()
        {
            InitializeVars();
            //generate fields
            GeneratePlayerStats();
        }

        //============ Initialize =============
        private void InitializeVars()
        {
            statTracker = GameStateManager.instance.statTracker;
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
    }
}
