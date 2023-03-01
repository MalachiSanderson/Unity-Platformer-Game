using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanavesa;

public class PlayerHurtBoxDamager : MonoBehaviour
{
    private double damage;
    private Entity playerEntity;
    private PlayerAttack playerAttack;
    public HashSet<Entity> attacked = new HashSet<Entity>(); 
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        damage = playerEntity.damagePerHit;
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        player = GameObject.FindGameObjectWithTag("Player");


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            attack(other);
            knockbackEffects(other);
        }
    }

    public void attack(Collider2D other)
    {
        //attacked.Clear();
        if (other.GetComponent<Entity>() != null)
        {
            

            Entity entity = other.GetComponent<Entity>();
            if (!attacked.Contains(entity))
            {
                AudioManager.Instance.PlaySound(AllSFX.getStrikingEnemySound());
                attacked.Add(entity);
                //print(other.tag); //USEFUL PRINTS THE TAG OF WHAT THE HURT BOX HIT!!!
                entity.DamageEntity(damage);
            }
               
               
        }
    }

    public void knockbackEffects(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
        {
            if(!other.CompareTag("Coin") && other.GetComponent<Entity>() == null)
            {
                AudioManager.Instance.PlaySound(AllSFX.getPlayerStrikeSound());

            }

            Rigidbody2D playerBody = player.GetComponent<Rigidbody2D>();
            Rigidbody2D otherBody = other.GetComponent<Rigidbody2D>();
            if ((playerBody.transform.position.y - other.transform.position.y) > 0.5)
            {
                if (playerAttack.doesAttackUseVelocity)
                {
                    otherBody.velocity = (new Vector2(0, -(float)(playerEntity.knockBackPerHit)));
                }
                else
                {
                    otherBody.AddForce(new Vector2(0, -(float)(playerEntity.knockBackPerHit)), ForceMode2D.Impulse);
                }
                if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Space))
                {
                    playerBody.velocity = (new Vector2(0, (float)(0.7 * playerEntity.knockBackPerHit)));
                    //GameMaster.applyForceToPlayer( 0f, (float) (0.5*playerEntity.knockBackPerHit), 0.01f );
                    player.GetComponent<PlayerControllerMain>().extraJumps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerMain>().amountOfJumpsAfterJumping;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    playerBody.velocity = (new Vector2(0, (float)(0.3 * playerEntity.knockBackPerHit)));
                    //GameMaster.applyForceToPlayer(0f, (float)(0.2*playerEntity.knockBackPerHit), 0.01f);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerMain>().extraJumps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerMain>().amountOfJumpsAfterJumping;
                }
            }
            //else
            //{
                if (other.transform.position.x > playerBody.transform.position.x)
                {
                    if (playerAttack.doesAttackUseVelocity)
                    {
                        otherBody.velocity = (new Vector2((float)(0.7 * playerEntity.knockBackPerHit), 0));
                    }
                    else
                    {
                        otherBody.AddForce(new Vector2((float)(0.7 * playerEntity.knockBackPerHit), 0), ForceMode2D.Impulse);

                    }
                    //playerBody.velocity = (new Vector2(-(float)(0.2 * playerEntity.knockBackPerHit), GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity.y));
                    GameMaster.applyForceToPlayer(-(float)(0.3 * playerEntity.knockBackPerHit), 0f, 0.01f);
                }
                else if (other.transform.position.x < playerBody.transform.position.x)
                {
                    if (playerAttack.doesAttackUseVelocity)
                    {
                        otherBody.velocity = (new Vector2(-(float)(0.7 * playerEntity.knockBackPerHit), 0));
                    }
                    else
                    {
                        otherBody.AddForce(new Vector2(-(float)(0.7 * playerEntity.knockBackPerHit), 0), ForceMode2D.Impulse);
                    }
                    // playerBody.GetComponent<Rigidbody2D>().velocity = (new Vector2((float)(0.2 * playerEntity.knockBackPerHit), GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity.y));
                    GameMaster.applyForceToPlayer((float)(0.3 * playerEntity.knockBackPerHit), 0f, 0.01f);
                }
            //}
        }
    }

}
