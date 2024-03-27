using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerCharacter playerCharacter;
    private float horizontal;
    private bool isFacingRight = true;
    private bool isKnockedBack = false; 

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
        if (playerCharacter == null)
        {
            Debug.LogError("PlayerCharacter component not found on the GameObject.");
        }
    }

    void Update()
    {
        if (isKnockedBack) return; // Disable input if player is being knocked back

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, playerCharacter.jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Knockback(10f); 
        }

        Flip();
    }
    public bool getFacingRight()
    {
        return isFacingRight;
    }
    private void FixedUpdate()
    {
        if (isKnockedBack && IsGrounded())
        {
            isKnockedBack = false; // Re-enable movement once grounded
            return;
        }
        else if (isKnockedBack) return;

        rb.velocity = new Vector2(horizontal * playerCharacter.speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void Knockback(float kbForce)
    {
        if (!playerCharacter.isInvulnerable)
        {
            isKnockedBack = true; // Disable player movement

            float direction = isFacingRight ? -1f : 1f; // Determine knockback direction

            rb.velocity = new Vector2(0, 0); 
            transform.position = transform.position + new Vector3(0, 0.2f, 0); //prevents ground catching
            Vector2 knockbackForce = new Vector2(kbForce * direction, kbForce).normalized;
            rb.AddForce(knockbackForce * kbForce, ForceMode2D.Impulse);
        }
    }
}
