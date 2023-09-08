using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasicGunBehaviour : MonoBehaviour
{
    public Camera Cam;
    public GameObject GunPoint;

    [Header("Gun stats")]
    public int Damage;
    public float FireRate;
    public int MaxAmmo;
    public int Ammo;
    public int ReloadTime;
    public int Range;

    //Logic
    bool canShoot = true, reloadning;

    [Header("UI")]
    public TextMeshProUGUI AmmoText;


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

        if(canShoot && Input.GetMouseButtonDown(0) && !reloadning)
        {
            canShoot = false;
            StartCoroutine(Shoot());
        }

        if(Ammo <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            if (!reloadning)
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
        RaycastHit hit;
        if (Physics.Raycast(Cam.transform.position, Camera.main.transform.forward, out hit, Range))
        {
            Debug.DrawRay(Cam.transform.position, Camera.main.transform.forward, Color.red, 100);
            Debug.Log(hit.transform);
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<Enemy>().TakeDamage(Damage);
            }
        }

        if(Cam.)
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(ReloadTime);
        reloadning = false;
        Ammo = MaxAmmo;
    }
}
