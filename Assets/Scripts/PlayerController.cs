using System.Collections;
using System.Collections.Generic;
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
    [Space(10)]
    [SerializeField] private Animator animator;
    private bool isWalking = false;

    bool IsGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Jumped");
        }
    }

    void Awake()
    {
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
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;
            Debug.Log("Moved right");
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;
            Debug.Log("Moved left");
        }
        else
        {
            isWalking = false;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetBool("isWalking", isWalking);
    }
}
