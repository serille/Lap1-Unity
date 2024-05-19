using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public IButtonBehavior buttonBehavior;
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
        buttonBehavior.OnButtonClick(eventData);
    }
}
