using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArrow : SpellScript
{

    Vector3 startPos;
    
    private void Start()
    {
        startPos = transform.position;
    }

    public override void DamageMultiplier()
    {
        float distance = (transform.GetChild(0).position - startPos).magnitude;

        if(distance >= 1)
        {
            damage = Mathf.RoundToInt(damage * distance / 2f);
        }
        
    }
}
