using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    
    public bool enableContactDamage;
    public bool onlyHurtsPlayer;
    public double contactDamage; 
    private GameObject player;
    private Rigidbody2D playerBody;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerBody = player.GetComponent<Rigidbody2D>();
         
    }

    

    void OnTriggerEnter2D(Collider2D other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (enableContactDamage && entity != null)
        {
            
            if (other.CompareTag("Player"))
            {
                if (entity.canDie)
                {
                    entity.DamageEntity(contactDamage);
                    //knockBackOnPlayer();
                    GameMaster.knockBackOnPlayer(transform.position.x,10,5);
                }
            }
            else if (!onlyHurtsPlayer)
            {
                entity.DamageEntity(contactDamage);
            }
            //print(other.gameObject + "Was Damaged by Contact Damage: " + contactDamage);
        }
    }

   


}
