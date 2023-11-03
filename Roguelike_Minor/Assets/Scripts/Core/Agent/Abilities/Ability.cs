using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core {
    [System.Serializable]
    public class Ability
    {
        public class AbilityVars { }

        public enum CoolDownMode { coolDown, attackSpeed }

        [HideInInspector] public Agent agent;

        public AbilitySO abilityData;
        
        [Header("Uses")]
        public int uses;
        public int maxUses = 1;
        
        [Header("Timings")]
        public float coolDown;
        public CoolDownMode coolDownMode;
        [HideInInspector] public bool isCoolingDown = false;

        [Header("Origin")]
        public Transform originPoint;

        [Header("Events")]
        public UnityEvent<Ability> onUse;
        public AK.Wwise.Event SFX;

        //vars
        [HideInInspector] public float coolDownTimer;
        private Coroutine coolDownRoutine;

        //runtime vars support
        public AbilityVars vars;

        //============ Initialize ============
        public void Initialize(Agent agent)
        {
            this.agent = agent;
            if (abilityData) { abilityData.InitializeVars(this); }
        }

        //============ Use Ability ================
        public void TryUse()
        {
            if (CanUse()) {
                Use();
                if (!isCoolingDown) { StartCoolDown(); }
            }
        }

        private bool CanUse()
        {
            return uses > 0;
        }

        private void Use()
        {
            uses--;
            onUse?.Invoke(this);
            SFX.Post(agent.gameObject);
            abilityData.Use(this);
        }

        //=============== CoolDown ================
        private void StartCoolDown()
        {
            isCoolingDown = true;
            coolDownRoutine = agent.StartCoroutine(CoolDownCo());
        }

        private void SetCooldownTimer()
        {
            coolDownTimer = coolDownMode switch
            {
                CoolDownMode.coolDown => coolDown / agent.stats.coolDownMultiplier,
                CoolDownMode.attackSpeed => coolDown / agent.stats.attackSpeed,
                _ => coolDown
            };
        }

        private IEnumerator CoolDownCo()
        {
            SetCooldownTimer();
            while (coolDownTimer > 0f) 
            {
                yield return null;
                coolDownTimer -= Time.deltaTime;
            }
            EndCoolDown();
        }

        private void EndCoolDown()
        {
            uses = Mathf.Clamp(uses + 1, 0, maxUses);
            isCoolingDown = uses != maxUses; //if not all uses recovered, keep cooling down
            coolDownRoutine = isCoolingDown ? agent.StartCoroutine(CoolDownCo()) : null;
        }

        //========= Force Reduce Cooldown ==========
        public void ReduceCoolDown(float seconds)
        {
            if (isCoolingDown)
            {
                coolDownTimer -= seconds;
                if (coolDownTimer < 0) { coolDownTimer = 0f; }
            }
        }

        //============= Reset Ability ===============
        public void Reset()
        {
            uses = maxUses;
            coolDownTimer = 0;
            if (coolDownRoutine != null) 
            { 
                agent.StopCoroutine(coolDownRoutine); //stop routine
            }
        }
    }
}
