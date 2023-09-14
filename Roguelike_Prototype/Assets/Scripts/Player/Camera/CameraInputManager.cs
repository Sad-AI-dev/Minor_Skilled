using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraInputManager : MonoBehaviour
{
    private CinemachineFreeLook camController;

    private string xAxisInputName, yAxisInputName;

    private void Start() 
    {
        GameManager.instance.camInputManager = this;
        camController = GetComponent<CinemachineFreeLook>();
        xAxisInputName = camController.m_XAxis.m_InputAxisName;
        yAxisInputName = camController.m_YAxis.m_InputAxisName;

        //register dont destroy on load
        transform.SetParent(null);
        DontDestroyRegister.instance.RegisterObject(gameObject);
        Camera.main.transform.SetParent(null);
        DontDestroyRegister.instance.RegisterObject(Camera.main.gameObject);
    }

    public void LockCamera()
    {
        camController.m_XAxis.m_InputAxisName = "";
        camController.m_YAxis.m_InputAxisName = "";
    }

    public void UnLockCamera()
    {
        camController.m_XAxis.m_InputAxisName = xAxisInputName;
        camController.m_YAxis.m_InputAxisName = yAxisInputName;
    }
}
