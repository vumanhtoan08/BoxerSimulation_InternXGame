using UnityEngine;
using UnityEngine.UI;

public class PopupWin : PopupBase
{
    [SerializeField] private Button rewardButton;

    public override void Init()
    {
        base.Init();

        // setup for rewardButton
        rewardButton.onClick.RemoveAllListeners();
        rewardButton.onClick.AddListener(() =>
        {
            CanvasManager.Instance.MovePlayerToTraining();
            Hide();
            GameManager.Instance.ChangeGameState(Game_State.Training);
        });
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }
}