using Game.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject visuals;
        [HideInInspector] public Agent agent;
        
        [Header("External Components")]
        public Camera cam;
        private GroundedChecker groundedChecker;
        private FrictionManager frictionManager;


        [Header("walk")]
        private float speed;
        private float walkSpeed { get { return agent.stats.walkSpeed; } }
        private float sprintSpeed { get { return agent.stats.sprintSpeed; } }
        [SerializeField] private float deceleration;
        [SerializeField] private float acceleration;
        private float speedMultiplier;
        private bool isSlowed = false;

        private Vector3 moveDirection;
        private Vector3 walkVelocity;
        private Vector3 lastMoveDir;
        private Vector3 excessVelocity;

        [Header("Jump")]
        [SerializeField] private float JumpForce;
        [SerializeField] private float gravity;
        [SerializeField] private float slowFallBounds;
        [SerializeField] private float slowFallMultiplier;
        [SerializeField] private float fastFallMultiplier;
        [HideInInspector] public float yVelocity;
        [HideInInspector] public float activeGravity;
        [HideInInspector] public bool grounded;
        [HideInInspector] public bool jumping;
        
        private CharacterController cc;

        private float smoothVelocity;
        private float smoothTime = 0.1f;
        
        private Coroutine slowCo;
        private Coroutine kbReset;

        public UnityEvent startRunning;
        public UnityEvent stopRunning;
        public UnityEvent jump;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            //cam = Camera.main;
            cc = GetComponent<CharacterController>();
            agent = GetComponent<Agent>();
            groundedChecker = GetComponent<GroundedChecker>();
            frictionManager = GetComponent<FrictionManager>();
            agent.OnKnockbackReceived.AddListener(ReceiveKnockback);
        }

        private void FixedUpdate()
        {
            UpdateSpeed();
            if(excessVelocity.magnitude > 0)
                UpdateExcessVelocity();

            if (!grounded)
                ApplyGravity();

            //allows to directly set position of player
            Physics.SyncTransforms();
            
            cc.Move((moveDirection * speed) + excessVelocity + new Vector3(0, yVelocity, 0));

            groundedChecker.CheckGrounded();
        }

        public void SetMoveDirection(Vector2 moveInput)
        {
            if(moveInput.magnitude >= 0.1f)
            {
                float dirAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(visuals.transform.eulerAngles.y, dirAngle, ref smoothVelocity, smoothTime);
                visuals.transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
                moveDirection = Quaternion.Euler(0, dirAngle, 0) * Vector3.forward;
                moveDirection.Normalize();
                lastMoveDir = moveDirection;  
                Accelerate(acceleration);
                startRunning.Invoke();
            }
            else if (lastMoveDir != Vector3.zero && speed > 0)
            {
                moveDirection = lastMoveDir;
                if(grounded)
                    Decelerate(deceleration);
            }
        }

        public void Accelerate(float acceleration)
        {
            speedMultiplier += acceleration * Time.deltaTime;
            speedMultiplier = Mathf.Clamp(speedMultiplier, 0, 1);
        }

        public void Decelerate(float deceleration)
        {
            speedMultiplier -= deceleration * Time.deltaTime;
            speedMultiplier = Mathf.Clamp(speedMultiplier, 0, 1);
        }

        private void UpdateSpeed()
        {
            if(moveDirection.magnitude > 0.1f)
            {
                if (!isSlowed) speed = walkSpeed * sprintSpeed;
                if (isSlowed) speed = walkSpeed;

                speed *= speedMultiplier;
            }

            if(speed > 0)
                startRunning.Invoke();
            else
                stopRunning.Invoke();
            
            speed /= 100;
        }

        public void Dash(Vector3 force)
        {
            if (grounded)
                frictionManager.SetFriction(frictionTypes.dash);
            else
                frictionManager.SetFriction(frictionTypes.air);

            ReceiveKnockback(force);
        }

        private void UpdateExcessVelocity()
        {
            excessVelocity = frictionManager.ApplyFriction(excessVelocity);
            if (excessVelocity.magnitude < 0.1f)
                excessVelocity = Vector3.zero;
        }

        public void Jump()
        {
            if (!grounded || jumping) return;
            jump.Invoke();
            yVelocity = 0;
            yVelocity += JumpForce / 100;
            jumping = true;
            frictionManager.SetFriction(frictionTypes.jump);
        }

        private void ApplyGravity()
        {
            if (yVelocity > -slowFallBounds && yVelocity < slowFallBounds && jumping) activeGravity = gravity * slowFallMultiplier;
            else if (yVelocity < 0 && jumping) activeGravity = gravity * fastFallMultiplier;
            else activeGravity = gravity;

            yVelocity -= activeGravity / 100;
        }

        public void ResetVelocity()
        {
            speed = 0;
            yVelocity = 0;
        }

        public void StartSlowCoroutine(float duration)
        {
            if(slowCo != null)
                StopCoroutine(slowCo);
            slowCo = StartCoroutine(SlowPlayerCo(duration));
        }

        private IEnumerator SlowPlayerCo(float duration)
        {
            isSlowed = true;
            yield return new WaitForSeconds(duration);
            isSlowed = false;
        }

        public void ReceiveKnockback(Vector3 kbForce)
        {
            excessVelocity += kbForce;
            jumping = false;
        }
    }
}
