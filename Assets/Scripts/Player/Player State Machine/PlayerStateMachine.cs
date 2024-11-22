using UnityEngine;

// Gerencia os Estados do Jogador, permintindo alternar entre eles
public class PlayerStateMachine : MonoBehaviour
{
    // Referência ao Controlador do Jogador, onde se encontra a lógica do Movimento
    [HideInInspector] public PlayerController controller;

    // Estado atual em que o Jogador se encontra
    public PlayerState currentState;

    // Estados específicos que o Jogador pode ter
    public MovingState movingState;
    public IdleState idleState;
    public PickUpState pickUpState;
    public ThrowState throwState;
    public TakeDamageState takeDamageState;

    // Nome do estado atual (para fins de Debug e Visualização no inspetor)
    public string currentStateName;

    private void Awake()
    {
        // Obtém o componente PlayerController do Jogador
        controller = gameObject.GetComponent<PlayerController>();

        // Cria as instâncias dos estados
        movingState = new MovingState(this);
        idleState = new IdleState(this);

        // Inicializa o estado inicial do Jogador
        SwitchState(idleState);
    }

    private void Update()
    {
        // Chama o método OnUpdate do estado atual para lidar com as atualizações
        currentState.OnUpdate();
    }

    // Método para alternar entre diferentes estados de Jogador
    public void SwitchState(PlayerState newState)
    {
        // Chama OnExit no estadoo atual, se houver, para preparar a transição
        currentState?.OnExit();

        // Atualiza o estado atual para o novo estado
        currentState = newState;

        // Atualiza o nome do estado atual (para visualização, se necessário)
        currentStateName = $"{currentState}";
        
        // Chama OnEnter no novo estado para executar qualquer lógica necessária quando o estado for ativado
        currentState.OnEnter();
    }
}
