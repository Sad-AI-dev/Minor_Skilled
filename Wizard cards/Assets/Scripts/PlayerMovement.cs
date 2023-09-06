using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float camRotationSpeed;
    [SerializeField] private Camera cam;
    private Vector3 inputDir;
    private float forwardAngle;
    private Rigidbody rb;

    Vector3 moveDir;

    private float turnSmoothVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        inputDir = new Vector3(horizontalInput, 0, verticalInput);
        inputDir.Normalize();

        if (inputDir.magnitude >= 0.1f)
        {
            forwardAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
            Debug.Log(forwardAngle);
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, forwardAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, forwardAngle, 0);

            moveDir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity =  moveDir.normalized * movementSpeed;
    }


}
