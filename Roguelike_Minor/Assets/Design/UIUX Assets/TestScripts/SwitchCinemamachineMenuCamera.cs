using UnityEngine;

public class SwitchCinemamachineMenuCamera : MonoBehaviour
{
    public GameObject CameraMainMenu;
    //public GameObject Character1;
    public GameObject Character2; 

    public Animator animator;
    bool Select; 

    void Start(){
        if (!animator){ 
            animator = gameObject.GetComponent<Animator>();
        }
        Select = false;
    } 

    public void SetFalse()
    {
        CameraMainMenu.SetActive(false);
        //Character1.SetActive(false);
        Character2.SetActive(false);
        //CharacterAnimation.enabled = false;
    }

    public void OnButtonClickMainCamera()
    {
        SetFalse();
        CameraMainMenu.SetActive(true);
    }

    // public void OnButtonClickCharacter1()
    // {
    //     SetFalse();
    //     Character1.SetActive(true);
    // }

    public void OnButtonClickCharacter2()
    {
        SetFalse();
        Character2.SetActive(true);
    }
    public void OnSelectClickButtonC2(){
        Debug.Log("test");
        animator.SetBool("Select", true);
        }
}

