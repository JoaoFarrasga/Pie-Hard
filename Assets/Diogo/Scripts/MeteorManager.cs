using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeteorManager : MonoBehaviour
{
    public GameObject objectToSpawn;        // Prefab do meteoro
    public GameObject spawnPlane;          // Plano invis�vel para spawnar meteoros
    public Transform[] spawnPositions;     // Posi��es predefinidas na arena
    public float respawnDelay = 2f;        // Tempo de espera para respawnar meteoros
    public GameObject spawnEffect;         // Efeito visual ao spawnar (opcional)

    [SerializeField] private List<GameObject> activeMeteors = new();    // Meteoros ativos em cada posi��o
    [SerializeField] private bool gameStarted = false;      // Indica se o jogo j� come�ou

    private bool check = false;
    [SerializeField] GameObject breakEffect;

    void Start()
    {
        // Inicializa o array de meteoros ativos
        //activeMeteors = new GameObject[spawnPositions.Count];
    }

    void Update()
    {
        if (GameManager.gameManager.State == GameState.GameStart && check == false)
        {
            StartGame();
            check = true;
        }

        if (!gameStarted) return; // S� executa se o jogo come�ou

        // Verifica se um meteoro foi removido e respawna
        for (int i = 0; i < activeMeteors.Count; i++)
        {
            if (activeMeteors[i] == null)
            {
                StartCoroutine(RespawnMeteor(i));
            }
        }

        if (GameManager.gameManager.State == GameState.GameEnd || GameManager.gameManager.State == GameState.InitialScreen)
        {
            gameStarted = false;
            check = false;

            foreach (GameObject meteor in activeMeteors)
            {
                GameObject go = Instantiate(breakEffect, meteor.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.parent = null;
                go.transform.rotation = Quaternion.LookRotation(Vector3.up, Vector3.up);
                Destroy(meteor);
            }
            activeMeteors.Clear();
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
        //activeMeteors[index] = meteor;
        activeMeteors.Insert(index, meteor);
    }

    private IEnumerator RespawnMeteor(int index)
    {
        if (activeMeteors[index] != null) yield break;

        yield return new WaitForSeconds(respawnDelay);
        
        if(activeMeteors.Count != 0)
        {
            if (activeMeteors[index] == null)
            {
                activeMeteors.RemoveAt(index);
                SpawnMeteorAt(index);
            }
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

