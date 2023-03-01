using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public bool doNotSpawnOnStart;

    // Start is called before the first frame update
    void Start()
    {
        if(!doNotSpawnOnStart)
        {
            if (transform.childCount == 0)
            {
                GameObject spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
                spawnedObject.transform.SetParent(this.transform);
                //spawnedObject.name = "Danny is trash";
                //print("Spawned " + objectToSpawn.name + " at " + this.name);
            }
        }
       
    }


    public void respawnGameObject()
    {
        if (transform.childCount == 0)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            spawnedObject.transform.SetParent(this.transform);
            //print("Spawned " + objectToSpawn.name + " at " + this.name);
        }
    }

    public void testMassSpawnMethod()
    {
        print(this.name + "CHECK!");
    }

    
   

}
