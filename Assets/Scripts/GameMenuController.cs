using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using UnityEditor.VersionControl;






public class GameMenuController : MonoBehaviour
{
    public struct MassageContainer
    {
        public string massageStr;
        public int colorInt;
    }

    public CanvasGroup playerHUDCG;
    public CanvasGroup breakHUDCG;
    public CanvasGroup transitionHUDCG;
    public CanvasGroup massageCG;
    public CanvasGroup massageTxtCG;

    [HideInInspector] public int[] teamColorIndex = new int[2];
    public Image[] memberBar;
    public Color[] teamColor;

    private bool _isBreak = false;

    public GameController gameController;
    public TMP_Text massageTxt;
    public List<MassageContainer> massageList = new();
    public bool canTriggerNewMessage = true;

    private void Start()
    {
        Time.timeScale = 1;

        massageTxtCG.alpha = 0;
        massageCG.alpha = 0;
        transitionHUDCG.alpha = 1;
        transitionHUDCG.blocksRaycasts = false;
        transitionHUDCG.DOFade(0, 0.2f).SetUpdate(true);
        breakHUDCG.blocksRaycasts = false;

        SetMemberbarColor(0, teamColorIndex[0]);
        SetMemberbarColor(1, teamColorIndex[1]);

        gameController.MatchStarted += OnMatchStarted;
    }

    private void OnMatchStarted(object sender, EventArgs e)
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

    public void TriggerMassage(string maString, int maColor= 4)  // Color 4 = default)
    {
        MassageContainer ma = new();
        ma.massageStr = maString;
        ma.colorInt = maColor;

        massageList.Add(ma);

        if (canTriggerNewMessage) ShowMassage();
    }

  
    private void ResetTrigger()
    {
        if (massageList.Count > 0) ShowMassage();
        else canTriggerNewMessage = true;
    }

    private void ShowMassage()
    {
        if (massageList.Count > 0)
        {
            canTriggerNewMessage = false;
            Invoke(nameof(ResetTrigger), 2f);

            massageTxtCG.DOKill();
            massageTxtCG.alpha = 0;

            massageTxt.text = massageList[0].massageStr;
            massageTxt.color = teamColor[massageList[0].colorInt]; 
            massageList.RemoveAt(0);

            massageCG.DOKill();
            if (massageCG.alpha != 1)
            {
                massageCG.DOFade(1f, 0.2f);
            }

            massageTxtCG.DOFade(1, 0.2f);
            massageTxtCG.DOFade(0f, 0.2f).SetDelay(2f).OnComplete(() =>
            {
                massageCG.DOFade(0f, 0.3f);
            });
        }
    }
}
