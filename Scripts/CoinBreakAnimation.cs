using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanavesa;

public class CoinBreakAnimation : MonoBehaviour
{
    private Animator anim;
    public GameObject collectedEffect;
    private Entity playerEntity;


   
    void Start()
    {
        anim = GetComponent<Animator>();
        playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //print("What entered coin trigger box : " + other.tag);
        if (other.CompareTag("Player") || (other.CompareTag("Player Hurt Box") &&  other.GetComponent<BoxCollider2D>().enabled == true ) )
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            //playerEntity.giveCoin();    //(as of 5-19-20) this is handled in the script that triggers after the coin break animation finishes. This is to remedy the prob where attacks gave multiple coins bec it detected 2 hits...
            AudioManager.Instance.PlaySound(AllSFX.getCoinCollectedSound());
            anim.SetTrigger("Break Coin");
            Instantiate(collectedEffect, transform.position, Quaternion.identity);
            
        }
        if(other.CompareTag("Out of Map"))
        {
            Destroy(this.gameObject);
            //print("A coin was destroyed because it fell off the map.");
        }
    }




}
