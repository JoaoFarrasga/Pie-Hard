using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine playerStateMachine;
    protected PlayerController playerController;

    public PlayerState(PlayerStateMachine player)
    {
        playerStateMachine = player;
        playerController = player.controller;
    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}

public class MovingState : PlayerState
{
    public MovingState(PlayerStateMachine player) : base(player) { }

    public override void OnEnter()
    {
        
    }

    public override void OnUpdate()
    {
        if (playerController.isIdle == true) 
            playerStateMachine.SwitchState(playerStateMachine.idleState);
    }

    public override void OnExit()
    {
        
    }
}

public class IdleState : PlayerState
{
    public IdleState(PlayerStateMachine player) : base(player) { }

    public override void OnEnter()
    {
            
    }

    public override void OnUpdate()
    {
        if (playerController.isMoving == true)
            playerStateMachine.SwitchState(playerStateMachine.movingState);
    }

    public override void OnExit()
    {

    }
}
