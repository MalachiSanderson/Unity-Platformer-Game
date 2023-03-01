using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanavesa;

public class Upgrade : MonoBehaviour
{

    public bool increasesAmountOfJumps;
    public bool givesDash;
    public bool increasesMoveSpeed;
    public bool increasejumpHeight;
    public bool buffsStrikeDamage;
    public double strikeDamageIncreased;
    public bool healthUpgrade;
    public bool armorUpgrade;

    public enum UpgradeType { ExtraJump, Dash, MoveSpeed, JumpHeight, MeleeDamageBuff, HealthUpgrade, ArmorUpgrade };
    public UpgradeType upgradeType;

    private double armorIncreased = 7;
    private int healthIncreased = 25;
    private Color originalColor;




    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetComponent<PlayerControllerMain>() != null && other.GetComponent<Entity>() != null)
        {
            GetComponent<Collider2D>().isTrigger = false;
            if (other.CompareTag("Player") && other.GetComponent<Entity>().isThePlayersTrueBody)
            {
                PlayerControllerMain controller = other.GetComponent<PlayerControllerMain>();
                Entity entity = other.GetComponent<Entity>();

               
                playUpgradeCollectedEffects(other);



                print("The Player has hit an Upgrade!");
                upgradeTypeChecker(controller,entity);

                DataGM.addProgress(3);//[TODO] GIVE PLAYER PROGRESS BASED ON NUMBER OF TOTAL NUMBER OF UPGRADES IN LEVEL.

                if (GetComponent<PlayerControllerMain>() != null && GetComponent<Entity>() != null)
                {

                    Destroy(GetComponent<Upgrade>());
                }
                else
                {
                    Destroy(gameObject);
                    //print("THIS UPGRADE SHOULD BE DESTROYED");
                }


            }
            else if (other.CompareTag("Player") && !other.GetComponent<Entity>().isThePlayersTrueBody && other.GetComponent<Upgrade>() == null)
            {
                other.gameObject.AddComponent<Upgrade>();
                Upgrade carriedUpgrade = other.GetComponent<Upgrade>();
                print("You've picked up an upgrade, in-order to actually pick it up you must take it back to your player and grab it from this");
                CopyComponentValues.GetCopyOf(this, carriedUpgrade);
                //upgradeComponentToFakePlayer(carriedUpgrade);


                Debug.Log("[TODO] --------> Create an effect that shows that the player is holding an upgrade!");

                Destroy(this.gameObject);

            }
            this.GetComponent<Collider2D>().isTrigger = true;
        }

    }


    private void upgradeTypeChecker(PlayerControllerMain controller, Entity entity)
    {
        switch(upgradeType)
        {
            case UpgradeType.ExtraJump:
                showUpgradeAquiredText("Double Jump Aquired!");
                GameMaster.changeToolTipText("Press the jump key once you are already in the air to double jump.\nYour jump resets on contact with the ground or on bounce hitting an " +
                    "enemy.\nWait for the jump to reach its apex to maximize your height.", 7f);
                controller.increaseNumberOfJumps(1);
                break;

            case UpgradeType.Dash:
                showUpgradeAquiredText("Dash Aquired!");
                GameMaster.changeToolTipText("Press [Left Shift] to dash in the direction you're facing.\nYou can dash once while in mid-air. Your dash resets on contact with the ground or " +
                    "on bounce hitting an enemy.\nDashing makes you invincible.", 7f);
                controller.canDash = true;
                break;

            case UpgradeType.MoveSpeed:
                showUpgradeAquiredText("Movement Speed Upgrade Aquired!");
                controller.speed = (float)(controller.playerMoveSpeed * 1.5);
                break;

            case UpgradeType.JumpHeight:
                showUpgradeAquiredText("Super Jump Upgrade Aquired!");
                controller.jumpSpeed = (float)(controller.jumpVelocity * 1.5);
                break;

            case UpgradeType.MeleeDamageBuff:
                showUpgradeAquiredText("Attack Damage Increased!");
                entity.damagePerHit += strikeDamageIncreased;
                print("Increased player damage by : " + strikeDamageIncreased);
                break;

            case UpgradeType.HealthUpgrade:
                showUpgradeAquiredText("Health Upgrade Aquired!");
                entity.increaseHealthMax(healthIncreased);
                break;

            case UpgradeType.ArmorUpgrade:
                showUpgradeAquiredText("Armor Increased by 7%");
                entity.updateArmor(entity.armorPercent + armorIncreased);
                break;

        }

    }


    private void upgradeComponentToFakePlayer(Upgrade newUpgradeCarrier)
    {
        switch(upgradeType)
        {
            case UpgradeType.ExtraJump:
                newUpgradeCarrier.upgradeType = UpgradeType.ExtraJump;
                break;

            case UpgradeType.Dash:
                newUpgradeCarrier.upgradeType = UpgradeType.Dash;
                break;

            case UpgradeType.MoveSpeed:
                newUpgradeCarrier.upgradeType = UpgradeType.MoveSpeed;
                break;

            case UpgradeType.JumpHeight:
                newUpgradeCarrier.upgradeType = UpgradeType.JumpHeight;
                break;

            case UpgradeType.MeleeDamageBuff:
                newUpgradeCarrier.upgradeType = UpgradeType.MeleeDamageBuff;
                break;

            case UpgradeType.HealthUpgrade:
                newUpgradeCarrier.upgradeType = UpgradeType.HealthUpgrade;
                break;

            case UpgradeType.ArmorUpgrade:
                newUpgradeCarrier.upgradeType = UpgradeType.ArmorUpgrade;
                break;
        }
    }

    private void playUpgradeCollectedEffects(Collider2D other)
    {
        //SOUND EFFECT
        try
        {
            AudioManager.Instance.PlaySound(AllSFX.getUpgradeSound());
        }
        catch (UnassignedReferenceException)
        {
            GameMaster.makePopupWorldText(GameAssets.i.genericWorldPopupText, this.transform.position, "[ERROR: SOUND EFFECT UNASSIGNED]", 1f, Color.red);
            Debug.LogWarning("WARNING: THE SOUND EFFECT CALLED WAS NOT PROPERLY ASSIGNED IN THE GAMEASSETS OBJ.");
            this.GetComponent<Collider2D>().isTrigger = false;
        }
        catch (UnityException e)
        {
            Debug.LogException(e);
        }

        //PARTICLE EFFECT
        try
        {
            Instantiate(GameAssets.i.upgradeCollectedParticleEffect, other.transform.position, Quaternion.identity);
        }
        catch (UnassignedReferenceException)
        {
            GameMaster.makePopupWorldText(GameAssets.i.genericWorldPopupText, this.transform.position, "[ERROR: PARTICLE EFFECT UNASSIGNED]", 1f, Color.red);
            Debug.LogWarning("WARNING: THE PARTICLE EFFECT CALLED WAS NOT PROPERLY ASSIGNED IN THE GAMEASSETS OBJ.");
            this.GetComponent<Collider2D>().isTrigger = false;
        }
        catch (UnityException e)
        {
            Debug.LogException(e);
        }
    }


    private void showUpgradeAquiredText(string s)
    {
        GameMaster.makePopupWorldText(GameAssets.i.genericWorldPopupText, this.transform.position, s, 1f, Color.blue);
    }
}
