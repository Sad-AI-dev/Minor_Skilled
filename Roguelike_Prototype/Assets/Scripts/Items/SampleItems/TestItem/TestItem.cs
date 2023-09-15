using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestItem : Item
{
    public float stackSpeedGain = 2f;

    //========= override gain object ============
    public override void AddStack(Player player)
    {
        player.movement.moveSpeed += stackSpeedGain;
    }

    //========= override remove object ===========
    public override void RemoveStack(Player player)
    {
        player.movement.moveSpeed -= stackSpeedGain;
    }
}
