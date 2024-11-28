using UnityEngine;

public class MeteorBehavior : MonoBehaviour
{
    public Transform target;           // Posição de destino
    public float fallSpeed = 10f;      // Velocidade da queda
    public AudioClip spawnSound;       // Som ao spawnar
    public AudioClip explosionSound;   // Som ao bater no chão
    public float spawnSoundVolume;     // Volume do som de spawn (0 a 1)
    public float explosionSoundVolume; // Volume do som de impacto (0 a 1)
    public GameObject smokeEffect, smokeInTrip;     // Prefab do efeito de fumaça
    public GameObject groundDamagePrefab; // Prefab para a marca no chão

    private bool hasLanded = false;
    private AudioSource audioSource;

    void Start()
    {
        // Adicionar o AudioSource
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f; // Configurar som 3D (localizado no espaço)
        audioSource.rolloffMode = AudioRolloffMode.Linear; // Ajuste de rolagem do áudio (linear)

        // Tocar o som de spawn assim que o meteoro é criado
        if (spawnSound != null)
        {
            audioSource.PlayOneShot(spawnSound, spawnSoundVolume);
        }

        // Configurar o Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false;  // Gravidade será controlada manualmente
        }

        // Configurar o Collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false; // O meteoro é sólido durante a queda
        }

        smokeInTrip = InstantiateSmoke();

        //GameObject go = Instantiate(smokeEffect, gameObject.transform, false);
        //go.transform.localPosition = Vector3.zero;
        //Vector3 distance = transform.position - target.position;
        //transform.rotation = Quaternion.LookRotation(distance, Vector3.up);
    }

    void Update()
    {
        if (!hasLanded)
        {
            // Move o meteoro em direção ao alvo
            transform.position = Vector3.MoveTowards(transform.position, target.position, fallSpeed * Time.deltaTime);

            // Verifica se chegou ao destino
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Land();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detectar quando o meteoro atinge o chão
        if (!hasLanded && collision.transform.CompareTag("Ground")) // Certifique-se que o chão tem a tag "Ground"
        {
            Land();
        }
    }

    private void Land()
    {
        hasLanded = true;

        // Parar movimento físico
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // Para o movimento
            rb.isKinematic = true;     // Desativa a física
        }

        // Configurar o Collider como trigger para permitir interação
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true; // Ativar interação com o jogador
        }

        // Parar som atual (queda) antes de tocar o som de explosão
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // Interrompe o som em execução
        }

        // Tocar som de impacto/explosão
        if (explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound, explosionSoundVolume);
        }

        // Criar o efeito de fumaça
        if (smokeEffect != null)
        {
            //GameObject smoke = Instantiate(smokeEffect, transform.position, Quaternion.identity);
            
        }

        // Criar a marca no chão
        if (groundDamagePrefab != null)
        {
            Instantiate(groundDamagePrefab, transform.position, Quaternion.Euler(90, 0, 0)); // Rotação para alinhar ao chão
        }

        smokeInTrip.GetComponent<ParticleSystem>().Stop();

        Debug.Log("Meteoro aterrou, marca criada no chão.");
    }

    private GameObject InstantiateSmoke()
    {
        GameObject go = Instantiate(smokeEffect, gameObject.transform, false);
        go.transform.localPosition = Vector3.zero;
        Vector3 distance = transform.position - target.position;
        transform.rotation = Quaternion.LookRotation(distance, Vector3.up);

        return go;
    }
}
