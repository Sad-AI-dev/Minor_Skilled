using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class SpellCasting : MonoBehaviour
{
    [SerializeField] private PlayerMana manaManager;
    [SerializeField] private GameObject model;
    
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

    private Camera cam;

    Vector3 localForward;

    private void Start()
    {
        cam = Camera.main;
    }

    public void Update()
    {
        //localForward = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.forward;


        if (canCast)
        {
            if (Input.GetKeyDown(KeyCode.Q) && manaManager.getMana() >= icicleSpell.manaCost)
            {
                //RotatePlayer();
                CastIcicleBurst();
            }
            if (Input.GetKeyDown(KeyCode.F) && manaManager.getMana() >= punchSpell.manaCost)
            {
                //RotatePlayer();
                StartCoroutine(CastMagicPunch());
            }
            if(Input.GetKeyDown(KeyCode.E) && manaManager.getMana() >= arrowSpell.manaCost)
            {
                //RotatePlayer();
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
        RotatePlayer();
        localForward = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y, 0) * Vector3.forward;
        GameObject fistObject = Instantiate(magicFist, transform.position + localForward, model.transform.rotation);
        fistObject.GetComponent<SpellScript>().damage = punchSpell.damage;
        manaManager.UseMana(punchSpell.manaCost);
        yield return new WaitForSeconds(0.5f);
        canCast = true;
    }

    IEnumerator SpawnIcicle()
    {
        canCast = false;
        yield return new WaitForSeconds(icicleCooldown);
        RotatePlayer();
        Quaternion icicleAngle = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y + angle, 0);
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
        RotatePlayer();
        localForward = Quaternion.Euler(0, model.transform.rotation.eulerAngles.y, 0) * Vector3.forward;
        GameObject arrowObject = Instantiate(arrow, transform.position + localForward * 2f, model.transform.rotation);
        arrowObject.transform.LookAt(GetAimLocation(40));
        arrowObject.GetComponent<SpellScript>().damage = arrowSpell.damage;
        manaManager.UseMana(arrowSpell.manaCost);
        yield return new WaitForSeconds(0.5f);
        canCast = true;
    }

    private void RotatePlayer()
    {
        model.transform.rotation = Quaternion.Euler(model.transform.eulerAngles.x, cam.transform.eulerAngles.y, model.transform.eulerAngles.z); 
    }

    private Vector3 GetAimLocation(float maxDistance)
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, maxDistance))
        {
            return hit.point;
        }

        return cam.transform.position + cam.transform.forward * maxDistance;
    }
}
