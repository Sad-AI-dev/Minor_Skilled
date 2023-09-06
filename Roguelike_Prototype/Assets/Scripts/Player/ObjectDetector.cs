using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectDetector : MonoBehaviour
{
    public enum FilterMode {
        None,
        WhiteList,
        BlackList
    }

    [Header("Events")]
    public UnityEvent onDetectFirstObject;
    public UnityEvent onDetectObject;
    public UnityEvent onLeaveLastObject;

    [Header("Filter Settings")]
    [Tooltip("Determines how detected objects are filtered.\n\n" +
        "None: objects will not be filtered.\n" +
        "WhiteList: objects will be ignored, unless their tag is in the tagsToFilter list.\n" +
        "BlackList: objects will be ignored if their tag is in the tagsToFilter list.")]
    public FilterMode filterMode;
    public List<string> tagsToFilter;

    //vars
    private List<GameObject> register;

    private void Start() {
        register = new();
    }

    //==================================== 3D detection ====================================
    private void OnTriggerEnter(Collider collision)
    {
        if (ValidObjectCheck(collision.transform)) {
            onDetectObject?.Invoke();
            if (register.Count <= 0) { onDetectFirstObject?.Invoke(); } //check if detected is first object
            if (!register.Contains(collision.gameObject)) { register.Add(collision.gameObject); } //register object
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (ValidObjectCheck(collision.transform)) {
            register.Remove(collision.gameObject); //remove object from register
            if (register.Count <= 0) { onLeaveLastObject?.Invoke(); } //check if last object was removed
        }
    }

    //===================== util =====================
    private bool ValidObjectCheck(Transform toCheck)
    {
        return filterMode switch {
            FilterMode.WhiteList => IsInTagList(toCheck),
            FilterMode.BlackList => !IsInTagList(toCheck),
            _ => true,
        };
    }

    private bool IsInTagList(Transform toCheck)
    {
        foreach (string tag in tagsToFilter) {
            if (toCheck.CompareTag(tag)) { return true; }
        }
        return false;
    }
}
