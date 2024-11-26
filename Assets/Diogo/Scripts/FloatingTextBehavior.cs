using UnityEngine;
using TMPro;

public class FloatingTextBehavior : MonoBehaviour
{
    public float floatSpeed = 2f;          // Velocidade de movimento para cima
    public float lifetime = 1.5f;         // Tempo antes de desaparecer
    public Vector3 floatOffset = new Vector3(0, 1, 0); // Posição inicial relativa
    private TextMeshPro textMesh;

    void Start()
    {
        // Aplica um deslocamento inicial
        transform.position += floatOffset;

        // Encontra o TextMeshPro para personalizar
        textMesh = GetComponent<TextMeshPro>();

        // Destroi o objeto após o tempo de vida
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Faz o texto flutuar para cima
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
    }

    public void SetColor(Color color)
    {
        if (textMesh != null)
        {
            textMesh.color = color;
        }
    }

    public void SetText(string text)
    {
        if (textMesh != null)
        {
            textMesh.text = text;
        }
    }
}
