using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : PopupBase
{
    [SerializeField] private Button soundBtn; 
    [SerializeField] private Button musicBtn; 
    [SerializeField] private Image activeSoundImg;  
    [SerializeField] private Image unActiveSoundImg;  
    [SerializeField] private Image ActiveMusicImg;  
    [SerializeField] private Image unActiveMusicImg;  

    [SerializeField] private Button homeBtn;
    [SerializeField] private Button closeBtn;

    [SerializeField] private Button settingTrainingBtn;
    [SerializeField] private Button settingBattleBtn;

    public override void Init()
    {
        base.Init();

        soundBtn.onClick.RemoveAllListeners();
        soundBtn.onClick.AddListener(() =>
        {
            OnSoundButtonClick();
        });

        musicBtn.onClick.RemoveAllListeners();
        musicBtn.onClick.AddListener(() =>
        {
            OnMusicButtonClick();
        });

        homeBtn.onClick.RemoveAllListeners();
        homeBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Hide();

            Sequence seq = DOTween.Sequence();

            seq.Append(CanvasManager.Instance.DarkPanelActive())
               .AppendCallback(() =>
               {
                   GameManager.Instance.ChangeGameState(Game_State.Training);
                   CanvasManager.Instance.MovePlayerToTraining();

                   SoundManager.Instance.StopBGM();
               })
               .Append(CanvasManager.Instance.DarkPanelUnActive())
               .AppendCallback(() =>
               {
                   EnemyManager.Instance.EnemyController.Health.Init(EnemyManager.Instance.EnemyController);
                   EnemyManager.Instance.EnemyController.StateMachine.ChangeState(new EnemyIdleState(EnemyManager.Instance.EnemyController));

                   CanvasManager.Instance.OnUpdateUIEnemy();
                   CanvasManager.Instance.MoveEnemyToBattle();
               });
        });

        settingBattleBtn.onClick.RemoveAllListeners();
        settingBattleBtn.onClick.AddListener(() =>
        {
            Show();
        });

        settingTrainingBtn.onClick.RemoveAllListeners();
        settingTrainingBtn.onClick.AddListener(() =>
        {
            Show();
        });

        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(() => Hide());
    }

    [SerializeField] private Game_State previourGameState; 

    public override void Show()
    {
        base.Show();
        Debug.Log("Show setitng");

        previourGameState = GameManager.Instance.GameState;

        SoundManager.Instance.PlaySound(SoundKey.Bubble, 0.7f, 0.7f);
        if (GameManager.Instance.GameState == Game_State.Battle)
        {
            OnHomeButtonShow(true);
        }
        else
        {
            OnHomeButtonShow(false);
        }

        GameManager.Instance.ChangeGameState(Game_State.Pause);
        EnemyManager.Instance.EnemyController.Animator.speed = 0f;
    }

    public override void Hide()
    {
        base.Hide();
        GameManager.Instance.ChangeGameState(previourGameState);
        EnemyManager.Instance.EnemyController.Animator.speed = 1f;
    }

    public void OnHomeButtonShow(bool isShow)
    {
        homeBtn.gameObject.SetActive(isShow);
    }


    bool isSoundMute = false; 
    public void OnSoundButtonClick()
    {
        isSoundMute = !isSoundMute;
        SoundManager.Instance.MuteSound(isSoundMute);
        activeSoundImg.gameObject.SetActive(!isSoundMute);
        unActiveSoundImg.gameObject.SetActive(isSoundMute);
    }

    bool isMusicMute = false;
    public void OnMusicButtonClick()
    {
        isMusicMute = !isMusicMute;
        SoundManager.Instance.MuteBGM(isMusicMute);
        ActiveMusicImg.gameObject.SetActive(!isMusicMute);
        unActiveMusicImg.gameObject.SetActive(isMusicMute);
    }
}