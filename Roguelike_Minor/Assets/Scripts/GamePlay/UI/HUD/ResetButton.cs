using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    [RequireComponent(typeof(Animator))]
    public class ResetButton : MonoBehaviour
    {
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
            animator.keepAnimatorStateOnDisable = true;
        }

        private void OnDisable()
        {
            animator.Play("Normal");
        }
    }
}
