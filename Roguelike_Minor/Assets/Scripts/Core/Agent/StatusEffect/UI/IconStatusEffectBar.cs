using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game.Core {
    public class IconStatusEffectBar : StatusEffectBar
    {
        public BehaviourPool<EffectIconElement> elementPool;
        public RectTransform effectHolder;

        //effect list
        private Dictionary<StatusEffectSO, EffectIconElement> elements;

        private void Start()
        {
            elements = new Dictionary<StatusEffectSO, EffectIconElement>();
        }

        //===== handle add / remove effect =====
        public override void HandleAddEffect(StatusEffectSO effect)
        {
            EffectIconElement element = elementPool.GetBehaviour();
            //setup element
            element.transform.SetParent(effectHolder);
            element.Initialize();
            element.icon.sprite = effect.icon;
            //store reference
            elements.Add(effect, element);
        }

        public override void HandleRemoveEffect(StatusEffectSO effect)
        {
            EffectIconElement element = elements[effect];
            //reset element
            element.ResetToDefault();
            element.gameObject.SetActive(false);
            //remove reference
            elements.Remove(effect);
        }

        //====== Handle stack changes ======
        public override void HandleUpdateStacks(StatusEffectSO effect, int stacks)
        {
            elements[effect].SetStacks(stacks);
        }
    }
}
