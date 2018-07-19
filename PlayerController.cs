//Title 		: PlayerController.cs
//Author 		: L.A.Steed
//Date 			: 05/11/2016
//Last Modified : 13/05/2017

//Modification  : 30/11/2016 - Edited movement script to prevent 'gliding' player movement.
//By            : M.K.Bunting

//Modification  : 07/02/2017 - Added scripts to make the player rotate facing the mouse.
//By            : M.K.Bunting

//Modification  : 07/02/2017 - Altered scripts that make the player rotate facing the mouse.
//                           - Rotation is now more smooth and less snappy.
//                           - The player now moves towards/away from the mouse when W/S is held, 
//                             however, mouse position does not update with camera movement.
//By            : M.K.Bunting

//Modification  : 01/03/2017 - Fixed the issue where the mouse position does not update with player movement,
//                             however, backwards movement has been removed.
//                           - Added code to change between idol and walking states for animations.
//By            : M.K.Bunting

//Modification  : 11/05/2017 - Added a weapon to the player.
//By            : M.K.Bunting

//Modification  : 13/05/2017 - Added the ability to dash.
//                           - Added statistics
//By            : M.K.Bunting

//Purpose: This script contains all the functions dealing with player movement and encumberence.
//         Also deals with weapon control, stats and other player functions.

//Default Unity Includes:
using UnityEngine;
using System.Collections;

//My Includes:
// N/A


//Public class containing the functions controlling player
//movement and encumberence.
public class PlayerController : MonoBehaviour
{
    //Different types of weapons the player cna have
    public enum Weapon { SHORTSWORD, IRONDAGGER, OAKSHORTBOW, HEALTHPOTION, STEELGREATAXE,
                         IRONCLAWS, SHORTSPEAR, BOARSPEAR, FIREBALL, NONE };

    //players active weapon
    public Weapon activeWeapon;

    //Players max speed.
    public float speed;

    //Players encumberence (weight of gear etc).
    public float encumberence;

    //Base drag to stop motion without movement.
    public float Friction;

    //Rigidbody component for use with collision and drag.
    private Rigidbody2D rb2d;

    //animator to control animation
    private Animator animator;

    //weapon of the player
    public attackManager weapon;

    //if the player is dashing or not
    private bool dashing;

    //how long the player dashes for
    private float dashTime;

    public Attributes stats;

    public AudioSource hit;

    public AudioSource itemSwitch;
    
    //Initialises the rigidbody component and its drag
    //coefficient. Also initialises the animator, dashing boolean
    //and the active weapon
    void Start()
    {

        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb2d.drag = Friction + encumberence;

        dashing = false;

        activeWeapon = PlayerController.Weapon.NONE;
    }//Start


    // Updates position based upon input and then adds a force
    //upon the rigidbody to act upon that input. Controls the animation
    // and also the switching of weapons
    void FixedUpdate()
    {

        //Code that handles rotation and causes the player to look at the mouse.
        //Stores the mouse position and the look position in Vector3s.
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);

        //Creates a vector.
        Vector3 lookTo = lookPos - transform.position;
        //Sets the angle to look at using trigonometry.
        float angle = Mathf.Atan2(lookTo.y, lookTo.x) * Mathf.Rad2Deg;

        //Does the rotation transformation.
        rb2d.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        //Checks to see if the player has pressed forward (W or up).
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            //Moves the player towards the mouse.
            rb2d.MovePosition(Vector3.MoveTowards(rb2d.position, lookPos, speed * Time.deltaTime));
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        //if the switch button is pressed, call switch weapon
        if (Input.GetButtonDown("Switch"))
        {
            SwitchWeapon();
            itemSwitch.Play();
        }

        if(Input.GetKeyDown("i"))
        {
            GameManager.instance.toggleInventory();
        }

        //if the dash button is pressed and the player is not currently dashing, set 
        //dashing to true and set the dash time
        if (Input.GetButtonDown("Dash") && !dashing)
        {
            dashing = true;
            dashTime = Time.time + 0.25f;
            
        }

        //if dashing is true, call the dash function, passing through the lookpos
        if (dashing)
        {
            Dash(lookPos);
        }


    }//FixedUpdate

    //Switch Weapon function, cycles through the weapon Enumerations
    void SwitchWeapon()
    {

        if (activeWeapon == PlayerController.Weapon.NONE)
        {
            activeWeapon = PlayerController.Weapon.SHORTSWORD;
        }
        else
        {
            activeWeapon += 1;
        }

        Debug.Log(activeWeapon);

    }

    //dash function, moves the player forward faster than walking speed
    //for a set time.
    void Dash(Vector3 dir)
    {  
        rb2d.MovePosition(Vector3.MoveTowards(rb2d.position, dir, speed * 2f * Time.deltaTime));
        if (Time.time >= dashTime)
        {
            dashing = false;
        }
    }

    //onTriggerStay2D which takes a collider, called when the collider is
    //triggered.
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy Projectile")
        {

            GameManager.instance.activateDamageText(other.gameObject.transform.position, 25);

            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
            hit.pitch = Random.Range(0.8f, 1.5f);
            hit.Play();
        }
    }


}//PlayerController
