using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Agent Prefabs")]
    public GameObject predatorPrefab;
    public GameObject preyPrefab;

    [Header("Agent Spawn Properties")]
    public int predatorCount = 3;
    public int preyCount = 10;
    public Vector3 spawnAreaMin = new Vector3(-25f, 0f, -20f);
    public Vector3 spawnAreaMax = new Vector3(25f, 0f, 20f);

    [Header("Prey Respawn Properties")]
    public float respawnDelay = 10f;
    public Vector3 respawnPosition = new Vector3(0f, 0f, 0f);

    public static GameManager Instance;

    /*
     * Initializes the singleton instance of the GameManager.
     */
    private void Awake()
    {
        Instance = this;
    }

   /*
     * Spawns the initial predators and prey at random positions within the defined spawn area.
     */
    private void Start()
    {
        for (int i = 0; i < predatorCount; i++) {
            Vector3 spawnPosition = GetRandomPosition();
            GameObject predator = Instantiate(predatorPrefab, spawnPosition, Quaternion.identity);
            predator.transform.SetParent(transform);
            predator.name = "Predator_" + i;
        }

        for (int i = 0; i < preyCount; i++) {
            Vector3 spawnPosition = GetRandomPosition();
            GameObject prey = Instantiate(preyPrefab, spawnPosition, Quaternion.identity);
            prey.transform.SetParent(transform);
            prey.name = "Prey_" + i;
        }
    }

    /*
     * Generates a random position within the defined spawn area.
     * @return A random Vector3 position within the spawn area.
     */
    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float z = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
        return new Vector3(x, 0f, z);
    }

    /*
     * Starts the coroutine to respawn prey after a delay.
     */
    public void RespawnPrey()
    {
        StartCoroutine(SpawnPreyAfterDelay());
    }

    /*
     * Coroutine to handle the delayed respawn of prey.
     */
    private IEnumerator SpawnPreyAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        Instantiate(preyPrefab, respawnPosition, Quaternion.identity);
    }
}