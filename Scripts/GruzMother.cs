using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruzMother : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Entity entity;
    private ContactDamage cD;
    [Range(1,3)]
    public int phase;
    private bool isColorOne;
    private float maxSpeed;
    private double initialDamage;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        entity = GetComponent<Entity>();
        cD = GetComponent<ContactDamage>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.green;
        maxSpeed = 20;
        initialDamage = cD.contactDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (entity.Health == entity.maxHealth)
        {
            sprite.color = Color.green;
            isColorOne = false;
            phase = 1;
            cD.contactDamage = initialDamage;
            maxSpeed = 20;
        }
        if (entity.Health < (0.5*entity.maxHealth) && phase < 2)
        {
            print("BOSS HAS ENTERED PHASE 2");
            maxSpeed = 70;
            sprite.color = Color.yellow;
            phase = 2;
        }
        if (entity.Health < (0.3 * entity.maxHealth) && phase < 3)
        {
            phase = 3;
            print("BOSS HAS ENTERED PHASE 3");
            maxSpeed = 200;
            cD.contactDamage = cD.contactDamage*1.7;
        }


        if (entity.Health < (0.3 * entity.maxHealth) && !isColorOne )
        {
            sprite.color = new Color(1, 0.75f, 0, 1);
            isColorOne = true;
        }
        else if(entity.Health < (0.3 * entity.maxHealth) && isColorOne)
        {
            isColorOne = false;
            sprite.color = Color.red;
        }


        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().isEntityDead)
            {
                //print("RESTORE BOSS HEALTH");
                if(entity.Health < entity.maxHealth)
                {
                    entity.DamageEntity(-entity.maxHealth);
                }
                
            }
        }
        

        if(body.velocity.x > maxSpeed)
        {
            body.velocity = new Vector2(body.velocity.x * 0.7f, body.velocity.y);
        }
        if (body.velocity.x < -maxSpeed)
        {
            body.velocity = new Vector2(body.velocity.x*0.7f, body.velocity.y);
        }
        if (body.velocity.y > maxSpeed)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y*0.7f);
        }
        if (body.velocity.y < -maxSpeed)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.7f);
        }

        if( (entity.Health == 0 ) || entity.isEntityDead)
        {
            GameMaster.changeToolTipText("Prey Slaughtered", 7f);
        }
    }

}
