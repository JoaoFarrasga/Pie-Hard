using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool isIdle { get; private set; } = true;
    public bool isMoving { get; private set; } = false;


    public float moveSpeed = 5f;

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
        Vector3 movement = movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
