using UnityEngine;

public class SpawnObjects1 : MonoBehaviour
{
    public GameObject objectToSpawn; // Prefab do objeto que será spawnado
    public Transform[] spawnPoints; // Array com os pontos de spawn
    public float delayBetweenSpawns = 0f; // Tempo de espera entre spawns (opcional)
    public GameObject spawnEffect; // Prefab de efeito visual (opcional)

    void Start()
    {
        // Inicia o spawn de todos os objetos assim que o jogo começa
        SpawnAllObjects();
    }

    public void SpawnAllObjects()
    {
        // Loop para criar os objetos em todos os pontos de spawn
        foreach (Transform spawnPoint in spawnPoints)
        {
            // Efeito visual (opcional)
            if (spawnEffect != null)
            {
                Instantiate(spawnEffect, spawnPoint.position, Quaternion.identity);
            }

            // Instanciar o objeto com rotação aleatória
            Instantiate(objectToSpawn, spawnPoint.position, Quaternion.Euler(0, Random.Range(0, 360), 0));

        }
    }
}
