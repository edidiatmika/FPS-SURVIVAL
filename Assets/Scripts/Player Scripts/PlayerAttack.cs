using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weaponManager;

    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;
    
    [SerializeField]
    private Animator zoomCameraAnim;
    private bool zoomed;

    private Camera mainCam;

    private GameObject crosshair;

    private bool isAiming;

    [SerializeField]
    private GameObject arrowPrefab, spearPrefab;

    [SerializeField]
    private Transform arrowSpearStartPosition;


    private void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        
        //zoomCameraAnim = transform.Find(Tags.lookRoot).transform.Find(Tags.zoomCamera).GetComponent<Animator>();

        crosshair = GameObject.FindWithTag(Tags.crosshair);

        mainCam = Camera.main;
    }
   
    void Start()
    {
        
    }

    void Update()
    {
        WeaponShoot();

        ZoomInAndOut();
    }

    void WeaponShoot()
    {
        if (weaponManager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            if (Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                
                BulletFired();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (weaponManager.GetCurrentSelectedWeapon().tag == Tags.axeTag)
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
                    if (isAiming)
                    {
                        weaponManager.GetCurrentSelectedWeapon().ShootAnimation();

                        if(weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            ThrowArrowOrSpear(true);
                        }
                        else if (weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                            ThrowArrowOrSpear(false);
                        }
                    }
                }
            }
        }
    }

    void ZoomInAndOut()
    {
        if (weaponManager.GetCurrentSelectedWeapon().weaponAim == WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomCameraAnim.Play(AnimationTags.zoomInAnim);

                crosshair.SetActive(false);
            }
            
            if (Input.GetMouseButtonUp(1))
            {
                zoomCameraAnim.Play(AnimationTags.zoomOutAnim);

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

    void ThrowArrowOrSpear(bool throwArrow)
    {
        if (throwArrow)
        {
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.transform.position = arrowSpearStartPosition.position;

            arrow.GetComponent<ArrowSpearScripts>().Launch(mainCam);
        }
        else
        {
            GameObject spear = Instantiate(spearPrefab);
            spear.transform.position = arrowSpearStartPosition.position;

            spear.GetComponent<ArrowSpearScripts>().Launch(mainCam);
        }
    }

    void BulletFired()
    {
        RaycastHit hit;

        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {
            print("WE HIT:" + hit.transform.gameObject.name);

            if(hit.transform.tag == Tags.enemyTag)
            {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }
        }
    }
}
