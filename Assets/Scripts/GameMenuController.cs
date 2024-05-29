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

    private Team[] teams = new Team[2] { null, null };
    [HideInInspector] public int[] teamColorIndex = new int[2];
    public Image[] memberBar;
    public Color[] teamColor;

    private bool isBreak = false;

    private void Start()
    {
        Time.timeScale = 1;

        teams[0] = GameObject.Find("Team0").GetComponent<Team>();
        teams[1] = GameObject.Find("Team1").GetComponent<Team>();

        transitionHUDCG.alpha = 1;
        transitionHUDCG.blocksRaycasts = false;
        transitionHUDCG.DOFade(0, 0.2f).SetUpdate(true);
        breakHUDCG.blocksRaycasts = false;

        SetMemberbarColor(0, teamColorIndex[0]);
        SetMemberbarColor(1, teamColorIndex[1]);

        SetMemberbar(0, Team.MaxTeamMembers, Team.MaxTeamMembers);
        SetMemberbar(1, Team.MaxTeamMembers, Team.MaxTeamMembers);
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
        float currentFillAmount;
        currentFillAmount = teamCurrentValue / teamMaxValue;

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
