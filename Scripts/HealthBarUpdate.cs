using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUpdate : MonoBehaviour
{
    public Slider slider;
    public GameObject damageParticleEffect;
    public GameObject healParticleEffect;


    public void SetHealth(int health)
    {
        float oldValue = slider.value;
        slider.value = health;
        Vector3 particlePoint = transform.position;
        particlePoint.x += 3f;
        if (health < oldValue)
        {
            Instantiate(damageParticleEffect, particlePoint, Quaternion.identity);
        }
        else
        {
            Instantiate(healParticleEffect, particlePoint, Quaternion.identity);
        }


    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    

}
