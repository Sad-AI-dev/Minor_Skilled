using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotVisuals : MonoBehaviour
{
    public List<Image> visuals;
    public Image mediumVisual;

    public GameObject AssignVisuals(Item item) //returns the gameObject visual was assigned to
    {
        if (item.size == ItemSlot.SlotSize.small) {
            Image img = GetAvailableImage();
            FillSmallVisual(item, img);
            return img.gameObject;
        }
        else {
            FillMediumVisual(item);
            return mediumVisual.gameObject;
        }
    }

    private void FillSmallVisual(Item item, Image img)
    {
        img.enabled = true;
        img.sprite = item.inventoryVisuals;
    }

    private void FillMediumVisual(Item item)
    {
        mediumVisual.enabled = true;
        mediumVisual.sprite = item.inventoryVisuals;
    }

    private Image GetAvailableImage()
    {
        for (int i = 0; i < visuals.Count; i++) {
            if (!visuals[i].enabled) return visuals[i];
        }
        return null;
    }

    //========== Hide ==========
    public void HideVisuals()
    {
        HideSmallVisuals();
        HideMediumVisuals();
    }

    private void HideSmallVisuals()
    {
        foreach (Image img in visuals) {
            img.enabled = false;
        }
    }
    private void HideMediumVisuals()
    {
        if (mediumVisual) {
            mediumVisual.enabled = false;
        }
    }
}
