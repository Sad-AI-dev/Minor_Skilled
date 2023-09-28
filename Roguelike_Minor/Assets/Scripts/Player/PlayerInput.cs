using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        
        void Update()
        {
            CheckMoveInput();

            if(Input.GetKeyDown(KeyCode.Space))
            {
                playerController.Jump();
            }
        }

        private void CheckMoveInput()
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            playerController.SetMoveDirection(moveInput);
        }
    }
}
