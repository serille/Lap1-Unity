using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuGameStarter : MonoBehaviour
{
    public LayerMask playerLayer;

    public GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < players.Length; i++)
        {
            GameData.playersInGame[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer))) {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<BoxCollider2D>().bounds.center.x > 0)
                {
                    // 0 should be the center of the scene, pull every rabbit on its right (x > 0) in the game
                    GameData.playersInGame[i] = true;
                }
            }
            SceneManager.LoadScene("Game");
        }
    }
}
