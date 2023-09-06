using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement2 : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] private float speed;

    Vector3 dir;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        dir = new Vector3(horizontal, 0f, vertical).normalized;
        
        if(dir.magnitude >= 0.1f)
        {
            
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = dir * speed * Time.deltaTime;
    }
}
