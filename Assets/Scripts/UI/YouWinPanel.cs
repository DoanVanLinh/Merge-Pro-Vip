using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class YouWinPanel : MonoBehaviour
{
    public Image label;
    public Sprite labelWin;
    public Sprite labelLose;
    public TextMeshProUGUI textYouEarn;
    public ArrowWheel arrowWheel;
    public Button buttonClaim;
    public Button buttonNoThank;
    public Animator panelAni;
    public CoinCollectAnimationUI coinsCollectAniClaim;
    public CoinCollectAnimationUI coinsCollectAniNoThank;

    private void OnEnable()
    {
        label.sprite = GameManager.Instance.GetFightStatus() == FightStatus.Win ? labelWin : labelLose;
        textYouEarn.text = GameManager.Instance.coinsEachLevel.ToString();
        arrowWheel.isRotate = true;
        buttonClaim.onClick.AddListener(ClaimButton);
        buttonNoThank.onClick.AddListener(NoThankButton);
    }
    void ClaimButton()
    {
        //Ads
        SoundManager.Instance.Play(Helper.SOUND_BUTTON_CLICK);
        arrowWheel.isRotate = false;
        coinsCollectAniClaim.CollectAnimationButton(delegate { panelAni.Play("Off"); UIManager.Instance.satePanel.UpdateStatePanel(); });

    }
    void NoThankButton()
    {
        SoundManager.Instance.Play(Helper.SOUND_BUTTON_CLICK);
        
        coinsCollectAniNoThank.CollectAnimationButton(delegate { panelAni.Play("Off"); UIManager.Instance.satePanel.UpdateStatePanel(); });
    }

    public void SetPanelOff()
    {
        UIManager.Instance.youWinPanel.gameObject.SetActive(false);
        UIManager.Instance.mainMenuPanel.gameObject.SetActive(true);
        GameManager.Instance.LoadCurrentTeam();
    }
    private void OnDisable()
    {
        buttonClaim.onClick.RemoveAllListeners();
        buttonNoThank.onClick.RemoveAllListeners();
    }
}
