using UnityEngine;
using System.Collections;

public class MeteorManager : MonoBehaviour
{
    public GameObject objectToSpawn;        // Prefab do meteoro
    public GameObject spawnPlane;          // Plano invis�vel para spawnar meteoros
    public Transform[] spawnPositions;     // Posi��es predefinidas na arena
    public float respawnDelay = 2f;        // Tempo de espera para respawnar meteoros
    public GameObject spawnEffect;         // Efeito visual ao spawnar (opcional)

    private GameObject[] activeMeteors;    // Meteoros ativos em cada posi��o
    private bool gameStarted = false;      // Indica se o jogo j� come�ou

    void Start()
    {
        // Inicializa o array de meteoros ativos
        activeMeteors = new GameObject[spawnPositions.Length];
    }

    void Update()
    {
        if (!gameStarted) return; // S� executa se o jogo come�ou

        // Verifica se um meteoro foi removido e respawna
        for (int i = 0; i < activeMeteors.Length; i++)
        {
            if (activeMeteors[i] == null)
            {
                StartCoroutine(RespawnMeteor(i));
            }
        }
    }

    public void StartGame()
    {
        // Inicia o spawn de meteoros
        gameStarted = true;

        // Spawn inicial de meteoros
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            SpawnMeteorAt(i);
        }
    }

    private void SpawnMeteorAt(int index)
    {
        // Posi��o aleat�ria no plano invis�vel
        Vector3 randomSpawnPosition = GetRandomPositionOnPlane();

        // Posi��o de destino
        Transform target = spawnPositions[index];

        // Efeito visual no spawn (opcional)
        if (spawnEffect != null)
        {
            Instantiate(spawnEffect, randomSpawnPosition, Quaternion.identity);
        }

        // Instanciar meteoro
        GameObject meteor = Instantiate(objectToSpawn, randomSpawnPosition, Quaternion.identity);

        // Configurar o meteoro
        MeteorBehavior behavior = meteor.GetComponent<MeteorBehavior>();
        if (behavior != null)
        {
            behavior.target = target;
        }

        // Armazenar o meteoro ativo
        activeMeteors[index] = meteor;
    }

    private IEnumerator RespawnMeteor(int index)
    {
        if (activeMeteors[index] != null) yield break;

        yield return new WaitForSeconds(respawnDelay);

        if (activeMeteors[index] == null)
        {
            SpawnMeteorAt(index);
        }
    }

    private Vector3 GetRandomPositionOnPlane()
    {
        Renderer planeRenderer = spawnPlane.GetComponent<Renderer>();
        if (planeRenderer == null) return Vector3.zero;

        Vector3 planeSize = planeRenderer.bounds.size;
        Vector3 planeCenter = spawnPlane.transform.position;

        float randomX = Random.Range(-planeSize.x / 2, planeSize.x / 2);
        float randomZ = Random.Range(-planeSize.z / 2, planeSize.z / 2);

        return new Vector3(planeCenter.x + randomX, planeCenter.y, planeCenter.z + randomZ);
    }
}