using UnityEngine;
using Game.Core;

namespace Game.Player
{
    public enum Animations
    {
        running,
        jump,
        hit,
        grenade,
        interact,
        dash,
        die,
        shoot
    }

    public class AnimationManager : MonoBehaviour
    {
        [SerializeField] Animator animator;
        private PlayerController controller;
        private GroundedManager groundedChecker;
        private Agent agent;
        private PlayerInput input;

        //animation parameter identifiers
        int running;
        int jump;
        int grounded;
        int hit;
        int grenade;
        int interact;
        int dashing;
        int die;
        int shoot;
        int special;
        int runSpeed;
     

        private void Start()
        {
            InitializeIdentifiers();

            groundedChecker = GetComponent<GroundedManager>();
            groundedChecker.TouchedGround.AddListener(SetAnimGrounded);
            groundedChecker.LeftGround.AddListener(SetAnimNotGrounded);

            input = GetComponent<PlayerInput>();
            input.stopShooting.AddListener(StopAnimPrimary);
            input.AdjustRunAnimSpeed.AddListener(SetRunAnimSpeed);

            controller = GetComponent<PlayerController>();
            controller.startRunning.AddListener(AnimRun);
            controller.stopRunning.AddListener(StopAnimRun);
            controller.jump.AddListener(Jump);
            controller.startDashing.AddListener(StartDash);
            controller.stopDashing.AddListener(StopDash);
        }

        private void InitializeIdentifiers()
        {
            running = Animator.StringToHash("isRunning");
            jump = Animator.StringToHash("Jump");
            grounded = Animator.StringToHash("Grounded");
            hit = Animator.StringToHash("Hit");
            grenade = Animator.StringToHash("Grenade");
            interact = Animator.StringToHash("Interact");
            dashing = Animator.StringToHash("Dashing");
            die = Animator.StringToHash("Die");
            shoot = Animator.StringToHash("Shoot");
            special = Animator.StringToHash("Special");
            runSpeed = Animator.StringToHash("RunSpeed");
        }


        private void SetRunAnimSpeed(float value)
        {
            animator.SetFloat(runSpeed, value);
        }
        private void SetAnimGrounded()
        {
            animator.SetBool(grounded, true);
        }
        private void SetAnimNotGrounded()
        {
            animator.SetBool(grounded, false);
        }

        private void AnimRun()
        {
            animator.SetBool(running, true);
        }
        private void StopAnimRun()
        {
            animator.SetBool(running, false);
        }
        private void Jump()
        {
            animator.SetTrigger(jump);
        }
        public void AnimPrimary()
        {
            animator.SetBool(shoot, true);
        }
        private void StopAnimPrimary()
        {
            animator.SetBool(shoot, false);
        }

        private void AnimSecondary()
        {
            animator.SetTrigger(grenade);
        }
        private void AnimSpecial()
        {
            animator.SetTrigger(special);
        }
        private void StartDash()
        {
            animator.SetBool(dashing, true);
        }

        private void StopDash()
        {
            animator.SetBool(dashing, false);
        }

        private void AnimHit()
        {
            animator.SetTrigger(hit);
        }

        private void AnimDeath()
        {
            animator.SetTrigger(die);
        }

        private void AnimInteract()
        {
            animator.SetTrigger(interact);
        }

    }
}
