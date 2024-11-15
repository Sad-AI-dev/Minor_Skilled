using UnityEngine;

namespace Game
{
    public class SFXFootstep : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private FootstepManager footstepManager;

        [Header("Sound Settings")]
        [SerializeField] private AK.Wwise.Event footstepSFX;
        [SerializeField] private AK.Wwise.Event servoSFX;
        [SerializeField] private GameObject PlayerObj;

        void playFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                //check ground
                footstepManager.CheckGround();
                //play sound
                footstepSFX.Post(PlayerObj);
            }
        }
        
        void playServo(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                servoSFX.Post(PlayerObj);
            }
        }
        
    }
}