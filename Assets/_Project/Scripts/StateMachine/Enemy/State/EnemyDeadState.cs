using UnityEngine;

public class EnemyDeadState : IState
{
    private EnemyController enemyController;
    AnimatorStateInfo info;

    public EnemyDeadState(EnemyController enemyController)
    {
        this.enemyController = enemyController;
    }

    public void Enter()
    {
        Debug.Log("Enter Dead");
        enemyController.Animator.SetTrigger("isDead");

        PopupManager.Instance.ShowPopup(Type_Popup.Win);

        enemyController.DynamicCollider.enabled = false;
    }

    public void Excute()
    {

    }
    public void Exit()
    {
        enemyController.Animator.ResetTrigger("isDead");
        enemyController.DynamicCollider.enabled = true;
    }
}
