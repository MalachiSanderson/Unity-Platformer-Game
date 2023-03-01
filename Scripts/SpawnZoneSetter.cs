using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZoneSetter : MonoBehaviour
{

    private GameObject player;
    private BoxCollider2D spawnPointCollider;
    private Transform newPointLoc;
    private GameObject oldPointObject;
    private Transform oldPointLoc;
    private SpriteRenderer newPointColor;
    private SpriteRenderer oldPointColor;



    // Start is called before the first frame update
    void Start()
    {

        newPointLoc = transform;
        spawnPointCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        newPointColor = GetComponent<SpriteRenderer>();
        

    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (GameObject.FindGameObjectWithTag("Spawn Areas"))
        {
            oldPointObject = GameObject.FindGameObjectWithTag("Spawn Areas");
            oldPointColor = oldPointObject.GetComponent<SpriteRenderer>();
            oldPointLoc = oldPointObject.transform;
        }
        if (other.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().DamageEntity(-player.GetComponent<Entity>().maxHealth); //Give max Health.
            if (!this.CompareTag("Spawn Areas"))
            {
                GameMaster.makePopupWorldText(GameAssets.i.genericWorldPopupText, this.transform.position, "Checkpoint Activated!", 1.5f, Color.green);
                DataGM.playerSpawnZone[0] = newPointLoc.position.x;
                DataGM.playerSpawnZone[1] = newPointLoc.position.y;
                DataGM.playerSpawnZone[2] = newPointLoc.position.z;
                //print("Spawn Location : "+ DataGM.playerSpawnZone[0] +","+ DataGM.playerSpawnZone[1]);
                DataGM.SaveCurrentGameData();
                //print("New Spawn Point Set!");

                if (GameObject.FindGameObjectWithTag("Spawn Areas"))
                {
                    oldPointObject.tag = "Untagged";
                    oldPointColor.color = new Color(0, 0, 1, 1);
                    spawnPointCollider.tag = "Spawn Areas";
                    newPointColor.color = new Color(1, (float)0.75, 0, 1);

                }
                else
                {
                    spawnPointCollider.tag = "Spawn Areas";
                    newPointColor.color = new Color(1, (float)0.75, 0, 1);
                    
                }
               
            }
           

        }
    }
    
    /*
    void OnTriggerStay2D(Collider2D other)
    {
        if (GameObject.FindGameObjectWithTag("Spawn Areas"))
        {
            oldPointObject = GameObject.FindGameObjectWithTag("Spawn Areas");
            oldPointColor = oldPointObject.GetComponent<SpriteRenderer>();
            oldPointLoc = oldPointObject.transform;
        }
        if (other.CompareTag("Player") && !GameObject.FindGameObjectWithTag("Spawn Areas") && Input.GetKeyDown(KeyCode.E))
        {
            spawnPointCollider.tag = "Spawn Areas";
            newPointColor.color = new Color(1, (float)0.75, 0, 1);
        }

        else if (GameObject.FindGameObjectWithTag("Spawn Areas") && other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            oldPointObject.tag = "Untagged";
            oldPointColor.color = new Color(0, 0, 1, 1);
            spawnPointCollider.tag = "Spawn Areas";
            newPointColor.color = new Color(1, (float)0.75, 0, 1);
        }
    }
    */
}
