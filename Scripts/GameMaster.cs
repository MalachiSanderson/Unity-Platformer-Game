using Sanavesa;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;
    private GameObject player;
    private static Entity entity;
    //public Transform playerPrefab;
    private Transform spawnPoint;
    public CameraFollowPlayer cameraFollowPlayer;
    public int respawnDelay = 2;
    public static GameObject truePlayer;
    public bool autoDestroyInactiveParticleSystems;


    #region Start And Update
    void Start()
    {
        DataGM.LoadGameDataValuesToDataGM();
        if (gm == null)
        {
            gm = FindObjectOfType<GameMaster>();
        }
        player = GameObject.FindGameObjectWithTag("Player");
        entity = player.GetComponent<Entity>();
        truePlayer = GameObject.Find("Main Player Character");

        changeToolTipText("[Left Click] to attack. Do this while pressing down and you'll do a down attack which can be used to bounce off of enemies and bounce pads. Bounce attacks reset jumps and dashes." +
            "\nPress [E] to take over sparkling entities. Press [G] at any moment to return to your player. ", 15f);

        //AudioManager.Instance.PlayMusic(AllSFX.getBuzzSawSound());
        //respawnAllUnspawnedGameObjects();

        if (DataGM.playerSpawnZone[0] != 0)
        {
            Vector3 spawnPos = new Vector3(DataGM.playerSpawnZone[0], DataGM.playerSpawnZone[1], DataGM.playerSpawnZone[2]);
            player.transform.position = spawnPos;
            //Debug.Log("Player being teleported to saved spawn loc");
        }

    }



    public void Update()
    {
        if (autoDestroyInactiveParticleSystems)
        {
            clearAllInactiveParticleSystems();
        }
    }

    #endregion


    #region Death And Respawning Functions

    public IEnumerator RespawnPlayer(GameObject player) //What is the type IEnumerator???
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Spawn Areas").transform;
        DataGM.SaveCurrentGameData();
        yield return new WaitForSeconds(respawnDelay);
        MainMenuManager.LoadGameScene(MainMenuManager.GameScenes.TestScene1);
        /*
        player.transform.position = spawnPoint.position;
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        if (player.CompareTag("Player"))
        {
            player.GetComponent<PlayerControllerMain>().IsControlAllowed = true;
            player.GetComponent<Entity>().healthBarUpdate.SetHealth((int)entity.maxHealth);
        }
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        player.GetComponent<Rigidbody2D>().freezeRotation = true;
        player.GetComponent<Entity>().isEntityDead = false;
        */
    }


    public static void KillPlayer(Entity player)
    {
        //This new method only applies to player as it doesnt actually destroy him, simply teleports him...
        if (player.gameObject.CompareTag("Player"))
        {
            player.healthBarUpdate.SetHealth(0);
            Debug.Log("PLAYER DIED--> [TODO] MAKE DEATH ANIMATION TO PLAY");
        }
        player.GetComponent<PlayerControllerMain>().IsControlAllowed = false;
        player.GetComponent<Rigidbody2D>().isKinematic = true;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        gm.StartCoroutine(gm.RespawnPlayer(player.gameObject));

    }


    public void respawnAllUnspawnedGameObjects()
    {
        GameObject[] allSpawners = GameObject.FindGameObjectsWithTag("Game Object Spawner");
        foreach (GameObject spawnLocation in allSpawners)
        {
            spawnLocation.GetComponent<ObjectSpawner>().respawnGameObject();
        }
    }


    public static void KillEntity(Entity entity)
    {
        Destroy(entity.gameObject);
        print("AN ENTITY HAS DIED!!!!");
    }

    #endregion

    /*
     * [TODO] Remake entire Death and respawning system so it functions by having
     * player's info saved to a class or something and just reset the scene.
     * YET, when reloading a scene, check what should be reloaded 
     * (i.e. DO NOT RELOAD THE BOSS OR UPGRADES IF THEY HAVE ALREADY BEEN COLLECTED).
     */


    public static void knockBackOnPlayer(float gameObjecttransformPositionX, float xForce, float yForce)
    {
        Rigidbody2D playerBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        gm.StartCoroutine(gm.knockBackWait((GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().hitStunLength)));
        if (playerBody.transform.position.x < gameObjecttransformPositionX)
        {
            playerBody.AddForce(new Vector2(-xForce, yForce), ForceMode2D.Impulse);
        }
        else
        {
            playerBody.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);
        }

    }

    public IEnumerator knockBackWait(float noControlTime)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerMain>().IsControlAllowed = false;

        //print("No Control");
        yield return new WaitForSeconds(noControlTime);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerMain>().IsControlAllowed = true;
        //print("Regained Control");

    }

    public static void applyForceToPlayer(float xForce, float yForce, float noControlTime)
    {
        Rigidbody2D playerBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        gm.StartCoroutine(gm.knockBackWait(noControlTime));
        playerBody.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);

    }

    public static GameObject getTruePlayer()
    {
        return truePlayer;
    }

    public static IEnumerator deleteGameObjectAfterWaiting(float waitTime, GameObject thisObject)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(thisObject);
    }

    #region Text Related Functions

    private static IEnumerator deletePopupWorldText(float waitTime, GameObject textObj)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(textObj);

    }

    /// <summary>
    /// Makes a floating (in-world) text object for aset amount of time then deletes it.
    /// </summary>
    /// <param name="textPrefab"></param>
    /// <param name="position"></param>
    /// <param name="text"></param>
    /// <param name="waitTime"></param>
    public static void makePopupWorldText(Transform textPrefab, Vector3 position, string text, float waitTime)
    {
        GenericTextPopup gTP = GenericTextPopup.Create(textPrefab, position, text);
        gm.StartCoroutine(deletePopupWorldText(waitTime, gTP.gameObject));
        //gm.StartCoroutine(deleteGameObjectAfterWaiting(waitTime, textPrefab.gameObject) );
    }
    /// <summary>
    /// Same as basic make popupworldtext() but it also allows user to specify a face color for the text.
    /// </summary>
    /// <param name="textPrefab"></param>
    /// <param name="position"></param>
    /// <param name="text"></param>
    /// <param name="waitTime"></param>
    /// <param name="c"></param>
    public static void makePopupWorldText(Transform textPrefab, Vector3 position, string text, float waitTime, Color c)
    {
        GenericTextPopup gTP =  GenericTextPopup.Create(textPrefab, position, text);
        gTP.setTextFaceColor(c);
        gm.StartCoroutine(deletePopupWorldText(waitTime, gTP.gameObject));
        //gm.StartCoroutine(deleteGameObjectAfterWaiting(waitTime, textPrefab.gameObject) );
    }

    /// <summary>
    /// Sets the in-game tooltip text (that has a set position in center of screen) to something.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="messageDuration"></param>
    public static void changeToolTipText(string text, float messageDuration)
    {
        GameObject toolTip = GameObject.FindGameObjectWithTag("UI Text Popup");
        toolTip.GetComponent<UIPlayerToolTip>().Setup(text);
        gm.StartCoroutine(emptyToolTipText(messageDuration));
    }

    /// <summary>
    /// Empties all the existing text in the tooltip text object. It still 
    /// exists in the game and has the same position, it is just empty of text 
    /// after this.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static IEnumerator emptyToolTipText(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject toolTip = GameObject.FindGameObjectWithTag("UI Text Popup");
        toolTip.GetComponent<UIPlayerToolTip>().Setup("");
    }

    #endregion


    #region Static General Utility Methods

    /// <summary>
    /// GAGE: A utility class for easily finding a child of a game object using its name. Not super reliable or ideal but 
    /// it's simple.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject GetChildWithName(GameObject parent, string name)
    {
        Transform trans = parent.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            Debug.LogWarning("Unable to find a child of " + parent.name + " with the specified name.");
            return null;
        }
    }

    /// <summary>
    /// GAGE: Pretty self explanatory...It force reloads the currently active scene in run-time.
    /// </summary>
    public static void reloadActiveScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }


    /// <summary>
    /// GAGE: This method searches the scene for all particle systems and checks if they are active, 
    /// destroying inactive ones.
    /// </summary>
    private static void clearAllInactiveParticleSystems()
    {
        ParticleSystem[] allParticleSystemsInScene = GameObject.FindObjectsOfType<ParticleSystem>();
        foreach (ParticleSystem particleSystem in allParticleSystemsInScene)
        {
            if (!particleSystem.IsAlive())
            {
                Destroy(particleSystem.gameObject);
            }
        }
    }
    /// <summary>
    /// Closes game whenever it is called (if in unity editor, testing the game, 
    /// it will just stop the play session).
    /// </summary>
    public static void CloseApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
              Application.Quit();
#endif
    }

    /// <summary>
    /// Basic static freeze game method that just sets the game's time scale to 0 if true 
    /// and 1 if false.
    /// </summary>
    /// <param name="freezeTheGame"></param>
    public static void FreezeGame(bool freezeTheGame)
    {
        if (freezeTheGame) Time.timeScale = 0;
        else Time.timeScale = 1;
    }





    #endregion

    /*
   private static GameMaster instance = null;

   /// <summary>
   /// The singleton access to <i>AudioManager</i> where all audio operations happen.
   /// </summary>
   public static GameMaster Instance
   {
       get
       {
           if (instance == null)
           {
               instance = GameObject.FindObjectOfType<GameMaster>();
           }

           if (instance == null)
           {
               Debug.LogWarning("No AudioManager instance was found. Will generate a default one.");
               var go = new GameObject("Generated Audio Manager");
               instance = go.AddComponent<GameMaster>();
           }

           return instance;
       }

       private set
       {
           if (instance == null && value != null)
           {
               instance = value;
           }
       }
   }
   */


}
