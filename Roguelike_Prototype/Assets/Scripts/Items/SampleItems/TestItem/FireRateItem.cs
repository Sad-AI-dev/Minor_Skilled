using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FireRateItem : Item
{
    public float stackAttackSpeed = 2f;

    //========= override gain object ============
    public override void AddStack(Player player)
    {
        player.abilities.attackSpeed += stackAttackSpeed;
    }

    //========= override remove object ===========
    public override void RemoveStack(Player player)
    {
        player.abilities.attackSpeed -= stackAttackSpeed;
    }
}
