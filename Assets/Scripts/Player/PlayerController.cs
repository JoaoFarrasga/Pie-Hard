using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

// Controlador principal para o movimento do Jogador
public class PlayerController : MonoBehaviour
{
    // Propriedades para serem usadas na Player State Machine
    public bool isIdle { get; private set; } = true;    // Indica que o Jogador esta parado
    public bool isMoving { get; private set; } = false; // Indica se o Jogador está a mover

    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Velocidade do movimento do Jogador

    [Header("Game Field Bounds")]
    public Vector2 minBounds = new Vector2(-10f, -10f); // Limites mínimos do campo de jogo (X e Z)
    public Vector2 maxBounds = new Vector2(10f, 10f);   // Limites máximos do campo de jogo (X e Z)

    [Header("Field Division")]
    public bool isPlayerOnLeftSide = true; // Indica se o Jogador está no lado esquerdo do Campo
    public float fieldBoundaryOffSet = 1f; // Variável para definir o offset da linha imaginária

    [SerializeField] int playerID;
    List<GameObject> handsList = new();

    [Header("PlayerInput")]
    [SerializeField] PlayerInput playerInput;
    private InputAction throwProjectile;

    private Vector2 moveInput;      // Entrada do Movimento do Jogador (Teclado ou Comando)
    private Rigidbody rb;           // Referência ao Rigidbody
    private Vector3 movementInput;  // Entrada de Movimento convertida para um vetor 3D

    private Vector3 lastFacingDirection = Vector3.forward; // Direção que o personagem está a "olhar"

    private PlayerStateMachine playerStateMachine;

    private void Awake()
    {
        // Inicializa a referência ao Rigidbody
        rb = GetComponent<Rigidbody>();

        // Obtém a referência para o PlayerStateMachine no mesmo GameObject
        playerStateMachine = GetComponent<PlayerStateMachine>();

        playerInput = GetComponent<PlayerInput>(); // Gets the player input component of this object
        throwProjectile = playerInput.actions.FindAction("Throw"); // Finds the action throw in the input system
        throwProjectile.Enable();
    }

    private void OnDisable()
    {
        throwProjectile.Disable();
    }

    private void Start()
    {
        throwProjectile.started += i => ThrowObject(); // Stats the action throw when clicked on Throw Key
    }

    private void ThrowObject()
    {
        foreach (Transform go in gameObject.transform)
        {
            if (go.GetComponent<HandSpaceVerification>().VerifySpaceState()) continue; // Verifies if is empty or not, if is empty do not throw anything and goes to other child

            Transform projectile = go.GetChild(0); // Gets the first Child of the Object
            // Gives movement to the projectile
            
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            projectile.GetComponent<Projectile>().GetRigidBodyComponent().linearVelocity = lastFacingDirection * projectile.GetComponent<Projectile>().GetSpeed();
            projectile.transform.parent = null; // Makes the Child without Parent
            // projectile.GetComponent<Projectile>().GetColliderComponent().isTrigger = false;
            go.GetComponent<HandSpaceVerification>().ChangeSpaceState(); // Changes the hand Space to empty
            return;
        }
    }

    public int GetID() { return playerID; } // Gets Player Id

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Chao")) return;

        if (other.gameObject.CompareTag("Projectile") && other.gameObject.GetComponent<Projectile>().GetID() == 0)  // Checks if there is contact with players
        {
            foreach (Transform go in gameObject.transform)
            {
                if (go.GetComponent<HandSpaceVerification>().VerifySpaceState()) // Checks if there are any empty hands left
                {
                    other.gameObject.transform.parent = go; // Parents the projectile to the hand
                    other.gameObject.transform.localPosition = Vector3.zero;
                    go.GetComponent<HandSpaceVerification>().ChangeSpaceState(); // Indicates that this hand is not empty now
                    other.gameObject.GetComponent<Projectile>().SetID(playerID);// the projectile gets the same id as the player
                    //other.gameObject.GetComponent<Projectile>().GetColliderComponent().isTrigger = false;
                    return;
                }
            }
        }
        else
        {
            GameManager.gameManager.OnScoreChanged(other.GetComponent<Projectile>().GetID());// changes the score of the players
            Destroy(other.gameObject);//destroys projectile
        }
    }

    // Função chamada pelo sistema de Input do Unity (Quando o jogador fornece entradade movimento)
    public void OnMove(InputValue value)
    {
        // Se o personagem está no estado de TakeDamage, ignora o input
        if (playerStateMachine.currentState is TakeDamageState)
        {
            moveInput = Vector2.zero;
            return;
        }

        // Armazena o valor de entrada do jogador
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        // Se o persongaem está no estado TakeDamage, não atualiza o movimento
        if (playerStateMachine.currentState is TakeDamageState)
        {
            movementInput = Vector3.zero;
            isIdle = true;
            isMoving = false;
            return;
        }

        // Converte a entrada do movimento apara um vetor 3D
        movementInput = new Vector3(moveInput.x, 0, moveInput.y);

        // Atualiza os estados de Movimento com base na entrada do Jogador
        if (movementInput != Vector3.zero)
        {
            // O Jogador está em movimento
            isIdle = false;
            isMoving = true;

            // Atualiza a última direção que o personagem está a olhar
            lastFacingDirection = movementInput.normalized;
        }
        else
        {
            // O Jogador está parado
            isIdle = true;
            isMoving = false;
        }
    }

    private void FixedUpdate()
    {
        // Se o personagem está no esdo de TakeDamage, não precisa de se movimentar
        if (playerStateMachine.currentState is TakeDamageState)
        {
            return;
        }

        // O movimento do Jogador é processado somente quando ele se está a mover
        if (isMoving)
        {
            MovePlayer(); // Move o Jogador
        }

        RotatePlayer(); // Atualiza a rotação do Personagem
    }

    // Função que realiza o movimento do jogador e aplica as suas restrições
    private void MovePlayer()
    {
        // Calcula a nova posição com base na entrada de movimento, velocidade e deltaTime
        Vector3 newPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;

        // Restringe a posição X e Z do jogador pelos limites do campo
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.z = Mathf.Clamp(newPosition.z, minBounds.y, maxBounds.y);

        // Ajusta o limite da linha Imaginária com base no lado em que o jogador está
        float boundary = isPlayerOnLeftSide ? -fieldBoundaryOffSet : fieldBoundaryOffSet;

        // Impede o jogador de atravessar a linha imaginária
        if (isPlayerOnLeftSide && newPosition.x > boundary) // Se o Jogador está na esquerda
        {
            newPosition.x = boundary;
        }
        else if (!isPlayerOnLeftSide && newPosition.x < boundary) // Se o Jogador está na Direita
        {
            newPosition.x = boundary;
        }

        // Move o jogador para a sua nova posição calculada
        rb.MovePosition(newPosition);
    }

    private void RotatePlayer()
    {
        // Apenas rotaciona o personagem se ele estiver-se a mover
        if (isMoving)
        {
            // Clacula a rotação alvo com base na direção do movimento
            Quaternion targetRotation = Quaternion.LookRotation(lastFacingDirection);

            // Faz o personagem olhar na direção do movimento
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
        }
    }
}