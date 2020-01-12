using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public GameObject dustEffect;
    public Transform groundCheck;
    public LayerMask whatIsGround;

    public float movementSpeed = 40f;
    public float checkRadius = 10f;

    float horizontalMoveSpeed = 0f;
    bool isJumping = false;
    
    private bool isGrounded;
    private bool spawnDust;



    void Update()
    {
        horizontalMoveSpeed = Input.GetAxisRaw("Horizontal") * movementSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMoveSpeed));

        if (isGrounded == true)
        {
            if (spawnDust ==true)
            {
                Instantiate(dustEffect, groundCheck.position, Quaternion.identity);
                spawnDust = false;
            }
        }
        else
        {
            spawnDust = true;
        }

        if (isGrounded == true && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        controller.Move(horizontalMoveSpeed * Time.fixedDeltaTime, isJumping);
        isJumping = false;
    }
}
