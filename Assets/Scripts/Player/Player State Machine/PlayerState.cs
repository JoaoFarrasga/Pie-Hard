using UnityEngine;

// Classe abstrata que define a estrutura b�sica de um estado do Jogador
public abstract class PlayerState
{
    // Refer�ncia � m�quina de estados do Jogador
    protected PlayerStateMachine playerStateMachine;

    // Refer�ncia ao controlador do Jogador
    protected PlayerController playerController;

    // Constrututor que inicializa as refer�ncias � m�quina de estados e ao controlador
    public PlayerState(PlayerStateMachine player)
    {
        playerStateMachine = player;
        playerController = player.controller;
    }

    // M�todos abstratos que devem ser implementdaos pelas classes que herdam PlayerState
    public abstract void OnEnter();     // Chamada quando o estado � ativado
    public abstract void OnUpdate();    // Chamada a cada frame para atualizar o comportamento
    public abstract void OnExit();      // Chamada quando o estado � desativado
}

// Estado do Jogadro enquanto ele se move
public class MovingState : PlayerState
{
    // Construtor que passa a refer�ncia da StateMachine
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

// Estado do Jogador quando ele est� parado (Idle)
public class IdleState : PlayerState
{
    // Construtor que passa a refer�ncia da StateMachine
    public IdleState(PlayerStateMachine player) : base(player) { }

    public override void OnEnter()
    {
            
    }

    public override void OnUpdate()
    {
        // Se o Jogador come�a a se mover, troca para o estado Moving
        if (playerController.isMoving == true)
            playerStateMachine.SwitchState(playerStateMachine.movingState);
    }

    public override void OnExit()
    {

    }
}
