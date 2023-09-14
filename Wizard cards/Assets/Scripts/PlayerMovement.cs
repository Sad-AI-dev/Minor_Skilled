using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerSpells spells;


    [SerializeField] private GameObject model;
    [SerializeField] private LayerMask mask;    

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;

    [Header("Physics")]
    [SerializeField] private float gravity;

    [Header("Camera")]
    [SerializeField] private float smoothTime = 0.1f;


    private float currentSpeed;

    private float smoothVelocity;
    
    Vector3 dir;
    Vector3 moveDir;

    private Camera cam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        spells = GetComponent<PlayerSpells>();

        Cursor.lockState = CursorLockMode.Locked;

        cam = Camera.main;
    }
    
    void Update()
    {
        Walk();
        Sprint();
        Jump();
    }

    private void Walk()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        dir = new Vector3(horizontal, 0f, vertical).normalized;

        if (dir.magnitude >= 0.1f)
        {
            float dirAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, dirAngle, ref smoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            moveDir = Quaternion.Euler(0, dirAngle, 0) * Vector3.forward;
        }
    }

    void Sprint()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && Grounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void ApplyGravity()
    {
        rb.AddForce(new Vector3(0, -gravity, 0), ForceMode.Force);
    }

    private void FixedUpdate()
    {
        if(dir.magnitude >= 0.1f)
        {
            moveDir *= currentSpeed;
            rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
        }

        ApplyGravity();
    }

    private bool Grounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.up * -1, out hit, 1.1f, mask))
        {
            return true;
        }

        Debug.DrawRay(transform.position, Vector3.up * -0.6f);
        return false;
    }
}
