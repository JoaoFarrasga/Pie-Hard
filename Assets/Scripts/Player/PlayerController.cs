using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// Controlador principal para o movimento do Jogador
public class PlayerController : MonoBehaviour
{
    // Propriedades para serem usadas na Player State Machine
    public bool isIdle { get; private set; } = true;    // Indica que o Jogador esta parado
    public bool isMoving { get; private set; } = false; // Indica se o Jogador est� a mover
    public bool isThrowing { get; private set; } = false;

    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Velocidade do movimento do Jogador

    [Header("Game Field Bounds")]
    public Vector2 minBounds = new Vector2(-10f, -17.6f); // Limites m�nimos do campo de jogo (X e Z)
    public Vector2 maxBounds = new Vector2(10f, 2.4f);   // Limites m�ximos do campo de jogo (X e Z)

    [Header("Field Division")]
    public bool isPlayerOnLeftSide = true; // Indica se o Jogador est� no lado esquerdo do Campo
    public float fieldBoundaryOffSet = 1f; // Vari�vel para definir o offset da linha imagin�ria

    [SerializeField] int playerID;
    [SerializeField] List<GameObject> handsList = new();
    public int handsOcupied = 0;

    [Header("PlayerInput")]
    [SerializeField] PlayerInput playerInput;
    private InputAction throwProjectile;

    private float timeSinceThrow = 0;

    private Vector2 moveInput;      // Entrada do Movimento do Jogador (Teclado ou Comando)
    private Rigidbody rb;           // Refer�ncia ao Rigidbody
    private Vector3 movementInput;  // Entrada de Movimento convertida para um vetor 3D

    private Vector3 lastFacingDirection = Vector3.forward; // Dire��o que o personagem est� a "olhar"

    private PlayerStateMachine playerStateMachine;
    private Animator animator;
    private bool check = true;

    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Color floatingTextColor;

    private void Awake()
    {
        // Inicializa a refer�ncia ao Rigidbody
        rb = GetComponent<Rigidbody>();

        // Obt�m a refer�ncia para o PlayerStateMachine no mesmo GameObject
        playerStateMachine = GetComponent<PlayerStateMachine>();
        animator = GetComponent<Animator>();

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
        throwProjectile.started += i => ThrowStateSwitch(); // Stats the action throw when clicked on Throw Key
    }

    private void ThrowStateSwitch() 
    {
        if (handsOcupied != 0 && !isThrowing)
        {
            playerStateMachine.SwitchState(playerStateMachine.throwState);
            isThrowing = true;
        }    
    }

    public void ThrowObject()
    {
        Debug.Log("1");
        if(GameManager.gameManager.State == GameState.InGame)
        {
            Debug.Log("2");
            for (int i = handsOcupied; i > 0; i--)
            {
                Debug.Log("3");
                if (handsList[i - 1].GetComponent<HandSpaceVerification>().VerifySpaceState()) continue; // Verifies if is empty or not, if is empty do not throw anything and goes to other child

                Debug.Log("4");
                Transform projectile = handsList[i - 1].transform.GetChild(0); // Gets the first Child of the Object
                                                                 // Gives movement to the projectile
                projectile.GetComponent<Rigidbody>().isKinematic = false;
                projectile.GetComponent<Rigidbody>().useGravity = true;
                projectile.GetComponent<Projectile>().GetRigidBodyComponent().linearVelocity = lastFacingDirection * projectile.GetComponent<Projectile>().GetSpeed();
                projectile.transform.parent = null; // Makes the Child without Parent
                handsList[i - 1].GetComponent<HandSpaceVerification>().ChangeSpaceState(); // Changes the hand Space to empty
                handsOcupied--;
                return;
            }
        }
        
    }

    public int GetID() { return playerID; } // Gets Player Id

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Chao")) return;

        if (other.CompareTag("Projectile")) 
        {
            if (other.gameObject.GetComponent<Projectile>().GetID() == 0)  // Checks if there is contact with players
            {
                foreach (GameObject hand in handsList)
                {
                    if (hand.GetComponent<HandSpaceVerification>() == null) continue;

                    if (hand.GetComponent<HandSpaceVerification>().VerifySpaceState()) // Checks if there are any empty hands left
                    {
                        other.gameObject.transform.parent = hand.transform; // Parents the projectile to the hand
                        other.gameObject.transform.localPosition = Vector3.zero;
                        hand.GetComponent<HandSpaceVerification>().ChangeSpaceState(); // Indicates that this hand is not empty now
                        other.gameObject.GetComponent<Projectile>().SetID(playerID);// the projectile gets the same id as the player
                        //other.gameObject.GetComponent<Projectile>().GetColliderComponent().isTrigger = false;
                        handsOcupied++;
                        return;
                    }
                }
            }
            else if (other.gameObject.GetComponent<Projectile>().GetID() != playerID)
            {
                SoundManager.soundManager.PlayAudio(other.gameObject.GetComponent<Projectile>().GetClip(), other.gameObject.GetComponent<Projectile>().GetVolume());
                //Instantiate +1 in the game
                Vector3 offSet = new Vector3(0, 2.5f, 0);
                GameObject go = Instantiate(floatingTextPrefab, gameObject.transform.position, Quaternion.identity);
                go.transform.parent = gameObject.transform;
                go.transform.localPosition += offSet;
                go.GetComponent<TextMeshPro>().color = floatingTextColor;

                GameManager.gameManager.OnScoreChanged(other.GetComponent<Projectile>().GetID());// changes the score of the players
                Destroy(other.gameObject);//destroys projectile
            }
        }   
    }

    // Fun��o chamada pelo sistema de Input do Unity (Quando o jogador fornece entradade movimento)
    public void OnMove(InputValue value)
    {
        // Se o personagem est� no estado de TakeDamage, ignora o input
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
        if(GameManager.gameManager.State == GameState.InGame)
        {
            // Verifica se o jogador est� no estado de "Throwing"
            if (isThrowing)
            {
                // Incrementa o tempo desde o �ltimo arremesso
                timeSinceThrow += Time.deltaTime;

                // Ap�s 1 segundo, define isThrowing como false
                if (timeSinceThrow >= 1f)
                {
                    isThrowing = false;
                    playerStateMachine.SwitchState(playerStateMachine.idleState);
                    timeSinceThrow = 0f; // Reseta o contador
                }
            }

            // Se o persongaem est� no estado TakeDamage, n�o atualiza o movimento
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
                // O Jogador est� em movimento
                isIdle = false;
                isMoving = true;

                // Atualiza a �ltima dire��o que o personagem est� a olhar
                lastFacingDirection = movementInput.normalized;
            }
            else
            {
                // O Jogador est� parado
                isIdle = true;
                isMoving = false;
            }
        }

        if (GameManager.gameManager.State == GameState.ShowResults)
        {

            if (GameManager.gameManager.GetWinner() == null)
            {
                animator.SetTrigger("TrLose");
                return;
            }

            else if (GameManager.gameManager.GetWinner()["PlayerID"] == playerID && check == true)
            {
                animator.SetTrigger("TrWin");
                check = false;
            }
            else if (GameManager.gameManager.GetWinner()["PlayerID"] != playerID && check == true)
            {
                animator.SetTrigger("TrLose");
                check = false;
            }
        }
        
    }

    private void FixedUpdate()
    {
        if(GameManager.gameManager.State == GameState.InGame)
        {
            // Se o personagem est� no esdo de TakeDamage, n�o precisa de se movimentar
            if (playerStateMachine.currentState is TakeDamageState)
            {
                return;
            }

            // O movimento do Jogador � processado somente quando ele se est� a mover
            if (isMoving)
            {
                MovePlayer(); // Move o Jogador
            }

            RotatePlayer(); // Atualiza a rota��o do Personagem
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

        // Ajusta o limite da linha Imagin�ria com base no lado em que o jogador est�
        float boundary = isPlayerOnLeftSide ? -fieldBoundaryOffSet : fieldBoundaryOffSet;

        // Impede o jogador de atravessar a linha imagin�ria
        if (isPlayerOnLeftSide && newPosition.x > boundary) // Se o Jogador est� na esquerda
        {
            newPosition.x = boundary;
        }
        else if (!isPlayerOnLeftSide && newPosition.x < boundary) // Se o Jogador est� na Direita
        {
            newPosition.x = boundary;
        }

        // Move o jogador para a sua nova posi��o calculada
        rb.MovePosition(newPosition);
    }

    private void RotatePlayer()
    {
        // Apenas rotaciona o personagem se ele estiver-se a mover
        if (isMoving)
        {
            // Clacula a rota��o alvo com base na dire��o do movimento
            Quaternion targetRotation = Quaternion.LookRotation(lastFacingDirection);

            // Faz o personagem olhar na dire��o do movimento
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
        }
    }
}