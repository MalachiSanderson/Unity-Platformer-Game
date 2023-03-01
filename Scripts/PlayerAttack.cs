using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackCenter;
    public Transform downAttackCenter;
    public GameObject hurtBox;
    public GameObject downHurtBox;
    public bool doesAttackUseVelocity;
    private bool canAttack;
    public float attackFrames;
    public float attackDelayTimer; //NOTE THIS NEEDS TO BE LONGER THAN ATTACK FRAMES!
    private bool straightAtk;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        canAttack = true;
        player = GameObject.FindGameObjectWithTag("Player");
        if (attackFrames > attackDelayTimer)
        {
            print("WARNING DELAY TIMER SHOULD BE GREATER THAN ATTACK FRAMES");
        }

    }

    public IEnumerator hitBoxDestroyer()
    {
        yield return new WaitForSeconds(attackFrames);
        Destroy(GameObject.FindGameObjectWithTag("Player Hurt Box"));
        straightAtk = false;
        //print("This Hurt box should be destroyed!");
    }

    public IEnumerator attackDelay()
    {
        yield return new WaitForSeconds(attackDelayTimer);
        canAttack = true;
        straightAtk = false;

    }

    void AttackStraight()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            if (!(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                straightAtk = true;

                //hurtBox.GetComponent<BoxCollider2D>().enabled = false; //THIS IS NEW
                Instantiate(hurtBox, new Vector2(attackCenter.position.x, attackCenter.position.y), Quaternion.identity);

                canAttack = false;
                if (GameObject.FindGameObjectWithTag("Player Hurt Box") != null)
                {
                    GameObject playerHurtBox = GameObject.FindGameObjectWithTag("Player Hurt Box");
                    //playerHurtBox.transform.position = new Vector3(attackCenter.position.x, attackCenter.position.y, attackCenter.position.z);
                   
                    playerHurtBox.GetComponent<BoxCollider2D>().enabled = true; //THIS IS NEW
                }
                StartCoroutine(hitBoxDestroyer());
                StartCoroutine(attackDelay());
            }
        }

    }




    void AttackDown()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                Instantiate(downHurtBox, new Vector2(downAttackCenter.position.x, downAttackCenter.position.y), Quaternion.identity);
                canAttack = false;
                //print("DOWN ATTACK");
                

                if (GameObject.FindGameObjectWithTag("Player Hurt Box") != null)
                {
                    GameObject playerHurtBox = GameObject.FindGameObjectWithTag("Player Hurt Box");
                    playerHurtBox.transform.position = new Vector2(downAttackCenter.position.x, downAttackCenter.position.y);
                    playerHurtBox.GetComponent<BoxCollider2D>().enabled = true;
                }
                StartCoroutine(hitBoxDestroyer());
                StartCoroutine(attackDelay());
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>() != null)
                {
                    player.GetComponent<PlayerAnimation>().airAttack = true;
                }
            }
        }

    }

    private void Update()
    {
        if ((this.gameObject.GetComponent<PlayerControllerMain>().IsControlAllowed))
        {
            AttackStraight();
            AttackDown();
        }
        if (GameObject.FindGameObjectWithTag("Player Hurt Box") != null)
        {
            GameObject playerHurtBox = GameObject.FindGameObjectWithTag("Player Hurt Box");

            if (straightAtk)
            {
                //playerHurtBox.GetComponent<BoxCollider2D>().enabled = false;
                playerHurtBox.transform.position = new Vector2(attackCenter.position.x, attackCenter.position.y);
                //playerHurtBox.GetComponent<BoxCollider2D>().enabled = true;
            }
            else
            {
                //playerHurtBox.GetComponent<BoxCollider2D>().enabled = false;
                playerHurtBox.transform.position = new Vector2(downAttackCenter.position.x, downAttackCenter.position.y);
                // playerHurtBox.GetComponent<BoxCollider2D>().enabled = true;
            }

        }
    }
}
