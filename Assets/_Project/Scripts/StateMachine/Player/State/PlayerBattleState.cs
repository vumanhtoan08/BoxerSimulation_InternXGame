using UnityEngine;

public class PlayerBattleState : IState
{
    [Header("Ref")]
    private PlayerController playerController;

    private Vector3 velocity;
    private bool isGrounded;

    public PlayerBattleState(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Enter()
    {
        playerController.Animator.SetTrigger("isBattle");
    }

    public void Excute()
    {
        Moving();
        RegenerateStamina();
    }

    public void Exit()
    {

    }

    private void Moving()
    {
        isGrounded = Physics.CheckSphere(playerController.transform.position, playerController.GroundCheckDistance, playerController.GroundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float joyX = playerController.FixedJoystick.Horizontal;
        float joyZ = playerController.FixedJoystick.Vertical;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        float finalX = Mathf.Abs(joyX) > 0.1f ? joyX : inputX;
        float finalZ = Mathf.Abs(joyZ) > 0.1f ? joyZ : inputZ;

        Vector3 move = playerController.transform.right * finalX + playerController.transform.forward * finalZ;
        playerController.CharacterController.Move(move * playerController.Speed * Time.deltaTime);

        velocity.y += playerController.Gravity * Time.deltaTime;
        playerController.CharacterController.Move(velocity * Time.deltaTime);
    }

    private void RegenerateStamina()
    {
        if (GameManager.Instance.GameState != Game_State.Battle) return;

        float current = playerController.Data.CurrentStamina;
        float max = playerController.Data.DataRuntime.Stamina;

        float regenRate = max / 4f;
        float regenAmount = regenRate * Time.deltaTime;

        if (current < max)
        {
            playerController.Data.ChangeStamina(regenAmount);
        }
    }
}