using UnityEngine;
using System.Collections;

public class CameraCoroutine : MonoBehaviour
{
    public float interpolateSmoothing = 1.0f;

    public float rotateSmoothing = 1.0f;

    public Transform target;
    public Transform Character1;
    public void OnButtonClick()
    {
        StartCoroutine(MoveCameraCo(target));
    }
    
    public void OnButtonClick2()
    {
        StartCoroutine(MoveCameraCo2(Character1));
    }
    private IEnumerator MoveCameraCo(Transform Character1)
    {

        while (Vector3.Distance(transform.position, Character1.position) > 3.0f)
        {

            transform.position = Vector3.Lerp(transform.position, Character1.position, interpolateSmoothing * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator MoveCameraCo2(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 2.5f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, interpolateSmoothing * Time.deltaTime);
            transform.position = transform.position + new Vector3(0, 0.5f, -1f) * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotateSmoothing * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 30, 0);
            yield return null;
        }
        
    }

}
