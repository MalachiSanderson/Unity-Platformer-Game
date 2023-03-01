using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanavesa;

public class HealthPack : MonoBehaviour
{
    public int amountHealed;
    public string healthPackID;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(other.GetComponent<Entity>().Health >= other.GetComponent<Entity>().maxHealth)
            {
                //print("Health Pack Not Used Because HP At Max");
            }
            else
            {
                print("Health Pack Used");
                other.GetComponent<Entity>().DamageEntity(-amountHealed);
                AudioManager.Instance.PlaySound(AllSFX.getHealingSound());
                Destroy(this.gameObject);
                //print("This Health Pack should be destroyed");
            }


        }
    }




}
