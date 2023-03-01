using UnityEngine;

public class BasicEnemyPatrol : MonoBehaviour
{

    public float speed;
    public float howFarDownIsRaycast;
    public Transform groundDetection;
   // public Rigidbody2D body;
    public float groundCheckDistance;
    private Vector3 gcPosition;
    private bool movingRight = true;



    private void Start()
    {
        //body = this.gameObject.GetComponent<Rigidbody2D>();
        gcPosition = groundDetection.position;
        gcPosition.x += groundCheckDistance;
        groundDetection.transform.position = gcPosition;

    }

    private void Update()
    {
        
        /*
        float bodyMoveX = speed * Time.deltaTime;
        body.velocity = new Vector2(bodyMoveX, body.velocity.y);
        */
        //NEED TO FIGURE OUT HOW TO MOVE IT USING RIGIDBODY.VELOCITY or RB.MovePosition....
        transform.Translate(Vector2.right*speed*Time.deltaTime);
        
        RaycastHit2D groundChecker = Physics2D.Raycast(groundDetection.position, Vector2.down, howFarDownIsRaycast);
        if(groundChecker.collider == false)
        {
            if(movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }



}
