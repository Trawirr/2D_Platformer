using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleController : MonoBehaviour
{
    [Range(0.01f, 10.0f)] [SerializeField] private float moveSpeed = 3.0f; // moving speed of the player
    [SerializeField] private Animator animator;
    private bool isFacingRight = false;
    private bool isMovingRight = false;
    private float startPositionX;
    private float moveRange = 5.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.gameObject.transform.position.y > transform.position.y)
            {
                animator.SetBool("isDead", true);
                StartCoroutine(KillOnAnimationEnd());
            }
        }
    }

    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        isMovingRight = isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        Debug.Log("Enemy flip");
    }

    void moveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void moveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        startPositionX = this.transform.position.x;
        animator = GetComponent<Animator>();
        /*rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();*/
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            if (this.transform.position.x - startPositionX < moveRange)
            {
                moveRight();
            }
            else
            {
                Flip();
                moveLeft();
            }
        }
        else
        {
            if (this.transform.position.x - startPositionX > -moveRange)
            {
                moveLeft();
            }
            else
            {
                Flip();
                moveRight();
            }
        }
    }
}
