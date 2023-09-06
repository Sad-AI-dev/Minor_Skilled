using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1;
    //movement vars
    private float speed;

    [Header("Rotation Settings")]
    public float rotateSpeed = 0.5f;
    private float rotateVelocity;

    [Header("Technical Settings")]
    //external components
    public ObjectDetector groundDetector;
    private Rigidbody rb;
    private Transform cam;

    private void Start()
    {
        speed = moveSpeed;
        //get external components
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        //TEMP
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveDir = InputToCamSpace(GetMoveInput());
        //move player
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z) * (Time.deltaTime * 100 * speed);
        Rotate(moveDir);
    }
    private void Rotate(Vector3 moveDir)
    {
        float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotateVelocity, rotateSpeed);
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private Vector3 InputToCamSpace(Vector2 input)
    {
        return cam.right * input.x + cam.forward * input.y;
    }

    //=============== Inputs ===============
    private Vector2 GetMoveInput()
    {
        return new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        ).normalized;
    }
}
