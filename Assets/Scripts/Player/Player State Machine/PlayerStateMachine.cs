using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [HideInInspector] public PlayerController controller;

    public PlayerState currentState;
    public MovingState movingState;
    public IdleState idleState;

    public string currentStateName;

    private void Awake()
    {
        controller = gameObject.GetComponent<PlayerController>();

        movingState = new MovingState(this);
        idleState = new IdleState(this);

        SwitchState(idleState);
    }

    private void Update()
    {
        currentState.OnUpdate();
    }

    public void SwitchState(PlayerState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentStateName = $"{currentState}";
        currentState.OnEnter();
    }
}
