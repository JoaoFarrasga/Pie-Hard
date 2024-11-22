using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("ProjectileID")]
    [SerializeField] int projectileID;

    [Header("Player Reference")]
    [SerializeField] Player player;

    [HideInInspector] [Header("Projectile Atributes")]
    private Rigidbody rb;
    private Collider collider;
    private int speed = 10;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); //initialize rigidbody component
        collider = GetComponent<Collider>();//initialize Collider component
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && projectileID != 0 )//checks collision with player
    //    {
    //        GameManager.gameManager.OnScoreChanged(projectileID);// changes the score of the players
    //        Destroy(this.gameObject);//destroys projectile
    //    }
    //}

    public Rigidbody GetRigidBodyComponent() { return rb; }
    
    public Collider GetColliderComponent() { return collider; }

    public int GetSpeed() { return speed; }

    public void SetID(int id) { projectileID = id; }
    
    public int GetID() { return projectileID; }
}
