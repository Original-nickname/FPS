using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weaponManager;

    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;

    public const string Axe = "AXE_TAG";
    public const string Crosshair = "CROSSHAIR";
    public const string ZoomIn = "ZoomIn";
    public const string ZoomOut = "ZoomOut";
    public const string Enemy = "Enemy";

    private Animator zoomCameraAnim;
    private bool zoomed;
    private Camera mainCamera;
    private GameObject crosshair;
    private bool isAiming;

    [SerializeField]
    private GameObject arrowPrefab, spearPrefab;

    [SerializeField]
    private Transform arrowBowStartPosition;

    public bool isBowActive = true;

    void Awake()
    {

        weaponManager = GetComponent<WeaponManager>();
        zoomCameraAnim = transform.GetChild(0)
                                  .transform
                                  .GetChild(0)
                                  .GetComponent<Animator>();
        crosshair = GameObject.FindWithTag(Crosshair);
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
    }

    private void ZoomInAndOut()
    {
        if (weaponManager.GetCurrentSelectedWeapon().weaponAim == WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomCameraAnim.Play(ZoomIn);
                crosshair.SetActive(false);
            }

            if (Input.GetMouseButtonUp(1))
            {
                zoomCameraAnim.Play(ZoomOut);
                crosshair.SetActive(true);
            }
        }
        
        if (weaponManager.GetCurrentSelectedWeapon().weaponAim == WeaponAim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                weaponManager.GetCurrentSelectedWeapon().Aim(true);
                isAiming = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                weaponManager.GetCurrentSelectedWeapon().Aim(false);
                isAiming = false;
            }
        }
    }

    private void WeaponShoot()
    {
        if(weaponManager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            if(Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                BulletFired();
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (weaponManager.GetCurrentSelectedWeapon().tag == Axe)
                {
                    weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                }

                if (weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                    BulletFired();
                }
                else
                {
                    if (isAiming && isBowActive)
                    {
                        
                        weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                        isBowActive = false;
                        //Invoke(nameof(ActivateBow), 2);
                        if(weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            ThrowArrowOrSpear(true);
                        }
                        else if(weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                            ThrowArrowOrSpear(false);
                        }
                    }
                }
            }
        }
    }

    public void ActivateBow()
    {
        isBowActive = true;
    }

    private void ThrowArrowOrSpear(bool isArrow)
    {
        if (isArrow)
        {
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.transform.position = arrowBowStartPosition.position;
            arrow.GetComponent<ArrowBowScript>().Launch(mainCamera);
        } 
        else
        {
            GameObject spear = Instantiate(spearPrefab);
            spear.transform.position = arrowBowStartPosition.position;
            spear.GetComponent<ArrowBowScript>().Launch(mainCamera);
        }
    }

    private void BulletFired()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
        {
            hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            Debug.Log("We hit: " + hit.transform.gameObject.name);
        }
    }
}
