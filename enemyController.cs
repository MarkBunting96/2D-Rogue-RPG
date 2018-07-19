//Title 		: enemyController.cs
//Author 		: M.K.Bunting
//Date 			: 11/05/2017
//Last Modified : 15/05/2017

//Modification  : Added stats
//By            : M.K.Bunting

//Modification  : Added a patrolling movement and an attacking method
//By            : M.K.Bunting


//Purpose: This script contains all the functions dealing with enemy movement, stats and taking damage

//Default Unity Includes:
using UnityEngine;
using System.Collections;

//My Includes:
// N/A


//Public class containing the functions controlling player
//movement and encumberence.
public class enemyController : MonoBehaviour
{

    //Different types of weapons the enemy have
    public enum Weapon
    {
        SHORTSWORD, IRONDAGGER, OAKSHORTBOW, HEALTHPOTION, STEELGREATAXE,
        IRONCLAWS, SHORTSPEAR, BOARSPEAR, FIREBALL, NONE
    };

    //enemies active weapon
    public Weapon activeWeapon;

    //Health of the enemy
    private float health;

    //if the player has been spotted
    public bool playerSpotted;

    //if a random location has been chosen
    private bool randLocChosen;

    //Stats component of the enemy
    public Attributes stats;

    //random location that the player will move towards
    private Vector3 randomLoc;

    //radius of the enemys sight
    private float radius;

    // max speed.
    public float speed;

    public float posXRange;             //The float which stores how far the enemy can wander on the x Axis

    public float negXRange;             //The float which stores how far the enemy can wander on the x Axis

    public float posYRange;             //The float which stores how far the enemy can wander on the y axis

    public float negYRange;             //The float which stores how far the enemy can wander on the y axis

    private bool rangesChosen;          //If the range the enemy can patrol has been assigned

    public float minDelayTime;          //The float which assigns the minimum delay between the enemy wandering

    public float maxDelayTime;          //The float which assigns the maximum delay between the enemy wandering

    //Rigidbody component for use with collision and drag.
    public Rigidbody2D rb2d;

    private AudioSource hit;

    //Initialises the rigidbody component, radius, booleans, delay times and ranges
    //for wandering
    void Start()
    {
        pickRandomWeapon();

        hit = GetComponent<AudioSource>();

        rb2d = GetComponent<Rigidbody2D>();

        health = 100.0f;

        speed = 100.0f;

        radius = 160f;

        playerSpotted = false;

        randLocChosen = false;

        posXRange = transform.position.x + radius;

        negXRange = transform.position.x - radius;

        posYRange = transform.position.y + radius;

        negYRange = transform.position.y - radius;

        rangesChosen = true;

        minDelayTime = 5f;

        maxDelayTime = 10f;

    }//Start

    //generates a random number between 1 and 100, if
    //50 or below, weapon is melee, if more than 50, 
    //weapon is ranged.
    private void pickRandomWeapon()
    {
        float randWeapon = Random.Range(1f, 100f);

        if (randWeapon <= 80f)
        {
            activeWeapon = Weapon.SHORTSPEAR;
        }
        else if (randWeapon > 80f)
        {
            activeWeapon = Weapon.OAKSHORTBOW;
        }
        
    }

    //Minuses damage from the health of the enemy.
    public void TakeDamage(float damage)
    {
        health = health - damage;
    }
    
    //Checks if the enemy has reached the target destination and returns true or false
    public bool ReachedLocation()
    {
        if ((transform.position.x >= randomLoc.x - 5 && transform.position.y >= randomLoc.y - 5) ||
            (transform.position.x <= randomLoc.x + 5 && transform.position.y <= randomLoc.y + 5))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //GetWeaponDamage, returns the damage value depending on the players
    //active weapon
    public float GetWeaponDamage()
    {

        float damage = 0;

        //switch (this.activeWeapon)
        //{
        //    case enemyController.Weapon.MELEE:
        //        damage = 20.0f;
        //        break;
        //    case enemyController.Weapon.RANGED:
        //        damage = 25.0f;
        //        break;
        //    case enemyController.Weapon.NONE:
        //        damage = 0.0f;
        //        break;
        //    default:
        //        break;
        //}


        return damage;
    }

    //Uses a line cast to check weather the player is behind any environment and if they are close enough to the enemy
    //and assigns player spotted to true or false
    void PlayerInSight()
    {

        if (!Physics2D.Linecast(transform.position, GameManager.instance.thePlayer.transform.position, 1 << LayerMask.NameToLayer("Background")) &&
            GetDistance() <= 120f)
        {
            playerSpotted = true;
            GameManager.instance.enemyAttack = true;
        }
        else
        {
            playerSpotted = false;
            //if (GameManager.instance.enemyAttack)
            //{
            //    GameManager.instance.enemyAttack = false;
            //}
        }
    }

    //Returns the distance between the player and the enemy
    public float GetDistance()
    {
        return (transform.position - GameManager.instance.thePlayer.transform.position).magnitude;
    }

    //changes the ranges based on the enemies current position
    void changeRanges()
    {
        posXRange = transform.position.x + radius;

        negXRange = transform.position.x - radius;

        posYRange = transform.position.y + radius;

        negYRange = transform.position.y - radius;

        rangesChosen = true;
    }


    //Checks if the enemies health is less than 0 and destroys it
    //also controls weather the enemy is wandering or following the enemy
    void FixedUpdate()
    {
        if (health <=0)
        {
            this.gameObject.SetActive(false);
            Destroy(this);
        }

        if(!randLocChosen)
        {
            ChooseRandomLocation();
        }

        if (!playerSpotted)
        {
            Wander();
        }
        else
        {
            AttackPlayer();
        }

        PlayerInSight();
    }//FixedUpdate

    //Chooses a random location around the enemies x and y ranges
    private void ChooseRandomLocation()
    {
        randomLoc = new Vector3(Random.Range(negXRange, posXRange), Random.Range(negYRange, posYRange), transform.position.z);
        randLocChosen = true;

    }

    //Sets the enemy to follow the player
    void AttackPlayer()
    {
        if (rangesChosen)
        {
            rangesChosen = false;
        }

        Vector3 lookTo = GameManager.instance.thePlayer.transform.position - transform.position;

        //Sets the angle to look at using trigonometry.
        float angle = Mathf.Atan2(lookTo.y, lookTo.x) * Mathf.Rad2Deg;

        //Does the rotation transformation.
        rb2d.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        rb2d.MovePosition(Vector3.MoveTowards(rb2d.position, GameManager.instance.thePlayer.transform.position, speed * 0.7f * Time.deltaTime));
    }

    //Makes the enemy move towards its random location, changes the location if it arrives
    private void Wander()
    {
        if (!rangesChosen)
        {
            changeRanges();
        }
        

        if(ReachedLocation())
        {
            randLocChosen = false;
        }
        else
        {
            Vector3 lookTo = randomLoc - transform.position;

            //Sets the angle to look at using trigonometry.
            float angle = Mathf.Atan2(lookTo.y, lookTo.x) * Mathf.Rad2Deg;

            //Does the rotation transformation.
            rb2d.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

            rb2d.MovePosition(Vector3.MoveTowards(rb2d.position, randomLoc, speed * 0.3f * Time.deltaTime));
        }


    }

    //onTriggerStay2D which takes a collider, called when the collider is
    //triggered.
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Weapon" && GameManager.instance.thePlayer.weapon.attack && GameManager.instance.thePlayer.weapon.isWeaponMelee())
        {
            TakeDamage(GameManager.instance.thePlayer.weapon.GetWeaponDamage());
            GameManager.instance.activateDamageText(transform.position, GameManager.instance.thePlayer.weapon.GetWeaponDamage());
            hit.pitch = Random.Range(0.8f, 1.5f);
            hit.Play();
        }
        else if (other.tag == "Projectile")
        {
            TakeDamage(GameManager.instance.thePlayer.weapon.GetWeaponDamage());
            GameManager.instance.activateDamageText(transform.position, GameManager.instance.thePlayer.weapon.GetWeaponDamage());
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
            hit.pitch = Random.Range(0.8f, 1.5f);
            hit.Play();
        }
    }

    //Checks if the enemy has collided with a wall
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Environment")
        {
            randLocChosen = false;
        }

    }


}//PlayerController
