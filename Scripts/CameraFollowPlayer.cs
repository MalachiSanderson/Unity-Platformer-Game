using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{



    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
      

    }

    public void UpdatePlayerTransform()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }



    void LateUpdate()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            UpdatePlayerTransform();
        }

        if (playerTransform != null)
        {
            //we store current camera pos in the temporary variable of it's temporary currentCamPos.
            Vector3 currentCamPos = transform.position;

            //we set the camera's position x to be equal to player's x pos.

            currentCamPos.x = playerTransform.position.x;
            currentCamPos.y = playerTransform.position.y;

            //we set the camera's stored position x to be equal to camera's current x pos.
            transform.position = currentCamPos;

        }


    }
} 


































