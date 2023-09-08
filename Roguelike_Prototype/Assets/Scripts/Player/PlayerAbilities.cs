using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [Header("General Stats")]
    public float attackSpeed;
    public float damage;

    [Header("Shoot Settings")]
    public LineRenderer line;
    public float lineShowTime = 0.2f;
    public Transform shootPoint;

    //external components
    private Camera cam;
    //shoot vars
    private bool canShoot;

    private void Start()
    {
        //abilities should be off cooldown on game start
        canShoot = true;
        //get external components
        cam = Camera.main;
        line.enabled = false;
    }

    //=============== Shoot Ability ================
    public void Shoot()
    {
        if (canShoot) {
            TryHurt();
        }
    }

    private void TryHurt() 
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f)); //shoot ray from center of screen
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
            StartCoroutine(ShowShootVisuals(hit.point));
            //attempt deal damage
            if (hit.transform.TryGetComponent(out HealthManager health)) {
                health.TakeDamage(damage);
            }
        }
        else { StartCoroutine(ShowShootVisuals(cam.transform.forward * 10000f)); }
        //cooldown
        StartCoroutine(ShootCooldownCo());
    }

    private IEnumerator ShowShootVisuals(Vector3 hitPoint)
    {
        line.SetPositions(new Vector3[] { shootPoint.position, hitPoint });
        line.enabled = true;
        yield return new WaitForSeconds(lineShowTime);
        line.enabled = false;
    }

    private IEnumerator ShootCooldownCo()
    {
        canShoot = false;
        yield return new WaitForSeconds(attackSpeed / 60f);
        canShoot = true;
    }
}
