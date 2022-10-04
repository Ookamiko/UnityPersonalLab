using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera mainCamera;
    private BoxCollider2D playerColl;
    private Rigidbody2D playerRb;

    private float offsetBorder = 0.1f;
    public bool canJump = true;

    private float LeftBorder
    {
        get
        {
            float cameraRatio = mainCamera.aspect;
            float cameraHalfWidth = mainCamera.orthographicSize * cameraRatio;
            Vector2 cameraPosition = mainCamera.gameObject.transform.position;

            float playerHalfWidth = playerColl.size.x / 2;

            return cameraPosition.x - cameraHalfWidth + playerHalfWidth + offsetBorder;
        }
    }

    private float RightBorder
    {
        get
        {
            float cameraRatio = mainCamera.aspect;
            float cameraHalfWidth = mainCamera.orthographicSize * cameraRatio;
            Vector2 cameraPosition = mainCamera.gameObject.transform.position;

            float playerHalfWidth = playerColl.size.x / 2;

            return cameraPosition.x + cameraHalfWidth - playerHalfWidth - offsetBorder;
        }
    }

    public float moveSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        CheckMovementConstraint();
    }

    private void Initialise()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerColl = GetComponent<BoxCollider2D>();
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalVelocity = horizontalInput * moveSpeed;

        playerRb.velocity = new Vector2(horizontalVelocity, 0);

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            playerRb.AddForce(Vector2.up * 200, ForceMode2D.Impulse);
            canJump = false;
        }
    }

    private void CheckMovementConstraint()
    {
        if (transform.position.x > RightBorder)
        {
            transform.position = new Vector2(RightBorder, transform.position.y);
        }
        else if (transform.position.x < LeftBorder)
        {
            transform.position = new Vector2(LeftBorder, transform.position.y);
        }
    }

    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Plateform"))
        {
            canJump = true;
        }
    }
}
