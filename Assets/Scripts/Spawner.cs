using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector3[] SpawnPositions;
    private Queue<int> closed;
    private Queue<float> closedTime;
    private List<int> opened;

    public GameObject[] playerPrefabs;

    public GameObject[] butterflyPrefabs;

    public float butterflySpawnMinX;
    public float butterflySpawnMaxX;
    public float butterflySpawnMinY;
    public float butterflySpawnMaxY;

    public int butterflyCount;

    public float reopenTime;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPositions = new Vector3[] {
            GameObject.Find("Spawn 1").transform.position,
            GameObject.Find("Spawn 2").transform.position,
            GameObject.Find("Spawn 3").transform.position,
            GameObject.Find("Spawn 4").transform.position,
            GameObject.Find("Spawn 5").transform.position,
            GameObject.Find("Spawn 6").transform.position,
            GameObject.Find("Spawn 7").transform.position,
            GameObject.Find("Spawn 8").transform.position
        };

        opened = new List<int>();
        opened.Add(0);
        opened.Add(1);
        opened.Add(2);
        opened.Add(3);
        opened.Add(4);
        opened.Add(5);
        opened.Add(6);
        opened.Add(7);
        closed = new Queue<int>();
        closedTime = new Queue<float>();

        for (int i = 0; i < GameData.playersInGame.Length; i++)
        {
            if (GameData.playersInGame[i])
            {
                this.SpawnPrefab(playerPrefabs[i]);
            }
        }

        if (butterflyCount > 0)
        {
            for (int i = 0; i < butterflyCount; i++)
            {
                Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(butterflySpawnMinX, butterflySpawnMaxX), UnityEngine.Random.Range(butterflySpawnMinY, butterflySpawnMaxY), 0);
                Instantiate(butterflyPrefabs[i % 2], spawnPosition, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        while (closedTime.Count > 0 && Time.time - reopenTime > closedTime.Peek())
        {
            opened.Add(closed.Dequeue());
            closedTime.Dequeue();
        }
    }

    public void SpawnPrefab(GameObject prefab) {
        // Force open a spawnPoint if none is available
        if (opened.Count < 1) {
            opened.Add(closed.Dequeue());
            closedTime.Dequeue();
        }
        int i = UnityEngine.Random.Range(0, opened.Count - 1);
        Instantiate(prefab, SpawnPositions[opened[i]], Quaternion.identity);
        closed.Enqueue(opened[i]);
        closedTime.Enqueue(Time.time);
        opened.RemoveAt(i);
    }
}
