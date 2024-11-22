using UnityEngine;

public class MeteorBehavior : MonoBehaviour
{
    public GameObject explosionEffect; // Prefab da explosão visual
    public GameObject smokeEffect;    // Prefab de fumaça contínua (opcional)
    public AudioClip spawnSound;      // Som ao spawnar (exemplo: som de assobio)
    public AudioClip explosionSound;  // Som da explosão ao bater no chão
    public float fallSpeed = 10f;     // Velocidade de queda

    private Rigidbody rb;             // Referência ao Rigidbody
    private AudioSource audioSource;  // Referência ao componente de áudio

    void Start()
    {
        // Certifique-se de que o meteoro tem um Rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Adiciona um AudioSource ao meteoro
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // Ativa o loop inicialmente

        // Toca o som de spawn imediatamente
        if (spawnSound != null)
        {
            audioSource.clip = spawnSound;
            audioSource.Play();

            // Desativa o loop para que o som toque apenas uma vez
            audioSource.loop = false;
        }
    }

    void FixedUpdate()
    {
        // Adiciona uma força constante para simular a queda
        rb.linearVelocity = new Vector3(0, -fallSpeed, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Detecta colisão com o chão
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Parar o som contínuo
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Tocar o som de explosão
            if (explosionSound != null)
            {
                audioSource.PlayOneShot(explosionSound);
            }

            // Criar a explosão visual
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            // Parar o meteoro no chão
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero; // Para o movimento do meteoro
                rb.isKinematic = true;     // Faz o meteoro parar no chão
            }

            // Criar fumaça contínua
            if (smokeEffect != null)
            {
                Instantiate(smokeEffect, transform.position, Quaternion.identity);
            }
        }
    }
}
