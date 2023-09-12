using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasicGunBehaviour : MonoBehaviour
{
    public Camera Cam;
    public GameObject GunPoint;
    public LayerMask ignoreLayer;

    [Header("Gun stats")]
    public int Damage;
    public float FireRate;
    public int MaxAmmo;
    public int Ammo;
    public float ReloadTime;
    public int Range;

    //Logic
    bool canShoot = true, reloadning;

    [Header("UI")]
    public TextMeshProUGUI AmmoText;

    [Header("Upgrade Cost")]
    public List<UpgradeCost> DamageUpgrade, FireRateUpgrade, MaxAmmoUpgrade, ReloadTimeUpgrade, RangeUpgrade;


    // Start is called before the first frame update
    void Start()
    {
        if (!Cam) Cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (reloadning) { AmmoText.text = "Reloading"; }
        else { AmmoText.text = $"{Ammo}/{MaxAmmo}"; }

        if(canShoot && Input.GetMouseButtonDown(0) && !reloadning && !Game.instance.Pause)
        {
            canShoot = false;
            StartCoroutine(Shoot());
        }

        if(Ammo <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            if (!reloadning && !Game.instance.Pause)
            {
                reloadning = true;
                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator Shoot()
    {
        ShootLogic();
        GunPoint.SetActive(true);
        Ammo--;
        yield return new WaitForSeconds(0.1f);
        GunPoint.SetActive(false);
        yield return new WaitForSeconds(FireRate-0.1f);
        canShoot = true;
    }
    void ShootLogic()
    {
        Ray rc = Cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(rc, out hit, Range, ~ignoreLayer))
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<Enemy>().TakeDamage(Damage);
            }
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(ReloadTime);
        reloadning = false;
        Ammo = MaxAmmo;
    }
}
