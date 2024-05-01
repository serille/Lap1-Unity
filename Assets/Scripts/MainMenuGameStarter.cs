using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuGameStarter : MonoBehaviour
{
    public LayerMask playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer))) {
            SceneManager.LoadScene("Game");
        }
    }
}
