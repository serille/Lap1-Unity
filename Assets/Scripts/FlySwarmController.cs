using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlySwarmController : MonoBehaviour
{
    public GameObject flyPrefab;

    public float outerRadius;
    public float innerRadius;

    public float playerClosenessThreshold;

    public int flyCount;
    public float flySpeed;

    private float audioVolume;

    private GameObject[] flies;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        flies = new GameObject[flyCount];
        for (int i = 0; i < flyCount; i++)
        {
            flies[i] = Instantiate(flyPrefab, (Random.insideUnitCircle * outerRadius) + (Vector2)this.transform.position, Quaternion.identity); 
        }

        EventManager.fliesVolumeChanged.AddListener(FliesVolumeChanged);
        audioSource.volume = GameData.fliesVolume;
        audioVolume = GameData.fliesVolume;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 center = GetSwarmCenter();
        UpdateSwarmVolume(center);
        for (int i = 0;i < flyCount; i++)
        {
            UpdateFly(center, flies[i]);
        }
    }

    public void OnDestroy()
    {
        EventManager.fliesVolumeChanged.RemoveListener(FliesVolumeChanged);
    }

    public void FliesVolumeChanged()
    {
        audioSource.volume = GameData.fliesVolume;
    }

    Vector2 GetSwarmCenter()
    {
        Vector2 center = Vector2.zero;
        for (int i = 0;i < flies.Length;i++)
        {
            center += (Vector2)flies[i].transform.position;
        }
        center /= flies.Length;
        return center;
    }

    void UpdateSwarmVolume(Vector2 center)
    {
        // The fly swarm sound plays continuously, and just has its volume modified
        GameObject closest = GetClosestPlayer(center);
        if (closest != null)
        {
            audioSource.volume = audioVolume - ((Vector2)closest.transform.position - center).magnitude / 10;
        }
    }

    void UpdateFly(Vector2 swarmCenter, GameObject fly)
    {
        GameObject closest = GetClosestPlayer(fly.transform.position);

        // Pick a random direction
        Vector2 direction = GetRandomDirection();

        // Check if inside the radii and closeness to players
        Vector2 directionModifier = Vector2.zero;
        Vector2 SwarmCenterToFly = (swarmCenter - (Vector2)fly.transform.position);
        if (SwarmCenterToFly.magnitude > outerRadius)
        {
            directionModifier = SwarmCenterToFly.normalized;
        }
        else if (SwarmCenterToFly.magnitude < innerRadius)
        {
            directionModifier = SwarmCenterToFly.normalized * -1;
        }
        if (closest != null)
        {
            Vector2 closestPlayerVec = ((Vector2)closest.transform.position - (Vector2)fly.transform.position);
            if (closestPlayerVec.magnitude < playerClosenessThreshold)
            {
                // Use a ratio of how close the player is to the fly so it'll move out of the way faster the closest it is
                directionModifier = closestPlayerVec.normalized * -1f * (playerClosenessThreshold - closestPlayerVec.magnitude) / playerClosenessThreshold;
            }
        }

        direction += directionModifier;
        direction.Normalize();
        direction *= flySpeed;
        fly.transform.position = fly.transform.position + (Vector3)direction;
    }

    Vector2 GetRandomDirection()
    {
        Vector2 result = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        return result.normalized;
    }

    GameObject GetClosestPlayer(Vector2 position)
    {
        GameObject closest = null;
        float closestMagnitude = 0;
        GameObject[] players = GameData.playerObjects;
        foreach(GameObject player in players)
        {
            if (player == null) continue;
            if (closest == null)
            {
                closest = player;
                closestMagnitude = (position - (Vector2)player.transform.position).magnitude;
                continue;
            }

            if ((position - (Vector2)player.transform.position).magnitude < closestMagnitude)
            {
                closest = player;
                closestMagnitude = (position - (Vector2)player.transform.position).magnitude;
            }
        }

        return closest;
    }
}
