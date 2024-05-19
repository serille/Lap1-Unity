using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "ButtonBehavior/BackToMain")]
public class BackToMainMenuButtonBehavior : IButtonBehavior
{
    public override void OnButtonClick(PointerEventData eventData)
    {
        SceneManager.LoadScene("MainMenu");
        ScoreTracker.Instance.ResetScores();
    }
}
