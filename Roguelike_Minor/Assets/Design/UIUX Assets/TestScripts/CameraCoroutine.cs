using UnityEngine;
using System.Collections;

public class CameraCoroutine : MonoBehaviour
{
    public float interpolateSmoothing = 1.0f;
    public Transform target;

    public void OnButtonClick()
    {
        StartCoroutine(MoveCameraCo(target));
    }

    private IEnumerator MoveCameraCo(Transform target)
    {

        while (Vector3.Distance(transform.position, target.position) > 3.0f)
        {

            transform.position = Vector3.Lerp(transform.position, target.position, interpolateSmoothing * Time.deltaTime);

            yield return null;
        }
    }
}
