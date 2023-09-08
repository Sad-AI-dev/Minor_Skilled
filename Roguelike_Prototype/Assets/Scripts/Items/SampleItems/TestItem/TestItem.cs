using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestItem : Item
{
    public float initialSpeedGain = 2f;
    public float stackSpeedGain = 1f;

    //========= override gain object ============
    public override void AddNewEffect(Player player)
    {
        player.movement.moveSpeed += initialSpeedGain;
    }

    public override void AddStack(Player player)
    {
        player.movement.moveSpeed += stackSpeedGain;
    }

    //========= override remove object ===========
    public override void RemoveEffect(Player player)
    {
        player.movement.moveSpeed -= initialSpeedGain;
    }

    public override void RemoveStack(Player player)
    {
        player.movement.moveSpeed -= stackSpeedGain;
    }
}
