//Title         : Spawner.cs
//Author        : M.K.Bunting
//Date          : 04/11/2016
//Last Modified : 06/11/2016

//Purpose: Spawns a GameObject set in the editor based on a random range 
//on the x and y axis around the location of the spawners position. It 
//only spawns a certain number of times and uses WaitForSeconds with random
//delays so the objects are spawned at random times. 

//Additional Information: The spawnLimit is public and set in the editor, 
//but another coroutine could be added to make the spawnLimit random based
//on an enumerator which defines the difficulty. 

//Default Unity includes:
using UnityEngine;
using System.Collections;

//My Includes:
// N/A

//Public class containing public variables that can be modified
//in the editor and private variables which only the functions 
//inside the class can use. It also contains the coroutine Spawn
//which handles the actual spawning of the objects.
public class Spawner : MonoBehaviour
{
    //Public GameObject which can be
    //assigned in the editor.
    public GameObject objectToSpawn;    //This is the object that will be spawned 
                                        //in the Coroutine Spawn().


    //Public floats which can be 
    //assigned in the editor.                                    
    public float xRange;                //The float which stores how far an object can 
                                        //spawn on the x axis in both directions.

    public float yRange;                //The float which stores how far an object can 
                                        //spawn on the y axis in both directions.  
                                                      
    public float minDelayTime;          //The float which assigns the minimum delay
                                        //time for a gameObject to spawn.

    public float maxDelayTime;          //The float which assigns the maximum delay
                                        //time for a gameObject to spawn.


    //Public int which can be 
    //assigned in the editor.
    public int spawnLimit;              //The int which indicates how many gameObjects
                                        //can spawn.

    //Private int which can only
    //be used in the functions of
    //Spawner.
    private int objectCount;            //The int which counts how many gameObjects
                                        //have spawned.


    //Start() is ran when the game starts (aslong as the script is set to 
    //an active object). It sets the objectCount to 0 and calls the coroutine
    //Spawn which starts the spawning process.
    void Start()
    {
        StartCoroutine(Spawn());
        objectCount = 0;
    } //Start


    //Spawn uses a series of yield's and a loop to spawn the gameObjects.
    //The location of where it spawns is dependant on a rectangular area
    //around the spawner defined by xRange and yRange. The delay is based
    //on a random float between the minDelayTime and the maxDelayTime.
    IEnumerator Spawn()
    {
        //This is the first delay. It gives the game a chance to load before it begins
        //spawning gameObjects.
        yield return new WaitForSeconds(Random.Range(minDelayTime, maxDelayTime));

        //This is the while loop that handles the spawning. The loop will continue
        //to spawn until the amount of objects spawned has reached the spawnLimit
        //set in the editor.
        while (objectCount < spawnLimit)
        {
            //This is the Vector3 which stores the location of what will be spawned.
            //The x value of the position is a random range between the x position of
            //the spawner - xRange, and the x position of the spawner + xRange. 
            //The y value is similar, however, it uses the y position and yRange instead.
            //The z value is always 0 as it is a 2D game and doesnt need a z value.
            Vector3 spawnPosition = new Vector3 
            (
                Random.Range((transform.position.x- xRange), (transform.position.x + xRange)),
                Random.Range((transform.position.y -yRange), (transform.position.y + yRange)),
                -1.0f
            );//spawnPosition;

            //The object also needs a rotation. We do not need a rotation for our objects
            //so we set it to 0 using .identity.
            Quaternion spawnRotation = Quaternion.identity;

            //This is where the object is spawned. It takes 3 parameters:
            //gameObject, the position and the rotation.
            Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            //Once an object is spawned the objectCount is incremented to prevent
            //an infinite loop.
            objectCount++;

            //Another yield is used to add a delay between spawns.
            yield return new WaitForSeconds(Random.Range(minDelayTime, maxDelayTime));

        }//while

    }//Spawn

}//Spawner
