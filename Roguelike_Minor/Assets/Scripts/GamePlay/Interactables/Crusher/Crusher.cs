using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    [RequireComponent(typeof(BoxCollider))]
    public class Crusher : MonoBehaviour
    {
        [Header("Detection Settings")]
        [SerializeField] private LayerMask mask;

        [Header("SlotPiece Settings")]
        [SerializeField] private GameObject itemTemplate;
        [SerializeField] private ItemDataSO slotPieceData;
        [SerializeField] private float yOffset = 0.2f;

        //vars
        private BoxCollider boxCollider;

        private void Start()
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        //======== Crush Item Logic ==========
        public void Crush(Interactor interactor)
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
                transform.position + boxCollider.center,
                boxCollider.size / 2,
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
            Vector3 centerPos = transform.position + boxCollider.center;
            Vector3 randomOffset = new Vector3(
                Random.Range(-boxCollider.size.x / 2f, boxCollider.size.x / 2f), 
                0f, 
                Random.Range(-boxCollider.size.z / 2f, boxCollider.size.z / 2f)
            );
            return new Vector3(centerPos.x, transform.position.y + yOffset, centerPos.z) + randomOffset;
        }
    }
}
