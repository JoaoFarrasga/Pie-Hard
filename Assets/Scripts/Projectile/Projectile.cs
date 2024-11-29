using System.Collections;
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

    //[Header("Audio")]
    //[SerializeField] AudioClip hitClip;
    //[SerializeField] float hitSoundVolume;

    //[SerializeField] MeteorBehavior meteorBehavior;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); //initialize rigidbody component
        collider = GetComponent<Collider>();//initialize Collider component
        //audioSource = GetComponent<AudioSource>();
    }

    public Rigidbody GetRigidBodyComponent() { return rb; }

    public Collider GetColliderComponent() { return collider; }

    public int GetSpeed() { return speed; }

    public void SetID(int id) { projectileID = id; }

    public int GetID() { return projectileID; }

    //public AudioClip GetClip() { return hitClip; }

    //public float GetVolume() { return hitSoundVolume; }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Chao"))
    //    {
    //        SoundManager.soundManager.PlayAudio(hitClip, hitSoundVolume);
    //        Destroy(gameObject);
    //    }
    //}
}
