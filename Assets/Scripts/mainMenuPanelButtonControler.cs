using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenuPanelButtonControler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.gameObject.GetComponent<Image>().color = Color.gray;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.GetComponent<Image>().color = Color.black;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene("MainMenu");
        ScoreTracker.Instance.ResetScores();
    }
}
