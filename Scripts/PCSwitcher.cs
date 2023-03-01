using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCSwitcher : MonoBehaviour
{
   
    private Entity selfEntity;
    private PlayerControllerMain selfController;
   
    void Start()
    {

         selfEntity = GetComponent<Entity>();
         selfController = GetComponent<PlayerControllerMain>();

    }

    public IEnumerator playerSwapDelay()
    {
        
        yield return new WaitForSeconds(0.3f);
        print("The Player Has Swapped Bodies");
        this.gameObject.tag = "Player";
        selfEntity.isThePlayer = true;
        selfController.IsControlAllowed = true;
        selfEntity.controller = GetComponent<PlayerControllerMain>(); //????????????
        selfController.enabled = true;
        
        selfEntity.updateHealthBar();
        //This updates the camera and background's target.
        GameObject.Find("Main Camera").GetComponent<CameraFollowPlayer>().UpdatePlayerTransform();
        GameObject.FindGameObjectWithTag("Tracks Player").GetComponent<CameraFollowPlayer>().UpdatePlayerTransform();


    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
           
            Instantiate(GameAssets.i.bodySwapParticleEffect, transform.position, Quaternion.identity);
            GetComponent<BoxCollider2D>().enabled = false;

            Entity otherEntity = other.GetComponent<Entity>();
            PlayerControllerMain otherController = other.GetComponent<PlayerControllerMain>();

            


            otherEntity.isThePlayer = false;
            if(other.GetComponent<PlayerAttack>() != null)
            {
                other.GetComponent<PlayerAttack>().enabled = false;
            }
            if(GetComponent<PlayerAttack>() != null)
            {
                GetComponent<PlayerAttack>().enabled = true;
            }

            other.tag = "Host";
            StartCoroutine(playerSwapDelay());

            otherController.IsControlAllowed = false;
            otherController.enabled = false;
            other.GetComponent<BoxCollider2D>().enabled = true;
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            
            other.GetComponent<Rigidbody2D>().sharedMaterial = GameAssets.i.highFrictionMaterial;
            GetComponent<Rigidbody2D>().sharedMaterial = GameAssets.i.frictionlessMaterial;
            
        }
    }

    
}
