using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public List<Team> Teams;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public GameMenuController menuController;

    private int _activeTeam;

    private void Awake()
    {
        Invoke(nameof(MatchBegin), 2f);
        bool[] usedTeamColors = new bool[4];
        
        for (short i = 0; i < Teams.Count; i++)
        {
            int teamColor;
            do
            {
                teamColor = Random.Range(0, 4);
            } while (usedTeamColors[teamColor]);

            Teams[i].teamColor = teamColor;
            menuController.teamColorIndex[i] = teamColor;
            usedTeamColors[teamColor] = true;
        }
    }

    void MatchBegin()
    {
        _activeTeam = 0;
        for (int i = 0; i < Teams.Count; i++)
        {
            Teams[i].turnEnded += OnturnEnded;
        }

        Turn();
    }

    void Turn()
    {
        cinemachineVirtualCamera.Follow = Teams[_activeTeam].GetActivePlayer().transform; 
        // Give control to the next character of the active player
        
        // Wait for player action
        Teams[_activeTeam].PlayerAction();
    }

    private void OnturnEnded(object sender, EventArgs e)
    {
        _activeTeam = (_activeTeam == 1) ? 0 : 1;
        Turn();
    }
}
