using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "ButtonBehavior/Quit")]
public class QuitButtonBehavior : IButtonBehavior
{
    public override void OnButtonClick(PointerEventData eventData)
    {
        Application.Quit();
    }
}
