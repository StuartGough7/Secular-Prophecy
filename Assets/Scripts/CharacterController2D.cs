using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                                                                                      // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;                                                              // How much to smooth out the movement

    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;                                                                                                      // For determining which way the player is currently facing.
    private Vector2 m_Velocity = Vector2.zero;


    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Move(float move, bool jump)
    {
        Vector2 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);                                                         // Move the character by finding the target velocity
        m_Rigidbody2D.velocity = Vector2.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);           // And then smoothing it out and applying it to the character

        if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip();
            }

        if (jump)
        {
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }


    private void Flip()
    {
        m_FacingRight = !m_FacingRight;                                                             // Switch the way the player is labelled as facing.
        Vector2 theScale = transform.localScale;                                                    // Multiply the player's x local scale by -1.
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
