using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour {

    [Header("References")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform cam;

    private string AMMOCOUNTTEXT = "Ammo: {n}/{p}";
    private string GUNNAME = "{name}";

    [SerializeField] private TextMeshProUGUI ammoCountText;
    [SerializeField] private TextMeshProUGUI gunNameText;

    float timeSinceLastShot;

    private void Start() {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }

    private void OnDisable() => gunData.reloading = false;

    public void StartReload() {
        if(!gunData.reloading && this.gameObject.activeSelf)
            StartCoroutine(Reload());
    }

    private IEnumerator Reload() {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Shoot() {
        if(gunData.currentAmmo > 0) {
            if(CanShoot()) {
                if(Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gunData.maxDistance)) {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(gunData.damage);
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        } else {
            StartReload();
        }
    }

    private void FixedUpdate() {
        string gunName = GUNNAME.Replace("{name}", gunData.name);
        string ammoCount = AMMOCOUNTTEXT.Replace("{n}", gunData.currentAmmo.ToString());
        ammoCount = ammoCount.Replace("{p}", gunData.magSize.ToString());

        ammoCountText.text = ammoCount;
        gunNameText.text = gunName;
    }

    private void Update() {
        timeSinceLastShot += Time.deltaTime;

        Debug.DrawRay(cam.position, cam.forward * gunData.maxDistance);
    }

    private void OnGunShot() { }
}
