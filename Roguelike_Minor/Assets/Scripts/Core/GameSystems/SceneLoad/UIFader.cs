using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Game.Util;

namespace Game.Core.GameSystems {
    public class UIFader : MonoBehaviour
    {
        public enum FadeMode {
            Single, Cycle, Blink, Blink_continuous
        }

        public CanvasGroup targetGroup;
        [Tooltip("Dictates how fader behaves when 'StartFade' is called.\n\n" +
            "Single: fades in, or fades out, based on current state.\n" +
            "Cycle: fades in and fades out.\n" +
            "Blink: blinks for a set time.\n" +
            "Blink_continuous: blinks until 'StopFade' is called.")]
        public FadeMode fadeMode;
        [SerializeField] private bool fadeOnStart;

        [Header("Fade Settings")]
        [Tooltip("Duration of full fade.")]
        public float fadeTime = 1f;
        [Tooltip("Delay between end of fade-in and start of fade-out.")]
        public float fadedInDelay = 0.5f;
        [Tooltip("Delay between end of fade-out and start of fade-in.")]
        public float fadedOutDelay = 1f;

        [Tooltip("If Fade Mode is set to 'blink', sets total time fader blinks.")]
        [HideIf(nameof(IsNotBlinkMode))]
        public float blinkTime = 1f;

        [Header("Technical")]
        public UnityEvent onFadeEnd;

        //editor conditionals
        public bool IsNotBlinkMode => fadeMode != FadeMode.Blink;

        //vars
        private float timer = 0f;
        //states
        private bool fading = false;
        private bool fadingIn;

        private bool fadedOnce; //keep track for 'Cycle' mode

        private void Start()
        {
            if (targetGroup == null) { Debug.LogError("No target canvas group was set on " + transform.name + "!"); }
            DetermineStartState();
            if (fadeOnStart) { StartFade(); }
        }

        private void DetermineStartState()
        {
            fadingIn = true;
            if (targetGroup.alpha >= 0.5f) { fadingIn = false; }
        }

        //========= state management =========
        public void StartFade()
        {
            timer = 0f;
            fading = true;
            fadedOnce = false;
            //blink mode
            if (fadeMode == FadeMode.Blink) { StartCoroutine(BlinkTimer()); }
        }

        public void StopFade()
        {
            fading = false;
            onFadeEnd?.Invoke();
        }

        //============ fading ============
        private void Update()
        {
            if (fading) {
                timer += Time.deltaTime;
                if (fadingIn) { FadeIn(); }
                else { FadeOut(); }
            }
        }

        private void FadeIn()
        {
            targetGroup.alpha = timer / fadeTime;
            if (timer >= fadeTime) { OnEndFade(); }
        }

        private void FadeOut()
        {
            targetGroup.alpha = 1 - (timer / fadeTime);
            if (timer >= fadeTime) { OnEndFade(); }
        }

        //============ reach end fade ============
        private void OnEndFade()
        {
            StopFadeCheck();
            //start timers
            if (fading) {
                if (fadingIn) { StartCoroutine(FadedInDelayCo()); }
                else { StartCoroutine(FadedOutDelayCo()); }
            }
            //update vars
            UpdateOnEndVars();
        }

        private void UpdateOnEndVars()
        {
            timer = 0;
            fadingIn = !fadingIn;
            fadedOnce = true;
        }

        private void StopFadeCheck()
        {
            switch (fadeMode) {
                case FadeMode.Single:
                    StopFade();
                    break;

                case FadeMode.Cycle:
                    if (fadedOnce) { StopFade(); }
                    break;
            }
        }

        //=============== Timers ===============
        private IEnumerator FadedInDelayCo()
        {
            fading = false;
            yield return new WaitForSeconds(fadedInDelay);
            fading = true;
        }

        private IEnumerator FadedOutDelayCo()
        {
            fading = false;
            yield return new WaitForSeconds(fadedOutDelay);
            fading = true;
        }

        private IEnumerator BlinkTimer()
        {
            yield return new WaitForSeconds(blinkTime);
            fading = false;
        }
    }
}
