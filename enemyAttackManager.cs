//Title         : enemyAttackManager.cs
//Author        : M.K.Bunting
//Date          : 15/05/2017
//Last Modified : 15/05/2017

//Purpose: Code used to control how the enemy attacks

//Default Unity includes:
using UnityEngine;
using System.Collections;

//My includes:
//N/A

//enemyAttackManager class which contains Start function, fixed
//update function and an OnTriggerStay2D function.
public class enemyAttackManager : MonoBehaviour
{

    private float damage;                                                               //private float damage, will contain the damage that this weapon does

    public GameObject projectile;

    private enemyController thisEnemy;

    private float attackRate;                                                                           //the rate which the enemy can attack.
    private float nextAttack;                                                                           //when the enemy can next attack.

    private float fireRate;                                                                           //the rate which the enemy can shoot.
    private float nextShot;                                                                           //when the enemy can next fire.

    public AudioSource swordSwing;

    public AudioSource bowRelease;

    //start function, gets enemy parent, sets damage, and the attack and fire rates
    void Start()
    {
        thisEnemy = GetComponentInParent<enemyController>();

        damage = 0;

        attackRate = 1.0f;                                                                              //fire rate is set to 0.5.
        nextAttack = attackRate;                                                                        //next shot is set to the fire rate.

        fireRate = 1.0f;                                                                              //fire rate is set to 0.5.
        nextShot = attackRate;                                                                        //next shot is set to the fire rate.
    }

    //fixed update function which checks if ranged enemies are in range of the enemy and shoots
    void FixedUpdate()
    {

        if (thisEnemy.playerSpotted && thisEnemy.GetDistance() <= 90f && thisEnemy.activeWeapon == enemyController.Weapon.OAKSHORTBOW)
        {
            thisEnemy.rb2d.MovePosition(thisEnemy.transform.position);
            if (Time.time > nextShot)
            {
                nextShot = Time.time + fireRate;
                Instantiate(projectile, transform);
                bowRelease.Play();
            }

        }

    }

    //GetWeaponDamage, returns the damage value depending on the enemies
    //active weapon
    public float GetWeaponDamage()
    {
        switch (thisEnemy.activeWeapon)
        {
 
            case enemyController.Weapon.OAKSHORTBOW:
                damage = (int)Random.Range(24f, 29f);
                break;
            case enemyController.Weapon.SHORTSPEAR:
                damage = (int)Random.Range(9f, 14f);
                break;
            case enemyController.Weapon.NONE:
                damage = 0.0f;
                break;
            default:
                break;
        }


        return damage;
    }



    //onTriggerStay2D which takes a collider, called when the collider is
    //triggered.
    void OnTriggerStay2D(Collider2D other)
    {
        //if the other collider is the player and this enemy has a melee weapon
        //attack the player.
        if (other.gameObject.tag == "Player" && thisEnemy.activeWeapon == enemyController.Weapon.SHORTSPEAR)
        {
            if (Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;
                GameManager.instance.activateDamageText(other.gameObject.transform.position, GetWeaponDamage());
                GameManager.instance.thePlayer.hit.pitch = Random.Range(0.8f, 1.5f);
                GameManager.instance.thePlayer.hit.Play();
                swordSwing.Play();
            }
        }

    }
}
