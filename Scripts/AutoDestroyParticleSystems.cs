﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticleSystems : MonoBehaviour
{
   
    private ParticleSystem ps;

    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                //print("Destroying completed particle system.");
                Destroy(this.gameObject);
            }
        }
    }


}


