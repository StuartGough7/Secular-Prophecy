using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    float horizontalMoveSpeed = 0f;
    public float movementSpeed = 40f;
    bool isJumping = false;

    void Update()
    {
        horizontalMoveSpeed = Input.GetAxisRaw("Horizontal") * movementSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMoveSpeed));
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMoveSpeed * Time.fixedDeltaTime, isJumping);
        isJumping = false;
    }
}
