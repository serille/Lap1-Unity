using UnityEngine;
using UnityEngine.EventSystems;

public abstract class IButtonBehavior : ScriptableObject
{
    public abstract void OnButtonClick(PointerEventData eventData);
}
