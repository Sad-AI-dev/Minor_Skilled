using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
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
        private Agent agent;
        private PlayerInput input;

        //animation parameter identifiers
        int running;
        int jump;
        int hit;
        int grenade;
        int interact;
        int dash;
        int die;
        int shoot;

        private void Start()
        {
            InitializeIdentifiers();

            input = GetComponent<PlayerInput>();
            input.stopShooting.AddListener(StopAnimPrimary);

            controller = GetComponent<PlayerController>();
            controller.startRunning.AddListener(AnimRun);
            controller.stopRunning.AddListener(StopAnimRun);
            controller.jump.AddListener(Jump);
        }

        private void InitializeIdentifiers()
        {
            running = Animator.StringToHash("isRunning");
            jump = Animator.StringToHash("Jump");
            hit = Animator.StringToHash("Hit");
            grenade = Animator.StringToHash("Grenade");
            interact = Animator.StringToHash("Interact");
            dash = Animator.StringToHash("Dash");
            die = Animator.StringToHash("Die");
            shoot = Animator.StringToHash("Shoot");
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

        public void AnimSecondary()
        {
            animator.SetTrigger(grenade);
        }
        public void AnimSpecial()
        {
            animator.SetTrigger(shoot);
        }
        public void AnimUtility()
        {
            animator.SetTrigger(dash);
        }

        public void AnimHit()
        {
            animator.SetTrigger(hit);
        }

        public void AnimDeath()
        {
            animator.SetTrigger(die);
        }

        public void AnimInteract()
        {
            animator.SetTrigger(interact);
        }

    }
}