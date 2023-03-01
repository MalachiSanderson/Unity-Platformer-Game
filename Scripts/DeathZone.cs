using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // public bool InstaKill;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("The Player has entered a death zone.");
        }
        else if(other.GetComponent<Entity>() != null)
        {
            print(other.gameObject.name + " has entered a death zone.");
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Entity>() != null)
        {
            Entity entity = other.GetComponent<Entity>();

            if (other.CompareTag("Player"))
            {
                entity.DamageEntity(entity.maxHealth);
                
            }

            else //if (other.CompareTag("Misc Entity"))
            {
                entity.DamageEntity(entity.maxHealth);
            }
        }
    }

}
