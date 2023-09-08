using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Loot", menuName = "Scriptable-Objects/Loot" )]
public class Loot_SO : ScriptableObject
{
    public string Name;
    public float Value;
    public string Description;
    public Sprite UISprite;
    public bool QuestItem;
}
