using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupInfoEnemy : PopupBase
{
    [SerializeField] private TextMeshProUGUI enemyAttack; 
    [SerializeField] private TextMeshProUGUI enemyHealth; 
    [SerializeField] private Image image;
    [SerializeField] private Button fightButton;
    [SerializeField] private Button closePopup; 

    public override void Init()
    {   
        base.Init();
        // 
        UpdateInfoEnemy(); 

        // setup for fight button 
        fightButton.onClick.RemoveAllListeners();
        fightButton.onClick.AddListener(() =>
        {
            CanvasManager.Instance.MovePlayerToBattle();
            Hide();
            GameManager.Instance.ChangeGameState(Game_State.Battle);
        });

        // setup for close button
        closePopup.onClick.RemoveAllListeners();
        closePopup.onClick.AddListener(Hide);
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void UpdateInfoEnemy()
    {
        EnemyRuntimeData enemyData = EnemyManager.Instance.EnemyController.RuntimeData;
        enemyAttack.text = enemyData.EnemyData.Attack.ToString();
        enemyHealth.text = enemyData.EnemyData.Health.ToString();
    }
}