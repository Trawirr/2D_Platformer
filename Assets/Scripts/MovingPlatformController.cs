using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [Range(0.01f, 10.0f)] [SerializeField] private float moveSpeed = 2.0f; // moving speed of the player
    private float startPositionX;
    private bool isMovingRight = false;
    private float moveRange = 3.0f;

    void moveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void moveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    void Awake()
    {
        startPositionX = this.transform.position.x;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            if (isMovingRight)
            {
                if (this.transform.position.x - startPositionX < moveRange)
                {
                    moveRight();
                }
                else
                {
                    isMovingRight = !isMovingRight;
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
                    isMovingRight = !isMovingRight;
                    moveRight();
                }
            }
        }
    }
}
