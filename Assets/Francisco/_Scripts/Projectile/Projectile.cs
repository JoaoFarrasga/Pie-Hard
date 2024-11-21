using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int projectileID;
    [SerializeField] Player player;
    private Rigidbody rb;
    private Collider collider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("PlayerContact");
            foreach (Transform go in other.gameObject.transform)
            {
                if (go.GetComponent<HandSpaceVerification>().VerifySpaceState())
                {
                    gameObject.transform.parent = go;
                    gameObject.transform.localPosition = Vector3.zero;
                    go.GetComponent<HandSpaceVerification>().ChangeSpaceState();
                    projectileID = other.gameObject.GetComponent<Player>().GetID();
                    return;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Player>().GetID() != projectileID)
        {
            GameManager.gameManager.OnScoreChanged(projectileID);
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            rb.linearVelocity = Vector3.forward;
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
