using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MedTestItem : Item
{
    public float damageIncrease = 2f;

    public override void AddStack(Player player)
    {
        player.abilities.damage += damageIncrease;
    }

    public override void RemoveStack(Player player)
    {
        player.abilities.damage -= damageIncrease;
    }
}
