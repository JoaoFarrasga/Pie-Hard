using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("ProjectileID")]
    [SerializeField] int projectileID;

    [HideInInspector]
    [Header("Projectile Atributes")]
    private Rigidbody rb;
    private Collider collider;
    [SerializeField] private int speed = 20;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); //initialize rigidbody component
        collider = GetComponent<Collider>();//initialize Collider component
    }

    public Rigidbody GetRigidBodyComponent() { return rb; }

    public Collider GetColliderComponent() { return collider; }

    public int GetSpeed() { return speed; }

    public void SetID(int id) { projectileID = id; }

    public int GetID() { return projectileID; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chao"))
        {
            Destroy(gameObject);
        }
    }
}
