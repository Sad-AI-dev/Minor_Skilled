using Codice.Client.Common.GameUI;
using Game.Core;
using Game.Core.GameSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Player {
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInput : MonoBehaviour
    {
        private PlayerController playerController;
        [SerializeField] private Agent agent;
        [SerializeField] private Interactor interactor;
        [SerializeField] private GameObject inventory;
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private CameraController camController;
        private PlayerRotationManager rotator;
        private FOVManager fovManager;

        [SerializeField] private Camera cam;

        [Header("Variables")]
        [SerializeField] private int jumpBufferFrames;
        private Coroutine jumpBufferCoroutine;

        [Header("Audio")]
        [SerializeField] private AudioPlayer audioPlayer;

        private bool canPlayFootstep = true;
        private bool gamePaused = false;
        [HideInInspector] public bool shooting;
        
        public UnityEvent stopShooting;
        public UnityEvent<float> AdjustRunAnimSpeed;

        private void Start()
        {
            playerController = GetComponent<PlayerController>();
            fovManager = GetComponent<FOVManager>();
            rotator = GetComponent<PlayerRotationManager>();
            //hide inventory by default
            inventory.SetActive(false);
        }

        void Update()
        {
            pauseInput();

            gamePaused = pauseMenu.paused;

            if (gamePaused) return;
            WalkInput();
            JumpInput();
            AbilitiesInput();
            InteractInput();
            InventoryInput();
        }
        
        private void WalkInput()
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            playerController.SetMoveDirection(moveInput);

            if (canPlayFootstep && moveInput.magnitude >= 0.1f)
            {
                StartCoroutine(FootstepCo());
            }

            if (!shooting)
            {
                AdjustRunAnimSpeed.Invoke(1);
                //walking = false;
            }   

            if(shooting)
            {
                if(moveInput.y < 0)
                    AdjustRunAnimSpeed.Invoke(-0.5f);
                else
                    AdjustRunAnimSpeed.Invoke(0.5f);

                //walking = true;
            }
        }

        private void JumpInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(jumpBufferCoroutine != null)
                    StopCoroutine(jumpBufferCoroutine);

                jumpBufferCoroutine = StartCoroutine(JumpBufferCo());
            }
        }

        private void AbilitiesInput()
        {
            if(Input.GetMouseButton(0))
            {
                if (!shooting)
                    shooting = true;

                fovManager.SetMinFOV();
                fovManager.lockFOV = true;

                rotator.RotatePlayer(cam.transform.eulerAngles.y);
                
                agent.abilities.primary.TryUse();
            }
            if(!Input.GetMouseButton(0) && shooting)
            {
                shooting = false;
                fovManager.ResetFOV();
                stopShooting.Invoke();
            }
            if (Input.GetMouseButtonDown(1))
            {
                agent.abilities.secondary.TryUse();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                agent.abilities.special.TryUse();
            }
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                agent.abilities.utility.TryUse();
            }
        }

        private void InteractInput()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactor.TryInteract();
            }
        }

        private void InventoryInput()
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                inventory.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                camController.LockCamera();
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                inventory.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                camController.UnlockCamera();
            }
        }

        private void pauseInput()
        {
            if (Input.GetKeyUp(KeyCode.Escape) && !gamePaused)
            {
                gamePaused = true;
                pauseMenu.ActivateMenu();
            }
            else if(Input.GetKeyUp(KeyCode.Escape) && gamePaused)
            {
                gamePaused = false;
                pauseMenu.DeactivateMenu();
            }
        }

        private IEnumerator JumpBufferCo()
        {
            int framesPassed = 0;
            while (framesPassed < jumpBufferFrames)
            {
                framesPassed++;
                if(playerController.TryJump())
                {
                    break;
                }
                yield return null;
            }
        }

        private IEnumerator FootstepCo()
        {
            canPlayFootstep = false;
            yield return new WaitForSeconds(0.5f);
            audioPlayer.Play();
            canPlayFootstep = true;
        }        
    }
}
