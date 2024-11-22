using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] int playerID;
    List<GameObject> handsList = new();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        }
        else
        {
            GameManager.gameManager.OnScoreChanged(other.GetComponent<Projectile>().GetID());// changes the score of the players
            Destroy(other.gameObject);//destroys projectile
        }
    }
}
