using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1;
    public float sprintMultiplier = 1.5f;
    //movement vars
    private float speed;
    private bool isSprinting;

    [Header("Rotation Settings")]
    public float rotateSpeed = 0.5f;
    private float rotateVelocity;

    [Header("Jump Settings")]
    public float jumpHeight = 1;
    public float gravityScale = 1f;
    private bool isGrounded;

    [Header("Technical Settings")]
    //external components
    public ObjectDetector groundDetector;
    public Transform visuals;
    private Rigidbody rb;
    private Transform cam;

    private void Start()
    {
        speed = moveSpeed;
        //get external components
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        cam = Camera.main.transform;
        //setup ground detector
        groundDetector.onDetectObject.AddListener(OnTouchGround);
        groundDetector.onLeaveLastObject.AddListener(OnLeaveGround);
        //TEMP
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }
        UpdateSprintState();
    }

    private void FixedUpdate()
    {
        Move();
        ApplyGravity();
    }

    //================== Movement =======================
    private void Move()
    {
        Vector3 moveDir = InputToCamSpace(GetMoveInput()) * (Time.deltaTime * 100 * speed);
        //move player
        rb.AddForce(new Vector3(moveDir.x, rb.velocity.y, moveDir.z));
        if (moveDir.magnitude > 0f) {
            Rotate(moveDir);
        }
    }
    private void Rotate(Vector3 moveDir)
    {
        float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(visuals.eulerAngles.y, targetAngle, ref rotateVelocity, 1 / rotateSpeed);
        visuals.rotation = Quaternion.Euler(0, angle, 0);
    }

    private Vector3 InputToCamSpace(Vector2 input)
    {
        return cam.right * input.x + cam.forward * input.y;
    }

    //================ Sprint =================
    private void UpdateSprintState()
    {
        if (!isSprinting && CanSprint() && Input.GetKeyDown(KeyCode.LeftControl)) {
            ToggleSprint();
        }
        else if (isSprinting && (!CanSprint() || Input.GetKeyDown(KeyCode.LeftControl))) {
            ToggleSprint();
        }
    }
    private bool CanSprint() {
        return Input.GetKey(KeyCode.W);
    }

    private void ToggleSprint()
    {
        isSprinting = !isSprinting;
        speed = isSprinting ? sprintMultiplier * moveSpeed : moveSpeed;
    }

    //================= Jump ========================
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
        isGrounded = false;
    }
    
    private void OnTouchGround() 
    {
        isGrounded = true;
    }

    private void OnLeaveGround() 
    {
        isGrounded = false;
    }

    //================= Gravity ===============
    private void ApplyGravity()
    {
        rb.AddForce(Vector3.down * gravityScale);
    }

    //=============== Inputs ===============
    private Vector2 GetMoveInput()
    {
        return new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;
    }
}
