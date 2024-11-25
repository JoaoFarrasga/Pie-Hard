using UnityEngine;

public class MeteorBehavior : MonoBehaviour
{
    public Transform target;      // Posição de destino
    public float fallSpeed = 10f; // Velocidade da queda
    private bool hasLanded = false;

    void Start()
    {
        // Garantir que o Rigidbody está configurado corretamente ao iniciar
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Permitir movimento físico durante a queda
            rb.useGravity = false;  // Gravidade é controlada manualmente
        }

        // O Collider não é trigger enquanto o meteoro cai
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false; // Sólido durante a queda
        }
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

        Debug.Log("Meteoro aterrou e está pronto para ser apanhado.");
    }
}
