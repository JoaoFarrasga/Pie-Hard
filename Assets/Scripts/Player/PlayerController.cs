using UnityEngine;
using UnityEngine.InputSystem;

// Controlador principal para o movimento do Jogador
public class PlayerController : MonoBehaviour
{
    // Propriedades para serem usadas na Player State Machine
    public bool isIdle { get; private set; } = true;    // Indica que o Jogador esta parado
    public bool isMoving { get; private set; } = false; // Indica se o Jogador est� a mover

    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Velocidade do movimento do Jogador

    [Header("Game Field Bounds")]
    public Vector2 minBounds = new Vector2(-10f, -10f); // Limites m�nimos do campo de jogo (X e Z)
    public Vector2 maxBounds = new Vector2(10f, 10f);   // Limites m�ximos do campo de jogo (X e Z)

    [Header("Field Division")]
    public bool isPlayerOnLeftSide = true; // Indica se o Jogador est� no lado esquerdo do Campo

    private Vector2 moveInput;      // Entrada do Movimento do Jogador (Teclado ou Comando)
    private Rigidbody rb;           // Refer�ncia ao Rigidbody
    private Vector3 movementInput;  // Entrada de Movimento convertida para um vetor 3D

    private void Awake()
    {
        // Inicializa a refer�ncia ao Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    // Fun��o chamada pelo sistema de Input do Unity (Quando o jogador fornece entradade movimento)
    public void OnMove(InputValue value)
    {
        // Armazena o valor de entrada do jogador
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        // Converte a entrada do movimento para um vetor 3D
        movementInput = new Vector3(moveInput.x, 0, moveInput.y);

        // Atualiza os estados de Movimento com base na entrada do Jogador
        if (movementInput != Vector3.zero)
        {
            // O Jogador est� em movimento
            isIdle = false;
            isMoving = true;
        }
        else
        {
            // O Jogador est� parado
            isIdle = true;
            isMoving = false;
        }
    }

    private void FixedUpdate()
    {
        // O movimento do Jogador � processado somente quando ele se est� a mover
        if (isMoving)
        {
            MovePlayer(); // Move o Jogador
        }
    }

    // Fun��o que realiza o movimento do jogador e aplica as suas restri��es
    private void MovePlayer()
    {
        // Calcula a nova posi��o com base na entrada de movimento, velocidade e deltaTime
        Vector3 newPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;

        // Restringe a posi��o X e Z do jogador pelos limites do campo
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.z = Mathf.Clamp(newPosition.z, minBounds.y, maxBounds.y);

        // Impede o jogador de atravessar a barreira imagin�ria no eixo X = 0
        if (isPlayerOnLeftSide && newPosition.x > 0) // Se o Jogador est� na esquerda
        {
            newPosition.x = 0;
        }
        else if (!isPlayerOnLeftSide && newPosition.x < 0) // Se o Jogador est� na Direita
        {
            newPosition.x = 0;
        }

        // Move o jogador para a sua nova posi��o calculada
        rb.MovePosition(newPosition);
    }
}
