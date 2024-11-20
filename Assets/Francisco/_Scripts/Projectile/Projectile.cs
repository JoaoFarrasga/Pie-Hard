using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int projectileID;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerScore>().GetID() != projectileID)
        {
            GameManager.gameManager.OnScoreChanged(projectileID);
            Destroy(this.gameObject);
        }
    }
}
