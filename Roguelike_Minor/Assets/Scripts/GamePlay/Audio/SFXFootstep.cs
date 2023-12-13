using UnityEngine;

namespace Game
{
    public class SFXFootstep : MonoBehaviour
    {
        [SerializeField] private AK.Wwise.Event footstepSFX;
        [SerializeField] private AK.Wwise.Event servoSFX;

        void playFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                footstepSFX.Post(gameObject);
            }
        }
        
         void playServo(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                servoSFX.Post(gameObject);
            }
        }
        
    }
}