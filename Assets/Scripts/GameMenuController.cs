using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    public CanvasGroup playerHUDCG;
    public CanvasGroup breakHUDCG;
    public CanvasGroup transitionHUDCG;
    
    [HideInInspector] public int[] teamColorIndex = new int[2];
    public Image[] memberBar;
    public Color[] teamColor;

    private bool isBreak = false;

    public GameController gameController;

    private void Start()
    {
        Time.timeScale = 1;
        
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
        gameController.Teams[0].TeamUpdated += OnTeamUpdated;
        gameController.Teams[1].TeamUpdated += OnTeamUpdated; 
    }

    private void OnTeamUpdated(short teamIndex, int memberCount)
    {
        SetMemberbar(teamIndex, Team.MaxTeamMembers, memberCount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isBreak == false)
        {
            breakHUDCG.DOFade(1, 0.2f).SetUpdate(true);
            breakHUDCG.blocksRaycasts = true;
            Time.timeScale = 0;
        }
    }


    // ------------------------------------
    // PlayerHUD
    public void SetMemberbar(int memberBarIndex, int teamMaxValue, int teamCurrentValue)
    {
        float currentFillAmount = (float)teamCurrentValue / teamMaxValue;

        memberBar[memberBarIndex].fillAmount = currentFillAmount;
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
}
