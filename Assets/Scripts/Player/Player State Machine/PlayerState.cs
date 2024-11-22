using UnityEngine;

// Classe abstrata que define a estrutura básica de um estado do Jogador
public abstract class PlayerState
{
    // Referência à máquina de estados do Jogador
    protected PlayerStateMachine playerStateMachine;

    // ReferÊncia ao controlador do Jogador
    protected PlayerController playerController;

    // Constrututor que inicializa as referências à máquina de estados e ao controlador
    public PlayerState(PlayerStateMachine player)
    {
        playerStateMachine = player;
        playerController = player.controller;
    }

    // Métodos abstratos que devem ser implementdaos pelas classes que herdam PlayerState
    public abstract void OnEnter();     // Chamada quando o estado é ativado
    public abstract void OnUpdate();    // Chamada a cada frame para atualizar o comportamento
    public abstract void OnExit();      // Chamada quando o estado é desativado
}

// Estado do Jogadro enquanto ele se move
public class MovingState : PlayerState
{
    // Construtor que passa a referência da StateMachine
    public MovingState(PlayerStateMachine player) : base(player) { }

    public override void OnEnter()
    {
        
    }

    public override void OnUpdate()
    {
        // Se o Jogador estiver Idle (Sem Movimento), troca para o estado Idle
        if (playerController.isIdle == true) 
            playerStateMachine.SwitchState(playerStateMachine.idleState);
    }

    public override void OnExit()
    {
        
    }
}

// Estado do Jogador quando ele está parado (Idle)
public class IdleState : PlayerState
{
    // Construtor que passa a referência da StateMachine
    public IdleState(PlayerStateMachine player) : base(player) { }

    public override void OnEnter()
    {
            
    }

    public override void OnUpdate()
    {
        // Se o Jogador começa a se mover, troca para o estado Moving
        if (playerController.isMoving == true)
            playerStateMachine.SwitchState(playerStateMachine.movingState);
    }

    public override void OnExit()
    {

    }
}

// Estado do Jogadro enquanto ele apanha um item
public class PickUpState : PlayerState
{
    // Construtor que passa a referência da StateMachine
    public PickUpState(PlayerStateMachine player) : base(player) { }

    public override void OnEnter()
    {

    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {

    }
}

// Estado do Jogadro quando ele atira o item
public class ThrowState : PlayerState
{
    // Construtor que passa a referência da StateMachine
    public ThrowState(PlayerStateMachine player) : base(player) { }

    public override void OnEnter()
    {

    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {

    }
}

// Estado do Jogadro quando ele leva dano
public class TakeDamageState : PlayerState
{
    private float timeSinceDamage; // Variável para contar o tempo desde que entrou no estado

    // Construtor que passa a referência da StateMachine
    public TakeDamageState(PlayerStateMachine player) : base(player) { }

    public override void OnEnter()
    {
        timeSinceDamage = 0f; // Reseta o contador de tempo ao entrar no estado
    }

    public override void OnUpdate()
    {
        timeSinceDamage += Time.deltaTime; // Incrementa o tempo no Estado

        // Torca de estado, após 2 Segundos
        if (timeSinceDamage >= 2f)
        {
            playerStateMachine.SwitchState(playerStateMachine.idleState);
        }
    }

    public override void OnExit()
    {

    }
}