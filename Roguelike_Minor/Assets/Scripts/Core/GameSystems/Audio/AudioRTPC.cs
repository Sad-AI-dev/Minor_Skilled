using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRTPC : MonoBehaviour
{
    [SerializeField] private AK.Wwise.RTPC[] RTPC;

    public void setRTPCValue(int value, int index = 0)
    {
        if (index < RTPC.Length)
        {
            RTPC[index].SetGlobalValue(value);
        }
    }
}
