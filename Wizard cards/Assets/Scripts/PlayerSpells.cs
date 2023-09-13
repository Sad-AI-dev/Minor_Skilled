using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class PlayerSpells : MonoBehaviour
{
    [Header("Magic punch")]
    [SerializeField] private GameObject magicFist;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float manaCost;
    [SerializeField] private float range;
    [SerializeField] private float destroyDelay;

    private Vector3 localForward;
    GameObject currentMF;

    public bool casting = false;

    private void Update()
    {
        localForward = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.forward;
        CastMagicPunch();
        
        
    }

    private void CastMagicPunch()
    {
        Vector3 destination = localForward;
        Vector3 start = transform.position;
        if (Input.GetMouseButtonDown(0) && !casting)
        {
            casting = true;
            currentMF = Instantiate(magicFist, transform.localPosition + localForward, transform.rotation);
            destination = localForward * range;
            start = currentMF.transform.TransformPoint(currentMF.transform.position);
            currentMF.GetComponent<Rigidbody>().AddForce(localForward * speed, ForceMode.Impulse);
        }

        if(currentMF != null)
        {
            if ((start - currentMF.transform.position).magnitude >= range)
            {
                StartCoroutine(destroyFist(currentMF));
            }
        }
    }

    IEnumerator destroyFist(GameObject fist)
    {
        fist.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(destroyDelay);
        casting = false;
        Destroy(fist);
    }
}
