using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    public class Crusher : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Interactable interactable;

        [Header("Detection Settings")]
        [SerializeField] private BoxCollider detectCollider;
        [SerializeField] private LayerMask mask;

        [Header("Animation Settings")]
        [SerializeField] private BoxCollider blockCollider;
        [SerializeField] private Animator animator;
        [SerializeField] private string animationClip;

        [Header("Timings")]
        [SerializeField] private float swapDelay = 0.5f;
        [SerializeField] private float blockedDelay = 1f;

        [Header("SlotPiece Settings")]
        [SerializeField] private GameObject itemTemplate;
        [SerializeField] private ItemDataSO slotPieceData;
        [SerializeField] private float yOffset = 0.2f;

        //vars
        private bool isCrushing;

        private void Start()
        {
            //initialize vars
            interactable.enabled = true;
            isCrushing = false;
            blockCollider.enabled = false;
            //setup animator
            animator.enabled = false;
        }

        //======== Crush Item Logic ==========
        public void Crush(Interactor interactor)
        {
            if (!isCrushing) { StartCoroutine(CrushCo()); }
        }

        private IEnumerator CrushCo()
        {
            InitializeCrushVars();
            //play animation
            PlayAnimation();
            yield return new WaitForSeconds(swapDelay);
            //crush items
            SwapItems();
            yield return new WaitForSeconds(blockedDelay);
            //reset
            ResetCrushVars();
        }

        private void InitializeCrushVars()
        {
            isCrushing = true;
            blockCollider.enabled = true;
            interactable.SetActive(false); //hide UI label while not available
        }
        private void ResetCrushVars()
        {
            blockCollider.enabled = false;
            isCrushing = false;
            animator.enabled = false;
            interactable.SetActive(true); //show UI label
        }

        private void PlayAnimation()
        {
            animator.enabled = true;
            animator.Play(animationClip);
        }

        private void SwapItems()
        {
            List<ItemDataSO> items = GetItemsInRange();
            int pieceValue = ItemsToPieceValue(items);
            CreateSlotPieces(pieceValue);
        }

        //===== Find Objects =====
        private List<ItemDataSO> GetItemsInRange()
        {
            List<ItemDataSO> itemsInRange = new List<ItemDataSO>();
            //find items
            Collider[] results = Physics.OverlapBox(
                transform.position + detectCollider.center,
                detectCollider.size / 2,
                transform.rotation,
                mask,
                QueryTriggerInteraction.Collide
            );
            //sort results
            foreach (Collider col in results)
            {
                if (col.TryGetComponent(out ItemPickup pickup))
                {
                    //ignore slotPieces
                    if (pickup.item == slotPieceData) { continue; }
                    //register normal items
                    itemsInRange.Add(pickup.item);
                    //destroy item
                    Destroy(pickup.gameObject);
                }
            }
            //return results
            return itemsInRange;
        }

        //===== Convert to pieceValue ====
        private int ItemsToPieceValue(List<ItemDataSO> items)
        {
            int value = 0;
            foreach (ItemDataSO item in items)
            {
                value += item.size.capacity;
            }
            return value;
        }

        //======== Create Slot Pieces ========
        private void CreateSlotPieces(int pieceValue)
        {
            for (int i = 0; i < pieceValue; i++)
            {
                ItemPickup pickup = Instantiate(itemTemplate).GetComponent<ItemPickup>();
                pickup.item = slotPieceData;
                pickup.transform.position = GetRandomBoundsPosition();
            }
        }

        private Vector3 GetRandomBoundsPosition()
        {
            Vector3 centerPos = transform.position + detectCollider.center;
            Vector3 randomOffset = new Vector3(
                Random.Range(-detectCollider.size.x / 2f, detectCollider.size.x / 2f), 
                0f, 
                Random.Range(-detectCollider.size.z / 2f, detectCollider.size.z / 2f)
            );
            return new Vector3(centerPos.x, transform.position.y + yOffset, centerPos.z) + randomOffset;
        }
    }
}
