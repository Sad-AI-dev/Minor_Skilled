using UnityEngine;

public class SwitchCinemamachineMenuCamera : MonoBehaviour
{
    public GameObject CameraMainMenu;

    public GameObject CharacterFirst;
    public GameObject CharacterSecond;

    public GameObject LoadingCanvas;

    public GameObject SelectButtonPressed;

    public GameObject CorporationCanvas; 
    public Animator LogoComingInScreen;
    public Animator animator;

    public Animator animationloading;
    bool Select;

    void Start()
    {
        if (!animator)
        {
            animator = gameObject.GetComponent<Animator>();
        }
        Select = false;
    }

    public void SetFalse()
    {
        CameraMainMenu.SetActive(false);
        CharacterFirst.SetActive(false);
        CharacterSecond.SetActive(false);
        //CharacterAnimation.enabled = false;
    }

    public void MainMenuActive()
    {
        SetFalse();
        CameraMainMenu.SetActive(true);
    }

    public void CharacterFirstActive()
    {
        SetFalse();
        CharacterFirst.SetActive(true);
    }

    public void CharacterSecondActive()
    {
        SetFalse();
        CharacterSecond.SetActive(true);
    }

    public void PlayLogoAnimation(){
    LogoComingInScreen.enabled = true;
    }

    public void PlayCorporationVideo(){
       CorporationCanvas.SetActive(true); 
    }
    public void ButtonSelectPressed()
    {
        Debug.Log("test");
        animator.SetBool("Select", true);
        SetFalse();
        SelectButtonPressed.SetActive(true);
        LoadingCanvas.SetActive(true);
        animationloading.enabled = true;
        Invoke("PlayLogoAnimation", 6.0f);
        //PlayLogoAnimation();
        Invoke("PlayCorporationVideo", 8.5f);
    }
}

