using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : PopupBase
{
    [Header("Sound")]
    [SerializeField] private Button soundBtn;
    [SerializeField] private Image activeSoundImg;
    [SerializeField] private Image unActiveSoundImg;
    [SerializeField] private Image soundContainImg;
    [SerializeField] private Sprite activeContainSound;
    [SerializeField] private Sprite unActiveContainSound;
    [SerializeField] private Text soundBtnTxt;

    [Header("Music")]
    [SerializeField] private Button musicBtn;
    [SerializeField] private Image ActiveMusicImg;
    [SerializeField] private Image unActiveMusicImg;
    [SerializeField] private Image musicContainImg;
    [SerializeField] private Sprite activeContainMusic;
    [SerializeField] private Sprite unActiveContainMusic;
    [SerializeField] private Text musicBtnTxt;

    [SerializeField] private Button homeBtn;
    [SerializeField] private Button closeBtn;

    [SerializeField] private Button settingTrainingBtn;
    [SerializeField] private Button settingBattleBtn;

    private bool isSoundMute = false;
    private bool isMusicMute = false;

    [SerializeField] private Game_State previourGameState;

    public override void Init()
    {
        base.Init();

        soundBtn.onClick.RemoveAllListeners();
        soundBtn.onClick.AddListener(() =>
        {
            AnimateButton(soundBtn.transform);
            OnSoundButtonClick();
        });

        musicBtn.onClick.RemoveAllListeners();
        musicBtn.onClick.AddListener(() =>
        {
            AnimateButton(musicBtn.transform);
            OnMusicButtonClick();
        });

        homeBtn.onClick.RemoveAllListeners();
        homeBtn.onClick.AddListener(() =>
        {
            AnimateButton(homeBtn.transform);
            SoundManager.Instance.PlaySound(SoundKey.ButtonClick, 0.7f, 0.7f);
            Hide();

            Sequence seq = DOTween.Sequence();

            seq.Append(CanvasManager.Instance.DarkPanelActive())
               .AppendCallback(() =>
               {
                   GameManager.Instance.ChangeGameState(Game_State.Training);
                   CanvasManager.Instance.MovePlayerToTraining();
                   SoundManager.Instance.PlayBGM(SoundKey.TrainingBGM, 1f);
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
            AnimateButton(settingBattleBtn.transform);
            Show();
        });

        settingTrainingBtn.onClick.RemoveAllListeners();
        settingTrainingBtn.onClick.AddListener(() =>
        {
            AnimateButton(settingTrainingBtn.transform);
            Show();
        });

        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(() =>
        {
            AnimateButton(closeBtn.transform);
            EnemyManager.Instance.EnemyController.Health.EnemyAuraEffect.SetTimescaleEffect(0);
            Hide();
        });
    }

    public override void Show()
    {
        base.Show();
        previourGameState = GameManager.Instance.GameState;

        SoundManager.Instance.PlaySound(SoundKey.Bubble, 0.7f, 0.7f);
        if (GameManager.Instance.GameState == Game_State.Battle)
        {
            OnHomeButtonShow(true);
            EnemyManager.Instance.EnemyController.Rigidbody.isKinematic = true;
            EnemyManager.Instance.EnemyController.Health.EnemyAuraEffect.SetTimescaleEffect(0);
        }
        else
        {
            OnHomeButtonShow(false);
        }

        GameManager.Instance.ChangeGameState(Game_State.Pause);
        EnemyManager.Instance.EnemyController.Animator.speed = 0f;
        PlayerController.Instance.Animator.speed = 0f;
    }

    public override void Hide()
    {
        base.Hide();
        GameManager.Instance.ChangeGameState(previourGameState);
        EnemyManager.Instance.EnemyController.Animator.speed = 1f;
        PlayerController.Instance.Animator.speed = 1f;
        EnemyManager.Instance.EnemyController.Rigidbody.isKinematic = false;
    }

    public void OnHomeButtonShow(bool isShow)
    {
        homeBtn.gameObject.SetActive(isShow);
    }

    public void OnSoundButtonClick()
    {
        isSoundMute = !isSoundMute;
        SoundManager.Instance.MuteSound(isSoundMute);
        activeSoundImg.gameObject.SetActive(!isSoundMute);
        unActiveSoundImg.gameObject.SetActive(isSoundMute);

        soundContainImg.sprite = isSoundMute ? unActiveContainSound : activeContainSound;
        soundBtnTxt.text = isSoundMute ? "OFF" : "ON";

        SoundManager.Instance.PlaySound(SoundKey.Bubble, 0.7f, 0.7f);
    }

    public void OnMusicButtonClick()
    {
        isMusicMute = !isMusicMute;
        SoundManager.Instance.MuteBGM(isMusicMute);
        ActiveMusicImg.gameObject.SetActive(!isMusicMute);
        unActiveMusicImg.gameObject.SetActive(isMusicMute);

        musicContainImg.sprite = isMusicMute ? unActiveContainMusic : activeContainMusic;
        musicBtnTxt.text = isMusicMute ? "OFF" : "ON";

        SoundManager.Instance.PlaySound(SoundKey.Bubble, 0.7f, 0.7f);
    }

    /// <summary>
    /// 🔹 Làm hiệu ứng scale cho nút khi bấm (1.0 → 1.2 → 1.0 trong 0.1s)
    /// </summary>
    private void AnimateButton(Transform target)
    {
        target.DOKill(); // hủy tween cũ nếu có
        target.localScale = Vector3.one;
        target.DOScale(1.1f, 0.05f)
              .SetEase(Ease.OutQuad)
              .OnComplete(() => target.DOScale(1f, 0.05f).SetEase(Ease.InQuad));
    }
}
