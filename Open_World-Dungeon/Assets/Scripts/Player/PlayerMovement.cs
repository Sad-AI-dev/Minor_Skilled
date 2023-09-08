using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera Cam;
    public Rigidbody rb;

    public float Speed = 4;
    public float SprintSpeed = 7;

    Vector3 input;

    public float JumpHeight;

    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    [Range(0.1f, 9f)] [SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)] [SerializeField] float yRotationLimit = 88f;

    Vector2 rotation = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        if(!rb) rb = GetComponent<Rigidbody>();
        if(!Cam) Cam = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        //Forward input
        Vector3 x = transform.forward * ver;
        Vector3 z = transform.right * hor;

        input = x + z;
        
        if(Input.GetKey(KeyCode.LeftShift)) rb.AddForce(input.normalized * SprintSpeed);
        else rb.AddForce(input.normalized * Speed);

        //Mouse movement
        rotation.x += Input.GetAxis("Mouse X") * sensitivity;
        rotation.y += Input.GetAxis("Mouse Y") * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        Cam.transform.localRotation = yQuat;
        transform.localRotation = xQuat; //Quaternions seem to rotate more consistently than EulerAngles. Sensitivity seemed to change slightly at certain degrees using Euler. transform.localEulerAngles = new Vector3(-rotation.y, rotation.x, 0);

    }

    private void FixedUpdate()
    {
        //Movement
    }
}
