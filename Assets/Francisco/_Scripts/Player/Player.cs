using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] int playerID;
    List<GameObject> handsList = new();

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

    public int GetID() { return playerID; } //Gets playerID

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile") && other.gameObject.GetComponent<Projectile>().GetID() == 0) //Checks if there is contact with players
        {
            foreach (Transform go in gameObject.transform) //iterates through the gameobject children
            {
                if (go.GetComponent<HandSpaceVerification>().VerifySpaceState())//checks if there are any empty hands left
                {
                    other.gameObject.transform.parent = go;//parents the projectile to the hand
                    other.gameObject.transform.localPosition = Vector3.zero;
                    go.GetComponent<HandSpaceVerification>().ChangeSpaceState();//indicates that this hand is not empty now
                    other.gameObject.GetComponent<Projectile>().SetID(playerID);// the projectile gets the same id as the player
                    //other.gameObject.GetComponent<Projectile>().GetColliderComponent().isTrigger = false;
                    return;
                }
            }
        }else
        {
            GameManager.gameManager.OnScoreChanged(other.GetComponent<Projectile>().GetID());// changes the score of the players
            Destroy(other.gameObject);//destroys projectile
        }
    }

}
