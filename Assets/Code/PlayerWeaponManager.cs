﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerWeaponManager : MonoBehaviour
{
    public float ReloadTime;
    public int AmmoPoolSize;
    public int CurrentAmmo;
    public float DelayBetweenShots;
    public float BulletSpeed;
    public GameObject ProjectilePrefab;
    public Transform ShootPoint;
    public float ProjectileSpreadAngle;
    
  
   

    public UnityAction OnShoot;
    
    private PlayerInputHandler inputHandler;
    private float lastTimeShot;
    private bool IsReloading;

    // Start is called before the first frame update
    void Start()
    {
        CurrentAmmo = AmmoPoolSize; //automatically set player's ammo as full at the start
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {

        //The "!IsReloading" makes sure that the coroutine doesn't happen multiple times during a reload 
        if (!IsReloading && (inputHandler.GetReloadWeaponHeldDown() || CurrentAmmo <= 0)) {
            StartCoroutine(ReloadWeapon());
            IsReloading = true;
        }

        if (inputHandler.GetFireInputHeld())
        {
            TryShoot();
        }




    }
    


    void TryShoot()
    {

        // Shoot if able to (not reloading, has ammo, and has waited the delay between shots)
        if (!IsReloading && CurrentAmmo > 0 && lastTimeShot + DelayBetweenShots < Time.time)
        {
            // Decrease ammo by the amount of projectiles shot
            CurrentAmmo -= 1;

            // For now, projectiles shoot straight forward, no spread yet
            Vector2 shootDirection = inputHandler.GetLookInput();
            GameObject projectile = Instantiate(ProjectilePrefab, ShootPoint.position, transform.rotation,
                    this.gameObject.transform);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), projectile.GetComponent<Collider2D>());

            // Give velocity to the projectile
            projectile.GetComponent<Rigidbody2D>().velocity = shootDirection * BulletSpeed;

            // Invoke OnShoot for any additional events/effects we want to happen
            if (OnShoot != null)
            {
                OnShoot.Invoke();
            }

            // Update last time shot to now
            lastTimeShot = Time.time;
        }

    }
    
    
    
    
    //"IEnumerator" indicates that this function is a coroutin
    IEnumerator ReloadWeapon() 
    {

        //initiate reload animation somehow***

        float TimeOfLastReload = Time.time;

        //coroutine line
        yield return new WaitForSeconds(ReloadTime);


        /* this code is probably also ok
        while (Time.time < TimeOfLastReload + ReloadTime)
        {
            IsReloading = true;
        }
        */

        IsReloading = false;




    }

}
