using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class scoreboardController : MonoBehaviour
{
    public GameObject textPrefab;

    public string[] playerNames;

    private float prefabHeight;
    private float prefabWidth;
    private float XPadding;
    private float YPadding;

    private Text[,] playerScoreTexts;

    // Start is called before the first frame update
    void Start()
    {
        prefabHeight = textPrefab.GetComponent<RectTransform>().rect.height;
        prefabWidth = textPrefab.GetComponent<RectTransform>().rect.width;

        XPadding = ((this.GetComponent<RectTransform>().rect.width - (prefabWidth * (playerNames.Length + 2))) / playerNames.Length + 3) / 2;
        YPadding = ((this.GetComponent<RectTransform>().rect.height - (prefabHeight * (playerNames.Length + 1))) / playerNames.Length + 2) / 2;

        BuildTextItems();
    }

    // Update is called once per frame
    void Update()
    {
        for (int y = 0; y < playerNames.Length; y++)
        {
            for (int x = 0; x < playerNames.Length; x++)
            {
                playerScoreTexts[y, x].text = ScoreTracker.Instance.GetScore(y, x) >= 0 ? ScoreTracker.Instance.GetScore(y, x).ToString() : "-";
            }
            playerScoreTexts[y, playerNames.Length].text = ScoreTracker.Instance.GetScore(y).ToString();
        }
    }

    private void BuildTextItems()
    {
        playerScoreTexts = new Text[playerNames.Length, playerNames.Length + 1];
        for (int y = 0; y < playerNames.Length + 1; y++)
        {
            for (int x = 0; x < playerNames.Length + 2; x++)
            {
                Vector2 position = new Vector2((int)(XPadding * (x + 1)) + (prefabWidth * x) + prefabWidth / 2, (int)-((YPadding * (y + 1)) + (prefabHeight * y) + prefabHeight / 2));
                if (y == 0)
                {
                    if (x == 0)
                    {
                        continue;
                    }
                    else if ( x >= playerNames.Length + 1)
                    {
                        continue;
                    }
                    AddText(playerNames[x - 1], position);
                    continue;
                }
                if (x == 0)
                {
                    AddText(playerNames[y - 1], position);
                    continue;
                }
                playerScoreTexts[y - 1, x - 1] = AddText("-", position).GetComponent<Text>();
            }
        }
    }

    private GameObject AddText(string text, Vector2 position)
    {
        GameObject nto = Instantiate(textPrefab, new Vector2(0, 0), Quaternion.identity);
        nto.transform.SetParent(this.transform);
        nto.GetComponent<RectTransform>().anchoredPosition = position;
        nto.GetComponent<RectTransform>().localScale = new Vector3(1f, 1.3f, 1f);
        nto.GetComponent<Text>().text = text;
        return nto;
    }
}
