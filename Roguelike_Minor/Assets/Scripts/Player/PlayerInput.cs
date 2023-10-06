using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Agent agent;

        bool sprinting = false;
        float inaccuracy;
        float accuracy;

        private void Start()
        {
            
        }

        void Update()
        {
            WalkInput();
            SprintInput();
            JumpInput();
            AbilitiesInput();
        }
        
        private void WalkInput()
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            playerController.SetMoveDirection(moveInput);
        }

        private void SprintInput()
        {
            if(Input.GetKeyDown(KeyCode.LeftShift) && !sprinting)
            {
                playerController.Sprint(true);
                sprinting = true;
                return;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && sprinting)
            {
                playerController.Sprint(false);
                sprinting = false;
            }
        }

        private void JumpInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerController.Jump();
            }
        }

        private void AbilitiesInput()
        {
            if(Input.GetMouseButton(0))
            {
                agent.abilities.primary.TryUse();
                inaccuracy += Time.deltaTime;
 
                //agent.abilities.primary.vars["inaccuracy"] = inaccuracy;
            }
            if(Input.GetMouseButton(1))
            {
                //agent.abilities.
            }
        }
    }
}
