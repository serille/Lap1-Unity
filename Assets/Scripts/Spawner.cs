using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Prefabs of objects to be spawned
    public GameObject[] playerPrefabs;
    public GameObject[] butterflyPrefabs;
    public GameObject flySwarmPrefab;

    // Player spawn position data
    private List<Vector2> spawnPositions = new List<Vector2>();
    private Queue<int> closed = new Queue<int>();
    private Queue<float> closedTime = new Queue<float>();
    private List<int> opened = new List<int>();

    public float reopenTime;

    // Random spawned objects data
    public Vector2 randomPosSpawnMin;
    public Vector2 randomPosSpawnMax;
    public int butterflyCount = 4;
    public int flySwarmCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize available spawn positions
        foreach (Transform child in transform)
        {
            spawnPositions.Add(child.position);
        }
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            opened.Add(i);
        }

        // Spawn only players that were picked for the start of the game
        for (int i = 0; i < GameData.playersInGame.Length; i++)
        {
            if (GameData.playersInGame[i])
            {
                this.SpawnPrefab(playerPrefabs[i]);
            }
        }

        // Spawn butterflies
        if (butterflyCount > 0)
        {
            for (int i = 0; i < butterflyCount; i++)
            {
                Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(randomPosSpawnMin.x, randomPosSpawnMax.x), UnityEngine.Random.Range(randomPosSpawnMin.y, randomPosSpawnMax.y), 0);
                Instantiate(butterflyPrefabs[i % 2], spawnPosition, Quaternion.identity);
            }
        }

        // Spawn fly swarms
        if (flySwarmCount > 0)
        {
            for (int i = 0; i < flySwarmCount; i++)
            {
                Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(randomPosSpawnMin.x, randomPosSpawnMax.x), UnityEngine.Random.Range(randomPosSpawnMin.y, randomPosSpawnMax.y), 0);
                Instantiate(flySwarmPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Reopen "closed" spawn points after the set delay has passed
        while (closedTime.Count > 0 && Time.time - reopenTime > closedTime.Peek())
        {
            opened.Add(closed.Dequeue());
            closedTime.Dequeue();
        }
    }

    public void SpawnPrefab(GameObject prefab) {
        // Force open a spawn point if none is available
        if (opened.Count < 1) {
            opened.Add(closed.Dequeue());
            closedTime.Dequeue();
        }

        // Spawn the prefab at a randomly picked spawn point
        int i = UnityEngine.Random.Range(0, opened.Count - 1);
        GameData.playerObjects[prefab.GetComponent<PlayerMovement>().GetPlayerNum()] = Instantiate(prefab, spawnPositions[opened[i]], Quaternion.identity);

        // "close" the spawn point used
        closed.Enqueue(opened[i]);
        closedTime.Enqueue(Time.time);
        opened.RemoveAt(i);
    }
}
