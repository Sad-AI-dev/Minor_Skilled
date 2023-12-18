using Game.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("External Components")]
        public Camera cam;

        [HideInInspector] public Agent agent;
        private CharacterController cc;
        private GroundedManager groundedChecker;//
        private FrictionManager frictionManager;//
        private FOVManager fovManager;
        private PlayerRotationManager rotator;//
 
        private PlayerInput input;

        [Header("walk")]
        [SerializeField] private float deceleration;
        [SerializeField] private float acceleration;
        private float speed;
        private float walkSpeed { get { return agent.stats.walkSpeed; } }
        private float sprintSpeed { get { return agent.stats.sprintSpeed; } }
        private float speedMultiplier;
        private bool isSlowed = false;

        private Vector3 moveDirection;
        private Vector3 lastMoveDir;
        private Vector3 excessVelocity;

        [Header("Jump")]
        [SerializeField] private float JumpForce;
        [HideInInspector] public float yVelocity;
        [HideInInspector] public bool jumping;

        [Header("Gravity")]
        [SerializeField] private float gravity;
        [HideInInspector] public float activeGravity;
        [SerializeField] private float slowFallBounds;
        [SerializeField] private float slowFallMultiplier;
        [SerializeField] private float fastFallMultiplier;


        private Coroutine decelerateCo;
        private bool canDecelerate = true;

        public bool Grounded 
        {
            get 
            {
                if (groundedChecker == null)
                    return true;
                return groundedChecker.grounded;
            } 
        }
        
        private Coroutine slowCo;

        [HideInInspector] public UnityEvent startRunning;
        [HideInInspector] public UnityEvent stopRunning;
        [HideInInspector] public UnityEvent jump;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            cc = GetComponent<CharacterController>();
            agent = GetComponent<Agent>();
            groundedChecker = GetComponent<GroundedManager>();
            frictionManager = GetComponent<FrictionManager>();
            fovManager = GetComponent<FOVManager>();
            rotator = GetComponent<PlayerRotationManager>();
            input = GetComponent<PlayerInput>();
            agent.OnKnockbackReceived.AddListener(ReceiveKnockback);
        }

        private void FixedUpdate()
        {
            UpdateSpeed();
            if(excessVelocity.magnitude > 0)
                UpdateExcessVelocity();

            if (!groundedChecker.grounded)
                ApplyGravity();

            //allows to directly set position of player
            Physics.SyncTransforms();

            Vector3 walkVelocity = (moveDirection * speed) + excessVelocity;
            cc.Move(walkVelocity + new Vector3(0, yVelocity, 0));
            fovManager.UpdateFOV(walkVelocity.magnitude);

            groundedChecker.CheckGrounded();
        }

        public void SetMoveDirection(Vector2 moveInput)
        {
            if(moveInput.magnitude >= 0.1f)
            {
                float dirAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                
                if(!input.shooting)
                    rotator.RotatePlayer(dirAngle);

                moveDirection = Quaternion.Euler(0, dirAngle, 0) * Vector3.forward;
                moveDirection.Normalize();
                lastMoveDir = moveDirection;
                Accelerate(acceleration);
                startRunning.Invoke();
            }
            else if (speed > 0)
            {
                moveDirection = lastMoveDir;
                if (groundedChecker.grounded && canDecelerate)
                    decelerateCo = StartCoroutine(Decelerate(deceleration));
            }
        }

        public void Accelerate(float acceleration)
        {
            if(decelerateCo!= null)
            {
                StopCoroutine(decelerateCo);
                canDecelerate = true;
            }

            speedMultiplier += acceleration * Time.deltaTime;
            speedMultiplier = Mathf.Clamp(speedMultiplier, 0, 1);
        }

        private IEnumerator Decelerate(float deceleration)
        {
            canDecelerate = false;
            while (speedMultiplier > 0)
            {
                speedMultiplier -= deceleration * Time.deltaTime;
                yield return null;
            }
            speedMultiplier = 0;
            canDecelerate = true;
        }

        private void UpdateSpeed()
        {
            if(moveDirection.magnitude > 0.1f)
            {
                if (!isSlowed) speed = speedMultiplier * (walkSpeed * sprintSpeed / 100);
                if (isSlowed) speed = speedMultiplier * (walkSpeed / 100);

                //speed *= speedMultiplier;
            }

            if(speedMultiplier > 0)
                startRunning.Invoke();
            else
                stopRunning.Invoke();
        }

        public void Dash(Vector3 force)
        {
            if (groundedChecker.grounded)
                frictionManager.SetFriction(frictionTypes.dash);
            else
                frictionManager.SetFriction(frictionTypes.air);

            ReceiveKnockback(force);
        }

        private void UpdateExcessVelocity()
        {
            excessVelocity = frictionManager.ApplyVectorFriction(excessVelocity);
            if (excessVelocity.magnitude < 0.1f)
                excessVelocity = Vector3.zero;
        }

        public bool TryJump()
        {
            if (groundedChecker.grounded || (!groundedChecker.grounded && jumping && agent.stats.currentJumps > 0))
            {
                Jump();
                return true;
            }

            return false;
        }

        private void Jump()
        {
            yVelocity = 0;

            if (agent.stats.currentJumps < agent.stats.totalJumps)
                yVelocity += JumpForce * 1.5f / 100;
            else
                yVelocity += JumpForce / 100;

            jump.Invoke();
            jumping = true;
            frictionManager.SetFriction(frictionTypes.jump);
            agent.stats.currentJumps--;
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
