using UnityEngine;

public class AfterAnimation : MonoBehaviour
{
    private Animator animator;
    private bool hasStarted = false;
    public string FadeInCorporationScreen;
    
    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Check if the state matches the desired stateName
        if (stateInfo.IsName(FadeInCorporationScreen))
        {
            // Check if the normalized time is greater than or equal to 1 (animation end)
            if (stateInfo.normalizedTime >= 1f)
            {
                hasStarted = false; // Reset flag
                OnAnimationFinished();
            
            }
        }
    }
    
    
    public void PlayAnimation()
    {
        if (!hasStarted){ 
        // Trigger the animation
        animator.Play("Fade in corporation screen");
        hasStarted = true;
        }
    }

    // Function to be called once the animation is finished
    void OnAnimationFinished()
    {
        Debug.Log("Animation Finished!");
        // Call your desired method or perform actions here after animation finishes
    }
}

