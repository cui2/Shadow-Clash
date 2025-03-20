using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(InputReader))]
public class PlayerControllerTest : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputReader inputReader;
    private Vector2 moveDirection;
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    private bool isGrounded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputReader = GetComponent<InputReader>();
    }

    private void OnEnable()
    {
        inputReader.OnMove += HandleMove;
        inputReader.OnJump += HandleJump;
    }

    private void OnDisable()
    {
        inputReader.OnMove -= HandleMove;
        inputReader.OnJump -= HandleJump;
    }

    private void HandleMove(Vector2 direction)
    {
        moveDirection = direction;
    }

    private void HandleJump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
