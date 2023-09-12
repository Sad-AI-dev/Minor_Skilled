using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    [Header("Slot Holders")]
    public RectTransform smallSlotHolder;
    public RectTransform mediumSlotHolder;

    [Header("Prefabs")]
    public GameObject smallSlotPrefab;
    public GameObject mediumSlotPrefab;

    //external components
    [HideInInspector] public ItemInventory inventory;
    private Dictionary<GameObject, Item> objLinks = new();

    //==================== Inventory Interaction ====================================
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && EventSystem.current.IsPointerOverGameObject()) {
            DropItemCheck();
        }
    }

    private void DropItemCheck()
    {
        //ray cast pointer
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, hits);

        //check results
        for (int i = 0; i < hits.Count; i++) {
            if (hits[i].gameObject.CompareTag("ItemSlot") && objLinks.ContainsKey(hits[i].gameObject)) {
                DropItem(objLinks[hits[i].gameObject]);
            }
        }
    }

    private void DropItem(Item item)
    {
        inventory.DropItem(item);
    }

    //=================== Manager inventory visuals ====================================
    public void GenerateVisuals()
    {
        DestroyVisuals();
        objLinks = new Dictionary<GameObject, Item>();
        //generate slot visuals
        for (int i = 0; i < inventory.slots.Count; i++) {
            GenerateSlot(inventory.slots[i]);
        }
    }

    //=========== Destroy Visuals =============
    private void DestroyVisuals()
    {
        foreach (RectTransform rt in smallSlotHolder) {
            Destroy(rt.gameObject);
        }
        foreach (RectTransform rt in mediumSlotHolder) {
            Destroy(rt.gameObject);
        }
    }

    //=========== Generate Visuals ============
    private void GenerateSlot(ItemSlot slot)
    {
        ItemSlotVisuals visuals = CreateSlotVisual(
            slot.size == ItemSlot.SlotSize.small ? smallSlotPrefab : mediumSlotPrefab,
            slot.size == ItemSlot.SlotSize.small ? smallSlotHolder : mediumSlotHolder
            );
        visuals.HideVisuals();
        for (int i = 0; i < slot.contents.Count; i++) {
            GameObject assignedObj = visuals.AssignVisuals(slot.contents[i]);
            objLinks.Add(assignedObj, slot.contents[i]);
        }
    }

    private ItemSlotVisuals CreateSlotVisual(GameObject prefab, RectTransform holder)
    {
        GameObject obj = Instantiate(prefab, holder);
        ItemSlotVisuals visuals =  obj.GetComponent<ItemSlotVisuals>();
        return visuals;
    }
}
