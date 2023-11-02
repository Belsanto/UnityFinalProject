using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject muzzlePoint;
    [SerializeField] private GameObject muzzleFireEffect;
    [SerializeField] private GameObject bloodParticles;
    [SerializeField] private GameObject lightParticles;
    [SerializeField] private AudioClip shootS;
    [SerializeField] private AudioClip reloadS;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;

    [SerializeField] private Transform playerRotation;

    [SerializeField] private Camera fpsCam;



    [SerializeField] public int maxAmmo = 10; // Maximum ammo capacity
    [SerializeField] private int currentAmmo; // Current ammo count
    [SerializeField] public float fireRate = 0.2f; // Time between shots
    [SerializeField] private float nextFireTime = 0f;
    [SerializeField] public float reloadTime = 2f; // Time it takes to reload
    [SerializeField] private bool isReloading = false;
    private AudioSource audioComponent;



    private void Start()
    {
        fpsCam = Camera.main;
        currentAmmo = maxAmmo;
        audioComponent = GetComponent<AudioSource>();
    }

    private void Update()
    {


        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && !isReloading)
        {
            if(currentAmmo > 0)
            {
                Shoot();
            } else
            {
                Reload();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void Shoot()
    {
        currentAmmo--;
        nextFireTime = Time.time + fireRate;

        RaycastHit hit;

        gameObject.GetComponent<Animation>().Play("PistolGun");
        //Quaternion.Euler(180f, 0f, 0f)
        Quaternion newRot = muzzlePoint.transform.rotation * Quaternion.Euler(0f, -90f, 0f);

        GameObject fireEffect = Instantiate(muzzleFireEffect, muzzlePoint.transform.position, newRot) as GameObject;
        fireEffect.GetComponent<ParticleSystem>().Play();
        Destroy(fireEffect, 1f);

        audioComponent.PlayOneShot(shootS);


        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
//            Debug.Log(hit.transform.name);

            Enemy target = hit.transform.GetComponent<Enemy>();
            if (target != null)
            {
                Quaternion hitRotation = Quaternion.LookRotation(hit.normal);
                GameObject bloodEffect = Instantiate(bloodParticles, hit.point, hitRotation) as GameObject;
                bloodEffect.GetComponent<ParticleSystem>().Play();
                Destroy(bloodEffect, 1f);
                target.TakeDamage(damage);
            } else
            {
                Quaternion hitRotation1 = Quaternion.LookRotation(hit.normal);
                GameObject lightPart = Instantiate(lightParticles, hit.point, hitRotation1) as GameObject;
                lightPart.GetComponent<ParticleSystem>().Play();
                Destroy(lightPart, 1f);
            }

        }
    }

    void Reload()
    {
        if (currentAmmo < maxAmmo)
        {
            isReloading = true;
            gameObject.GetComponent<Animation>().Play("ReloadPistol");

            audioComponent.PlayOneShot(reloadS);
            // You can add reload animations or effects here
            // Reset the current ammo after the specified reload time
            Invoke("FinishReload", reloadTime);
        }
    }

    void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
