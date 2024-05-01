using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScoreDisplay : MonoBehaviour
{
    public int playerNum;

    public Sprite[] numberSprites;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int currentScore = ScoreTracker.Instance.GetScore(playerNum);
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = numberSprites[currentScore % 10];
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = numberSprites[(currentScore % 100) / 10];
    }
}
