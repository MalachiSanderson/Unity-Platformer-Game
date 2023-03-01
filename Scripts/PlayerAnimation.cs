using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public bool airAttack;




    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isThePlayerGrounded();

        if (body.velocity.x > 0.5 || body.velocity.x < -0.5)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        if (isThePlayerGrounded())
        {
            anim.SetBool("falling", false);
            anim.SetBool("jumping", false);
        }
        if (GameObject.FindGameObjectWithTag("Player Hurt Box") != null && GetComponent<PlayerControllerMain>().IsControlAllowed)
        {
            if (airAttack)
            {
               // print("Down Attack Animation");
                anim.SetTrigger("DownAttacking");
                anim.SetBool("jumping", false);
                anim.SetBool("falling", false);
                anim.SetBool("isRunning", false);
                airAttack = false;
            }
            else 
            {
                anim.SetTrigger("Attacking");
                anim.SetBool("jumping", false);
                anim.SetBool("falling", false);
                anim.SetBool("isRunning", false);
            }
        }
        else
        {
            //anim.SetBool("Attacking", false);
        }
    }
   
    void FixedUpdate()
    {
        anim.SetBool("jumping", !isThePlayerGrounded() && body.velocity.y > 0);
        anim.SetBool("falling", !isThePlayerGrounded() && body.velocity.y < 0);
    }

    public bool isThePlayerGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        return isGrounded;
    }
}
