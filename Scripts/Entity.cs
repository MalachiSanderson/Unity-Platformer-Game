using Sanavesa;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public class EntityStats
    {
        public double entityHealth;


    }


    public EntityStats entityStats = new EntityStats();
    private Animator anim;
    private int money;
    public bool isThePlayer;
    public bool isThePlayersTrueBody;
    public bool canDie;
    public bool respawns;
    public bool doesItHaveHitIFrames;
    public bool doesNotHaveHitFlash;
    public float iFramesAfterGettingHit;
    public float hitStunLength; //This will determine how long they cannot move/control themselves after being hit.
    public double maxHealth;
    public double damagePerHit;
    public double armorPercent; //damage mitigation percentage.
    private double damageMitigation;
    public double knockBackPerHit;
    public int coinsDroppedOnDeath;
    public GameObject bloodParticleEffect;
    public GameObject healParticleEffect;
    public HealthBarUpdate healthBarUpdate;
    public PlayerControllerMain controller;
    //public bool hasDoubleJump; 
    //public bool hasSpeed; 
    //public bool hasDash; 
    //public bool hasBigJump; 
   [HideInInspector] public bool isEntityDead; //Don't mess with in Inspector
   [HideInInspector] public double Health; //don't mess with in inspector


    public void DamageEntity(double damageDealt)
    {
        double currentHealth;
        if (canDie && !isEntityDead)
        {
            if ((-damageDealt) < 0)
            {
                 currentHealth = entityStats.entityHealth - (damageDealt*damageMitigation);
            }
            else
            {
                 currentHealth = entityStats.entityHealth - damageDealt;
            }

            if ((-damageDealt) < 0)
            {
                spawnBlood();
                if (!isThePlayer)
                {
                    GameMaster.makePopupWorldText(GameAssets.i.genericWorldPopupText, transform.position, ""+(int)(damageDealt * damageMitigation),0.7f,Color.yellow);   
                }
                else
                { 
                    GameMaster.makePopupWorldText(GameAssets.i.genericWorldPopupText, transform.position, "" + (int)(damageDealt * damageMitigation), 0.7f, Color.red);
                    
                }
            }
            else if (isThePlayer)
            {
                Instantiate(healParticleEffect, transform.position, Quaternion.identity);
            }

            if (currentHealth <= maxHealth)
            {
                entityStats.entityHealth = currentHealth;

            }
            else if (currentHealth > maxHealth)
            {
                //print("Already at max hp.");
                entityStats.entityHealth = maxHealth;
            }
            if (doesItHaveHitIFrames && (-damageDealt) < 0)
            {

                StartCoroutine(invincibilityFrames(iFramesAfterGettingHit)); //this should give some I-Frames after getting hit.
                if (isThePlayer && !doesNotHaveHitFlash)
                {
                    StartCoroutine(playerInvinciblilityFlashing());
                }
                else if (!doesNotHaveHitFlash)
                {
                    StartCoroutine(entityInvincibilityFlashing());
                }
            }

            updateHealthBar();


            if ((-damageDealt) > 0)
            {
                //print(this.gameObject.name + " Was Healed: + " + (-damageDealt) + "\nCurrent Health: " + entityStats.entityHealth);
            }
            else
            {
                //print(this.gameObject.name + " Was Damaged: " + (-damageDealt) + "\nCurrent Health: " + entityStats.entityHealth);
            }

            checkIfEntityIsDead(currentHealth);


        }
        Health = entityStats.entityHealth;
        //StartCoroutine(GameMaster.deleteDamagePopupText(2f));
    }

    public void increaseHealthMax(double amountofHealthChange)
    {
        maxHealth += amountofHealthChange;
        //entityStats.entityHealth = maxHealth;
        Instantiate(healParticleEffect, transform.position, Quaternion.identity);
        updateHealthBar();

        print(this.gameObject.name + "'s Max Health Was Increased By: + " + (amountofHealthChange) + "\nCurrent Total Health: " + maxHealth);

    }


    public void giveCoin(int amount)
    {
        if (isThePlayer)
        {
            money += amount;
            // print("Gained " + amount + " coin(s)" + "\nCurrent Money: " + money);
            DataGM.playerMoney = money;
        }
    }

    public void takeCoin(int amount)
    {
        if (isThePlayer)
        {
            money -= amount;
            print("Lost " + amount + " coin(s)" + "\nCurrent Money: " + money);
            DataGM.playerMoney = money;
        }
        else
        {
            money -= amount;
            print(this.gameObject.name + " Dropped " + amount + " coin(s)");
        }
    }

    void spawnBlood()
    {
        Instantiate(bloodParticleEffect, transform.position, Quaternion.identity);
    }


    public void updateHealthBar()
    {
        if (isThePlayer)
        {
            healthBarUpdate.SetMaxHealth((int) maxHealth);
            healthBarUpdate.SetHealth((int)entityStats.entityHealth);
        }
    }

    public void updateArmor(double newArmorPercent)
    {
        if(isThePlayer)
        {
            print("Increased " + this.gameObject.name + " armor from : " + armorPercent + "% ");
        }
        armorPercent = newArmorPercent;
        if (isThePlayer)
        {
            print(this.gameObject.name + "'s current Armor is : " + newArmorPercent + "%");
        }
        damageMitigation = 1 - (armorPercent * 0.01);
    }

    public IEnumerator dropCoinsOnKill()
    {

        yield return new WaitForSeconds(1);
        int droppedCoins = 0;
        while (droppedCoins < coinsDroppedOnDeath)
        {
            Instantiate(GameAssets.i.coinObject, transform.position, Quaternion.identity); //Spawn a coin where enemy died.
            droppedCoins++;
        }
        if (this.gameObject.GetComponent<PlayerControllerMain>() != null)
        {
            GameMaster.KillPlayer(this);
        }
        else
        {
            GameMaster.KillEntity(this);

        }
    }

    private void checkIfEntityIsDead(double health)
    {
        if (health <= 0 && isThePlayer)
        {
            isEntityDead = true;

            takeCoin((int)(money * 0.5)); //Death Punishment: Player looses 50% of money on death.
                                          //print("Is entity Dead? " + isEntityDead);
                                          //Debug.Log("KILL PLAYER");
            GameMaster.KillPlayer(this);
            entityStats.entityHealth = maxHealth;
            AudioManager.Instance.PlaySound(AllSFX.getDeathSound());
            if (this.gameObject.GetComponent<Animator>() != null)
            {
                anim.SetTrigger("Death");
            }


        }
        else if (health <= 0 && !isThePlayer)
        {
            if (respawns)
            {
                coinsDroppedOnDeath = money;
                takeCoin(money);
                isEntityDead = true;
                print("A UNPOSESSED PLAYER HAS DIED AND SHOULD BE RESPAWNED!");
                entityStats.entityHealth = maxHealth;
                AudioManager.Instance.PlaySound(AllSFX.getDeathSound());
                if (this.gameObject.GetComponent<Animator>() != null)
                {
                    anim.SetTrigger("Death");
                }
            }
            else
            {

                //canDie = false;
                isEntityDead = true;
                //Debug.Log("KILL ENTITY");
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                GetComponent<BoxCollider2D>().enabled = false;
                if (GetComponent<BasicEnemyPatrol>() != null)
                {
                    GetComponent<BasicEnemyPatrol>().enabled = false;
                }
                AudioManager.Instance.PlaySound(AllSFX.getDeathSound());
            }
            StartCoroutine(dropCoinsOnKill());
        }
    }

    public IEnumerator invincibilityFrames(float time)
    {
        canDie = false;
        //print("CANNOT take damage");
        yield return new WaitForSeconds(time);
        canDie = true;
        //print("CAN take damage");

    }

    public IEnumerator playerInvinciblilityFlashing()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().enabled = true;

    }

    public IEnumerator entityInvincibilityFlashing()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnGUI()
    {
        if (CompareTag("Player"))
        {
            GUILayout.Space(150);
            GUILayout.Box("Coins: " + money);
            GUILayout.Box("Max Health: " + maxHealth);
            GUILayout.Box("DPH: " + damagePerHit);
            GUILayout.Box("Damage Mit.: -" + armorPercent + "%");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //coinObject = GameObject.FindGameObjectWithTag("Coin");
        if (canDie)
        {
            entityStats.entityHealth = maxHealth;
            Health = maxHealth;

            updateArmor(armorPercent);

            isEntityDead = false;
            //print(tag + " Current Health: " + Health);        //This shows the tag and current health of all entities that can die on their creation.

        }
        if (isThePlayer)
        {
            isEntityDead = false;
            controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerMain>();
            healthBarUpdate = GameObject.FindGameObjectWithTag("Health Bar").GetComponent<HealthBarUpdate>();
            healthBarUpdate.SetMaxHealth((int)maxHealth);
            anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
            DataGM.LoadGameDataValuesToDataGM();
            money = DataGM.playerMoney;
        }

    }

}
