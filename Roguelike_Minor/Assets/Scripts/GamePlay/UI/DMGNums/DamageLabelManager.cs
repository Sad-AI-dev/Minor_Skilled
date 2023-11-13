using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Core;
using Game.Core.Data;

namespace Game {
    public class DamageLabelManager : MonoBehaviour
    {
        [SerializeField] private BehaviourPool<NumLabel> labelPool;
        [SerializeField] private Camera cam;

        [Header("Num Visual Settings")]
        [SerializeField] private Vector3 originPosOffset;
        [SerializeField] private Vector3 randomOffsetBounds;

        //refs
        private Agent player;

        private void Start()
        {
            player = GameStateManager.instance.player;
            InitializeEvents();
        }

        //============= Initialize =============
        private void InitializeEvents()
        {
            EventBus<AgentTakeDamageEvent>.AddListener(OnAgentTakeDamage);
            EventBus<AgentHealEvent>.AddListener(OnAgentHeal);
        }

        //============== Handle Events ==============
        private void OnAgentTakeDamage(AgentTakeDamageEvent eventData)
        {
            if (eventData.hitEvent.target.agent != player) //don't show damage number for player getting hit
            {
                HitEvent hit = eventData.hitEvent;
                FormatNumberLabel(
                    hit.target.transform.position + GetOffset(), 
                    hit.isCrit ? hit.critColor : hit.labelColor, 
                    hit.GetTotalDamage()
                );
            }
        }

        private void OnAgentHeal(AgentHealEvent eventData)
        {
            HealEvent healEvent = eventData.healEvent;
            if (healEvent.createNumLabel)
            {
                FormatNumberLabel(
                    healEvent.target.transform.position + GetOffset(),
                    healEvent.labelColor, healEvent.GetTotalHeal()
                );
            }
        }

        private Vector3 GetOffset()
        {
            return originPosOffset + new Vector3(
                Random.Range(-randomOffsetBounds.x, randomOffsetBounds.x),
                Random.Range(-randomOffsetBounds.y, randomOffsetBounds.y),
                Random.Range(-randomOffsetBounds.z, randomOffsetBounds.z)
            );
        }

        //============ Format number label ===============
        private void FormatNumberLabel(Vector3 worldPos, Color color, float value)
        {
            if (PointIsInFrostum(worldPos))
            {
                NumLabel numLabel = labelPool.GetBehaviour();
                //configure label
                numLabel.cam = cam;
                numLabel.trackPos = worldPos;
                //configure transform
                numLabel.label.rectTransform.SetParent(transform);
                //format text
                numLabel.label.text = Mathf.RoundToInt(value).ToString();
                numLabel.label.color = color;
            }
        }

        private bool PointIsInFrostum(Vector3 worldPos)
        {
            Vector3 viewPortPos = cam.WorldToViewportPoint(worldPos);
            return Is01Range(viewPortPos.x) && Is01Range(viewPortPos.y) && viewPortPos.z > 0;
        }
        private bool Is01Range(float num)
        {
            return num > 0f && num < 1f;
        }

        //============ Handle Destroy =============
        private void OnDestroy()
        {
            EventBus<AgentTakeDamageEvent>.RemoveListener(OnAgentTakeDamage);
            EventBus<AgentHealEvent>.RemoveListener(OnAgentHeal);
        }
    }
}
