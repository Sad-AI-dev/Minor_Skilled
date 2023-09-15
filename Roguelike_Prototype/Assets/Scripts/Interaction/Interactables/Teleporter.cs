using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Teleporter : MonoBehaviour
{
    [Header("Time Settings")]
    public float timeToComplete = 20;

    [Header("Technical Settings")]
    public TMP_Text label;
    public Interactable interactable;
    public ObjectDetector playerDetector;

    //vars
    private float timer;
    private bool isCharging = false;

    private void Start()
    {
        interactable.gameObject.SetActive(false);
        label.text = "0%";
        timer = 0;
        //setup player detection
        playerDetector.onDetectObject.AddListener(OnPlayerEnter);
        playerDetector.onLeaveLastObject.AddListener(OnPlayerExit);
    }

    //============= Charge Teleporter ==============
    private void Update()
    {
        if (isCharging) { Charge(); }
    }

    private void Charge()
    {
        timer += Time.deltaTime;
        //update UI
        label.text = Mathf.RoundToInt(timer / timeToComplete * 100).ToString() + "%";
        if (timer >= timeToComplete) { //charger is done check
            OnComplete();
        }
    }

    private void OnComplete()
    {
        label.text = "Ready!";
        interactable.gameObject.SetActive(true);
        enabled = false;
    }

    //========= Player Detection ===============
    private void OnPlayerEnter()
    {
        isCharging = true;
    }

    private void OnPlayerExit()
    {
        isCharging = false;
    }
}
