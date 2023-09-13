using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class SpellCasting : MonoBehaviour
{
    [SerializeField] PlayerMana manaManager;

    [Header("Magic Punch")]
    [SerializeField] private GameObject magicFist;
    [SerializeField] private Spell punchSpell;


    [Header("Icicle Burst")]
    [SerializeField] private GameObject icicle;
    [SerializeField] private Spell icicleSpell;
    [SerializeField] private float angleDelta;
    [SerializeField] private float icicleCooldown;

    [Header("Magic Arrow")]
    [SerializeField] private GameObject arrow;
    [SerializeField] private Spell arrowSpell;


    private float angle = 0;
    private bool canCast = true;

    Vector3 localForward;

    public void Update()
    {
        localForward = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.forward;


        if (canCast)
        {
            if (Input.GetKeyDown(KeyCode.Q) && manaManager.getMana() >= icicleSpell.manaCost)
            {
                CastIcicleBurst();
            }
            if (Input.GetKeyDown(KeyCode.F) && manaManager.getMana() >= punchSpell.manaCost)
            {
                StartCoroutine(CastMagicPunch());
            }
            if(Input.GetKeyDown(KeyCode.E) && manaManager.getMana() >= arrowSpell.manaCost)
            {
                StartCoroutine(CastMagicArrow());
            }
        }
    }

    void CastIcicleBurst()
    {
        manaManager.UseMana(icicleSpell.manaCost);
        if (angle < 360)
        {
            StartCoroutine(SpawnIcicle());
        }
    }

    IEnumerator CastMagicPunch()
    {
        canCast = false;
        GameObject fistObject = Instantiate(magicFist, transform.position + localForward, transform.rotation);
        fistObject.GetComponent<SpellScript>().damage = punchSpell.damage;
        manaManager.UseMana(punchSpell.manaCost);
        yield return new WaitForSeconds(0.5f);
        canCast = true;
    }

    IEnumerator SpawnIcicle()
    {
        canCast = false;
        yield return new WaitForSeconds(icicleCooldown);
        Quaternion icicleAngle = Quaternion.Euler(0, transform.rotation.eulerAngles.y + angle, 0);
        Vector3 spawnPos = transform.position + (icicleAngle * Vector3.forward);
        GameObject icicleObject = Instantiate(icicle, spawnPos, icicleAngle);
        icicleObject.GetComponent<SpellScript>().damage = icicleSpell.damage;
        angle += angleDelta;
        
        if(angle <= 360)
        {
            StartCoroutine(SpawnIcicle());
        }
        if(angle > 360)
        {
            yield return new WaitForSeconds(0.2f);
            angle = 0;
            canCast = true;
        }
    }

    IEnumerator CastMagicArrow()
    {
        canCast = false;
        GameObject arrowObject = Instantiate(arrow, transform.position + localForward * 2f, transform.rotation);
        arrowObject.GetComponent<SpellScript>().damage = arrowSpell.damage;
        manaManager.UseMana(arrowSpell.manaCost);
        yield return new WaitForSeconds(0.5f);
        canCast = true;
    }
}
