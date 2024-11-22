using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    [Header("PlayerInput")]
    [SerializeField] PlayerInput playerInput;
    private InputAction throwProjectile;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();//Gets the plyer input component of this object
        throwProjectile = playerInput.actions.FindAction("Throw");// Finds the action throw in the input system
        throwProjectile.Enable();
    }

    private void OnDisable()
    {
        throwProjectile.Disable();
    }

    private void Start()
    {
        throwProjectile.started += i => ThrowObject();//starts the action throw when clicked on the E key
    }

    private void ThrowObject()
    {
        foreach(Transform go in gameObject.transform)
        {
            if (go.GetComponent<HandSpaceVerification>().VerifySpaceState()) continue; //verifies if is empty or not, if is empty do not throw anything and goes to other child

            Transform projectile = go.GetChild(0);// gets the first child of the object
            //Gives movement to the projectile
            projectile.GetComponent<Projectile>().GetRigidBodyComponent().linearVelocity = Vector3.forward * projectile.GetComponent<Projectile>().GetSpeed();
            projectile.transform.parent = null; // makes the child without parent
            //projectile.GetComponent<Projectile>().GetColliderComponent().isTrigger = false;
            go.GetComponent<HandSpaceVerification>().ChangeSpaceState(); //Chnages the hand space to empty
            return;
        }
    }
}
