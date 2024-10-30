using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GunData gunData;
    [SerializeField] private Transform cam;

    public Text ammoCount;
    public GameObject reloadUI;
    public GameObject crosshair;
   
    
    float timeSinceLastShot;

    public WeaponSway weaponSway;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
        gunData.currentAmmo = gunData.magSize;
        reloadUI.SetActive(false);
        crosshair.gameObject.SetActive(true);

    }

    private void OnDisable() => gunData.reloading = false;

    public void StartReload()
    {
        if (!gunData.reloading && this.gameObject.activeSelf)
        {
            // reload
            
            StartCoroutine(Reload());
            Debug.Log("Reloaded!");
            
        }
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;
        reloadUI.SetActive(true);
        StartCoroutine(weaponSway.PlayLoadingAnimation());

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;
        reloadUI.SetActive(false);
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(gunData.damage);
                    Debug.Log(hitInfo.transform.name);
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
            
        }
        
    }


    
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        Debug.DrawRay(cam.position, cam.forward * gunData.maxDistance);
        ammoCount.text = gunData.currentAmmo.ToString("F0");

        if (gunData.currentAmmo <= 0)
        {
            crosshair.gameObject.SetActive(false);
        }
        else
        {
            crosshair.gameObject.SetActive(true);
        }
    }

    private void OnGunShot()
    {
        // Implement any additional functionality for when the gun is shot
        FireWeapon();
    }

    private void FireWeapon()
    {
        weaponSway.ApplyRecoil();
    }

}
