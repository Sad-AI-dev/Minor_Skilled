using UnityEngine;
using UnityEngine.UI;
public class MainMenuCamera : MonoBehaviour
{

    public float cameraForce = 5f;
    public float yPositionCamera = 2.07f;
    
    // public Button ButtonPressedA;
    // public Button ButtonPressedB;
    // public Button ButtonPressedC;
   
    // void FixedUpdate()
    // {
    //     transform.position += new Vector3(0, 0, cameraForce) * Time.deltaTime; 

    //     Debug.Log (cameraForce + " " + transform.position);
        
    //     if (transform.position.z >= -3f && cameraForce > 0f){
    //        cameraForce -= 0.01f; 
    //     }
    //         else if(cameraForce <= 0f)
    //         {
    //             cameraForce = 0f;
    //         }
     
    //         Debug.Log (cameraForce);
    //     }
    
    public void CameraTutorial()
    {
        cameraForce = 0f;
    }

    void cameraState1(){

        transform.position += new Vector3(0, 0, cameraForce) * Time.deltaTime; 
     
        Debug.Log (cameraForce + " " + transform.position);
        
        // if (transform.position.z >= -3f && cameraForce > 0f){
        //    cameraForce -= 0.01f; 
        // }
        //     else if(cameraForce <= 0f)
        //     {
        //         cameraForce = 0f;
        //     }
     
        //     Debug.Log (cameraForce);

    }
    

    void cameraState2(){
        transform.position += new Vector3(0, 0, cameraForce) * Time.deltaTime; 

        Debug.Log (cameraForce + " " + transform.position);
        
        if (transform.position.z >= -5f && cameraForce > 0f){
           cameraForce -= 0.01f; 
        }
            else if(cameraForce <= 0f)
            {
                cameraForce = 0f;
            }
     
            Debug.Log (cameraForce);
    }

    void cameraState3(){
        transform.position += new Vector3(0, 0, cameraForce) * Time.deltaTime; 

        Debug.Log (cameraForce + " " + transform.position);
        
        if (transform.position.z >= -7f && cameraForce > 0f){
           cameraForce -= 0.01f; 
        }
            else if(cameraForce <= 0f)
            {
                cameraForce = 0f;
            }
     
            Debug.Log (cameraForce);
    }

    public void ButtonPressed1()
    {
        if (CompareTag("MainCamera"))
            {
                Debug.Log("cameraState1");
                cameraState1();
            }

    }

     public void ButtonPressed2()
    {
        if (CompareTag("MainCamera"))
            {
                Debug.Log("cameraState2");
                cameraState2();
            }

    }

     public void ButtonPressed3()
    {
        if (CompareTag("MainCamera"))
            {
                Debug.Log("cameraState3");
                cameraState3();
            }

    }
 
}

