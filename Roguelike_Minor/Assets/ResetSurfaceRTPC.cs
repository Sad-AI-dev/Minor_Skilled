using System.Collections;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;

public class ResetSurfaceRTPC : MonoBehaviour
{
    public RTPC metalFootstepFix; 
    // Start is called before the first frame update
    void Start()
    {
        metalFootstepFix.SetGlobalValue(0);
    }

   
}
