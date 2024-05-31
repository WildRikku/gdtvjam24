using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;


public class GameMenuController : MonoBehaviour
{
    public struct MessageContainer
    {
        public string messageStr;
        public int colorInt;
    }

    public CanvasGroup playerHUDCG;
    public CanvasGroup breakHUDCG;
    public CanvasGroup transitionHUDCG;
    public CanvasGroup messageCG;
    public CanvasGroup messageTxtCG;
    public CanvasGroup weaponChooseCG;
    public CanvasGroup gameEndHudCG;

    private List<GameObject> _weaponChooseLayoutPanels;
    public GameObject weaponChooseLayoutPanelPrefab;
    public GameObject weaponChooseButtonPrefab;

    [HideInInspector] public int[] teamColorIndex = new int[2];
    public Image[] memberBar;
    public Color[] teamColor;
    public Image weaponChooseBarImage;
    public TMP_Text gameEndText;
    public TMP_Text roundTimerText;
    public Transform timerContainer;

    private bool _isBreak = false;

    public GameController gameController;
    public TMP_Text messageTxt;
    public List<MessageContainer> messageList = new();
    private bool canTriggerNewMessage = true;
    private float remainingTime = 0;
    private float timeSinceLastUpdate = -10;

    public event EventHandler TurnTimeIsUp;

    private void Start()
    {
        Time.timeScale = 1;

        playerHUDCG.alpha = 0;
        playerHUDCG.DOFade(1f, 2f);
        weaponChooseCG.alpha = 0;
        messageTxtCG.alpha = 0;
        messageCG.alpha = 0;
        transitionHUDCG.alpha = 1;
        transitionHUDCG.blocksRaycasts = false;
        transitionHUDCG.DOFade(0, 0.2f).SetUpdate(true);
        breakHUDCG.alpha = 0;
        breakHUDCG.blocksRaycasts = false;
        gameEndHudCG.alpha = 0;
        gameEndHudCG.blocksRaycasts = false;

        SetMemberbarColor(0, teamColorIndex[0]);
        SetMemberbarColor(1, teamColorIndex[1]);
        
        // generate weapon choose buttons
        _weaponChooseLayoutPanels = new();
        foreach (Team t in gameController.teams)
        {
            GameObject lp = Instantiate(weaponChooseLayoutPanelPrefab, weaponChooseCG.transform);
            _weaponChooseLayoutPanels.Add(lp);
            for(int i = 0; i < t.weapons.Count; i++)
            {
                Weapon w = t.weapons[i];
                GameObject btn = Instantiate(weaponChooseButtonPrefab, lp.transform);
                WeaponChooseBtn btnclass = btn.GetComponent<WeaponChooseBtn>();
                btnclass.btnName = w.name;
                btnclass.btnDescription = w.description;
                btnclass.btnSprite = w.buttonSprite;
                btnclass.btnIndex = i;
            }
        }

        gameController.MatchStarted += OnMatchStarted;
        gameController.TurnStarted += OnTurnStarted;
    }

    private void OnTurnStarted(GameController gameController)
    {
        ResetTurnTimerText(teamColorIndex[gameController.activeTeam]);
        ShowWeaponHUD(teamColorIndex[gameController.activeTeam]);
        foreach (GameObject lp in _weaponChooseLayoutPanels)
        {
            lp.SetActive(false);
        }
        _weaponChooseLayoutPanels[gameController.activeTeam].SetActive(true);
    }

    private void OnMatchStarted(GameController gameController)
    {
        SetMemberbar(0, Team.MaxTeamMembers, Team.MaxTeamMembers);
        SetMemberbar(1, Team.MaxTeamMembers, Team.MaxTeamMembers);
        gameController.teams[0].TeamUpdated += OnTeamUpdated;
        gameController.teams[1].TeamUpdated += OnTeamUpdated;
    }

    private void OnTeamUpdated(short teamIndex, int memberCount)
    {
        SetMemberbar(teamIndex, Team.MaxTeamMembers, memberCount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _isBreak == false)
        {
            breakHUDCG.DOFade(1, 0.2f).SetUpdate(true);
            breakHUDCG.blocksRaycasts = true;
            Time.timeScale = 0;
        }

        //timer
        remainingTime -= Time.deltaTime;
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= 1f)
        {
            TimerStep();
            timeSinceLastUpdate = 0f;
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }

    // ------------------------------------
    // PlayerHUD
    private void SetMemberbar(int memberBarIndex, int teamMaxValue, int teamCurrentValue)
    {
        float currentFillAmount = (float)teamCurrentValue / (float)teamMaxValue;
        memberBar[memberBarIndex].DOFillAmount(currentFillAmount, 0.2f);
    }

    private void SetMemberbarColor(int memberBarIndex, int colorindex)
    {
        memberBar[memberBarIndex].color = teamColor[colorindex];
    }

    public void ShowWeaponHUD(int panelColorIndex = 4)
    {
//        weaponChooseBarImage.color = teamColor[panelColorIndex];

        weaponChooseCG.DOFade(1, 0.4f);
        weaponChooseCG.blocksRaycasts = true;
    }

    public void HideWeaponHUD()
    {
        weaponChooseCG.DOFade(0, 0.3f);
        weaponChooseCG.blocksRaycasts = false;
    }

    public void ResetTurnTimerText(int teamColorIndex, int timerValue = 30)
    {
        remainingTime = timerValue;
        timeSinceLastUpdate = 0f;
        roundTimerText.color = teamColor[teamColorIndex];
        TimerStep();
    }

    private void TimerStep()
    {
        int timerText = Mathf.Max(0, Mathf.RoundToInt(remainingTime));

        roundTimerText.text = timerText.ToString();
        timerContainer.DOShakeScale(0.5f, 0.1f,10,90,true,ShakeRandomnessMode.Harmonic);

        if (remainingTime <= 0f)
        {
            TurnTimeIsUp?.Invoke(this, EventArgs.Empty);
        }
    }

    // TODO: Delete and use this from another place
    void triggerWeaponBar()
    {
        ShowWeaponHUD(1);
    }

    // ------------------------------------
    // BreakHUD
    public void BackToGame()
    {
        breakHUDCG.DOFade(0, 0.2f).SetUpdate(true);
        breakHUDCG.blocksRaycasts = false;
        Time.timeScale = 1;
    }

    public void GameRetry()
    {
        transitionHUDCG.blocksRaycasts = true;
        transitionHUDCG.DOFade(1, 0.2f).SetUpdate(true).OnComplete(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    public void BackToMenu()
    {
        transitionHUDCG.blocksRaycasts = true;
        transitionHUDCG.DOFade(1, 0.2f).SetUpdate(true).OnComplete(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MenuScene");
        });
    }

    // ------------------------------------
    // Game End HUD
    public void ShowGameEndHUD(short teamIndex)
    {
        // Teamindex loose
        gameEndText.text = $"Congratulations Team {teamIndex+1} wins the competition.";

        Time.timeScale = 0;
        gameEndHudCG.DOFade(1,0.2f).SetUpdate(true);
        gameEndHudCG.blocksRaycasts = true;
        
    }


    // ------------------------------------
    // Message System
    public void TriggerMessage(string maString, int maColor = 4)  // Color 4 = default)
    {
        MessageContainer ma = new();
        ma.messageStr = maString;
        ma.colorInt = maColor;

        messageList.Add(ma);

        if (canTriggerNewMessage) ShowMessage();
    }

    private void ResetTrigger()
    {
        if (messageList.Count > 0) ShowMessage();
        else canTriggerNewMessage = true;
    }

    private void ShowMessage()
    {
        if (messageList.Count > 0)
        {
            canTriggerNewMessage = false;
            Invoke(nameof(ResetTrigger), 2f);

            messageTxtCG.DOKill();
            messageTxtCG.alpha = 0;

            messageTxt.text = messageList[0].messageStr;
            messageTxt.color = teamColor[messageList[0].colorInt];
            messageList.RemoveAt(0);

            messageCG.DOKill();
            if (messageCG.alpha != 1)
            {
                messageCG.DOFade(1f, 0.2f);
            }

            messageTxtCG.DOFade(1, 0.2f);
            messageTxtCG.DOFade(0f, 0.2f).SetDelay(2f).OnComplete(() =>
            {
                messageCG.DOFade(0f, 0.3f);
            });
        }
    }
}
