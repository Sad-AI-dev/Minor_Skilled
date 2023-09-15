using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UniversalInputReceiver : MonoBehaviour
{
    //============ define input reading types ============
    [Serializable]
    private struct ButtonInput
    {
        public string name; //label for editor
        public UnityEvent onButtonDown;
        public UnityEvent onButtonHeld;
        public UnityEvent onButtonUp;
        [Header("Input codes")]
        public List<KeyCode> codes;
        [Header("Input Manager Button Codes")]
        [Tooltip("Used to read buttons from the input manager.")]
        public List<string> buttonCodes;
    }

    [Serializable]
    private struct AxisInput
    {
        public string name; //label for editor
        public UnityEvent<float> output;
        [Header("Input codes")]
        public List<KeyCode> posInput;
        public List<KeyCode> negInput;
        [Header("Input Manager Axis Codes")]
        [Tooltip("Used to read virtual axis from the input manager.")]
        public List<string> axisCodes;
    }

    [Serializable]
    private struct DirectionalInput
    {
        public string name; //label for editor
        public UnityEvent<Vector2> output;
        [Header("Input codes")]
        public List<KeyCode> xPosInput;
        public List<KeyCode> xNegInput;
        public List<KeyCode> yPosInput;
        public List<KeyCode> yNegInput;
        [Header("Input Manager Axis Codes")]
        [Tooltip("Used to read virtual axis from the input manager.")]
        public List<string> xAxisCodes;
        public List<string> yAxisCodes;
    }

    //=============== vars ===============
    [Tooltip("Used to read key presses.")]
    [SerializeField] private List<ButtonInput> buttonInputs;
    [Tooltip("Used to read a set of keys as an opposing axis.\nReturns a float between -1 and 1")]
    [SerializeField] private List<AxisInput> axisInputs;
    [Tooltip("Used to read a set of keys as 2 opposing axis.\nReturns a Vector2 between (-1,-1) and (1,1)")]
    [SerializeField] private List<DirectionalInput> directionalInputs;

    private Action onReadInputs;

    private void Start()
    {
        if (buttonInputs != null && buttonInputs.Count > 0) { onReadInputs += ReadButtonInputs; }
        if (axisInputs != null && axisInputs.Count > 0) { onReadInputs += ReadAxisInputs; }
        if (directionalInputs != null && directionalInputs.Count > 0) { onReadInputs += ReadDirectionalInputs; }
    }

    //============ read inputs ============
    private void Update()
    {
        if (Time.timeScale > 0f) { //Don't read inputs when paused
            onReadInputs?.Invoke();
        }
    }

    private void ReadButtonInputs()
    {
        foreach (ButtonInput input in buttonInputs) {
            //read keys
            foreach (KeyCode code in input.codes) {
                if (Input.GetKeyDown(code)) { input.onButtonDown?.Invoke(); } //dont break here, getkey will be called, which breaks
                if (Input.GetKey(code)) { input.onButtonHeld?.Invoke(); break; }
                else if (Input.GetKeyUp(code)) { input.onButtonUp?.Invoke(); break; }

            }
            //read input manager strings
            foreach (string code in input.buttonCodes) {
                try {
                    if (Input.GetButtonDown(code)) { input.onButtonDown?.Invoke(); } //dont break here, getButton will be called, which breaks
                    if (Input.GetButton(code)) { input.onButtonHeld?.Invoke(); break; }
                    else if (Input.GetButtonUp(code)) { input.onButtonUp?.Invoke(); break; }
                }
                catch { Debug.LogError("'" + code + "' is not a valid key code!\n" + transform.name); }
            }
        }
    }

    private void ReadAxisInputs()
    {
        foreach (AxisInput input in axisInputs) {
            float output = 0;
            //read pos keys
            foreach (KeyCode code in input.posInput) {
                if (Input.GetKey(code)) { output += 1f; break; }
            }
            //read min keys
            foreach (KeyCode code in input.negInput) {
                if (Input.GetKey(code)) { output -= 1f; break; }
            }
            //read input manager virtual axis
            foreach (string s in input.axisCodes) {
                try { output = Input.GetAxisRaw(s); }
                catch { Debug.LogError("'" + s + "' is not a valid virtual axis code!\n" + transform.name); }
            }
            //call event
            input.output?.Invoke(output);
        }
    }

    private void ReadDirectionalInputs()
    {
        foreach (DirectionalInput input in directionalInputs) {
            Vector2 output = Vector2.zero;
            //read keys
            foreach (KeyCode code in input.xPosInput) { //read x pos keys
                if (Input.GetKey(code)) { output.x += 1f; break; }
            }
            foreach (KeyCode code in input.xNegInput) { //read x min keys
                if (Input.GetKey(code)) { output.x -= 1f; break; }
            }
            foreach (KeyCode code in input.yPosInput) { //read y pos keys
                if (Input.GetKey(code)) { output.y += 1f; break; }
            }
            foreach (KeyCode code in input.yNegInput) { //read y min keys
                if (Input.GetKey(code)) { output.y -= 1f; break; }
            }
            //read input manager virtual axis
            output.x = GetLargestInput(input.xAxisCodes, output.x);
            output.y = GetLargestInput(input.yAxisCodes, output.y);
            //call event
            input.output?.Invoke(output);
        }
    }

    //=============== sort inputs ===============
    private float GetLargestInput(List<string> inputs, float compare)
    {
        float largest = compare;
        foreach (string s in inputs) {
            try {
                float input = Input.GetAxisRaw(s);
                if (Mathf.Abs(input) > Mathf.Abs(largest)) {
                    largest = input;
                }
            }
            catch { Debug.LogError("'" + s + "' is not a valid virtual axis code!\n" + transform.name); }
        }
        return largest;
    }
}
