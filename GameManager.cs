//Title         : GameManager.cs
//Author        : M.K.Bunting
//Date          : 09/05/2017
//Last Modified : 09/05/2017

//Purpose: The main code for this game. Contains lots of public variables  and functions 
//which other objects within my game can access and use. 
//Is a singleton so that only one instance of the game manager can exist at a time.

//Additional Information: Could be further modified and separated into other managers
//(such as sound, player, animation etc.).


//Default Unity includes:
using UnityEngine;
using System.Collections;

//My includes:
using UnityEngine.SceneManagement;                                                                    //To check and control scenes.
using UnityEngine.UI;

//class which contains an awake, initGame, loadPlayer, loadCamera
//getPlayerPosition and Update function
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;                                                        //an instance of the game manager is initialised to NULL.

    //potential player manager instead of having all stored in the 
    //game manager                                                               

    //potentially useful in the future
    //public int level = 1;                                                                             //the current level the player is on, initialised to 1.


    public bool levelStarted = false;                                                                 //boolean to declare if the level has started, initialised to false.
    //potentially useful in the future
    //public bool levelFinished = false;                                                                //boolean to declare if the level is finished, initialised to false.
    //public bool loadingLevel = false;                                                                 //boolean to declare if the level is being loaded, initialised to false.
    //public bool playerDead = false;                                                                   //boolean to declare if the player is dead, initiaised to false.
    //public bool gameOver;                                                                             //boolean to declare if the game is over.

    public Text DamageUI;

    public GameObject parent;

    
    public Camera mainCam;                                                                            //main camera to be spawned.

    public PlayerController thePlayer;                                                                //thePlayer to be spawned 

    public AudioSource wastelandAudio;

    public AudioSource enemyAttackAudio;

    public bool enemyAttack;

    public GameObject inventoryCanvas;

    public GameObject inventory;


    //Awake function which makes sure only on gameManager exists and sets
    //the manager to dontdestroyonload.
    void Awake()
    {
        //if statement that checks if instance is null, then 
        //sets the instance to this object.
        if (instance == null)
        {
            instance = this;
        }
        //else if statement that checks if the instance is not
        //this object, then destroys it.
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);                                                                //Sets this object to not be destroyed when a new scene is loaded.

        enemyAttack = false;

    }

    public void playWasteland()
    {
        if (wastelandAudio.mute)
        {
            wastelandAudio.mute = false;
        }
    }

    public void muteWasteland()
    {
        if (!wastelandAudio.mute)
        {
            wastelandAudio.mute = true;
        }
    }

    public void playEnemyAttack()
    {
        if (enemyAttackAudio.mute)
        {
            enemyAttackAudio.mute = false;
        }
    }

    public void muteEnemyAttack()
    {
        if (!enemyAttackAudio.mute)
        {
            enemyAttackAudio.mute = true;
        }
    }

    //InitGame function which is called when the appropriate scene is loaded.
    //Calls the LoadPlayer and LoadCamera function.
    void InitGame()
    {
        LoadPlayer();                                                                                 //Load player is called.                                                                      
        LoadCamera();                                                                                 //the camera is loaded.  
        parent = Instantiate(parent);
        wastelandAudio = Instantiate(wastelandAudio);
        enemyAttackAudio = Instantiate(enemyAttackAudio);

        inventory = GameObject.Find("Inventory");
        inventoryCanvas = GameObject.Find("InventoryCanvas");

        toggleInventory();
    }

    //LoadPlayer is used to instantiate the player.
    void LoadPlayer()
    {
        thePlayer = Instantiate(thePlayer);
    }

    //LoadCamera is used to instantiate the main camera.
    void LoadCamera()
    {
       mainCam = Instantiate(mainCam);
    }

    //GetPlayerPosition finds the player in the game and returns the transform.
    public Transform GetPlayerPosition()
    {
        return thePlayer.transform;
    }

    public void toggleInventory()
    {
        inventoryCanvas.active = !inventoryCanvas.active;
    }

    //activateDamageText, finds the position of where the text is to be displayed
    //and translates it to canvas space and displays the text above the location
    public void activateDamageText(Vector3 canvasPosition, float damage)
    {

        DamageUI.transform.position = new Vector3(0, 75, 0);

        Text damageText = DamageUI;

        canvasPosition = thePlayer.transform.position - canvasPosition;

        damageText.transform.position -= canvasPosition * 5.66f;

        damageText.text = "" + damage;

        Instantiate(damageText, parent.transform);

    }

    //Update function handles what happens during what scene using the scene manager.
    void Update()
    {
        //if active scene index =3 and level not started, level started
        //set to true, initgame called.
        if (SceneManager.GetActiveScene().buildIndex == 3 && !levelStarted)
        {
            levelStarted = true;

            InitGame();


        }

        if (enemyAttack)
        {
            muteWasteland();
            playEnemyAttack();
        }

    }

};
