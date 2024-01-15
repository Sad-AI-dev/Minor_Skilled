using Game.Core;
using Game.Core.GameSystems;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Player {
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInput : MonoBehaviour
    {
        private PlayerController playerController;
        [SerializeField] private Agent agent;
        [SerializeField] private Interactor interactor;
        [SerializeField] private InventoryUI inventory;
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private CameraController camController;
        [SerializeField] private SettingsSO settings;
        private PlayerRotationManager rotator;
        private FOVManager fovManager;

        [SerializeField] private Camera cam;

        [Header("Variables")]
        [SerializeField] private int jumpBufferFrames;
        private Coroutine jumpBufferCoroutine;

        private bool gamePaused = false;
        [HideInInspector] public bool shooting;
        
        public UnityEvent stopShooting;
        public UnityEvent<float> AdjustRunAnimSpeed;

        private float delayFOVChange = 0;

        private void Awake()
        {
            EventBus<SceneLoadedEvent>.AddListener(OnSceneLoad);
        }

        private void OnDestroy()
        {
            EventBus<SceneLoadedEvent>.RemoveListener(OnSceneLoad);
        }

        private void OnSceneLoad(SceneLoadedEvent eventData)
        {
            if(inventory.gameObject.activeSelf)
            {
                inventory.gameObject.SetActive(false);
                camController.UnlockCamera();
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void Start()
        {
            playerController = GetComponent<PlayerController>();
            fovManager = GetComponent<FOVManager>();
            rotator = GetComponent<PlayerRotationManager>();
            //hide inventory by default
            inventory.gameObject.SetActive(false);

            agent.abilities.utility.onUse.AddListener((Ability abilty) => rotator.RotatePlayer(cam.transform.eulerAngles.y));
            agent.abilities.secondary.onUse.AddListener((Ability abilty) => rotator.RotatePlayer(cam.transform.eulerAngles.y));
            agent.abilities.special.onUse.AddListener((Ability abilty) => rotator.RotatePlayer(cam.transform.eulerAngles.y));
        }

        void Update()
        {
            PauseInput();

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
            Vector2 moveInput = GetMoveInput();
            playerController.SetMoveDirection(moveInput);

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
        private Vector2 GetMoveInput()
        {
            Vector2 dir = new Vector2();
            //read inputs
            if (Input.GetKey(settings.keyBinds[InputBinding.Forward])) { dir.y += 1f; }
            if (Input.GetKey(settings.keyBinds[InputBinding.Left])) { dir.x -= 1f; }
            if (Input.GetKey(settings.keyBinds[InputBinding.Right])) { dir.x += 1f; }
            if (Input.GetKey(settings.keyBinds[InputBinding.Backward])) { dir.y -= 1f; }
            //return result
            return dir;
        }

        private void JumpInput()
        {
            if (Input.GetKeyDown(settings.keyBinds[InputBinding.Jump]))
            {
                if(jumpBufferCoroutine != null)
                    StopCoroutine(jumpBufferCoroutine);

                jumpBufferCoroutine = StartCoroutine(JumpBufferCo());
            }
        }

        private void AbilitiesInput()
        {
            if (Input.GetKeyDown(settings.keyBinds[InputBinding.Primary]))
            {
                if (!shooting)
                    shooting = true;

                if (delayFOVChange < 0.2f)
                    delayFOVChange += Time.deltaTime;
                else
                {
                    fovManager.SetMinFOV();
                    fovManager.lockFOV = true;
                }

                rotator.RotatePlayer(cam.transform.eulerAngles.y);
                
                agent.abilities.primary.TryUse();
            }
            if(!Input.GetKeyDown(settings.keyBinds[InputBinding.Primary]) && shooting)
            {
                delayFOVChange = 0;
                shooting = false;
                fovManager.ResetFOV();
                stopShooting.Invoke();
            }
            if (Input.GetKeyDown(settings.keyBinds[InputBinding.Secondary]))
            {
                agent.abilities.secondary.TryUse();
            }
            if (Input.GetKeyDown(settings.keyBinds[InputBinding.Special]))
            {
                agent.abilities.special.TryUse();
            }
            if(Input.GetKeyDown(settings.keyBinds[InputBinding.Utility]))
            {
                agent.abilities.utility.TryUse();
            }
        }

        private void InteractInput()
        {
            if (Input.GetKeyDown(settings.keyBinds[InputBinding.Interact]))
            {
                interactor.TryInteract();
            }
        }

        private void InventoryInput()
        {
            //open / close inventory
            if(Input.GetKeyDown(settings.keyBinds[InputBinding.OpenInventory]))
            {
                inventory.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                camController.LockCamera();
                //disable interactor while inventory is open
                interactor.enabled = false;
            }
            if (Input.GetKeyUp(settings.keyBinds[InputBinding.OpenInventory]))
            {
                inventory.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                camController.UnlockCamera();
                //re-enable interactor
                interactor.enabled = true;
            }

            //drop item input
            if (inventory.gameObject.activeSelf && Input.GetKeyDown(settings.keyBinds[InputBinding.DropItem]))
            {
                inventory.TryDropItem();
            }
        }

        private void PauseInput()
        {
            if (Input.GetKeyUp(KeyCode.Escape) && !gamePaused)
            {
                gamePaused = true;
                pauseMenu.ActivateMenu();

                if (inventory.gameObject.activeSelf)
                {
                    inventory.gameObject.SetActive(false);
                    camController.UnlockCamera();
                }
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

        //private void SetTimeScale()
        //{
        //    if(Input.GetKeyDown(KeyCode.P))
        //    {
        //        if (Time.timeScale == 1)
        //            Time.timeScale = 0;
        //        else
        //            Time.timeScale = 1;
        //    }

        //    if (Input.GetKeyDown(KeyCode.Period))
        //        Time.timeScale += 0.05f;
        //    if (Input.GetKeyDown(KeyCode.Comma))
        //        Time.timeScale -= 0.05f;
        //}

        //=========== Manage Game Paused ===================
        public void PauseGame(bool paused)
        {
            gamePaused = paused;
        }
    }
    }

