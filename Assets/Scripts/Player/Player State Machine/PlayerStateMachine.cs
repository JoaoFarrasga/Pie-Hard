using UnityEngine;

// Gerencia os Estados do Jogador, permintindo alternar entre eles
public class PlayerStateMachine : MonoBehaviour
{
    // Refer�ncia ao Controlador do Jogador, onde se encontra a l�gica do Movimento
    [HideInInspector] public PlayerController controller;

    // Estado atual em que o Jogador se encontra
    public PlayerState currentState;

    // Estados espec�ficos que o Jogador pode ter
    public MovingState movingState;
    public IdleState idleState;
    public PickUpState pickUpState;
    public ThrowState throwState;
    public TakeDamageState takeDamageState;

    // Nome do estado atual (para fins de Debug e Visualiza��o no inspetor)
    public string currentStateName;

    private void Awake()
    {
        // Obt�m o componente PlayerController do Jogador
        controller = gameObject.GetComponent<PlayerController>();

        // Cria as inst�ncias dos estados
        movingState = new MovingState(this);
        idleState = new IdleState(this);

        // Inicializa o estado inicial do Jogador
        SwitchState(idleState);
    }

    private void Update()
    {
        // Chama o m�todo OnUpdate do estado atual para lidar com as atualiza��es
        currentState.OnUpdate();
    }

    // M�todo para alternar entre diferentes estados de Jogador
    public void SwitchState(PlayerState newState)
    {
        // Chama OnExit no estadoo atual, se houver, para preparar a transi��o
        currentState?.OnExit();

        // Atualiza o estado atual para o novo estado
        currentState = newState;

        // Atualiza o nome do estado atual (para visualiza��o, se necess�rio)
        currentStateName = $"{currentState}";
        
        // Chama OnEnter no novo estado para executar qualquer l�gica necess�ria quando o estado for ativado
        currentState.OnEnter();
    }
}
