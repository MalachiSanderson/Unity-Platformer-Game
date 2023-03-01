using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sanavesa;
using System;


public class PlayerControllerMain : MonoBehaviour
{
    private Vector2 playerVelocity;
    private Rigidbody2D body;
    private Animator anim;


    public bool IsControlAllowed;  //if you have the true you can move the player...

    [Range(1, 20)] //Max Speed the player moves...
    public float playerMoveSpeed;
    //public AudioClip walkSound;
    [HideInInspector] public float speed;

    [Range(1, 25)] //Max jump height of the player...
    public float jumpVelocity;
    [HideInInspector] public float jumpSpeed;

    public int amountOfJumpsAfterJumping; //Controls how many jumps the player has...
    [HideInInspector] public int extraJumps;     //IMPORTANT (do not change this in the component) this is only public so it can be changed.
    public GameObject doubleJumpParticleEffect;

    public bool canDash;
    [SerializeField] private float dashSpeed = 10; //# units per sec
    [SerializeField] private float dashDuration = 0.2f; //# seconds dash lasts
    private bool isDashing = false;
    private float remainingDashDuration = 0;
    [SerializeField] private int numberOfAirDashes;
    private int airDashes;
    public GameObject dashParticleEffect;

    public float fallMultiplier = 1.5f; //The falling speed multiplier for when holding jump...
    public float lowJumpMultiplier = 1f; //The falling speed multiplier for when tapping jump...
    [Range(10, 90)] //Acceleration the player gets when starting to walk...
    public float walkAcceleration = 20;
    [Range(10, 90)]  //Deceleration applied when character is grounded and not attempting to move.
    public float groundDeceleration = 70;
    private float moveInput;
    [HideInInspector] public bool facingRight; //Tells you if the player is facing right or left...

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private bool isWallBehind;
    public Transform wallCheckBehind;
    private bool isWallInFront;
    public Transform wallCheckInFront;
    public float wallCheckRadius;
    public LayerMask whatIsWall;


    private Entity entity;
    private bool currentlySwappingBodies;


    //Acceleration while in the air.
    //float airAcceleration = 30;

    void Start()
    {
        extraJumps = amountOfJumpsAfterJumping;
        airDashes = numberOfAirDashes;
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        speed = playerMoveSpeed;
        jumpSpeed = jumpVelocity;
        facingRight = true;
        entity = GetComponent<Entity>();
        if(DataGM.player_MoveSpeed != 0)
        {
            DataGM.SetupSavedControllerValues(this);
        }
    }



    // Update is called once per frame
    void Update()
    {
        quitGameInput(); //[TODO] Make pause menu...
        returnToPlayerFromHostCommand();

        if (isGrounded)
        {
            resetJumps();
            resetDashes();
        }


        if (isDashing)
        {
            entity.canDie = false;
            remainingDashDuration -= Time.deltaTime;
            if (facingRight && isWallInFrontOfPlayer())
            {
                //print("Ended Dash Early right");
                remainingDashDuration = 0;
            }
            else if (!facingRight && isWallInFrontOfPlayer())
            {
                //print("Ended Dash Early left");
                remainingDashDuration = 0;
            }

            //If dash is over...
            if (remainingDashDuration <= 0)
            {
                isDashing = false;
                if (facingRight && isWallInFrontOfPlayer())
                {
                    //print("Gave player extra boost right");
                    body.transform.position = new Vector2((float)(transform.position.x + 0.25), transform.position.y);
                }
                else if (!facingRight && isWallInFrontOfPlayer())
                {
                    //print("Gave player extra boost left");
                    body.transform.position = new Vector2((float)(transform.position.x - 0.25), transform.position.y);
                }
                body.velocity = Vector2.zero;
                body.gravityScale = 1.2f;
                extraJumps = amountOfJumpsAfterJumping;
            }
        }

        if (IsControlAllowed)
        {
            if (!isDashing)
            {
                entity.canDie = true;
                moveCharacterINPUTS();
                if (canDash && !isWallInFrontOfPlayer())
                {
                    if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
                    {
                        dash();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftShift) && airDashes > 0)
                    {
                        dash();
                        airDashes--;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftShift) && airDashes == 0 && isGrounded == true)
                    {
                        dash();
                        airDashes = numberOfAirDashes;
                    }
                }
            }
            checkJump();
        }
    }

    private void resetDashes()
    {
        airDashes = numberOfAirDashes;
    }

    private void resetJumps()
    {
        extraJumps = amountOfJumpsAfterJumping;
    }

    //Fixed update is run after every physics step... Use when working with phsyics...
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (IsControlAllowed)
        {

            if (body.velocity.y < 0)
            {
                //print("fall 1 triggered");
                body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (body.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                //print("fall 2 triggered");
                body.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }


            if (facingRight == false && moveInput > 0)
            {
                flipPlayerSprite();
                //print("Facing right");
            }
            else if (facingRight == true && moveInput < 0)
            {
                flipPlayerSprite();
                //print("Facing left");
            }

        }


    }

    void moveCharacterINPUTS()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if (isThePlayerGrounded())
        {
            if (moveInput != 0)
            {
                playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, speed * moveInput, walkAcceleration * Time.deltaTime);
            }
            else if (moveInput == 0)
            {
                playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, 0, groundDeceleration * Time.deltaTime);
            }
        }
        else
        {
            if (moveInput != 0)
            {
                playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, (float)(speed * moveInput * 1.17), ((float)(walkAcceleration * 1.5)) * Time.deltaTime);

            }
            else if (moveInput == 0)
            {
                playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, 0, groundDeceleration * Time.deltaTime);
            }
        }
        body.velocity = new Vector2(playerVelocity.x, body.velocity.y);

    }


    void flipPlayerSprite()
    {
        facingRight = !facingRight;
        Vector3 scalar = transform.localScale;
        scalar.x *= -1;
        transform.localScale = scalar;
    }

    public bool isThePlayerGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        return isGrounded;
    }

    public bool isWallBehindPlayer()
    {
        isWallBehind = Physics2D.OverlapCircle(wallCheckBehind.position, wallCheckRadius, whatIsWall);
        print("WALL IS ON LEFT: " + isWallBehind);
        return isWallBehind;
    }

    public bool isWallInFrontOfPlayer()
    {
        isWallInFront = Physics2D.OverlapCircle(wallCheckInFront.position, wallCheckRadius, whatIsWall);

        return isWallInFront;
    }

    public void increaseNumberOfJumps(int number)
    {
        amountOfJumpsAfterJumping = number;
    }

    private void dash()
    {

        if (!isDashing)
        {

            if (facingRight)
            {
                isDashing = true;
                remainingDashDuration = dashDuration;
                body.velocity = new Vector2(dashSpeed, 0);
                body.gravityScale = 0;
                Instantiate(dashParticleEffect, transform.position, Quaternion.identity);
                AudioManager.Instance.PlaySound(AllSFX.getDashSound());
            }
            else if (!facingRight)
            {
                isDashing = true;
                remainingDashDuration = dashDuration;
                body.velocity = new Vector2(-dashSpeed, 0);
                body.gravityScale = 0;
                Instantiate(dashParticleEffect, transform.position, Quaternion.identity);
                AudioManager.Instance.PlaySound(AllSFX.getDashSound());
            }


        }

    }

    /// <summary>
    /// Just a method that calls jump() if the user presses space bar.
    /// </summary>
    private void checkJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
    }

    /// <summary>
    /// checks if player is grounded and if so jumps normally, and if not but the player has more than zero 
    /// extra jumps remaining it does "double jump" effects and subtracts one from extraJumps.
    /// If extra jumps is zero but player is grounded reset extra jumps and jump.
    /// </summary>
    private void jump()
    {
        if (isGrounded)
        {
            body.velocity = Vector2.up * jumpSpeed;
        }
        else if (extraJumps > 0)
        {
            body.velocity = Vector2.up * jumpSpeed;
            extraJumps--;
            AudioManager.Instance.PlaySound(AllSFX.getDoubleJumpSound());
            Instantiate(doubleJumpParticleEffect, transform.position, Quaternion.identity);
        }
        else if (extraJumps == 0 && isGrounded)
        {
            Debug.LogWarning("WARNING: NOTE THAT THIS CONDITION (WHICH SHOULDN'T BE POSSIBLE) WAS MET SOMEHOW!!!!");
            GameMaster.FreezeGame(true);
            body.velocity = Vector2.up * jumpSpeed;
            extraJumps = amountOfJumpsAfterJumping;
        }
    }


    void returnToPlayerFromHostCommand()
    {
        if (!entity.isThePlayersTrueBody && Input.GetKeyDown(KeyCode.G) && !currentlySwappingBodies && IsControlAllowed)
        {
            currentlySwappingBodies = true;
            GameObject realPlayer = GameMaster.getTruePlayer(); //WARNING IF YOU GET AN ERROR HERE IT'S BECAUSE YOUR PLAYER ISN'T NAMED THE SAME AS IT SAYS IN GAMEMASTER SCRIPT!!!!!!!
            Entity realPlayerEntity = realPlayer.GetComponent<Entity>();
            PlayerControllerMain realPlayerController = realPlayer.GetComponent<PlayerControllerMain>();

            entity.isThePlayer = false;

            if (GetComponent<PlayerAttack>() != null)
            {
                GetComponent<PlayerAttack>().enabled = false;
            }
            realPlayer.GetComponent<PlayerAttack>().enabled = true;
            this.tag = "Untagged";
            IsControlAllowed = false;
            StartCoroutine(returnToTruePlayerDelay(realPlayer, realPlayerEntity, realPlayerController));
            GetComponent<BoxCollider2D>().enabled = true;
            body.velocity = new Vector2(0, 0);
            Instantiate(GameAssets.i.bodySwapParticleEffect, transform.position, Quaternion.identity);
            body.sharedMaterial = GameAssets.i.highFrictionMaterial;
            realPlayer.GetComponent<Rigidbody2D>().sharedMaterial = GameAssets.i.frictionlessMaterial; ;

        }

    }

    public IEnumerator returnToTruePlayerDelay(GameObject realPlayer, Entity realPlayerEntity, PlayerControllerMain realPlayerController)
    {

        yield return new WaitForSeconds(0.3f);
        print("The Player Has Returned To Their True Body!");

        realPlayer.gameObject.tag = "Player";
        realPlayerEntity.isThePlayer = true;
        realPlayerController.IsControlAllowed = true;
        //?????????realPlayerEntity.controller = GetComponent<PlayerControllerMain>();
        realPlayerController.enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        realPlayerEntity.updateHealthBar();

        //This updates the camera and background's target.
        GameObject.Find("Main Camera").GetComponent<CameraFollowPlayer>().UpdatePlayerTransform();
        GameObject.FindGameObjectWithTag("Tracks Player").GetComponent<CameraFollowPlayer>().UpdatePlayerTransform();
        Instantiate(GameAssets.i.bodySwapParticleEffect, realPlayer.transform.position, Quaternion.identity);
        currentlySwappingBodies = false;

        this.enabled = false;

    }

    void quitGameInput() //************LETS PLAYER QUIT GAME...
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            DataGM.PlayerControllerSave(this);
            DataGM.SaveCurrentGameData();
            print("Player Speed Is: "+ DataGM.player_MoveSpeed);
            print("Player Number of Jumps Is: " + (1+ DataGM.player_Jumps));
            MainMenuManager.LoadMainMenuScene();
        }


    }

}
