using UnityEngine;
using UnityEngine.Events;

public class AfterCameraAnimation : MonoBehaviour
{
    public string LogoAnimation;
    public UnityEvent onAnimationFinishedLogo;

    private Animator animatorLogo;
    private bool hasStarted = false;

    void Start()
    {
        // Get the Animator component attached to the GameObject
        animatorLogo = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animatorLogo.GetCurrentAnimatorStateInfo(0);

        // Check if the state matches the desired stateName
        if (stateInfo.IsName(LogoAnimation))
        {
            // Check if the normalized time is greater than or equal to 1 (animation end)
            if (stateInfo.normalizedTime >= 1f)
            {
                hasStarted = false; // Reset flag
                OnAnimationFinishedLogo();
                //turn off
                enabled = false;
            }
        }
    }
    
    
    public void PlayAnimation()
    {
        if (!hasStarted){ 
        // Trigger the animation
        animatorLogo.Play("Logo");
        hasStarted = true;
        }
    }

    // Function to be called once the animation is finished
    void OnAnimationFinishedLogo()
    {
        // Call your desired method or perform actions here after animation finishes
        onAnimationFinishedLogo?.Invoke();
    }
}
