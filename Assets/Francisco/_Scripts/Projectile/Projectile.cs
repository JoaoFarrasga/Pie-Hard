using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int projectileID;
    [SerializeField] Player player;
    private Rigidbody rb;
    private Collider collider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); //initialize rigidbody component
        collider = GetComponent<Collider>();//initialize Collider component
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) //Checks if there is contact with players
        {
            foreach (Transform go in other.gameObject.transform) //iterates through the gameobject children
            {
                if (go.GetComponent<HandSpaceVerification>().VerifySpaceState())//checks if there are any empty hands left
                {
                    gameObject.transform.parent = go;//parents the projectile to the hand
                    gameObject.transform.localPosition = Vector3.zero;
                    go.GetComponent<HandSpaceVerification>().ChangeSpaceState();//indicates that this hand is not empty now
                    projectileID = other.gameObject.GetComponent<Player>().GetID();// the projectile gets the same id as the player
                    return;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Player>().GetID() != projectileID)//checks collision with player
        {
            GameManager.gameManager.OnScoreChanged(projectileID);// changes the score of the players
            Destroy(this.gameObject);//destroys projectile
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            rb.linearVelocity = Vector3.forward; //testing
            gameObject.transform.parent = null;
            collider.isTrigger = false;
        }
    }

    private void Throw()
    {
        rb.linearVelocity = Vector3.forward;
        gameObject.transform.parent = null;
        collider.isTrigger = false;
    }  //Implementar
}
