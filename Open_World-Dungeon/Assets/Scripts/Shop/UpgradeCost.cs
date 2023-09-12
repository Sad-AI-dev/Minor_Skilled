using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Scriptable-Objects/upgrade")]
public class UpgradeCost : ScriptableObject
{
    [Tooltip("The amount you want the upgrade to increase or decrease")]
    public float Amount;
    public List<ItemAmount> cost = new List<ItemAmount>();
}
