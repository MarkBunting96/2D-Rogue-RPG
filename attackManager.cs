//Title         : AttackManager.cs
//Author        : M.K.Bunting
//Date          : 09/05/2017
//Last Modified : 09/05/2017

//Purpose: Code used to control how the player attacks.

//Default Unity includes:
using UnityEngine;
using System.Collections;

//My includes:
//N/A

//attackManager class which contains Start function, fixed
//update function and an OnTriggerStay2D function.
public class attackManager : MonoBehaviour
{

    public bool attack;                                                                //private bool attack, true when the player will attack

    private float knockback;                                                            //private float knockback, will contain the force of knockback applied after an attack

    private float damage;                                                               //private float damage, will contain the damage that this weapon does

    public GameObject projectile;

    private float fireRate;                                                                           //the rate which the player can shoot.
    private float nextShot;                                                                           //when the player can next fire.

    private float attackRate;                                                                           //the rate which the player can attack.
    private float nextAttack;                                                                           //when the player can next attack.

    public AudioSource swordSwing;

    public AudioSource bowRelease;


    //start function, initialises knockback to 150000
    void Start()
    {
        knockback = 150000;
        damage = 0;

        fireRate = 1.0f;                                                                              //fire rate is set to 0.5.
        nextShot = fireRate;                                                                          //next shot is set to the fire rate.

        attackRate = 1.0f;                                                                              //fire rate is set to 0.5.
        nextAttack = attackRate;                                                                        //next shot is set to the fire rate.
    }

    //fixed update function which assigns attack to true when the Attack button is pressed.
    void FixedUpdate()
    {
        if (Time.time > nextAttack && Input.GetButtonDown("Attack"))
        {
            nextAttack = Time.time + attackRate;

            attack = true;
        }
        else
        {
            attack = false;
        }

        if (attack && GameManager.instance.thePlayer.activeWeapon == PlayerController.Weapon.OAKSHORTBOW)
        {
            //if (Time.time > nextShot)
            //{
            //    nextShot = Time.time + fireRate;
                Instantiate(projectile);
                bowRelease.Play();
            //}

        }

    }

    //GetWeaponDamage, returns the damage value depending on the players
    //active weapon
    public float GetWeaponDamage()
    {
        switch (GameManager.instance.thePlayer.activeWeapon)
        {
            case PlayerController.Weapon.SHORTSWORD:
                damage = (int)Random.Range(19f, 24f);
                break;
            case PlayerController.Weapon.IRONDAGGER:
                damage = (int)Random.Range(9f, 14f);
                break;
            case PlayerController.Weapon.OAKSHORTBOW:
                damage = (int)Random.Range(24f, 29f);
                break;
            case PlayerController.Weapon.STEELGREATAXE:
                damage = (int)Random.Range(19f, 24f);
                break;
            case PlayerController.Weapon.IRONCLAWS:
                damage = (int)Random.Range(19f, 24f);
                break;
            case PlayerController.Weapon.SHORTSPEAR:
                damage = (int)Random.Range(9f, 14f);
                break;
            case PlayerController.Weapon.BOARSPEAR:
                damage = (int)Random.Range(14f, 19f);
                break;
            case PlayerController.Weapon.FIREBALL:
                damage = (int)Random.Range(19f, 24f);
                break;
            case PlayerController.Weapon.NONE:
                damage = 0.0f;
                break;
            default:
                break;
        }


        return damage;
    }

    public bool isWeaponMelee()
    {
        if  (GameManager.instance.thePlayer.activeWeapon == PlayerController.Weapon.SHORTSWORD ||
             GameManager.instance.thePlayer.activeWeapon == PlayerController.Weapon.IRONDAGGER ||
             GameManager.instance.thePlayer.activeWeapon == PlayerController.Weapon.STEELGREATAXE ||
             GameManager.instance.thePlayer.activeWeapon == PlayerController.Weapon.IRONCLAWS ||
             GameManager.instance.thePlayer.activeWeapon == PlayerController.Weapon.SHORTSPEAR ||
             GameManager.instance.thePlayer.activeWeapon == PlayerController.Weapon.BOARSPEAR)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    
    //onTriggerStay2D which takes a collider, called when the collider is
    //triggered.
    void OnTriggerStay2D(Collider2D other)
    {
        //if the other object has the "NPC" tag and attack is true, apply the force
        //to knockback the other object
        if (other.tag == "NPC" && attack && isWeaponMelee())
        {
            //if (Time.time > nextAttack)
            //{
            //nextAttack = Time.time + attackRate;
            swordSwing.Play();
                other.attachedRigidbody.AddForce(-transform.up * knockback);
            //}

        }
    }
}
