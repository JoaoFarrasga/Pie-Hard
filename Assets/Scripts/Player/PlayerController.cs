using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool isIdle { get; private set; } = true;
    public bool isMoving { get; private set; } = false;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Game Field Bounds")]
    public Vector2 minBounds = new Vector2(-10f, -10f);
    public Vector2 maxBounds = new Vector2(10f, 10f);

    [Header("Field Division")]
    public bool isPlayerOnLeftSide = true;

    private Vector2 moveInput;
    private Rigidbody rb;
    private Vector3 movementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        movementInput = new Vector3(moveInput.x, 0, moveInput.y);

        if (movementInput != Vector3.zero)
        {
            isIdle = false;
            isMoving = true;
        }
        else
        {
            isIdle = true;
            isMoving = false;
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        Vector3 newPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.z = Mathf.Clamp(newPosition.z, minBounds.y, maxBounds.y);

        if (isPlayerOnLeftSide && newPosition.x > 0)
        {
            newPosition.x = 0;
        }
        else if (!isPlayerOnLeftSide && newPosition.x < 0)
        {
            newPosition.x = 0;
        }

        rb.MovePosition(newPosition);
    }
}
