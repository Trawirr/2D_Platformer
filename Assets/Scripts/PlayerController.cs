using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PLayerController : MonoBehaviour
{
    [Header("Movement parameters")]
    [Range(0.01f, 20.0f)] [SerializeField] private float moveSpeed = 0.1f; // moving speed of the player
    [SerializeField] private float jumpForce = 6.0f; // jump force of the player
    [Space(10)]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public float rayLength = 2.0f; 
    [SerializeField] public float horizontalRayOffset = 0.55f;
    [Space(10)]
    [SerializeField] private Animator animator;
    private bool isWalking = false;
    private bool isFalling = false;
    private bool isFacingRight = true;
    private uint score = 0;
    private float fallHeight = 0.0f;
    private int lives = 3;
    private int keysFound = 0;
    private int keysNumber = 3;
    private Vector2 startPosition;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            score++;
            Debug.Log("Score: " + score);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Enemy"))
        {
            // Enemy killed
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                score++;
                Debug.Log("Killed an enemy");
                Vector3 velocity = rigidBody.velocity;
                velocity.y = 0;
                rigidBody.velocity = velocity;
                Jump(true);
            }
            else
            {
                LoseLife();
            }
        }
        else if (other.CompareTag("Key"))
        {
            keysFound++;
            Debug.Log("Collected a key!");
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Heart"))
        {
            lives++;
            Debug.Log("Collected a heart! Now you have " + lives + " lives");
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
        else if (other.CompareTag("FallLevel"))
        {
            LoseLife();
        }
        else if (other.CompareTag("Checkpoint"))
        {
            startPosition = other.transform.position;
            Debug.Log("Checkpoint reached!");
        }
        else if (other.CompareTag("Finish"))
        {
            if (keysFound == keysNumber)
            {
                Debug.Log("You won! Score: " + score);
            }
            else
            {
                Debug.Log("You have found " + keysFound + "/" + keysNumber + " keys");
            }
        }
    }

    void LoseLife()
    {
        lives--;
        if (lives > 0)
        {
            Debug.Log("You died! Now you have " + lives + " lives");
            transform.position = startPosition;
        }
        else
        {
            Debug.Log("Game over! Score: " + score);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        if (isFacingRight) Debug.Log("Moved right");
        else if (!isFacingRight) Debug.Log("Moved left");
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        Debug.Log("Flip");
    }

    bool IsGrounded()
    {
        //return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
        Vector2 leftEdge = new Vector2(transform.position.x - horizontalRayOffset, transform.position.y);
        Vector2 rightEdge = new Vector2(transform.position.x + horizontalRayOffset, transform.position.y);

        RaycastHit2D hitLeft = Physics2D.Raycast(leftEdge, Vector2.down, rayLength, groundLayer.value);
        RaycastHit2D hitRight = Physics2D.Raycast(rightEdge, Vector2.down, rayLength, groundLayer.value);

        // Check if either of the edge rays hit the ground
        return hitLeft || hitRight;
    }

    void Jump(bool forceJump=false)
    {
        if (IsGrounded() || forceJump)
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Jumped");
        }
    }

    void Awake()
    {
        startPosition = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) && 
            Input.GetKey(KeyCode.LeftArrow) || 
            Input.GetKey(KeyCode.A) && 
            Input.GetKey(KeyCode.D)) 
        { }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (!isFacingRight) Flip();
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (isFacingRight) Flip();
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (!IsGrounded() && rigidBody.velocity.y < 0)
        {
            isFalling = true;
            if (fallHeight == 0)
            {
                fallHeight = rigidBody.position.y;
            }
        }
        else
        {
            isFalling = false;
            if (fallHeight != 0)
            {
                Debug.Log("Fell from " + (fallHeight - rigidBody.position.y) + "m");
                if (fallHeight - rigidBody.position.y > 10.0f)
                {
                    LoseLife();
                }
                fallHeight = 0;
            }
        }

        // Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isFalling", isFalling);
    }
}
